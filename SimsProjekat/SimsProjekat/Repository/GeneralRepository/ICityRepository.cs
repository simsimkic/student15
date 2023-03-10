// File:    ICityRepository.cs
// Created: Sunday, May 24, 2020 12:26:25 AM
// Purpose: Definition of Interface ICityRepository

using Model.Users;
using System;
using System.Collections.Generic;

namespace Repository.GeneralRepository
{
   public interface ICityRepository : ICreate<City>, IGetAll<City>, IGet<City, int>
   {
      IEnumerable<City> GetAllCitiesByState(Model.Users.State state);
      bool CheckIfExists(City city);
        City GetCityByName(City city);
    }
}