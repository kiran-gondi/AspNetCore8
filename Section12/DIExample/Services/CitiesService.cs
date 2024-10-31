using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService, IDisposable
    {
        private List<string> _cities;

        public CitiesService() {
            _serviceInstanceId = Guid.NewGuid();
            _cities = new List<string>()
            {
                "London",
                "Paris",
                "Newyork",
                "Tokyo",
                "Rome"
            };
            //TODO: Add logic to open the DB Connection
        }
        private Guid _serviceInstanceId;
        public Guid ServiceInstanceId { 
            get
            {
                return _serviceInstanceId;
            }
        }

        public List<string> GetCities()
        {
            return _cities;
        }

        public void Dispose()
        {
            //TODO: add logic to close db connection

        }
    }
}
