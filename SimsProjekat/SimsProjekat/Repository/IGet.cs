// File:    IGet.cs
// Created: Friday, May 29, 2020 2:29:17 AM
// Purpose: Definition of Interface IGet

using Repository.ReportRepository;
using System;

namespace Repository
{
   public interface IGet<T,ID>
   {
      T GetObject(ID id);
   
   }
}