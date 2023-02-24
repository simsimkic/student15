using Model.Schedule;
using Model.Users;
using Service.ScheduleService;
using SimsProjekat.Repository.ScheduleRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Controller.ScheduleController
{
    public class AvailableAppointmentController
    {
        public AvailableAppointmentController(AvailableAppointmentService appointmentService)
        {
            this.availableAppointmentService = appointmentService;
        }

        public Appointment RecommendAppointment(PriorityParameters parameters) => availableAppointmentService.RecommendAppointment(parameters);
        public Dictionary<int, Appointment> GetAvailableForDay(DateTime date, TypeOfAppointment type, bool ifUrgent) => availableAppointmentService.GetAvailableForDay(date, type, ifUrgent);
        public Dictionary<int, Appointment> GetAvailableForDayAndDoctor(DateTime date, Doctor doctor, TypeOfAppointment type, bool ifUrgent) => availableAppointmentService.GetAvailableForDayAndDoctor(date, doctor, type, ifUrgent);


        public AvailableAppointmentService availableAppointmentService;
    }

}
