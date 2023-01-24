using DAL.Models;
using System.Threading.Tasks;

namespace DAL
{
    public interface IGeocodingRepository
    {
        public Task<Coordinates> GetCoordinatesByCityNameAsync(string cityName);
    }
}
