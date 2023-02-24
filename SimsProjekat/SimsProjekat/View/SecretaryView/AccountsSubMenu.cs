using Controller.UserController;
using Model.Users;
using SimsProjekat.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SimsProjekat.View.SecretaryView
{
    class AccountsSubMenu
    {

        private SecretaryView secretaryView;

        public AccountsSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Prikazite sve pacijente");
                Console.WriteLine("\t2. Prikazite sve lekare");
                Console.WriteLine("\t3. Kreiraj nalog pacijentu");
                Console.WriteLine("\t4. Obrisi nalog pacijenta");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    ShowAllPatients();
                }
                else if (selectedOption.Equals("2"))
                {
                    ShowAllDoctors();
                }
                else if (selectedOption.Equals("3"))
                {
                    CreatePatientAccount();
                }
                else if (selectedOption.Equals("4"))
                {
                    DeletePatientAccount();
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

        private void CreatePatientAccount()
        {
            Console.WriteLine("\nRegistracija:");

            string name = "", surname = "",
                emailAddress = "", identificationNumber = "", password = "", username = "";

            while (name.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite ime: ");
                    name = Console.ReadLine();
                    secretaryView.validate.IsNameValid(name);

                } catch (IncorrectNameFormat)
                {
                    name = "";
                    Console.WriteLine("Unesite pravilno ime: [VELIKO SLOVO][mala slova]");
                }
            }

            while (surname.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite prezime: ");
                    surname = Console.ReadLine();
                    secretaryView.validate.IsSurnameValid(name);
                } catch (IncorrectSurnameFormat)
                {
                    surname = "";
                    Console.WriteLine("Unesite pravilno prezime: [VELIKO SLOVO][mala slova]");
                }
            }
            while (identificationNumber.Equals(""))
            {
                try {
                    Console.WriteLine("Unesite JMBG: ");

                    identificationNumber = Console.ReadLine();
                    secretaryView.validate.IsIdentificationNumberValid(name);
                } catch (IncorrectIdentificationNumberFormat)
                {
                    identificationNumber = "";
                    Console.WriteLine("JMBG mora imati 13 cifara.");
                }
            }

            Console.WriteLine("Uneste broj telefona: ");
            string phoneNumber = Console.ReadLine();

            while (emailAddress.Equals(""))
            {
                try
                {
                    Console.WriteLine("Uneste email adresu: ");

                    emailAddress = Console.ReadLine();
                    secretaryView.validate.IsEmailValid(name);
                } catch (IncorrectEmailAddress)
                {
                    emailAddress = "";
                    Console.WriteLine("Nevalidan format email adrese.");
                }
            }

            while (username.Equals(""))
            {
                try {
                    Console.WriteLine("Uneste korisničko ime: ");
                    username = Console.ReadLine();
                    secretaryView.validate.IsUsernameValid(name);
                } catch (IncorrectUsernameFormat)
                {
                    emailAddress = "";
                    Console.WriteLine("Nevalidan format korisnickog imena.");
                }
            }

            while (password.Equals(""))
            {
                try
                {
                    Console.WriteLine("Uneste lozinku: ");
                    password = Console.ReadLine();
                    secretaryView.validate.IsPasswrodValid(name);
                } catch (IncorrectPasswordFormat)
                {
                    password = "";
                    Console.WriteLine("Nedovoljno jaka šifra. (mora sadržati jedno veliko slovo i broj.");
                }
            }

            Console.WriteLine("Unesite svoje zanimanje: ");
            string profession = Console.ReadLine();

            Console.WriteLine("Unesite grad i državu (Grad, Drzava): ");
            string input = Console.ReadLine();
            string[] elem = input.Split(',');

            State state = new State(elem[1]);
            if (!secretaryView.stateController.CheckIfExists(state))
            {
                secretaryView.stateController.CreateState(state);
            }

            City city = new City(elem[0], state);
            if (!secretaryView.cityController.CheckIfExists(city))
            {
                city = secretaryView.cityController.CreateCity(city);
            }
            else
            {
                city = secretaryView.cityController.GetCityByName(city);
            }

            Console.WriteLine("Unesite adresu (Ulica,Broj,Stan,Sprat): ");
            string input2 = Console.ReadLine();
            string[] elem2 = input2.Split(',');

            Address address = new Address(elem2[0], int.Parse(elem2[1]), int.Parse(elem2[2]), int.Parse(elem2[3]), city);
            if (!secretaryView.addressController.CheckIfExists(address))
            {
                address = secretaryView.addressController.CreateAddress(address);
            } else
            {
                address = secretaryView.addressController.GetExistentAddress(address);
            }

            PatientBuilder patientBuilder = new PatientBuilder();
            patientBuilder.SetName(name);
            patientBuilder.SetSurname(surname);
            patientBuilder.SetIDNumber(identificationNumber);
            patientBuilder.SetPhone(phoneNumber);
            patientBuilder.SetEmail(emailAddress);
            patientBuilder.SetUsername(username);
            patientBuilder.SetPassword(password);
            patientBuilder.SetProfession(profession);
            patientBuilder.SetPlaceOfBirth(city);
            patientBuilder.SetCurrentResidence(address);
            patientBuilder.SetGuestAccount(false);

            try
            {
                secretaryView.userController.RegisterUser(patientBuilder.BuildPatient());
                Console.WriteLine("Registracija uspesna!");
            }
            catch (Exception)
            {
                Console.WriteLine("Registracija pacijenta neuspesna!");
            }
        }

        private void DeletePatientAccount()
        {
            ShowAllPatients();

            Console.WriteLine("Unesite korisnicko ime pacijenta kojeg zelite da obrisete:");
            string patientUsername = Console.ReadLine();
            try
            {
                Patient patient = (Patient)secretaryView.userController.GetUser(patientUsername);
                secretaryView.userController.DeleteUser(patient);
                Console.WriteLine("Pacijent je uspesno izbrisan!");
            }
            catch (Exception)
            {
                Console.WriteLine("\nGreska prilikom brisanja pacijenta!");
            }
        }

        private void ShowAllPatients()
        {
            var allPatients = secretaryView.userController.GetAllPatients();
            Console.WriteLine("\n{0, -20} {1, -10} {2, -15} {3,-20} {4, -20} {5, -15}\n", "Id", "Ime", "Prezime", "Korisnicko ime", "Email", "Broj telefona");

            foreach (Patient patient in allPatients)
            {
                Console.WriteLine("{0, -20} {1, -10} {2, -15} {3,-20} {4, -20} {5, -15}", patient.IdentificationNumber, patient.Name, patient.Surname, patient.Username, patient.Email, patient.Phone);
            }

        }

        private void ShowAllDoctors()
        {
            var allDoctors = secretaryView.userController.GetAllDoctors();
            Console.WriteLine("\n{0, -20} {1, -10} {2, -15} {3, -15}  {4,-20} {5, -20} {6, -15}\n", "Id", "Ime", "Prezime", "Specijalizacija", "Korisnicko ime", "Email", "Broj telefona");

            foreach (Doctor doctor in allDoctors)
            {
                Console.WriteLine("{0, -20} {1, -10} {2, -15} {3, -15}  {4,-20} {5, -20} {6, -15}", doctor.IdentificationNumber, doctor.Name, doctor.Surname,
                    doctor.Specializations[0].SpecializationName, doctor.Username, doctor.Email, doctor.Phone);
            }

        }
    }
}
