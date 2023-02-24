using Model.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.SecretaryView
{
    public class NotificationsSubMenu
    {

        SecretaryView secretaryView;

        public NotificationsSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            Console.WriteLine("\nNotifikacije:");
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Prikazi sve notifikacije");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    ShowAllNotifications();
                }
                else if (selectedOption.Equals("0"))
                {
                    return;
                }
                else
                {
                    Console.WriteLine("\nNepravilan unos!");
                }
            }
        }

        private void ShowAllNotifications()
        {
            var allNotifications = secretaryView.notificationController.GetNotificationsForUser(secretaryView.loggedInSecretary.Username);

            Console.WriteLine("\n{0, -10} {1, -10} {2, -15} \n", "Id", "Kategorija", "Od koga");
            foreach (Notification notification in allNotifications)
            {
                string notificationCategory = "";
                if (notification.NotificationCategory == NotificationCategory.SCHEDULE)
                {
                    notificationCategory = "Raspored";
                }
                else if(notification.NotificationCategory == NotificationCategory.RENOVATION)
                {
                    notificationCategory = "Renoviranje";
                }
                else if (notification.NotificationCategory == NotificationCategory.BLOG)
                {
                    notificationCategory = "Blog";
                }
                else if (notification.NotificationCategory == NotificationCategory.VACATION_REQUEST)
                {
                    notificationCategory = "Zahtev za odmor";
                }

                Console.WriteLine("{0, -10} {1, -10} {2, -15}", notification.Id, notificationCategory, notification.NotificationFrom.Username);
            }
        }

    }
}
