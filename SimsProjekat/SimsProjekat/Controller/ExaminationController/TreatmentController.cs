// File:    TreatmentController.cs
// Created: Wednesday, May 20, 2020 3:29:31 AM
// Purpose: Definition of Class TreatmentController

using Model.ExaminationSurgery;
using Service.ExaminationService;
using System;
using System.Collections.Generic;

namespace Controller.ExaminationController
{
   public class TreatmentController
   {
        public TreatmentController(TreatmentService treatmentService)
        {
            this.treatmentService = treatmentService;
        }

        public Treatment CreateTreatment(Treatment treatment) => treatmentService.CreateTreatment(treatment);
        public Treatment UpdateTreatment(Treatment treatment) => treatmentService.UpdateTreatment(treatment);
        public bool DeleteTreatment(Treatment treatment) => treatmentService.DeleteTreatment(treatment);
        public IEnumerable<HospitalTreatment> GetUnapprovedHospitalTreatments() => treatmentService.GetUnapprovedHospitalTreatments();
        public IEnumerable<Prescription> GetAllPrescriptions() => treatmentService.GetAllPrescriptions();
        public IEnumerable<Prescription> GetAllPrescriptionsInPeriodOfTime(DateTime startDate, DateTime endDate) => treatmentService.GetAllPrescriptionsInPeriodOfTime(startDate, endDate);
        public HospitalTreatment ApproveHospitalTreatment(HospitalTreatment hospitalTreatment) => treatmentService.ApproveHospitalTreatment(hospitalTreatment);

        public TreatmentService treatmentService;
   
   }
}