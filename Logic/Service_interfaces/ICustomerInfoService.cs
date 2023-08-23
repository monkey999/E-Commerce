using B_DataAccess.Contracts.V1.DTO_requests.CREATE;
using B_DataAccess.Contracts.V1.DTO_requests.UPDATE;
using B_DataAccess.Contracts.V1.DTO_responses.GET;
using System.Threading.Tasks;

namespace C_Logic.Interfaces
{
    public interface ICustomerInfoService
    {
        void AddCustomerInfo(CreateCustomerInfoRequestDTO customerInformation);
        Task<GetCustomerInfoResponseDTO> GetCustomerInfoAsync();
    }
}
