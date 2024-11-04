using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonService
    {
        private readonly PersonsDbContext _dbContext;
        private readonly ICountriesService _countriesService;

        public PersonsService(PersonsDbContext personsDbContext, ICountriesService countriesService)
        {
            _dbContext = personsDbContext;
            _countriesService = countriesService;
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
            //_dbContext.Persons.Add(person);
            //_dbContext.SaveChanges();
            _dbContext.sp_InsertPerson(person);

            //Convert the Person object into PersonResponse type
            return ConvertPersonToPersonReponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            //return _persons.Select(x=>x.ToPersonResponse()).ToList();
            //return _dbContext.Persons.ToList().Select(x=>ConvertPersonToPersonReponse(x)).ToList();
            return _dbContext.sp_GetAllPersons().Select(x=>ConvertPersonToPersonReponse(x)).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = _dbContext.Persons.FirstOrDefault(temp => temp.PersonID == personID);

            if(person == null) return null;

            //return person.ToPersonResponse();
            return ConvertPersonToPersonReponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) && string.IsNullOrEmpty(searchString)) return matchingPersons;

            switch(searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.PersonName) ? 
                    t.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Email) ?
                    t.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(t => (t.DateOfBirth != null) ?
                    t.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Gender) ?
                    t.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.CountryID):
                    matchingPersons = allPersons.Where(t => (!string.IsNullOrEmpty(t.Country) ?
                    t.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
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
            Person? matchingPerson = _dbContext.Persons.FirstOrDefault(t => t.PersonID == personUpdateRequest.PersonID);
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
            matchingPerson.TIN = personUpdateRequest.Tin;
            
            _dbContext.SaveChanges(); //UPDATE

            return ConvertPersonToPersonReponse(matchingPerson);
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = _dbContext.Persons.FirstOrDefault(x => x.PersonID == personID);

            if (person == null) return false;

            //IQueryable<Person> personsToRemove = _dbContext.Persons.Where(temp => temp.PersonID == personID);
            //_dbContext.Persons.RemoveRange(personsToRemove);
            _dbContext.Persons.Remove(_dbContext.Persons.First(temp => temp.PersonID == personID));
            _dbContext.SaveChanges();

            return true;
        }
        
    }
}
