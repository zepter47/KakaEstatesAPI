using System.Collections.ObjectModel;
using JamilNativeAPI.Models;
using JamilNativeAPI.Respositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JamilNativeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ITenantManager _estateManager;

        public TenantsController(ITenantManager estateManager)
        {
            _estateManager = estateManager;
        }

        [HttpPost("MaritalStatus")]
        public async Task<ActionResult> AddMaritalStatus(MaritalStatusDto MStatus)
        {
            await _estateManager.AddMaritalStatus(MStatus);

            return Ok();
        }

        [HttpPost("NokRelationship")]
        public async Task<ActionResult> AddNokRelationship(NokRelationshipDto relation)
        {
            await _estateManager.AddNokRelationship(relation);

            return Ok();
        }

        [HttpGet("MaritalStatus")]
        public async Task<ActionResult<ObservableCollection<MaritalStatusDto>>> GetmaritalDetails()
        {
            try
            {
                var MaritalCol = await _estateManager.GetMaritalStatus();

                return Ok(MaritalCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");
            }
        }

        [HttpGet("NokRelationship")]
        public async Task<ActionResult<ObservableCollection<NokRelationshipDto>>> GetNokDetails()
        {
            try
            {
                var NokCol = await _estateManager.GetNokRelationships();

                return Ok(NokCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpPost("TenantDetails")]
        public async Task<ActionResult> AddTenantsDetals(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House)
        {
            await _estateManager.AddTenantDetails(tenant, MaritalStatus, Nok, House);

            return Ok();
        }

        [HttpPost("HouseNumber")]
        public async Task<ActionResult> AddHouseNumber(HouseDto house)
        {
            await _estateManager.AddHouseNumber(house);

            return Ok();
        }

        [HttpGet("HouseNumber")]
        public async Task<ActionResult<ObservableCollection<HouseDto>>> RetrieveHouseNumber()
        {
            try
            {
                var houses = await _estateManager.GetHouseNumber();

                return Ok(houses);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");
            }

        }

        [HttpGet("TenantsDetails")]
        public async Task<ActionResult<ObservableCollection<TenantDetailsDto>>> RetrieveTenantsDetails()
        {
            try
            {
                var tenants = await _estateManager.GetTenantDetails();
                return Ok(tenants);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");

            }
        }

        //[HttpPost("WaterAmount")]
        //public async Task<ActionResult> AddWaterAmounts(WaterFactorDto water)
        //{
        //    await _estateManager.AddWaterAmount(water);

        //    return Ok();
        //}


        [HttpGet("TenantFirstName")]
        public async Task<ActionResult<ObservableCollection<TenantNamesDto>>> RetrieveTenantFirstName()
        {
            try
            {
                var FirstCol = await _estateManager.GetTenantFirstName();

                return Ok(FirstCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpGet("TenantLastName")]
        public async Task<ActionResult<ObservableCollection<TenantNamesDto>>> RetrieveTenantLastName()
        {
            try
            {
                var LastCol = await _estateManager.GetTenantLastName();

                return Ok(LastCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpGet("Tenant/{id}")]
        public async Task<ActionResult<TenantDetailsDto>> GetResident(int id)
        {
            try
            {
                var residentsDetails = await _estateManager.GetTenant(id);

                return Ok(residentsDetails);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Failure: {ex.Message}");
            }
        }
    }
}
