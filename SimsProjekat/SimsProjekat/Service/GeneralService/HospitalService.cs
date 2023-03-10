// File:    HospitalService.cs
// Created: Monday, May 18, 2020 9:25:10 PM
// Purpose: Definition of Class HospitalService

using Model.Users;
using Repository.GeneralRepository;
using System;
using System.Collections.Generic;

namespace Service.GeneralService
{
   public class HospitalService
   {
        public HospitalService(IHospitalRepository hospitalRepository)
        {
            this.hospitalRepository = hospitalRepository;
        }

        public Hospital GetHospital(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Hospital> GetAllHospitals()
        {
             throw new NotImplementedException();
        }
      
        public IHospitalRepository hospitalRepository;
   
   }
}