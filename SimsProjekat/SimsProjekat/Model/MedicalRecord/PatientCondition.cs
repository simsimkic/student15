/***********************************************************************
 * Module:  PatientCondition.cs
 * Purpose: Definition of the Class HealthCorporation.MutualClasses.PatientCondition
 ***********************************************************************/

using Model.Users;
using System;

namespace Model.MedicalRecord
{
   public enum PatientCondition
   {
      stable,
      urgent,
      hospitalTreatment,
      homeTreatment
   }
}