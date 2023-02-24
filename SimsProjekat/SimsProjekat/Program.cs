using Controller.UserController;
using Model.Users;
using SimsProjekat.View;
using System;
using SimsProjekat.SIMS.Exceptions;
using SimsProjekat.Exceptions;
using SimsProjekat.View.SecretaryView;

namespace SimsProjekat
{
    class Program
    {
        public static Application app = new Application();
        static void Main(string[] args)
        {
            Console.WriteLine("\tDobrodošli u Zdravo Korporaciju!");
            Console.WriteLine("\tAko želite da se ulogujete, unesite 1,\n\ta u slučaju da želite da se registrujete za naše usluge \n\tili samo da zakažete termin, unesite 2.");
            int choice = 0;
            while (true)
            {
                try
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice == 2)
                    {
                        PatientView view = new PatientView(null);
                        view.Register();
                    }
                    else if (choice == 1)
                        break;

                }
                catch (Exception e)
                {
                    Console.WriteLine("Molim Vas, unesite validan broj!");
                }
            }
            
            RegisteredUser user = null;
            while (true)
            {
                Console.WriteLine("Korisničko ime: ");
                string username = Console.ReadLine();

                Console.WriteLine("Lozinka: ");
                string password = Console.ReadLine();

                UserController userController = app.userController;
          
                try
                {
                    user = userController.Login(username, password);
                    break;
                }
                catch (EntityNotFound)
                {
                    Console.WriteLine("Nalog ne postoji!");
                } catch (InvalidPassword)
                {
                    Console.WriteLine("Pogrešna lozinka/korisničko ime. Pokušajte ponovo.");
                } catch (GuestAccount)
                {
                    Console.WriteLine("Vaš nalog je privremeni. Ukoliko želite da koristite naše usluge sa ovim nalogom, kontaktirajte sekretara!");
                } catch (Exception)
                {
                    Console.WriteLine("Došlo je do neke greške!");
                }
            }
            SwitchMenu(user);
        }
        private static void SwitchMenu(RegisteredUser user)
        {
            if (user is Doctor)
            {
                DoctorView view = new DoctorView((Doctor) user);
                view.ShowMenu();
            } else if (user is Manager)
            {
                ManagerView view = new ManagerView((Manager)user);
                view.ShowMenu();
            } else if (user is Secretary)
            {
                SecretaryView view = new SecretaryView((Secretary)user);
                view.ShowMenu();
            } else
            {
                PatientView view = new PatientView((Patient)user);
                var patient = (Patient)user;
                if (patient.IsGuestAccount)
                    view.ShowMenuForGuestAcoount();
                else
                    view.ShowMenu();
            }
        }
    }
}
