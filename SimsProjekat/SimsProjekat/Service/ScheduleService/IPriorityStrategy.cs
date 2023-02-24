// File:    IPriorityStrategy.cs
// Created: Tuesday, June 02, 2020 1:09:09 AM
// Purpose: Definition of Interface IPriorityStrategy

using System;

namespace Service.ScheduleService
{
   public interface IPriorityStrategy
   {
      Model.Schedule.Appointment Recommend(PriorityParameters parameters);
   
   }
}