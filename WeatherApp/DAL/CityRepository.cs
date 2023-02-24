using DAL.Context;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CityRepository : ICityRepository
    {
        private readonly WeatherDbContext _context;

        public CityRepository(WeatherDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<City>> FindByNames(params string[] cityNames)
        {
            var cities = await (from c in _context.Cities
                          where cityNames.Contains(c.Name)
                          select c).ToListAsync();

            return cities;
        }

        public async Task InsertMany(params City[] cities)
        {
            await _context.Cities.AddRangeAsync(cities);
            await _context.SaveChangesAsync();
        }
    }
}
