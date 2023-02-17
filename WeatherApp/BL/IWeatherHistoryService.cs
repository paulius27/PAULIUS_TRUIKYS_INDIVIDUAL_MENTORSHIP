using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public  interface IWeatherHistoryService
    {
        Task UpdateWeatherHistory(IEnumerable<string> cityNames);
    }
}