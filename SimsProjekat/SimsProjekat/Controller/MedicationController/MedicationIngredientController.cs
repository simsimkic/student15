// File:    MedicationIngredientController.cs
// Created: Wednesday, May 20, 2020 2:45:06 AM
// Purpose: Definition of Class MedicationIngredientController

using Model.Medications;
using System;
using System.Collections.Generic;
using Service.MedicationService;

namespace Controller.MedicationController
{
   public class MedicationIngredientController
   {
        public MedicationIngredientController(MedicationIngredientService medicationIngredientService)
        {
            this.medicationIngredientService = medicationIngredientService;
        }

        public MedicationIngredient AddIngredient(MedicationIngredient ingredient) => medicationIngredientService.AddIngredient(ingredient);
        public IEnumerable<MedicationIngredient> GetAllIngredients() => medicationIngredientService.GetAllIngredients();
      
        public MedicationIngredientService medicationIngredientService;
   
   }
}