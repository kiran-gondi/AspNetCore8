using Entities;

namespace RepositoryContracts
{
    /// <summary>
    /// Represents data access logic for mappng Person entity
    /// </summary>
    public interface ICountriesRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        Task<Country> AddCountry(Country country);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Country>> GetAllCountries();

        Task<Country?> GetCountryByCountryID(Guid countryID);
        Task<Country?> GetCountryByCountryName(string countryName);
    }
}
