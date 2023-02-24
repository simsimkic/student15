using Model.MedicalRecord;
using Model.Rooms;
using SimsProjekat.Exceptions;
using SimsProjekat.SIMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.SecretaryView
{
    public class BedSubMenu
    {
        SecretaryView secretaryView;

        public BedSubMenu(SecretaryView secretaryView)
        {
            this.secretaryView = secretaryView;
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nOpcije:");
                Console.WriteLine("\t1. Prikazite sve krevete");
                Console.WriteLine("\t2. Prikazite krevete u nekoj sobi");
                Console.WriteLine("\t3. Prikazite zauzetost odredjenog kreveta");
                Console.WriteLine("\t4. Zauzimanje kreveta");
                Console.WriteLine("\t5. Oslobadjanje kreveta");
                Console.WriteLine("\t0. Izlaz");

                Console.WriteLine("Izaberite opciju:");
                string selectedOption = Console.ReadLine();

                if (selectedOption.Equals("1"))
                {
                    ShowAllBeds();
                }
                else if (selectedOption.Equals("2"))
                {
                    Console.WriteLine("Unesite broj sobe:");
                    string roomNumber = Console.ReadLine();
                    ShowBedsByRoomNumber(Int32.Parse(roomNumber));
                }
                else if (selectedOption.Equals("3"))
                {
                    Console.WriteLine("Unesite id kreveta:");
                    string id = Console.ReadLine();
                    ShowBedOccupations(Int32.Parse(id));
                }
                else if (selectedOption.Equals("4"))
                {
                    Console.WriteLine("Unesite id kreveta:");
                    string idString = Console.ReadLine();
                    int id;
                    if (!Int32.TryParse(idString, out id))
                    {
                        Console.WriteLine("Nepravilan unos!");
                        continue;
                    }
                    OccupyBed(id);
                }
                else if (selectedOption.Equals("5"))
                {
                    Console.WriteLine("Unesite id kreveta:");
                    string id = Console.ReadLine();
                    FreeBed(Int32.Parse(id));
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

        private void ShowAllBeds()
        {
            var allBeds = secretaryView.bedController.GetAllBeds();

            Console.WriteLine("\n****************************************************");
            Console.WriteLine("Spisak svih kreveta:");

            foreach (Bed bed in allBeds)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("Id kreveta: " + bed.BedId.ToString());
                Console.WriteLine("Broj sobe u kojoj se krevet nalazi: " + bed.Room.RoomNumber.ToString());
                Console.WriteLine("-----------------------------------\n");
            }

            Console.WriteLine("****************************************************\n");
        }

        private void ShowBedsByRoomNumber(int roomNumber)
        {
            try
            {
                var bedsByRoomNumber = secretaryView.bedController.GetBedsByRoomNumber(roomNumber);
                Console.WriteLine("\n****************************************************");
                Console.WriteLine("Spisak kreveta u sobi {0}:", roomNumber);
                foreach (Bed bed in bedsByRoomNumber)
                {
                    Console.WriteLine("\n-----------------------------------");
                    Console.WriteLine("Id kreveta: " + bed.BedId.ToString());
                    Console.WriteLine("Broj sobe u kojoj se krevet nalazi: " + bed.Room.RoomNumber.ToString());
                    Console.WriteLine("-----------------------------------");
                }
                Console.WriteLine("****************************************************\n");
            }
            catch (EntityNotFound)
            {
                Console.WriteLine("Soba sa unetim brojem ne postoji!");
            }
        }

        private void OccupyBed(int bedId)
        {
            Console.WriteLine("Unesite pocetni datum (dd.MM.yyyy.):");
            string startDateString = Console.ReadLine();
            Console.WriteLine("Unesite krajnji datum (dd.MM.yyyy.):");
            string endDateString = Console.ReadLine();
            Console.WriteLine("Unesite id kartona pacijenta:");
            string recordIdString = Console.ReadLine();

            DateTime startDate = DateTime.ParseExact(startDateString, "dd.MM.yyyy.", null);
            DateTime endDate = DateTime.ParseExact(endDateString, "dd.MM.yyyy.", null);
            MedicalRecord patient = Program.app.medicalRecordController.GetMedicalRecord(Int32.Parse(recordIdString));

            var bed = secretaryView.bedController.GetBed(bedId);

            try
            {
                secretaryView.bedController.OccupyBed(bed, new Occupation(startDate, endDate, new MedicalRecord(Int32.Parse(recordIdString))));
                Console.WriteLine("\nZauzimanje kreveta uspesno!\n");
            }
            catch(Exception e)
            {
                if(e is InvalidDate)
                {
                    Console.WriteLine("\nZauzimanje kreveta neuspesno! Nepravilan unos datuma!\n");
                }
                else if(e is AlreadyOccupied)
                {
                    Console.WriteLine("\nZauzimanje kreveta neuspesno! Krevet je vec zauzet u uneto periodu!\n");
                }
                else if (e is FormatException)
                {
                    Console.WriteLine("\nZauzimanje kreveta neuspesno! Nepravilan format datuma!\n");
                }
                else
                {
                    throw;
                }
            }
        }

        private void FreeBed(int bedId)
        {
            Console.WriteLine("Unesite pocetni datum (dd.MM.yyyy.):");
            string startDateString = Console.ReadLine();
            Console.WriteLine("Unesite krajnji datum (dd.MM.yyyy.):");
            string endDateString = Console.ReadLine();

            DateTime startDate = DateTime.ParseExact(startDateString, "dd.MM.yyyy.", null);
            DateTime endDate = DateTime.ParseExact(endDateString, "dd.MM.yyyy.", null);

            var bed = secretaryView.bedController.GetBed(bedId);
            if (secretaryView.bedController.FreeBed(bed, startDate, endDate))
            {
                Console.WriteLine("\nOslobadjanje kreveta uspesno!\n");
            }
            else
            {
                Console.WriteLine("\nOslobadjanje kreveta neuspesno\n");
            }

        }

        private void ShowBedOccupations(int bedId)
        {
            var bed = secretaryView.bedController.GetBed(bedId);

            Console.WriteLine("\n****************************************************");
            Console.WriteLine("Raspored zauzetosti za krevet {0}: ", bedId);
            foreach (Occupation occupation in bed.Occupations)
            {
                Console.WriteLine("\n-----------------------------------");
                Console.WriteLine("Pocetni datum: " + occupation.OccupiedFromDate.ToString("dd.MM.yyyy."));
                Console.WriteLine("Krajnji datum: " + occupation.OccupiedToDate.ToString("dd.MM.yyyy."));
                Console.WriteLine("Korisnicko ime pacijenta: " + occupation.Patient.Patient.Username);
                Console.WriteLine("-----------------------------------\n");
            }
            Console.WriteLine("****************************************************\n");
        }
    }
}
