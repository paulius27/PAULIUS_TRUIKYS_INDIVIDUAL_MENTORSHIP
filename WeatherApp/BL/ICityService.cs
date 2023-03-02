using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface ICityService
    {
        Task<City> FindCity(string cityName);

        Task<IEnumerable<City>> FindOrAddCities(params string[] cityNames);
    }
}
