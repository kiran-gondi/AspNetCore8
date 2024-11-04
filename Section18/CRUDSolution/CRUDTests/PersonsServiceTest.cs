using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testoutputHelper;
        private readonly PersonsDbContext _dbContext;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _countriesService = new CountriesService(new PersonsDbContext(
                new DbContextOptionsBuilder<PersonsDbContext>().Options));

            _dbContext = new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options);

            _personService = new PersonsService(_dbContext, _countriesService);

            _testoutputHelper = testOutputHelper;
        }

        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>( async () =>
                {
                    await _personService.AddPerson(personAddRequest);
                });
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = null };

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public async Task AddPerson_ProperPerson()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = "Rob", Email = "rob@one.com", 
                Address="sample address",
            CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01")};

            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);
            List<PersonResponse> personList = await _personService.GetAllPersons();

            Assert.True(personResponse.PersonID != Guid.Empty);
            Assert.Contains(personResponse, personList);
        }

        #endregion

        #region GetPersonByPersonID
        [Fact]
        public async Task GetPersonByPersonID_NullPersonID()
        {
            Guid? personId = null;

            PersonResponse? personResponse = await _personService.GetPersonByPersonID(personId);

            Assert.Null(personResponse);
        }

        [Fact]
        public async Task GetPersonByPersonID_ValidPersonID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "US"};
            CountryResponse countryResponse = await _countriesService.AddCountry(countryAddRequest);

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

            PersonResponse personRespone = await _personService.AddPerson(personAddRequest);

            PersonResponse? personResultFromGet = await _personService.GetPersonByPersonID(personRespone.PersonID);

            Assert.Equal(personRespone, personResultFromGet);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            List<PersonResponse> personsFromGet = await _personService.GetAllPersons();

            Assert.Empty(personsFromGet);
        }

        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_req_2);

            PersonAddRequest personRequest_1 = new PersonAddRequest {
                PersonName = "Rob",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_2 = new PersonAddRequest
            {
                PersonName = "Rob2",
                Email = "rob2@one.com",
                Address = "sample address2",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-03"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_3 = new PersonAddRequest
            {
                PersonName = "Rob3",
                Email = "rob3@one.com",
                Address = "sample address3",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Other,
                DateOfBirth = DateTime.Parse("2000-01-04"),
                ReceiveNewsLetters = false
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest> { personRequest_1, 
                personRequest_2, personRequest_3 };
            List<PersonResponse> personResponseFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in personRequests)
            {
               PersonResponse personResponse = await _personService.AddPerson(person_request);
               personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromGet = await _personService.GetAllPersons();

            //print personsListFromGet
            _testoutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personsListFromGetItem in personsListFromGet)
            {
                _testoutputHelper.WriteLine(personsListFromGetItem.ToString());
            }

            //Assert
            foreach (var personFromAdd in personResponseFromAdd)
            {
                Assert.Contains(personFromAdd, personsListFromGet);
            }
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_req_2);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Rob",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_2 = new PersonAddRequest
            {
                PersonName = "Rob2",
                Email = "rob2@one.com",
                Address = "sample address2",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-03"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_3 = new PersonAddRequest
            {
                PersonName = "Rob3",
                Email = "rob3@one.com",
                Address = "sample address3",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Other,
                DateOfBirth = DateTime.Parse("2000-01-04"),
                ReceiveNewsLetters = false
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest> { personRequest_1,
                personRequest_2, personRequest_3 };
            List<PersonResponse> personResponseFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //print personsListFromGet
            _testoutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personsListFromGetItem in personsListFromSearch)
            {
                _testoutputHelper.WriteLine(personsListFromGetItem.ToString());
            }

            //Assert
            foreach (var personFromAdd in personResponseFromAdd)
            {
                Assert.Contains(personFromAdd, personsListFromSearch);
            }
        }

        [Fact]
        public async Task GetFilteredPersons_SearchByPersonNameText()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_req_2);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Rob",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_2 = new PersonAddRequest
            {
                PersonName = "Rob2",
                Email = "rob2@one.com",
                Address = "sample address2",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-03"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_3 = new PersonAddRequest
            {
                PersonName = "Rob3",
                Email = "rob3@one.com",
                Address = "sample address3",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Other,
                DateOfBirth = DateTime.Parse("2000-01-04"),
                ReceiveNewsLetters = false
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest> { personRequest_1,
                personRequest_2, personRequest_3 };
            List<PersonResponse> personResponseFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSearch = await _personService.GetFilteredPersons(nameof(Person.PersonName), "Ro");

            //print personsListFromGet
            _testoutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personsListFromGetItem in personsListFromSearch)
            {
                _testoutputHelper.WriteLine(personsListFromGetItem.ToString());
            }

            //Assert
            foreach (var personFromAdd in personResponseFromAdd)
            {
                if(personFromAdd.PersonName != null) { 
                    if(personFromAdd.PersonName.Contains("Ro", StringComparison.OrdinalIgnoreCase)) { 
                        Assert.Contains(personFromAdd, personsListFromSearch);
                    }
                }
            }
        }

        #endregion

        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPersons_Desc()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = await _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = await _countriesService.AddCountry(country_req_2);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Rob",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponse1.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_2 = new PersonAddRequest
            {
                PersonName = "Rob2",
                Email = "rob2@one.com",
                Address = "sample address2",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Female,
                DateOfBirth = DateTime.Parse("2000-01-03"),
                ReceiveNewsLetters = false
            };

            PersonAddRequest personRequest_3 = new PersonAddRequest
            {
                PersonName = "Rob3",
                Email = "rob3@one.com",
                Address = "sample address3",
                CountryID = countryResponse2.CountryID,
                Gender = GenderOptions.Other,
                DateOfBirth = DateTime.Parse("2000-01-04"),
                ReceiveNewsLetters = false
            };

            List<PersonAddRequest> personRequests = new List<PersonAddRequest> { personRequest_1,
                personRequest_2, personRequest_3 };
            List<PersonResponse> personResponseFromAdd = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in personRequests)
            {
                PersonResponse personResponse = await _personService.AddPerson(person_request);
                personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            //Act
            List<PersonResponse> personsListFromSort = await _personService.GetSortedPersons(allPersons,
                nameof(Person.PersonName), SortOrderOptions.DESC);

            //print personsListFromGet
            _testoutputHelper.WriteLine("Actual: ");
            foreach (PersonResponse personsListFromGetItem in personsListFromSort)
            {
                _testoutputHelper.WriteLine(personsListFromGetItem.ToString());
            }

            personResponseFromAdd = personResponseFromAdd.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for (int i = 0; i < personResponseFromAdd.Count; i++)
            {
                Assert.Equal(personResponseFromAdd[i], personsListFromSort[i]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest { PersonID = Guid.NewGuid()};

            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "UK" };
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Jon",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponseFromAdd.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = false
            };
            PersonResponse personResponseAdded = await _personService.AddPerson(personRequest_1);

            PersonUpdateRequest? personUpdateRequest = personResponseAdded.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public async Task UpdatePerson_Proper()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "UK" };
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Jon",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponseFromAdd.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponseAdded = await _personService.AddPerson(personRequest_1);

            PersonUpdateRequest? personUpdateRequest = personResponseAdded.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "Jimmy";
            personUpdateRequest.Email = "jim@one.bsil.com";

            PersonResponse personResponseForUpdate = await _personService.UpdatePerson(personUpdateRequest);

            PersonResponse personResponseFromGet = await _personService.GetPersonByPersonID(personResponseForUpdate.PersonID);

            Assert.Equal(personResponseFromGet, personResponseForUpdate);
        }

        #endregion

        #region DeletePerson
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest { CountryName = "UK" };
            CountryResponse countryResponseFromAdd = await _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personRequest_1 = new PersonAddRequest
            {
                PersonName = "Jon",
                Email = "rob@one.com",
                Address = "sample address",
                CountryID = countryResponseFromAdd.CountryID,
                Gender = GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-11-01"),
                ReceiveNewsLetters = true
            };
            PersonResponse personResponseAdded = await _personService.AddPerson(personRequest_1);

            bool isDeleted = await _personService.DeletePerson(personResponseAdded.PersonID);

            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeletePerson_InValidPersonID()
        {
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            Assert.False(isDeleted);
        }
        #endregion
    }
}
