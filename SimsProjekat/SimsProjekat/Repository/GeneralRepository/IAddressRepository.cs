// File:    IAddressRepository.cs
// Created: Friday, May 29, 2020 4:15:45 AM
// Purpose: Definition of Interface IAddressRepository

using Model.Users;
using System;
using System.Collections.Generic;

namespace Repository.GeneralRepository
{
   public interface IAddressRepository : ICreate<Address>, IGetAll<Address>, IGet<Address, int>
   {
      IEnumerable<Address> GetAdressesByCity(Model.Users.City city);
      bool CheckIfExists(Address address);
        Address GetExistentAddress(Address address);
    }
}