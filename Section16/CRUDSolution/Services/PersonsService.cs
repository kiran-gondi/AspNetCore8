using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService(bool initialzie = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialzie)
            {
                _persons.Add(new Person
                            {
                                PersonID = Guid.Parse("93189268-3D6B-4096-B61E-8CF4C660C109"),
                                PersonName = "Penny",
                                Email = "pkaliszewski0@mashable.com",
                                DateOfBirth = DateTime.Parse("2006-03-22"),
                                Gender = "Female",
                                Address = "481 Monica Alley",
                                ReceiveNewsLetters = false,
                                CountryID = Guid.Parse("387F0A4B-2C38-4ACD-AE8F-16917AD94B10")
                            });
                _persons.Add(new Person
                        {
                            PersonID = Guid.Parse("EEBAEAE6-1627-4D5D-AB1D-9B602BE62584"),
                            PersonName = "Rourke",
                            Email = "risaacson1@dyndns.org",
                            DateOfBirth = DateTime.Parse("2009-02-01"),
                            Gender = "Male",
                            Address = "5733 Rieder Park",
                            ReceiveNewsLetters = true,
                            CountryID = Guid.Parse("387F0A4B-2C38-4ACD-AE8F-16917AD94B10")
                        });
                _persons.Add(new Person
                {
                    PersonID = Guid.Parse("55C873DF-CD6B-4A2B-94A3-BBD1F1EDB04C"),
                    PersonName = "Danya",
                    Email = "dacuna1@mozilla.com",
                    DateOfBirth = DateTime.Parse("2005-02-02"),
                    Gender = "Female",
                    Address = "6th Way",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("25614871-FCBD-47F8-917F-A7302BDE4AC2")
                });
                _persons.Add(new Person
                {
                    PersonID = Guid.Parse("AAF9FC4C-DB68-448D-9579-35A3259B6662"),
                    PersonName = "Yovonnda",
                    Email = "ydealmeida2@youtube.com",
                    DateOfBirth = DateTime.Parse("1992-04-05"),
                    Gender = "Female",
                    Address = "24 Marcy Street",
                    ReceiveNewsLetters = true,
                    CountryID = Guid.Parse("25614871-FCBD-47F8-917F-A7302BDE4AC2")
                });
                _persons.Add(new Person
                {
                    PersonID = Guid.Parse("F0132384-868E-45D4-A21B-59DEDB031329"),
                    PersonName = "Dannie",
                    Email = "dovery3@facebook.com",
                    DateOfBirth = DateTime.Parse("2016-12-01"),
                    Gender = "Male",
                    Address = "66 Summit Point",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("25614871-FCBD-47F8-917F-A7302BDE4AC2")
                });
                _persons.Add(new Person
                {
                    PersonID = Guid.Parse("6CD15CBF-70FE-48FD-B87F-367B0B8A9065"),
                    PersonName = "Cullie",
                    Email = "ccollelton4@posterous.com",
                    DateOfBirth = DateTime.Parse("1994-04-21"),
                    Gender = "Male",
                    Address = "8724 Mariners Cove Park",
                    ReceiveNewsLetters = false,
                    CountryID = Guid.Parse("38FC6B05-95DE-491E-B045-F6D7ACB67EBD")
                });
            }
        }

        private PersonResponse ConvertPersonToPersonReponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //if (string.IsNullOrEmpty(personAddRequest.PersonName))
            //{
            //    throw new ArgumentException("PersonName can't be blank");
            //}

            //Model Validations
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest to Person type
            Person person = personAddRequest.ToPerson();

            //generate personid
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(person);

            //Convert the Person object into PersonResponse type
            return ConvertPersonToPersonReponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(x=>x.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = _persons.FirstOrDefault(temp => temp.PersonID == personID);

            if(person == null) return null;

            return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString)) return matchingPersons;

            switch(searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.PersonName) ? 
                    t.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Email) ?
                    t.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(t => (t.DateOfBirth != null) ?
                    t.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Gender) ?
                    t.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Country) ?
                    t.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Address) ?
                    t.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.Gender).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.Gender).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.Country).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.Country).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.Address).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.Address).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC)
                    => allPersons.OrderBy(t => t.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC)
                    => allPersons.OrderByDescending(t => t.ReceiveNewsLetters).ToList(),

                    _=>allPersons
            };
            return sortedPersons;

        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(t => t.PersonID == personUpdateRequest.PersonID);
            if(matchingPerson == null)
            {
                throw new ArgumentException("Give person id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = _persons.FirstOrDefault(x => x.PersonID == personID);

            if (person == null) return false;

            _persons.RemoveAll(temp => temp.PersonID == personID);
            return true;
        }
        
    }
}
