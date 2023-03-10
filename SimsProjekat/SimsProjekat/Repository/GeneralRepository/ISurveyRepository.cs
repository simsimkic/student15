// File:    ISurveyRepository.cs
// Created: Sunday, May 24, 2020 12:15:56 AM
// Purpose: Definition of Interface ISurveyRepository

using Model.Users;
using System;

namespace Repository.GeneralRepository
{
   public interface ISurveyRepository : ICreate<Survey>, IGetAll<Survey>
   {
   }
}