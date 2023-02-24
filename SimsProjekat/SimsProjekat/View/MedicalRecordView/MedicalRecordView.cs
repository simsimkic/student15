using Controller.MedicalRecordController;
using Model.MedicalRecord;
using SimsProjekat.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.View.MedicalRecordView
{
    public class MedicalRecordView
    {
        public MedicalRecordController medicalRecordController;
        
        public MedicalRecordView()
        {
            medicalRecordController = Program.app.medicalRecordController;
            ShowRecord();
        }

        private void ShowRecord()
        {
            string noRecord = "";
            MedicalRecord record = null;
            try
            {
              record = medicalRecordController.GetRecordByPatient(PatientView.loggedInPatient);
            } catch (NoMedicalRecord)
            {
                Console.WriteLine("Nemate zdravstveni karton u našoj bolnici. Želite li da otvorite?");
                Console.WriteLine("1. DA 2. NE");
                noRecord = Console.ReadLine();
                if (noRecord.Equals("1"))
                {
                    var created = medicalRecordController.CreateNewRecord(new MedicalRecord(BloodType.abNeg, PatientView.loggedInPatient, PatientCondition.stable));
                    record = medicalRecordController.GetMedicalRecord(created.IdRecord);
                } else if (noRecord.Equals("2"))
                {
                    return;
                }

            }
            string input = "";
            Console.WriteLine("Unesite x ili X za povratak na meni!");

            Console.WriteLine("Vaša istorija bolesti:");
            foreach (var diagnose in record.IllnessHistory)
            {
                Console.WriteLine("Dijagnoza: " + diagnose.Name);
            }
            Console.WriteLine();


            Console.WriteLine("Vaša porodična istorija: ");
            foreach (var diagnose in record.FamilyIllnessHistory)
            {
                string familyMember = "";
                if (diagnose.RelativeMember == Model.MedicalRecord.Relative.father)
                {
                    familyMember = "Otac";
                } else if (diagnose.RelativeMember == Model.MedicalRecord.Relative.mother)
                {
                    familyMember = "Majka";
                }
                else if (diagnose.RelativeMember == Model.MedicalRecord.Relative.sibling)
                {
                    familyMember = "Braća i sestre";
                }
                else if (diagnose.RelativeMember == Model.MedicalRecord.Relative.grandparents)
                {
                    familyMember = "Baba i deda";
                }
                Console.WriteLine("Član porodice: " + familyMember + ":");
                Console.WriteLine("Dijagnoze:");
                foreach (var illness in diagnose.Diagnosis)
                {
                    Console.WriteLine("Dijagnoza: " + illness.Name);
                }
            }
            Console.WriteLine("Vaše alergije:");
            foreach (var allergens in record.Allergies)
            {
                Console.WriteLine("Alergija: " + allergens.Allergen);
            }
            Console.WriteLine();

            Console.WriteLine("Vaše primljene vakcine:");
            foreach (var vaccines in record.Vaccines)
            {
                Console.WriteLine("Vakcina: " + vaccines.Name);
            }
            Console.WriteLine();

            Console.WriteLine("Vaše terapije i satnica korišćenja:");
            foreach (var med in record.Therapies)
            {
                Console.WriteLine("Lek: " + med.Medication.Med + " Upotreba na: " + med.HourConsumption + " sati.");
            }
            Console.WriteLine();


            Console.ReadLine();

            if (input.ToLower().Equals("x"))
            {
                return;
            }
            
        }
    }
}
