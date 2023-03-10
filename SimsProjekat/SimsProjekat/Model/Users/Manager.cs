/***********************************************************************
 * Module:  Manager.cs
 * Purpose: Definition of the Class HealthCorporation.Manager.Manager
 ***********************************************************************/

using System;
using ZdravoKorporacija.Model.Users;

namespace Model.Users
{
   public class Manager : RegisteredUser
   {
        public Manager() { }
        public Manager(string id) { }
        public Manager(string name, string surname, DateTime dateOfBirth,
            string identificationNumber, string email, string username, string phone,
            string password, EducationLevel educationLevel, Gender gender,
            string profession, City city, Address currResidence, InsurancePolicy insurancePolicy)
            : base(name, surname, dateOfBirth, identificationNumber, email, username, phone, password,
                 educationLevel, gender, profession, city, currResidence, insurancePolicy)
        { }
   }
}