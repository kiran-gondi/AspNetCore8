using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly PersonsDbContext _dbContext;

        public CountriesService(PersonsDbContext personsDbContext)
        {
            _dbContext = personsDbContext;
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validaton: countryName can't be null
            if (countryAddRequest.CountryName == null) {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if(_dbContext.Countries.Count(x=>x.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into countries.
            _dbContext.Countries.Add(country);
            _dbContext.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _dbContext.Countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? CountryID)
        {
            if (CountryID == null) { return null; }

            Country? countryResponse = _dbContext.Countries.FirstOrDefault(temp => temp.CountryID == CountryID);

            if (countryResponse == null)
            {
                return null;
            }
            return countryResponse.ToCountryResponse();
        }
    }
}
