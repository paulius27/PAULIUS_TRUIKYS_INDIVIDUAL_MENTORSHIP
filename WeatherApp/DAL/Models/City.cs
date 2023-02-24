using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Models
{
    public class City
    {
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        public List<WeatherHistoryEntry> WeatherHistory { get; set; }
    }
}
