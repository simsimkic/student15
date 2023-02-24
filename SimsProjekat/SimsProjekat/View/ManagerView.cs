using Model.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SimsProjekat.View
{
    class ManagerView
    {
        public Manager loggedInManager;
        public ManagerView(Manager manager)
        {
            loggedInManager = manager;
        }

        public void ShowMenu()
        {
            Console.WriteLine("Dobrodošli upravniče, " + loggedInManager.Name + " " + loggedInManager.Surname +"\n\n");
            Console.WriteLine("MENI:");
            Console.WriteLine("1- Zaposleni");
            Console.WriteLine("2- Rasporedi");
            Console.WriteLine("3- Oprema");
            Console.WriteLine("4- Prostorije");
            Console.WriteLine("5- Lekovi");
            Console.WriteLine("6- Blog");
            Console.WriteLine("7- Ocena softvera");
            Console.WriteLine("8- Moj profil");
            Console.WriteLine("9- Podešavanja");
            Console.WriteLine("10- Obaveštenja");

            var option = int.Parse(Console.ReadLine());

            if(option==1)
            {
                ZaposleniMenu();
            }
            else if (option == 2)
            {
                RasporediMenu();
            }
            else if (option == 3)
            {
                OpremaMenu();
            }
            else if (option == 4)
            {
                ProstorijeMenu();
            }
            else if (option == 5)
            {
                LekoviMenu();
            }
            else if (option == 6)
            {
                BlogMenu();
            }
            else if (option == 7)
            {
                OcenaSoftveraMenu();
            }
            else if (option == 8)
            {
                MojProfilMenu();
            }
            else if (option == 9)
            {
                PodesavanjaMenu();
            }
            else
            {
                ObavestenjaMenu();
            }

        }

        public void ZaposleniMenu()
        {
            Console.WriteLine("Izaberite opciju:");
            Console.WriteLine("1- Izlistaj sve zaposlene");
            Console.WriteLine("2- Dodaj zaposlenog");
            Console.WriteLine("3- Izmeni zaposlenog");
            Console.WriteLine("4- Obriši zaposlenog");
            Console.WriteLine("5- Pretraži zaposlene");
        }

        public void RasporediMenu()
        {

        }
        public void OpremaMenu()
        {

        }
        public void ProstorijeMenu()
        {

        }
        public void LekoviMenu()
        {

        }
        public void BlogMenu()
        {

        }
        public void OcenaSoftveraMenu()
        {

        }
        public void ObavestenjaMenu()
        {

        }
        public void PodesavanjaMenu()
        {

        }
        public void MojProfilMenu()
        {

        }

    }
}

