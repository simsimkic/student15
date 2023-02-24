using Model.Users;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace SimsProjekat.View.SecretaryView
{
    class FeedbackSubMenu
    {

        public SecretaryView secretaryView;

        public FeedbackSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Popunite anketu");
                Console.WriteLine("\t2. Prikazite sve ankete");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    SendFeedback();
                }
                else if (selectedOption.Equals("2"))
                {
                    ShowAllFeedbacks();
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

        private void SendFeedback()
        {
            Console.WriteLine("1. Ocena softvera (0 - 5):");
            string softwareGradeString = Console.ReadLine();
            if(!Regex.IsMatch(softwareGradeString, @"^[0-5]{1}$"))
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("2. Ocena pozicioniranja komponenti (0 - 5):");
            string componentPlacementGrade = Console.ReadLine();
            if (!Regex.IsMatch(componentPlacementGrade, @"^[0-5]{1}$"))
            {
                Console.WriteLine("Nepravilan unos!");
                return;
            }

            Console.WriteLine("3. Dodatne primedbe (0 - za prekid unosa):");
            StringBuilder additionalNotes = new StringBuilder();
            string line;
            while (!(line = Console.ReadLine()).Equals("0"))
            {
                additionalNotes.Append(line);
            }

            //secretaryView.feedbackController.CreateFeedback(new Feedback(DateTime.Now, additionalNotes.ToString(), (Model.Users.Grade)Int32.Parse(softwareGradeString),
            //                                               (Model.Users.Grade)Int32.Parse(componentPlacementGrade), secretaryView.loggedInSecretary,));

        }

        private void ShowAllFeedbacks()
        {
            var allFeedbacks = secretaryView.feedbackController.GetAllFeedbacks();
            Console.WriteLine("\nPrikaz popunjenih anketa:");
            foreach(Feedback feedback in allFeedbacks)
            {
                Console.WriteLine("\n----------------------------------------------");
                Console.WriteLine("Datum: " + feedback.Date.ToString("dd.MM.yyyy. HH:mm"));
                Console.WriteLine("Ocena softvera: " + ((int)feedback.SoftwareGrade).ToString());
                Console.WriteLine("Dodatne primedbe: " + feedback.AdditionalNotes);
                Console.WriteLine("Korisnik koji je popunio anketu: " + feedback.RegisteredUser.Username);
                Console.WriteLine("----------------------------------------------\n");
            }
        }
    }
}
