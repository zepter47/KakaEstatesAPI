//using JamilNativeAPI.DataContext;
//using JamilNativeAPI.Entities;
using JamilNativeAPI.Respositories.Interfaces;
using JamilNativeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using JamilNativeAPI.Context;
using JamilNativeAPI.Entities;
using Microsoft.VisualBasic;
using JamilNativeAPI.TimeHelper;

namespace JamilNativeAPI.Respositories
{
    public class TenantRepository : ITenantManager
    {
        //create instance of KakaireEstatesContext to interact with the database
        private readonly KakaireEstatesContext _context;

        public TenantRepository(KakaireEstatesContext context)
        {
            _context = context;
        }

        public async Task AddHouseNumber(HouseDto houseNo)
        {
            var house = new TblHouse { HouseNumber = houseNo.HouseNumber};

           await _context.TblHouses.AddAsync(house);

            await _context.SaveChangesAsync();  
        }

        public async Task AddMaritalStatus(MaritalStatusDto status)
        {
			try
            {

                var TenantStatus = new TblMaritalstatus { Status = status.TStatus };

                //Adding the TblMariralStauts objects to the TblMariralStautses Dbset
                     //This prepares the object to be inserted into the database
                    await _context.TblMaritalstatuses.AddAsync(TenantStatus);

                //Save the changes to the database
                   // This actually performs the INSERT operation in the database
                    await _context.SaveChangesAsync();



            }
            catch (Exception)
			{

				throw;
			}
        }

        public async Task AddNokRelationship(NokRelationshipDto nok)
        {
            try
            {
                var relation = new TblNokRelationship { Relatioship = nok.Status };

                await _context.TblNokRelationships.AddAsync(relation);

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddTenantDetails(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House)
        {
            var marital = tenant.MaritalstatusId;
            var house = tenant.HouseId;
            var obudde = GetEastFricanTime.RetrieveEastFricanTime();


            //Get MaritalStatusId for the selected maritals status
            marital =  _context.TblMaritalstatuses.Where(y => y.Status == MaritalStatus).Select(x => x.MaritalstatusId).FirstOrDefault();

            //Get NokRelationshipId for the selected relationship
            tenant.NokRelationshipId = _context.TblNokRelationships.Where(x => x.Relatioship == Nok).Select(y => y.NokRelationshipId).FirstOrDefault();
            //Get HouseId for the selected house
            house = _context.TblHouses.Where(y => y.HouseNumber == House).Select(x => x.HouseId).FirstOrDefault();

            //Give the tbltenants columns new values in the textBoxes
            var tenantDetails = new TblTenant
            {
                FirstName = tenant.FirstName,
                LastName = tenant.LastName,
                NinNumber = tenant.NinNumber,
                BirthDate = tenant.BirthDate,
                Gender = tenant.Gender,
                PhoneNumber = tenant.PhoneNumber,
                OccupantsNumber = tenant.OccupantsNumber,
                MaritalstatusId = marital,
                NextofkinName = tenant.NextofkinName,
                NokRelationshipId = tenant.NokRelationshipId,
                NokPhonenumber = tenant.NokPhonenumber,
                HouseId = house,
                AddedOn = obudde
            };

            Console.WriteLine($"The date in uganda is {obudde}");

            //Adding the TblTenant objects to the TblTenantss Dbset
            //This prepares the object to be inserted into the database
            await _context.TblTenants.AddAsync(tenantDetails);

            //Save the changes to the database
            // This actually performs the INSERT operation in the database
            await _context.SaveChangesAsync();

        }


        public async Task<ObservableCollection<HouseDto>> GetHouseNumber()
        {
            var HouseCollection = new ObservableCollection<HouseDto>();
            try
            {
                var HouseCol = await _context.TblHouses.Select(x => new HouseDto() {
                    HouseNumber = x.HouseNumber }).ToListAsync();

                HouseCollection = new ObservableCollection<HouseDto>(HouseCol);
            }
            catch (Exception)
            {

                throw;
            }
            return HouseCollection;
        }

        public async Task<ObservableCollection<MaritalStatusDto>> GetMaritalStatus()
        {
            var maritalCollection = new ObservableCollection<MaritalStatusDto>();
            try
            {
                 var Mstatus = await _context.TblMaritalstatuses.Select(x => new MaritalStatusDto()
                 {
                     TStatus = x.Status
                 }).ToListAsync();

                maritalCollection = new ObservableCollection<MaritalStatusDto>(Mstatus);
            }
            catch (Exception)
            {

                throw;
            }
            return maritalCollection;
        }

        public async Task<ObservableCollection<NokRelationshipDto>> GetNokRelationships()
        {
            var relationshipCollection = new ObservableCollection<NokRelationshipDto>();

            try
            {
                var NRelation = await _context.TblNokRelationships.Select(y => new NokRelationshipDto()
                {
                    Status = y.Relatioship
                }).ToListAsync();

                relationshipCollection = new ObservableCollection<NokRelationshipDto>(NRelation);
            }
            catch (Exception)
            {

                throw;
            }
            return relationshipCollection;
        }

        public async Task<TenantDetailsDto> GetTenant(int id)
        {
            TenantDetailsDto tenant = new();

            try
            {
                var resident = await _context.TblTenants.Where(y => y.TenantId == id).Include(t => t.Maritalstatus).Include(t => t.NokRelationship)
                    .Include(t => t.House).Select(x => new TenantDetailsDto()
                    {
                        TenantId = x.TenantId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        NinNumber = x.NinNumber,
                        BirthDate = x.BirthDate,
                        Gender = x.Gender,
                        PhoneNumber = x.PhoneNumber,
                        OccupantsNumber = x.OccupantsNumber,
                        TdMaritalstatus = x.Maritalstatus.Status,
                        NextofkinName = x.NextofkinName,
                        TdNokRelationship = x.NokRelationship.Relatioship,
                        NokPhonenumber = x.NokPhonenumber,
                        TdHouse = x.House.HouseNumber,
                        AddedOn = x.AddedOn,
                    }).FirstOrDefaultAsync();

                tenant = resident;
            }
            catch (Exception)
            {

                throw;
            }

            return tenant;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantDetails()
        {
            var tenantCollection = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var tenantCol = await _context.TblTenants.Include(t => t.Maritalstatus)
                    .Include(t => t.NokRelationship)
                    .Include(t => t.House)
                    .Select(x => new TenantDetailsDto()
                {
                    TenantId = x.TenantId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    NinNumber = x.NinNumber,
                    BirthDate  = x.BirthDate,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    OccupantsNumber = x.OccupantsNumber,
                    TdMaritalstatus = x.Maritalstatus.Status,
                    NextofkinName = x.NextofkinName,
                    TdNokRelationship = x.NokRelationship.Relatioship,
                    NokPhonenumber = x.NokPhonenumber,
                    TdHouse = x.House.HouseNumber,
                    AddedOn = x.AddedOn,
                    //Period = EF.Functions.DateDiffDay(x.AddedOn, DateAndTime.Now)


                }).ToListAsync();

                Console.WriteLine($"The curren date is :{GetEastFricanTime.RetrieveEastFricanTime()}");

                tenantCollection = new ObservableCollection<TenantDetailsDto>(tenantCol);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollection;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantFirstName()
        {
            var tenantCollections = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var FirstNames = await _context.TblTenants.Select(w => new TenantDetailsDto()
                {
                    FirstName = w.FirstName
                }).ToListAsync();

                tenantCollections = new ObservableCollection<TenantDetailsDto>(FirstNames);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollections;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantLastName()
        {
            var tenantCollection = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var LastNames = await _context.TblTenants.Select(w => new TenantDetailsDto()
                {
                    LastName = w.LastName
                }).ToListAsync();

                tenantCollection = new ObservableCollection<TenantDetailsDto>(LastNames);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollection;

        }

    }
}
