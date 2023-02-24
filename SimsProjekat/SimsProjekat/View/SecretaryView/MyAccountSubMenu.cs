using Model.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.SecretaryView
{
    class MyAccountSubMenu
    {
        SecretaryView secretaryView;

        public MyAccountSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            var secretary = secretaryView.userController.GetUser(secretaryView.loggedInSecretary.Username);

            Console.WriteLine("\nVas profil:");
            Console.WriteLine("Id: " + secretary.IdentificationNumber);
            Console.WriteLine("Ime: " + secretary.Name);
            Console.WriteLine("Prezime: " + secretary.Surname);
            Console.WriteLine("Korisnicko ime: " + secretary.Username);
            Console.WriteLine("Datum rodjenja: " + secretary.DateOfBirth);
            Console.WriteLine("Mejl: " + secretary.Email);
            Console.WriteLine("Adresa: {0} {1}/{2}", secretary.CurrResidence.Street, secretary.CurrResidence.Number, secretary.CurrResidence.Apartment);
            Console.WriteLine("Stepen strucne spreme: ", secretary.EducationLevel.ToString());

            Console.WriteLine("\nOpcije:");
            Console.WriteLine("\t1. Izmeni profil");
            Console.WriteLine("\t0. Izlaz");
            Console.WriteLine("Izaberite opciju:");

            string selectedOption = Console.ReadLine();

            if (selectedOption.Equals("1"))
            {
                EditMyProfileMenu();
            }
        }

        private void EditMyProfileMenu()
        {
            while (true)
            {
                Console.WriteLine("\nSelektujte stavku koju zelite da izmenite:");
                Console.WriteLine("\t1. Ime \n\t2. Prezime \n\t3. Korisnicko ime \n\t4. Lozinku \n\t5. Mejl \n\t6. Adresu \n\t7. Stepen strucne spreme \n\t0. Izlaz");
                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    Console.WriteLine("Unesite ime (Trenutno: {0})", secretaryView.loggedInSecretary.Name);
                    string name = Console.ReadLine();
                    secretaryView.loggedInSecretary.Name = name;
                    EditMyProfile();
                }
                else if (selectedOption.Equals("2"))
                {
                    Console.WriteLine("Unesite prezime (Trenutno: {0})", secretaryView.loggedInSecretary.Surname);
                    string surname = Console.ReadLine();
                    secretaryView.loggedInSecretary.Surname = surname;
                    EditMyProfile();
                }
                else if (selectedOption.Equals("3"))
                {
                    Console.WriteLine("Unesite korisnicko ime (Trenutno: {0})", secretaryView.loggedInSecretary.Username);
                    string username = Console.ReadLine();
                    secretaryView.loggedInSecretary.Username = username;
                    EditMyProfile();
                }
                else if (selectedOption.Equals("4"))
                {
                    return;
                }
                else if (selectedOption.Equals("5"))
                {
                    return;
                }
                else if (selectedOption.Equals("0"))
                {
                    return;
                }
                else
                {
                    Console.WriteLine("\nNepravilan unos!\n");
                }
            }
           
        }

        private void EditMyProfile()
        {
            try
            {
                secretaryView.userController.UpdateUserProfile(secretaryView.loggedInSecretary);
                Console.WriteLine("\nProfil je uspesno izmenjen!");
            }
            catch (Exception)
            {
                Console.WriteLine("Greska prilikom izmene korisnika!");
            }
        }
    }
}
