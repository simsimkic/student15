// File:    ISpecializationRepository.cs
// Created: Sunday, May 24, 2020 1:08:29 AM
// Purpose: Definition of Interface ISpecializationRepository

using Model.Users;
using System;

namespace Repository.UserRepository
{
   public interface ISpecializationRepository : ICreate<Specialization>, IDelete<Specialization>, IGetAll<Specialization>
   {
        Specialization GetGeneralSpecialization();
   }
}