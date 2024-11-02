using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;

        public PersonsServiceTest()
        {
            _personService = new PersonsService();
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

    }
}
