using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
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

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
            if(await _dbContext.Countries.CountAsync(x=>x.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            //Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            //generate CountryID
            country.CountryID = Guid.NewGuid();

            //Add country object into countries.
            _dbContext.Countries.Add(country);
            await _dbContext.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _dbContext.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? CountryID)
        {
            if (CountryID == null) { return null; }

            Country? countryResponse = await _dbContext.Countries.FirstOrDefaultAsync(temp => temp.CountryID == CountryID);

            if (countryResponse == null)
            {
                return null;
            }
            return countryResponse.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            int countriesInserted = 0;

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = workSheet.Dimension.Rows;
                

                for (int row = 2; row <= rowCount; row++) {
                     string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value);
                    if (!string.IsNullOrEmpty(cellValue)) {
                        string? countryName = cellValue;

                        if(_dbContext.Countries.Where(t => t.CountryName == countryName).Count() == 0)
                        {
                            Country country = new Country() {
                                CountryName = countryName
                            };
                            
                            _dbContext.Countries.Add(country);
                            await _dbContext.SaveChangesAsync();
                            countriesInserted++;
                        }
                    }
                    
                }
            }
            return countriesInserted;
        }
    }
}
