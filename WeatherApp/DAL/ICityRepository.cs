using DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface ICityRepository
    {
        Task<City> FindByName(string cityName);

        Task<IEnumerable<City>> FindByNames(params string[] cityNames);

        Task InsertMany(params City[] cities);
    }
}
