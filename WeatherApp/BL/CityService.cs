using BL.Validation;
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
        private readonly IValidator<string> _cityNameValidator;

        public CityService(ICityRepository cityRepository, IValidator<string> cityNameValidator)
        {
            _cityRepository = cityRepository;
            _cityNameValidator = cityNameValidator;
        }

        public async Task<City> FindCity(string cityName)
        {
            if (!_cityNameValidator.Validate(cityName))
                throw new ArgumentException("city name is not valid", nameof(cityName));

            return await _cityRepository.FindByName(cityName);
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
