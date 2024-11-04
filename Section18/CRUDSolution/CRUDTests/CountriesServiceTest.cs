using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly PersonsDbContext _dbContext;

        public CountriesServiceTest(PersonsDbContext personsDbContext)
        {
            _countriesService = new CountriesService(new PersonsDbContext(
                                            new DbContextOptionsBuilder<PersonsDbContext>().Options));
            _dbContext = personsDbContext;
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async  () =>
            {
                //Act
                await _countriesService.AddCountry(request);
            });
        }

        //When CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public async Task AddCountry_CountryNameIsDuplicate()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "India" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "India" };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });
        }

        //When CountryName is proper, it should insert (add) the country to the list of existing countries.
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse countryResponse = await _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Equal(request.CountryName, countryResponse.CountryName);
            Assert.Contains(countryResponse, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public async Task GetAllCountries_EmptyList()
        {
            //Acts
           List<CountryResponse> actualCountryResponesList =  await _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponesList);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>
            {
                new CountryAddRequest(){ CountryName = "India" },
                new CountryAddRequest(){ CountryName = "China" },
                new CountryAddRequest(){ CountryName = "US" }
            };

            //Acts
            List<CountryResponse> addedCountries = new List<CountryResponse>();
            foreach (var country in country_request_list)
            {
                addedCountries.Add(await _countriesService.AddCountry(country));
            }
            List<CountryResponse> actualCountryResponesList = await _countriesService.GetAllCountries();

            //read each element from the actualCountryResponesList
            foreach (var expectedCountry in actualCountryResponesList)
            {
                Assert.Contains(expectedCountry, actualCountryResponesList);
            }
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        //If we supply null as CountryID, it should returns null as CountryRespone.
        public async Task GetCoutryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //A
            CountryResponse? countryResponse = await _countriesService.GetCountryByCountryId(countryID);

            //A
            Assert.Null(countryResponse);
        }

        [Fact]
        //If we supply valid CountryID, it should returns matching country as CountryRespone object
        public async Task GetCoutryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest { CountryName = "india" };
            CountryResponse countryResponse =  await _countriesService.AddCountry(countryAddRequest);

            //A
            CountryResponse? countryResponseByCountryId = await _countriesService.GetCountryByCountryId(countryResponse.CountryID);

            //A
            Assert.NotNull(countryResponseByCountryId);
            Assert.Equal(countryAddRequest.CountryName, countryResponseByCountryId.CountryName);
        }
        #endregion

    }
}
