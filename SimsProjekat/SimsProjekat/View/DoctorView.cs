using Controller.GeneralController;
using Controller.MedicationController;
using Controller.UserController;
using Model.MedicalRecord;
using Model.Medications;
using Model.Schedule;
using Model.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimsProjekat.View
{
    class DoctorView
    {
        public Doctor loggedInDoctor;
        ValidationMedicationController validationMedicationController;
        MedicationController medicationController;
        UserController userController;
        NotificationController notificationController;

        public DoctorView(Doctor doctor) 
        {
            loggedInDoctor = doctor;
            validationMedicationController = Program.app.validationMedicationController;
            medicationController = Program.app.medicationController;
            userController = Program.app.userController;
            notificationController = Program.app.notificationController;


        }

        public void ShowMenu()
        {
            Console.WriteLine("Šta želite da odradite?");
            Console.WriteLine("1. Validacija lekova");
            Console.WriteLine("2. Pregled notifikacija");



            try
            {
                int choice = int.Parse(Console.ReadLine());
                if (choice == 1)
                {
                    ValidateMedication();
                }
                if (choice == 2)
                {
                    ShowNotifications();
                }
            } catch (Exception e)
            {
                Console.WriteLine("Molim Vas, unesite broj!");
            }

        }

        private void ShowNotifications()
        {
            foreach (Notification notification in notificationController.GetNotificationsForUser(loggedInDoctor.Username))
            {
                Console.WriteLine(notification.Id.ToString() + "\t" + notification.ContentOfNotification + "\t" + "\t" + notification.NotificationCategory.ToString());
            }
        }

        private void ValidateMedication()
        {
          foreach (Medication medication in medicationController.GetAllOnValidationForDoctor(loggedInDoctor))
            {
                Console.WriteLine(medication.Med);
            }
        }
    }
}
