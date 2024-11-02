using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });
        }

        //When CountryName is duplicate, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsDuplicate()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "India" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "India" };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });
        }

        //When CountryName is proper, it should insert (add) the country to the list of existing countries.
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse countryResponse = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(countryResponse.CountryID != Guid.Empty);
            Assert.Equal(request.CountryName, countryResponse.CountryName);
            Assert.Contains(countryResponse, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries
        [Fact]
        //The list of countries should be empty by default (before adding any countries)
        public void GetAllCountries_EmptyList()
        {
            //Acts
           List<CountryResponse> actualCountryResponesList =  _countriesService.GetAllCountries();

            //Assert
            Assert.Empty(actualCountryResponesList);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
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
                addedCountries.Add(_countriesService.AddCountry(country));
            }
            List<CountryResponse> actualCountryResponesList = _countriesService.GetAllCountries();

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
        public void GetCoutryByCountryID_NullCountryID()
        {
            //Arrange
            Guid? countryID = null;

            //A
            CountryResponse? countryResponse = _countriesService.GetCountryByCountryId(countryID);

            //A
            Assert.Null(countryResponse);
        }

        [Fact]
        //If we supply valid CountryID, it should returns matching country as CountryRespone object
        public void GetCoutryByCountryID_ValidCountryID()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest { CountryName = "india" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            //A
            CountryResponse? countryResponseByCountryId = _countriesService.GetCountryByCountryId(countryResponse.CountryID);

            //A
            Assert.NotNull(countryResponseByCountryId);
            Assert.Equal(countryAddRequest.CountryName, countryResponseByCountryId.CountryName);
        }
        #endregion

    }
}
