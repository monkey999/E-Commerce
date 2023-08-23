using System;

namespace E_Commerce_Shop.Contracts.V1.DTO_requests.CREATE
{
    public class CreateUserRequestDTO
    {
        public Guid UserId { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Adress { get; set; }
        public string Password { get; set; }
    }
}
