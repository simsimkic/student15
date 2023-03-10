// File:    IAllergensRepository.cs
// Created: Friday, May 22, 2020 4:36:48 AM
// Purpose: Definition of Interface IAllergensRepository

using Model.Medications;
using System;

namespace Repository.MedicalRecordRepository
{
   public interface IAllergensRepository : ICreate<Allergens>, IGetAll<Allergens>
   {
   }
}