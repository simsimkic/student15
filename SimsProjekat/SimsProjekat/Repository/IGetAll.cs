// File:    IGetAll.cs
// Created: Friday, May 29, 2020 2:29:18 AM
// Purpose: Definition of Interface IGetAll

using SimsProjekat.Repository;
using System;
using System.Collections.Generic;

namespace Repository
{
   public interface IGetAll<T>
   {
      IEnumerable<T> GetAll();

    }
}