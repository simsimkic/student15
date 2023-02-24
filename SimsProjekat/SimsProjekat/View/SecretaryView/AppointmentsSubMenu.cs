using Controller.MedicalRecordController;
using Controller.RoomController;
using Controller.ScheduleController;
using Controller.UserController;
using Model.Rooms;
using Model.Schedule;
using Model.Users;
using SimsProjekat.SIMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.SecretaryView
{
    public class AppointmentsSubMenu
    {

        SecretaryView secretaryView;

        public AppointmentsSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Prikazite zakazane termine za datum");
                Console.WriteLine("\t2. Prikazite slobodne termine za datum");
                Console.WriteLine("\t3. Zakazi termin");
                Console.WriteLine("\t4. Otkazi termin");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    Console.WriteLine("Unesite datum (dd.MM.yyyy.):");
                    string dateString = Console.ReadLine();
                    DateTime date;
                    try
                    {
                        date = DateTime.ParseExact(dateString, "dd.MM.yyyy.", null);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Nepravilan unos!");
                        continue;
                    }
                    ShowScheduledAppointmentsForDate(date);
                }
                else if (selectedOption.Equals("2"))
                {
                    Console.WriteLine("Unesite datum (dd.MM.yyyy.):");
                    string dateString = Console.ReadLine();
                    DateTime date;
                    try
                    {
                        date = DateTime.ParseExact(dateString, "dd.MM.yyyy.", null);
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Nepravilan unos!");
                        continue;
                    }
                    ShowAvailableAppointmentsForDate(date);
                }
                else if (selectedOption.Equals("3"))
                {
                    ScheduleAppointment();
                }
                else if (selectedOption.Equals("4"))
                {
                    CancelAppointment();
                }
                else if (selectedOption.Equals("0"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Nepravilan unos!");
                }
            }
        }

        private void ShowAvailableAppointmentsForDate(DateTime date)
        {
            var appointments = secretaryView.availableAppointmentController.GetAvailableForDay(date, TypeOfAppointment.examination, true);

            foreach (Appointment appointment in appointments.Values)
            {
                Console.WriteLine("{0} {1} {2} {3}", appointment.Doctor.Surname, appointment.Room.RoomNumber, appointment.StartTime.ToString("HH:mm"), appointment.EndTime.ToString("HH:mm"));
            }
        }

        private void ShowScheduledAppointmentsForDate(DateTime date)
        {
            var appointments = secretaryView.appointmentController.GetScheduledByDay(date);

            Console.WriteLine("Id\t  Pacijent\t  \tLekar\t\t  Sala\t Pocetak termina\t Kraj termina\t  Tip\t  Hitan");
            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine("{0}\t  {1} {2}\t{3} {4}\t  {5}\t  {6}\t\t\t{7}\t  {8}\t  {9}", appointment.IdAppointment, appointment.MedicalRecord.Patient.Name, appointment.MedicalRecord.Patient.Surname,
                                                                                                 appointment.Doctor.Name, appointment.Doctor.Surname, appointment.Room.RoomNumber,
                                                                                                 appointment.StartTime.ToString("HH:mm"), appointment.EndTime.ToString("HH:mm"),
                                                                                                 appointment.TypeOfAppointment == TypeOfAppointment.examination ? "Pregled" : "Operacija",
                                                                                                 appointment.Urgent ? "Da" : "Ne");
            }
        }

        private void ScheduleAppointment()
        {
            Console.WriteLine("Unesite korisnicko ime pacijenta:");

            string patientUsername = Console.ReadLine();
            var patient = secretaryView.userController.GetUser(patientUsername);
            var medicalRecord = secretaryView.medicalRecordController.GetRecordByPatient((Patient)patient);

            Console.WriteLine("Unesite korisnicko ime lekara:");

            string doctorUsername = Console.ReadLine();
            var doctor = secretaryView.userController.GetUser(doctorUsername);

            Console.WriteLine("Tip termina: 1. Pregled  2. Operacija");

            string typeOfAppointmentString = Console.ReadLine();
            TypeOfAppointment typeOfAppointment;
            if (typeOfAppointmentString.Equals("1"))
            {
                typeOfAppointment = TypeOfAppointment.examination;
            }
            else if (typeOfAppointmentString.Equals("2"))
            {
                typeOfAppointment = TypeOfAppointment.surgery;
            }
            else
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("Unesite id sale:");

            string roomIdString = Console.ReadLine();
            int roomId;
            Room room;
            if (Int32.TryParse(roomIdString, out roomId))
            {
                room = secretaryView.roomController.GetRoom(roomId);
            }
            else
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("Datum (dd.MM.yyyy.):");

            string dateString = Console.ReadLine();
            DateTime date;
            try
            {
                date = DateTime.ParseExact(dateString, "dd.MM.yyyy.", null);
            }
            catch (FormatException)
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("Pocetak termina:");

            string startTimeString = Console.ReadLine();
            DateTime startTime;
            try
            {
                startTime = DateTime.ParseExact(startTimeString, "HH:mm", null);
            }
            catch (FormatException)
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("Kraj termina:");

            string endTimeString = Console.ReadLine();
            DateTime endTime;
            try
            {
                endTime = DateTime.ParseExact(endTimeString, "HH:mm", null);
            }
            catch (FormatException)
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("Hitan: 1. DA  2. NE");

            string urgentString = Console.ReadLine();
            bool urgent;
            if (urgentString.Equals("1"))
            {
                urgent = true;
            }
            else if (urgentString.Equals("2"))
            {
                urgent = false;
            }
            else
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            try
            {
                secretaryView.appointmentController.AddAppointment(new Appointment(new DateTime(date.Year, date.Month, date.Day, startTime.Hour, startTime.Minute, startTime.Second),
                                                                 new DateTime(date.Year, date.Month, date.Day, endTime.Hour, endTime.Minute, endTime.Second),
                                                                 typeOfAppointment, null, urgent, false, room, medicalRecord, (Doctor)doctor), urgent);
                Console.WriteLine("\nTermin je uspesno zakazan!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("\nGreska prilikom zakazivanja termina!");
            }


        }

        private void CancelAppointment()
        {
            Console.WriteLine("\nUnesite datum termina koji zelite da otkazete:");

            string dateString = Console.ReadLine();
            DateTime date;
            try
            {
                date = DateTime.ParseExact(dateString, "dd.MM.yyyy.", null);
            }
            catch (FormatException)
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            ShowScheduledAppointmentsForDate(date);

            Console.WriteLine("Unesite id termina koji zelite da otkazete:");
            string appointmentIdString = Console.ReadLine();
            int appointmentId;

            if (Int32.TryParse(appointmentIdString, out appointmentId))
            {
                try
                {
                    Appointment appointment = secretaryView.appointmentController.GetAppointment(appointmentId);
                    secretaryView.appointmentController.DeleteAppointment(appointment);
                    secretaryView.notificationController.DeletedAppointment(appointment);

                    Console.WriteLine("Termin je uspesno otkazan!");
                }
                catch (EntityNotFound)
                {
                    Console.WriteLine("Ne postoji zakazani termin sa unetim id-em!");
                }
            }
            else
            {
                Console.WriteLine("\nNepravilan unos!");
            }

        }

    }
}
