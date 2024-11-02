﻿using Entities;
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

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testoutputHelper = testOutputHelper;
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

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> personsFromGet = _personService.GetAllPersons();

            Assert.Empty(personsFromGet);
        }

        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_req_2);

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
               PersonResponse personResponse = _personService.AddPerson(person_request);
               personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromGet = _personService.GetAllPersons();

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
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_req_2);

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
                PersonResponse personResponse = _personService.AddPerson(person_request);
                personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

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
        public void GetFilteredPersons_SearchByPersonNameText()
        {
            CountryAddRequest country_req_1 = new CountryAddRequest { CountryName = "USA" };
            CountryAddRequest country_req_2 = new CountryAddRequest { CountryName = "UK" };

            CountryResponse countryResponse1 = _countriesService.AddCountry(country_req_1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(country_req_2);

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
                PersonResponse personResponse = _personService.AddPerson(person_request);
                personResponseFromAdd.Add(personResponse);
            }

            //print personResponseFromAdd
            _testoutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse personResponseFromAddItem in personResponseFromAdd)
            {
                _testoutputHelper.WriteLine(personResponseFromAddItem.ToString());
            }

            //Act
            List<PersonResponse> personsListFromSearch = _personService.GetFilteredPersons(nameof(Person.PersonName), "Ro");

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
    }
}
