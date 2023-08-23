using A_Domain.Models;
using A_Domain.Repo_interfaces;
using C_Logic.Interfaces;
using E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace C_Logic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IGenericRepository<RefreshToken, string> _refreshTokenRepository;
        private readonly SignInManager<UserIdentity> _signInManager;

        public IdentityService(UserManager<UserIdentity> userManager, JwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, IGenericRepository<RefreshToken, string> refreshTokenRepository, SignInManager<UserIdentity> signInManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = refreshTokenRepository;
            _signInManager = signInManager;
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "User with this email already exists!" }
                };
            }

            var newUser = new UserIdentity()
            {
                Email = request.Email,
                UserName = request.UserName
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthenticationResultForUserAsync(newUser, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthenticationResult> LoginAsync(string usernameOrEmail, string password)
        {
            var user = await _userManager.FindByNameAsync(usernameOrEmail) ?? await _userManager.FindByEmailAsync(usernameOrEmail);

            if (user == null)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "User does not exist!" }
                };
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
            {
                return new AuthenticationResult()
                {
                    Success = false,
                    Errors = new[] { "User password combination is wrong!" }
                };
            }

            bool isCompanyEmployee = false;
            if (user.UserName == "Admin" || user.UserName == "Manager")
            {
                isCompanyEmployee = true;
            }

            return await GenerateAuthenticationResultForUserAsync(user, isCompanyEmployee);
        }

        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(UserIdentity user, bool isCompanyEmployee)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            List<Claim> claims;

            if (isCompanyEmployee)
            {
                claims = new List<Claim>
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("id", user.Id)
                }
                .Union(await _userManager.GetClaimsAsync(user)).ToList();
            }

            else
            {
                claims = new List<Claim>
                {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim("id", user.Id),
                        new Claim("Role", "User")
                }
                .Union(await _userManager.GetClaimsAsync(user)).ToList();
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _refreshTokenRepository.AddAsync(refreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthenticationResult()
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }

        /// в идеале все эти проверки нужно вынести в другой вспомoгательный метод
        /// где проверить через case switch и юзеру писать всегда просто invalid token не объясняя причины
        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult() { Errors = new[] { "Invalid Jwt Token" } };
            }

            var expiryDateUnix =
                    long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(year: 1970, month: 1, day: 1, hour: 0, minute: 0, second: 0, DateTimeKind.Utc)
                                            .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult() { Errors = new[] { "This Jwt token hasn't expired yet!" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _refreshTokenRepository.GetAll().AsTracking().SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult() { Errors = new[] { "This refresh token does't exist!" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult() { Errors = new[] { "This refresh token has expired!" } };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult() { Errors = new[] { "This refresh token has been invalidated!" } };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult() { Errors = new[] { "This refresh token has been used!" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult() { Errors = new[] { "This refresh token doesn't match this JWT" } };
            }

            storedRefreshToken.Used = true;
            _refreshTokenRepository.Update(storedRefreshToken);
            await _refreshTokenRepository.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

            bool isCompanyEmployee = false;
            if (user.UserName == "Admin" || user.UserName == "Manager")
            {
                isCompanyEmployee = true;
            }

            return await GenerateAuthenticationResultForUserAsync(user, isCompanyEmployee);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }

            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(value: SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

