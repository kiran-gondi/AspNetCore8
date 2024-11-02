using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if (initialize) 
            {
                _countries.AddRange(new List<Country>()
                {
                    new Country() { CountryID = Guid.Parse("387F0A4B-2C38-4ACD-AE8F-16917AD94B10"), CountryName = "USA" },
                    new Country() { CountryID = Guid.Parse("25614871-FCBD-47F8-917F-A7302BDE4AC2"), CountryName = "Canada" },
                    new Country() { CountryID = Guid.Parse("38FC6B05-95DE-491E-B045-F6D7ACB67EBD"), CountryName = "UK" },
                    new Country() { CountryID = Guid.Parse("95B3D99D-1876-4C88-AFFA-3E6BCB328BA9"), CountryName = "India" },
                    new Country() { CountryID = Guid.Parse("C3344753-92F8-40B1-BD0E-1CC0C502435A"), CountryName = "Australia" }
                }); 
            }
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
            if(_countries.Where(x=>x.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into countries.
            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? CountryID)
        {
            if (CountryID == null) { return null; }

            Country? countryResponse = _countries.FirstOrDefault(temp => temp.CountryID == CountryID);

            if (countryResponse == null)
            {
                return null;
            }
            return countryResponse.ToCountryResponse();
        }
    }
}
