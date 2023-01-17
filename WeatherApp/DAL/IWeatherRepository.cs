using System.Threading.Tasks;

namespace DAL
{
    public interface IWeatherRepository
    {
        public Task<double> GetTemperatureByCityNameAsync(string cityName);
    }
}
