using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;

        public CityService(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        public async Task<IEnumerable<City>> FindOrAddCities(params string[] cityNames)
        {
            var cities = await _cityRepository.FindByNames(cityNames);

            if (cities.Count() != cityNames.Length)
            {
                var foundCityNames = cities.Select(city => city.Name);
                var notFoundCities = cityNames
                    .Where(cityName => !foundCityNames.Contains(cityName))
                    .Select(cityName => new City { Name = cityName })
                    .ToArray();

                await _cityRepository.InsertMany(notFoundCities);

                cities = await _cityRepository.FindByNames(cityNames);
            }

            return cities;
        }
    }
}
