using DAL.Models;
using System.Threading.Tasks;

namespace DAL
{
    public interface IGeocodingRepository
    {
        Task<Coordinates> GetCoordinatesByCityNameAsync(string cityName);
    }
}
