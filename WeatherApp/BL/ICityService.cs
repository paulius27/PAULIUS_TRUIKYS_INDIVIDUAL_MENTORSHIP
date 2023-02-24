using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface ICityService
    {
        Task<IEnumerable<City>> FindOrAddCities(params string[] cityNames);
    }
}
