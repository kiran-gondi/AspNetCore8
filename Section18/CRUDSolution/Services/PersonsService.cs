using CsvHelper;
using CsvHelper.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Globalization;

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

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
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
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
            //_dbContext.sp_InsertPerson(person);

            //Convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //return _persons.Select(x=>x.ToPersonResponse()).ToList();

            /*var persons = _dbContext.Persons.ToList();
            var persons = _dbContext.Persons.Include("Country").ToList();*/
            var persons = await _dbContext.Persons.Include("Country").ToListAsync();
            //return persons.Select(x=>ConvertPersonToPersonReponse(x)).ToList();
            return persons.Select(x=> x.ToPersonResponse()).ToList();
            //return _dbContext.sp_GetAllPersons().Select(x=>ConvertPersonToPersonReponse(x)).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;

            Person? person = await _dbContext.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonID == personID);

            if (person == null) return null;

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
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

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _dbContext.Persons.FirstOrDefaultAsync(t => t.PersonID == personUpdateRequest.PersonID);
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
            
            await _dbContext.SaveChangesAsync(); //UPDATE

            return matchingPerson.ToPersonResponse();
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = _dbContext.Persons.FirstOrDefault(x => x.PersonID == personID);

            if (person == null) return false;

            //IQueryable<Person> personsToRemove = _dbContext.Persons.Where(temp => temp.PersonID == personID);
            //_dbContext.Persons.RemoveRange(personsToRemove);
            _dbContext.Persons.Remove(_dbContext.Persons.First(temp => temp.PersonID == personID));
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.CurrentCulture);

            //CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);
            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration);

            //csvWriter.WriteHeader<PersonResponse>();//PersonID,PersonName,...
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            //csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.ReceiveNewsLetters));

            csvWriter.NextRecord();

            List<PersonResponse> persons = _dbContext.Persons.Include("Country").Select(x=>x.ToPersonResponse()).ToList();

            foreach (var person in persons)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                    csvWriter.WriteField(person.DateOfBirth.Value.ToString("yyyy-MM-dd"));
                else
                    csvWriter.WriteField("");
                csvWriter.WriteField(person.Age);
                //csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.ReceiveNewsLetters);
                csvWriter.NextRecord();
                csvWriter.Flush();
            }

            //await csvWriter.WriteRecordsAsync(persons);
            //1,abc,........

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
