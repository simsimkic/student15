/***********************************************************************
 * Module:  SymptomsRepository.cs
 * Purpose: Definition of the Class Repository.SymptomsRepository
 ***********************************************************************/

using Model.MedicalRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using SimsProjekat.SIMS.Exceptions;

namespace Repository.MedicationRepository
{
   public class SymptomsRepository : ISymptomsRepository
   {
        public Stream<Symptoms> stream;

        private const string ALREADY_EXISTS = "Symptom already exists!";

        public SymptomsRepository(Stream<Symptoms> stream)
        {
            this.stream = stream;
        }

        public Symptoms Create(Symptoms entity)
        {
            if (!ExistsInSystem(entity))
            {
                var allSymptoms = stream.GetAll().ToList();
                allSymptoms.Add(entity);
                stream.SaveAll(allSymptoms);
                return entity;
            } else
            {
                throw new EntityAlreadyExists(ALREADY_EXISTS);
            }
        }

        public IEnumerable<Symptoms> GetAll() => stream.GetAll();
        
        public bool ExistsInSystem(Symptoms symptoms)
        {
            var allSymptoms = stream.GetAll().ToList();
            return allSymptoms.Any(item => item.Name == symptoms.Name);
        }
    }
}