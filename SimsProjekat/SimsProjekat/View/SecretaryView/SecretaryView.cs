using Controller.GeneralController;
using Controller.MedicalRecordController;
using Controller.RoomController;
using Controller.ScheduleController;
using Controller.UserController;
using Model.ExaminationSurgery;
using Model.MedicalRecord;
using Model.Rooms;
using Model.Schedule;
using Model.Users;
using SimsProjekat.Controller.ScheduleController;
using SimsProjekat.SIMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.SecretaryView
{
    public class SecretaryView
    {
        public Secretary loggedInSecretary;
        public BedController bedController;
        public UserController userController;
        public AppointmentController appointmentController;
        public MedicalRecordController medicalRecordController;
        public RoomController roomController;
        public AddressController addressController;
        public StateController stateController;
        public CityController cityController;
        public NotificationController notificationController;
        public FeedbackController feedbackController;
        public Validations validate;
        public AvailableAppointmentController availableAppointmentController;

        private AppointmentsSubMenu appointmentsSubMenu;
        private AccountsSubMenu accountsSubMenu;
        private BedSubMenu bedSubMenu;
        private NotificationsSubMenu notificationsSubMenu;
        private FeedbackSubMenu feedbackSubMenu;
        private MyAccountSubMenu myAccountSubMenu;
        
        public SecretaryView(Secretary secretary)
        {
            loggedInSecretary = secretary;
            this.bedController = Program.app.bedController;
            this.userController = Program.app.userController;
            this.appointmentController = Program.app.appointmentController;
            this.medicalRecordController = Program.app.medicalRecordController;
            this.roomController = Program.app.roomController;
            this.addressController = Program.app.addressController;
            this.stateController = Program.app.stateController;
            this.cityController = Program.app.cityController;
            this.notificationController = Program.app.notificationController;
            this.feedbackController = Program.app.feedbackController;
            this.availableAppointmentController = Program.app.availableAppointmentController;

            validate = Program.app.validations;

            appointmentsSubMenu = new AppointmentsSubMenu(this);
            bedSubMenu = new BedSubMenu(this);
            accountsSubMenu = new AccountsSubMenu(this);
            notificationsSubMenu = new NotificationsSubMenu(this);
            feedbackSubMenu = new FeedbackSubMenu(this);
            myAccountSubMenu = new MyAccountSubMenu(this);
        }

        internal void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Evidencija termina");
                Console.WriteLine("\t2. Evidencija naloga");
                Console.WriteLine("\t3. Evidencija kreveta");
                Console.WriteLine("\t4. Notifikacije");
                Console.WriteLine("\t5. Blog");
                Console.WriteLine("\t6. Anketa");
                Console.WriteLine("\t7. Moj profil");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    appointmentsSubMenu.ShowMenu();
                }
                else if (selectedOption.Equals("2"))
                {
                    accountsSubMenu.ShowMenu();
                }
                else if (selectedOption.Equals("3"))
                {
                    bedSubMenu.ShowMenu();
                }
                else if (selectedOption.Equals("4"))
                {
                    notificationsSubMenu.ShowMenu();
                }
                else if (selectedOption.Equals("5"))
                {
                    break;
                }
                else if (selectedOption.Equals("6"))
                {
                    feedbackSubMenu.ShowMenu();
                }
                else if (selectedOption.Equals("7"))
                {
                    myAccountSubMenu.ShowMenu();
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
       
    }
}
