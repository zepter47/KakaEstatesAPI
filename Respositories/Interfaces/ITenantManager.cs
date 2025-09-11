using System.Collections.ObjectModel;
//using JamilNativeAPI.Entities;
using JamilNativeAPI.Models;

namespace JamilNativeAPI.Respositories.Interfaces
{
    public interface ITenantManager
    {
        public Task AddMaritalStatus(MaritalStatusDto status);

        public Task AddNokRelationship(NokRelationshipDto nok);

        public Task<ObservableCollection<MaritalStatusDto>> GetMaritalStatus();

        public Task<ObservableCollection<NokRelationshipDto>> GetNokRelationships();

        public Task AddTenantDetails(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House);

        public Task AddHouseNumber(HouseDto house);

        public Task<ObservableCollection<HouseDto>> GetHouseNumber();

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantDetails();

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantFirstName();

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantLastName();

        public Task<TenantDetailsDto>GetTenant(int id);

    }
}
