using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public  interface IWeatherHistoryService
    {
        Task UpdateWeatherHistory(params string[] cityNames);
    }
}