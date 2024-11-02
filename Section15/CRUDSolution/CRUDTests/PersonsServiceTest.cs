using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        public PersonsServiceTest()
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
        }

        #region AddPerson
        [Fact]
        public void AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            Assert.Throws<ArgumentNullException>(() =>
                {
                    _personService.AddPerson(personAddRequest);
                });
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = null };

            Assert.Throws<ArgumentException>(() =>
            {
                _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPerson()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = "Rob", Email = "rob@one.com", 
                Address="sample address",
            CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01")};

            PersonResponse personResponse = _personService.AddPerson(personAddRequest);
            List<PersonResponse> personList = _personService.GetAllPersons();

            Assert.True(personResponse.PersonID != Guid.Empty);
            Assert.Contains(personResponse, personList);
        }

        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personId = null;

            PersonResponse? personResponse = _personService.GetPersonByPersonID(personId);

            Assert.Null(personResponse);
        }

        [Fact]
        public void GetPersonByPersonID_ValidPersonID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "US"};
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "Rob",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponse.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonResponse personRespone = _personService.AddPerson(personAddRequest);

            PersonResponse? personResultFromGet = _personService.GetPersonByPersonID(personRespone.PersonID);

            Assert.Equal(personRespone, personResultFromGet);
        }
        #endregion
    }
}
