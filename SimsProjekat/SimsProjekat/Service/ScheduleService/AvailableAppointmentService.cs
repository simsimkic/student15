using Model.Schedule;
using Model.Users;
using Repository.ScheduleRepository;
using Service.ScheduleService;
using Service.UserService;
using SimsProjekat.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Repository.ScheduleRepository
{
    public class AvailableAppointmentService
    {
        public AvailableAppointmentService(IAppointmentRepository appointmentRepository, WorkDayService workDayService,
            int allowedPeriodOfTime, int appointmentTimePeriod, int surgeryPeriod, int appointmentHourStart, int appointmentHourEnd)
        {
            this.appointmentHourStart = appointmentHourStart;
            this.appointmentHourEnd = appointmentHourEnd;
            this.appointmentRepository = appointmentRepository;
            this.workDayService = workDayService;
            this.allowedPeriodOfTime = allowedPeriodOfTime;
            this.appointmentTimePeriod = appointmentTimePeriod;
            this.surgeryPeriod = surgeryPeriod;
        }


        public Dictionary<int, Appointment> GetAvailableForDay(DateTime date, TypeOfAppointment type, bool ifUrgent)
        {
            Dictionary<int, Appointment> availableAppointments = new Dictionary<int, Appointment>();
            if (date.Date.CompareTo(DateTime.Today.Date.AddHours(allowedPeriodOfTime)) <= 0 && !ifUrgent)
                return availableAppointments;
            var allScheduledForDay = appointmentRepository.GetAppointmentsByDate(date.Date);
            var allWorkingDoctorsForDay = workDayService.GetWorkingDoctorsForDay(date.Date);
            foreach (WorkDay workDay in allWorkingDoctorsForDay)
            {
                FindFreeAppointments(type, allScheduledForDay, availableAppointments, workDay);
            }
            CheckIfRoomsAreAvailable(availableAppointments, date);
            return availableAppointments;
        }

        public Dictionary<int, Appointment> GetAvailableForDayAndDoctor(DateTime date, Doctor doctor, TypeOfAppointment type, bool ifUrgent)
        {
            var allAvailableByDate = GetAvailableForDay(date, type, ifUrgent);
            Dictionary<int, Appointment> availableAppointments = new Dictionary<int, Appointment>();
            foreach (Appointment appointment in allAvailableByDate.Values)
            {
                if (appointment.Doctor.Username.Equals(doctor.Username))
                    availableAppointments.Add(appointment.GetHashCode(), appointment);

            }
            return availableAppointments;
        }


        private void CheckIfRoomsAreAvailable(Dictionary<int, Appointment> availableAppointments, DateTime date)
        {
            var allScheduledForDay = appointmentRepository.GetAppointmentsByDate(date.Date);
            foreach (Appointment appointment in allScheduledForDay.Values)
            {
                foreach (Appointment availableAppointment in availableAppointments.Values)
                {
                    if (appointment.StartTime.CompareTo(availableAppointment.StartTime) >= 0 && appointment.StartTime.CompareTo(availableAppointment.EndTime) < 0
                        && appointment.Room.RoomID == availableAppointment.Room.RoomID)
                    {
                        availableAppointments.Remove(availableAppointment.GetHashCode());
                    }
                }
            }
        }

        private void FindFreeAppointments(TypeOfAppointment type, Dictionary<int, Appointment> allScheduled,
            Dictionary<int, Appointment> availableAppointments, WorkDay workDay)
        {
            int ifSurgeryMultiply = type == TypeOfAppointment.surgery ? (int)Math.Ceiling((double)surgeryPeriod / (double)appointmentTimePeriod) : 1;
            DateTime startTime = new DateTime(workDay.Date.Year, workDay.Date.Month, workDay.Date.Day, workDay.Shift.StartHour, 0, 0);
            DateTime endTime = new DateTime(workDay.Date.Year, workDay.Date.Month, workDay.Date.Day, workDay.Shift.EndHour, 0, 0);
            while (startTime.CompareTo(endTime) < 0)
            {
                Appointment appointment = new Appointment(startTime, startTime.AddMinutes(appointmentTimePeriod * ifSurgeryMultiply),
                    (Doctor)workDay.Employee, TypeOfAppointment.examination);
                if (!allScheduled.ContainsKey(appointment.GetHashCode()) && !(appointment.EndTime.Hour > workDay.Shift.EndHour))
                {
                    if (type == TypeOfAppointment.surgery)
                        FreeSurgeryAppointments(allScheduled, availableAppointments, appointment);
                    else
                        FreeExaminationAppointments(availableAppointments, appointment);
                }
                startTime = startTime.AddMinutes(appointmentTimePeriod);
            }
        }

        private void FreeExaminationAppointments(Dictionary<int, Appointment> availableAppointments, Appointment appointment)
        {
            appointment.TypeOfAppointment = TypeOfAppointment.examination;
            availableAppointments.Add(appointment.GetHashCode(), appointment);
        }

        private void FreeSurgeryAppointments(Dictionary<int, Appointment> allScheduled, Dictionary<int, Appointment> availableAppointments, Appointment appointment)
        {
            bool somethingBetweenScheduled = false;
            DateTime checkFrom = appointment.StartTime;
            while (checkFrom.CompareTo(appointment.EndTime) < 0)
            {
                Appointment appointmentToCheck = new Appointment(checkFrom, checkFrom.AddMinutes(appointmentTimePeriod), appointment.Doctor, TypeOfAppointment.surgery);
                if (allScheduled.ContainsKey(appointmentToCheck.GetHashCode()))
                    somethingBetweenScheduled = true;
                checkFrom = checkFrom.AddMinutes(appointmentTimePeriod);
            }
            if (!somethingBetweenScheduled)
            {
                appointment.TypeOfAppointment = TypeOfAppointment.surgery;
                availableAppointments.Add(appointment.GetHashCode(), appointment);
            }
        }

        public Appointment RecommendAppointment(PriorityParameters parameters)
        {
            SwitchStrategy(parameters.Priority);
            Appointment toRecommend = strategy.Recommend(parameters);
            if (toRecommend == null)
                throw new CantFindAppointment(CANT_RECOMMEND);
            return toRecommend;
        }

        public void SwitchStrategy(PriorityType priority)
        {
            if (priority == PriorityType.doctor)
                strategy = new DoctorPriorityStrategy(this, appointmentHourStart, appointmentHourEnd);
            else
                strategy = new DatePrirorityStrategy(this, appointmentHourStart, appointmentHourEnd);
        }


        private const string CANT_RECOMMEND = "Can't find any appointment to recommend!";

        public int allowedPeriodOfTime;
        public int appointmentTimePeriod;
        public int appointmentHourStart;
        public int appointmentHourEnd;
        public int surgeryPeriod;


        public IAppointmentRepository appointmentRepository;
        public WorkDayService workDayService;
        public IPriorityStrategy strategy;
    }
}
