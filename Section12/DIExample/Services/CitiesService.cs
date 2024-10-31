using ServiceContracts;

namespace Services
{
    public class CitiesService : ICitiesService
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
    }
}
