using Controller.ExaminationController;
using Controller.GeneralController;
using Controller.MedicalRecordController;
using Controller.ScheduleController;
using Controller.UserController;
using Model.ExaminationSurgery;
using Model.MedicalRecord;
using Model.Schedule;
using Model.Users;
using Repository.GeneralRepository;
using Service.ScheduleService;
using SimsProjekat.Controller.ScheduleController;
using SimsProjekat.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SimsProjekat.View
{
    class PatientView
    {
        UserController userController;
        CityController cityController;
        AddressController addressController;
        StateController stateController;
        AppointmentController appointmentController;
        MedicalRecordController medicalRecordController;
        SpecializationController specializationController;
        QuestionController questionController;
        FeedbackController feedbackController;
        DoctorReviewController doctorReviewController;
        ArticleController articleController;
        NotificationController notificationController;
        Validations validate;
        AvailableAppointmentController availableAppointmentController;
        ExaminationSurgeryController examinationSurgeryController;


        public static Patient loggedInPatient = null;

        public PatientView(Patient patient)
        {
            loggedInPatient = patient;
            userController = Program.app.userController;
            cityController = Program.app.cityController;
            addressController = Program.app.addressController;
            stateController = Program.app.stateController;
            appointmentController = Program.app.appointmentController;
            medicalRecordController = Program.app.medicalRecordController;
            specializationController = Program.app.specializationController;
            questionController = Program.app.questionController;
            feedbackController = Program.app.feedbackController;
            doctorReviewController = Program.app.doctorReviewController;
            articleController = Program.app.articleController;
            notificationController = Program.app.notificationController;
            availableAppointmentController = Program.app.availableAppointmentController;
            examinationSurgeryController = Program.app.examinationSurgeryController;

            validate = Program.app.validations;
        }
        public void ShowMenu()
        {
            int choice = 0;
            while (true)
            { 
                // TODO! Dodaj da može da pregleda svoj zdravstveni karton 

                Console.WriteLine();
                List<string> menuOptions = new List<string>();
                menuOptions.Add("Preporuči mi termin.");
                menuOptions.Add("Zakaži termin.");
                menuOptions.Add("Pogledaj i izmeni zakazane termine");
                menuOptions.Add("Postavi pitanje.");
                menuOptions.Add("Pregled postavljenih pitanja.");
                menuOptions.Add("Oceni softver.");
                menuOptions.Add("Pogledaj ocene softvera.");
                menuOptions.Add("Oceni lekara.");
                menuOptions.Add("Pogledaj ocene lekara.");
                menuOptions.Add("Postavi željenog lekara.");
                menuOptions.Add("Pogledaj željenog lekara.");
                menuOptions.Add("Čitaj blog.");
                menuOptions.Add("Pogledaj notifikacije.");
                menuOptions.Add("Pregledaj svoj zdravstveni karton.");
                menuOptions.Add("Pogledaj istoriju pregleda.");
                choice = choiceConsoleParser(menuOptions, true);

                if (choice == 0)
                {
                    break;
                }
                if (choice == 1)
                {
                    recommendAppointment();
                }
                if (choice == 2) 
                {
                    makeAnAppointmentConsole();
                }
                if (choice == 3) 
                {
                    seeAppointmentsConsole();
                }
                if (choice == 4) 
                {
                    askQuestionConsole();
                }
                if (choice == 5)
                {
                    readQuestionsConsole();
                }
                if (choice == 6) 
                {
                    gradeSoftwareConsole();
                }
                if (choice == 7)
                {
                    readSoftwareGradesConsole();
                }
                if (choice == 8) 
                {
                    gradeDoctorConsole();
                }
                if (choice == 9)
                {
                    readDoctorGradesConsole();
                }
                if (choice == 10)
                {
                    setChosenDoctorConsole();
                }
                if (choice == 11)
                {
                    seeChosenDoctorConsole();
                }
                if (choice == 12)
                {
                    readBlogConsole();
                }
                if (choice == 13)
                {
                    checkNotificationsConsole();
                } 
                if (choice == 14)
                {
                    MedicalRecordView.MedicalRecordView view = new MedicalRecordView.MedicalRecordView();
                }
                if (choice == 15)
                {
                    readAppointmentHistoryConsole();
                }
            }
            
            
        }

        public void recommendAppointment() {

            int priority = 0;

            PriorityParameters parameters = new PriorityParameters();

            Console.WriteLine("Po čemu želite da preusmerimo našu pretragu?");
            List<string> options = new List<string>();
            options.Add("Po lekaru.");
            options.Add("Po datumu.");
            priority = choiceConsoleParser(options, false);
            if (priority == 1)
                parameters.Priority = PriorityType.doctor;
            else if(priority == 2)
                parameters.Priority = PriorityType.date;

            Console.WriteLine("Lista svih lekara za pregled: ");

            List<string> doctors = returnAllDoctorsString();
            List<string> licenseNumbers = new List<string>();
            foreach (string d in doctors) {
                string[] items = d.Split('\t');
                licenseNumbers.Add(items[0]);
            }
            
            Console.WriteLine("Vaš izbor: ");

            int doctorChoice = choiceConsoleParser(doctors, false);
            parameters.ChosenDoctor = (Doctor)userController.GetDoctor(licenseNumbers[doctorChoice - 1]);

            Console.WriteLine("Unesite početni datum: (Format dd.MM.yyyy.)");
            DateTime startDate = dateConsoleParser();
            parameters.ChosenStartDate = startDate;

            Console.WriteLine("Unesite krajnji datum: (Format dd.MM.yyyy.)");
            DateTime endDate = dateConsoleParser();
            parameters.ChosenEndDate = endDate;

            try
            {
                Appointment recommended = availableAppointmentController.RecommendAppointment(parameters);
                Console.WriteLine("DATUM I VREME: " + recommended.StartTime.ToString("dd.MM.yyyy. HH:mm:ss") + " KOD: " + recommended.Doctor.Name + " " + recommended.Doctor.Surname);
                Console.Write("Želite li da zakažete? (DA/NE)  ");
                string schedule = Console.ReadLine();
                if (schedule.ToLower().Equals("da"))
                {
                    recommended.MedicalRecord = medicalRecordController.GetRecordByPatient(loggedInPatient);
                    recommended.Urgent = false;
                    recommended.ShortDescription = "";
                    appointmentController.AddAppointment(recommended, false);
                    Console.WriteLine("Hvala sto ste zakazali pregled. ");
                }
                else
                {
                    Console.WriteLine("Vaš odustanak je potvrđen! ");
                }
            } catch (CantFindAppointment)
            {
                Console.WriteLine("Ne možemo naći termin za vas! Verovatno nema slobodnih lekara u tom periodu!"); 

            } catch (Exception)
            {
                Console.WriteLine("Došlo je do neke greške!");
            }

           
        }
        public void makeAnAppointmentConsole() {

            Doctor doc;
            if (loggedInPatient.ChosenDoctor == null)
                doc = noChosenDoctorConsole();
            else 
                doc = haveChosenDoctorConsole();

            Appointment chosenAppointment = dateAndAppointmentConsole(doc);
            if (chosenAppointment != null)
            {
                chosenAppointment.MedicalRecord = medicalRecordController.GetRecordByPatient(loggedInPatient);
                chosenAppointment.Urgent = false;
                chosenAppointment.ShortDescription = "";
                appointmentController.AddAppointment(chosenAppointment, false);
                Console.WriteLine("Hvala što ste zakazali pregled. ");
            }
        }

        public Appointment dateAndAppointmentConsole(Doctor doc) {
            Console.WriteLine("Unesite željeni datum za pregled: (Format dd.MM.yyyy.)");
            DateTime date = dateConsoleParser();

            var availableAppointments = availableAppointmentController.GetAvailableForDayAndDoctor(date, doc, TypeOfAppointment.examination, false);

            List<Appointment> apps = new List<Appointment>();
            List<string> options = new List<string>();
            foreach (KeyValuePair<int, Appointment> kvp in availableAppointments)
            {
                options.Add("\t" + kvp.Value.StartTime.ToString() + " \t" + kvp.Value.EndTime.ToString());
                apps.Add(kvp.Value);
            }
            if (options.Count == 0)
                Console.WriteLine("Ne postoji termin u datom danu, pokušajte neki drugi.");
            else
            {
                int choice = choiceConsoleParser(options, false);
                Appointment chosenAppointment = apps[choice - 1];
                return chosenAppointment;
            }
            return null;
        }
        public Doctor haveChosenDoctorConsole() 
        {
            Console.WriteLine("Vaš odabrani lekar je:  " + loggedInPatient.ChosenDoctor.LicenseNumber + "  " + loggedInPatient.ChosenDoctor.Name + "  " + loggedInPatient.ChosenDoctor.Surname);
            List<string> options = new List<string>();
            options.Add("Nastavi sa odabranim lekarom.");
            options.Add("Odaberi drugog lekara.");
            int choice = choiceConsoleParser(options, false);
            if (choice == 2)
            {
                Doctor doc = returnChosenDoc();
                return doc;
            }else
                return loggedInPatient.ChosenDoctor;
        }
        public Doctor noChosenDoctorConsole()
        {
            Console.WriteLine("Nemate odabranog lekara!");
            Console.WriteLine("Odaberite lekara sa sledeće liste: ");
            return returnChosenDoc();
        }
        public List<string> returnAllDoctorsString() {
            List<string> list = new List<string>();
            int i = 1;
            foreach (Doctor doctor in userController.GetAllDoctorsBySpecialization(specializationController.GetGeneralSpecialization()))
            {
                list.Add(doctor.Username + "\t" + doctor.Name + "\t" + doctor.Surname);
            }
            return list;
        }
        public void seeAppointmentsConsole() {
            Console.WriteLine("Vaši zakazani pregledi: ");
            Console.WriteLine();
            Appointment appointment = appointmentController.GetScheduledForPatient(loggedInPatient);
            if (appointment == null)
                Console.WriteLine("Nemate zakazanih pregleda!");
            else {
                Console.WriteLine(appointment.Doctor.Name + " " + appointment.Doctor.Surname + " \t" + appointment.StartTime.ToString() + " \t" + appointment.EndTime.ToString());
                List<string> options = new List<string>();
                options.Add("Izmeni termin.");
                options.Add("Odustanak.");
                int choice = choiceConsoleParser(options, false);
                if (choice == 2)
                    return;
                else {
                    Doctor doc = appointment.Doctor;
                    Appointment chosenAppointment = dateAndAppointmentConsole(doc);
                    if (chosenAppointment != null)
                    {
                        chosenAppointment.MedicalRecord = medicalRecordController.GetRecordByPatient(loggedInPatient);
                        chosenAppointment.Urgent = false;
                        chosenAppointment.ShortDescription = "";
                        appointmentController.DeleteAppointment(appointment);
                        appointmentController.AddAppointment(chosenAppointment, false);
                        Console.WriteLine("Hvala što ste zakazali pregled. ");
                    }
                }
            }
        }
        public void askQuestionConsole() {
            Console.WriteLine("Unesite naslov pitanja: ");
            string title = Console.ReadLine();
            Console.WriteLine("Unesite pitanje: ");
            string question = Console.ReadLine();
            DateTime now = DateTime.Today;
            PostContent pc = new PostContent(question, title);
            Question askedQuestion = new Question(now, false, loggedInPatient, pc);
            questionController.AskQuestion(askedQuestion);
        }

        public void readQuestionsConsole() {
            var allQuestions = questionController.GetAll();
            foreach (Question q in allQuestions)
            {
                Console.WriteLine("Autor:\t\t\t" + q.Author.Username + " \t\t ");
                Console.WriteLine("Datum: \t\t\t" + q.Date.ToString());
                Console.WriteLine("Naslov:\t\t\t" + q.PostContent.ContentTitle);
                Console.WriteLine("Pitanje:\t\t" + q.PostContent.Content);
                Console.WriteLine();
            }
        }

        public void gradeSoftwareConsole() {
            string message1 = "Unesite ocenu softvera od 1-5. [1 - Veoma zadovoljan/na ... 5 - Nimalo nisam zadovoljan/na]";
            string message2 = "Unesite ocenu da li je sve na svom mestu od 1-5. [1 - Veoma zadovoljan/na ... 5 - Nimalo nisam zadovoljan/na]";
            int grade1 = gradeConsoleParser(1, 5, message1);
            int grade2 = gradeConsoleParser(1, 5, message2);
            int option = 0;
            List<string> lista = new List<string>();
            lista.Add("Napiši utisak o softveru.");
            lista.Add("Oceni bez utiska.");
            option = choiceConsoleParser(lista, false);
            Feedback feedback;
            DateTime now = DateTime.Today;
            if (option == 1)
            {
                Console.WriteLine("Upišite vas utisak o softveru: ");
                string feed = Console.ReadLine();
                feedback = new Feedback(now, feed, gradeParser(grade1), gradeParser(grade2), loggedInPatient);
                Feedback f = feedbackController.CreateFeedback(feedback);
                if (f == null)
                {
                    Console.WriteLine("Softveru mozete dati ocenu samo jednom! ");
                }
                else {
                    Console.WriteLine("Hvala vam na oceni softvera!");
                }
            }
            else if (option == 2)
            {
                feedback = new Feedback(now, "Nema utiska", gradeParser(grade1), gradeParser(grade2), loggedInPatient);
                Feedback f = feedbackController.CreateFeedback(feedback);
                if (f == null)
                {
                    Console.WriteLine("Softveru možete dati ocenu samo jednom! ");
                }
                else {
                    Console.WriteLine("Hvala vam na oceni softvera!");
                }
            }
            else {
                Console.WriteLine("Došlo je do greške!");
            }
        }

        public int choiceConsoleParser(List<string> list, bool exit) {
            int option = 0;
            while (option == 0)
            {
                for (int i = 1; i <= list.Count; i++) {
                    Console.WriteLine(string.Format("{0} - {1}", i, list[i - 1]));
                }
                if (exit == true)
                {
                    Console.WriteLine("x - Za izlazak unesite X ili x.");
                }
                string line = Console.ReadLine();
                if (exit == true)
                {
                    if (line.ToLower().Equals("x"))
                        return 0;
                }
                try
                {
                    int input = int.Parse(line);
                    if (input >= 1 && input <= list.Count)
                    {
                        option = input;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(string.Format("Molim Vas, unesite opciju od 1 do {0}", list.Count));
                }
            }
            return option;
        }

        public int gradeConsoleParser(int floor, int ceiling, string message) {
            bool flag = false;
            int grade = 0;
            while (flag == false)
            {
                Console.WriteLine(message);
                try
                {
                    grade = int.Parse(Console.ReadLine());
                    if (grade >= floor && grade <= ceiling)
                    {
                        flag = true;
                    }
                    else 
                    {
                        Console.WriteLine(string.Format("Molim Vas, unesite ocenu od {0} do {1}!", floor, ceiling));
                    }
                }
                catch (Exception e)
                {
                    flag = false;
                    Console.WriteLine(string.Format("Molim Vas, unesite ocenu od {0} do {1}!", floor, ceiling));
                }
            }
            return grade;
        }
        public Grade gradeParser(int grade) {
            if (grade == 1)
                return Grade.excellent;
            if (grade == 2)
                return Grade.veryGood;
            if (grade == 3)
                return Grade.good;
            if (grade == 4)
                return Grade.poor;
            if (grade == 5)
                return Grade.veryPoor;
            return Grade.veryPoor;
        }
        public DateTime dateConsoleParser() {
            DateTime date = DateTime.Today;
            bool flag = true;
            while (flag)
            {
                try
                {
                    date = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy.", null);
                    flag = false;
                }
                catch (Exception e)
                {
                    flag = true;
                    Console.WriteLine("Molim Vas, unesite validan format!");
                }
            }
            return date;
        }
        public void readSoftwareGradesConsole()
        {

            var allFeedbacks = feedbackController.GetAllFeedbacks();
            if (allFeedbacks == null)
                return;
            foreach (Feedback f in allFeedbacks)
            {
                Console.Write("Korisnik:\t\t\t");
                Console.WriteLine(f.RegisteredUser.Username);
                Console.Write("Ocena softvera:\t\t\t");
                Console.WriteLine(f.SoftwareGrade.ToString());
                Console.Write("Ocena da li je sve na mestu:\t");
                Console.WriteLine(f.EverythingInGoodPlace.ToString());
                Console.Write("Dodatni utisci:\t\t\t");
                Console.WriteLine(f.AdditionalNotes);
                Console.WriteLine();
            }
        }

        public void gradeDoctorConsole() {
            Console.WriteLine("Odaberite lekara kojeg želite da ocenite: ");
            Doctor doc = returnChosenDoc();
            string message = "Unesite ocenu lekara od 1-5. [1 - Veoma zadovoljan/na ... 5 - Nimalo nisam zadovoljan/na]";
            int grade = gradeConsoleParser(1, 5, message);
            DateTime now = DateTime.Today;
            DoctorReview review = new DoctorReview(now, gradeParser(grade), loggedInPatient, doc);
            DoctorReview r = doctorReviewController.EvaluateDoctor(review);
            if(r == null)
            {
                Console.WriteLine("Ne možete oceniti istog lekara više puta!");
            }else
                Console.WriteLine("Hvala vam na vasoj oceni!");
        }
        public void readDoctorGradesConsole() {
            Console.WriteLine("Lista lekara i njihovih prosečnih ocena: ");
            Console.WriteLine();
            List<string> options = new List<string>();
            List<string> licenseNumbers = new List<string>();
            var doctors = userController.GetAllDoctors();
            Console.WriteLine("Korisničko ime:" + "   \t" + "Ime:" + "   \t\t\t" + "Prezime:" + "   \t\t" + "Prosečna ocena:");
            foreach (Doctor d in doctors)
            {
                Console.WriteLine(d.Username + "   \t\t" + d.Name + "   \t\t" + d.Surname + "   \t\t" + doctorReviewController.GetGradeByDoctor(d));
            }
        }
        public void setChosenDoctorConsole() {
            Console.WriteLine("Lista lekara: ");
            Doctor doc = returnChosenDoc();
            loggedInPatient.ChosenDoctor = doc;
            Console.WriteLine(doc.LicenseNumber + "   \t" + doc.Name + "   \t" + doc.Surname + "   \t" + "je postavljen/na za vašeg odabranog lekara.");
            userController.UpdateUserProfile(loggedInPatient);
            loggedInPatient = (Patient)userController.GetUser(loggedInPatient.Username);
        }
        public void seeChosenDoctorConsole() {
            if (loggedInPatient.ChosenDoctor != null)
                Console.WriteLine("Vaš odabrani lekar je: " + loggedInPatient.ChosenDoctor.LicenseNumber + "   \t" + loggedInPatient.ChosenDoctor.Name + "   \t" + loggedInPatient.ChosenDoctor.Surname);
            else
                Console.WriteLine("Nemate postavljenog odabranog lekara.");
        }

        public Doctor returnChosenDoc() {
            List<string> options = new List<string>();
            List<string> licenseNumbers = new List<string>();
            var doctors = userController.GetAllDoctors();
            foreach (Doctor d in doctors)
            {
                licenseNumbers.Add(d.Username);
                options.Add(d.Username + "\t\t" + d.Name + "\t\t" + d.Surname);
            }
            int a = choiceConsoleParser(options, false);
            Doctor doc = (Doctor)userController.GetDoctor(licenseNumbers[a - 1]);
            return doc;
        }

        public void readBlogConsole() {
            Console.WriteLine("Odaberite članak koji želite da pročitate: ");
            Console.WriteLine();
            var allArticles = articleController.GetAllArticles().ToList();
            List<String> options = new List<string>();
            
            foreach (Article a in allArticles) {
                options.Add(a.PostContent.ContentTitle + " \t Autor: " + a.Author.Name + "  " + a.Author.Surname);
            }
            if (options.Count == 0)
            {
                Console.WriteLine("Na blogu nema nijednog članka!");
            }
            else
            {
                int choice = choiceConsoleParser(options, false);
                Console.WriteLine(options[choice - 1]);
                Console.WriteLine();
                Console.WriteLine(allArticles[choice - 1].PostContent.Content);
            }
        }

        public void checkNotificationsConsole() {
            Console.WriteLine("Notifikacije: ");
            Console.WriteLine();
            var allNotifications = notificationController.GetNotificationsForUser(loggedInPatient.Username).ToList();
            if (allNotifications.Count == 0)
            {
                Console.WriteLine("Trenutno nemate notifikacije!");
            }
            else 
            {
                List<string> options = new List<string>();
                foreach (Notification n in allNotifications) {
                    options.Add("Notifikacija od: " + n.NotificationFrom.Username);
                }
                int choice = choiceConsoleParser(options, false);
                Console.WriteLine();
                Console.WriteLine("Notifikacija od: " + allNotifications[choice - 1].NotificationFrom.Username);
                Console.WriteLine(allNotifications[choice - 1].ContentOfNotification);
            }
        }
        public void readAppointmentHistoryConsole() {
            Console.WriteLine("Istorija pregleda:");
            Console.WriteLine();
            MedicalRecord medicalRecord = medicalRecordController.GetRecordByPatient(loggedInPatient);
            if (medicalRecord == null)
            {
                Console.WriteLine("Nemate prethodnih pregleda!");
            }
            else {
                var allExaminations = examinationSurgeryController.GetAllByRecord(medicalRecord).ToList();
                if (allExaminations.Count == 0)
                    Console.WriteLine("Nemate prethodnih pregleda!");
                else
                {
                    foreach (ExaminationSurgery es in allExaminations)
                    {
                        Console.WriteLine("Pregled kod: " + es.Doctor.Name + " \t" + es.Doctor.Surname + " \tDatuma: " + es.StartTime.ToString());
                    }
                }
            }
        }

        public void Register()
        {
            Console.WriteLine("Dobrodošli!");

            int choice = -1;
            while (choice != 0) 
            {
                Console.WriteLine("Da li želite da se registrujete kao naš stalni korisnik\n ili želite da zakažete pregled privremenim nalogom?");
                Console.WriteLine("\t1 za potpunu registraciju, 2 za privremenu.");


                try
                {
                    choice = int.Parse(Console.ReadLine());
                    if (choice == 1)
                    {
                        FullRegistration();
                        choice = 0;
                    }
                    else if (choice == 2)
                    {
                        GuestAccountRegistration();
                        choice = 0;
                    }
                    else
                    {
                        choice = 0;
                    }

                }

                catch (UnderageException e) {
                    Console.WriteLine("Ne možete se registrovati ukoliko ste maloletni!");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Molim Vas, unesite validan broj!");
                }                     
            }
        }
        
        private void GuestAccountRegistration()
        {
            string name = "", surname = "", phoneNumber = "", username = "", identificationNumber = "";
            while (name.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite ime: ");
                    name = Console.ReadLine();
                    validate.IsNameValid(name);
                }
                catch (IncorrectNameFormat e)
                {
                    name = "";
                    Console.WriteLine("Nevalidan format imena!");
                }
            }
            while (surname.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite prezime: ");
                    surname = Console.ReadLine();
                    validate.IsSurnameValid(surname);
                }
                catch (IncorrectSurnameFormat e)
                {
                    surname = "";
                    Console.WriteLine("Nevalidan format prezimena!");
                }
            }
            while (identificationNumber.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite JMBG: ");
                    identificationNumber = Console.ReadLine();
                    validate.IsIdentificationNumberValid(identificationNumber);
                }
                catch (IncorrectIdentificationNumberFormat e)
                {
                    Console.WriteLine("Nevalidan format identifikacionog broja!");
                }

            }
            Console.WriteLine("Uneste broj telefona za obaveštenja za izmene termina: ");
            phoneNumber = Console.ReadLine();
            
            while (username.Equals(""))
            {
                try
                {
                    Console.WriteLine("Uneste ime po kom ćemo Vas zabeležiti u bazu: ");
                    username = Console.ReadLine();
                    validate.IsUsernameValid(username);
                }
                catch (IncorrectUsernameFormat e)
                {
                    username = "";
                    Console.WriteLine("Nevalidan format korisnickog imena!");
                }
            }

            PatientBuilder patientBuilder = new PatientBuilder();
            patientBuilder.SetName(name);
            patientBuilder.SetSurname(surname);
            patientBuilder.SetUsername(username);
            patientBuilder.SetIDNumber(identificationNumber);
            patientBuilder.SetPhone(phoneNumber);
            patientBuilder.SetGuestAccount(true);

            try
            {
                userController.RegisterUser(patientBuilder.BuildPatient());
                loggedInPatient = (Patient)userController.GetUser(username);
                ShowMenuForGuestAcoount();
            } catch (Exception)
            {
                Console.WriteLine("Došlo je do neke greške!");
            }
        }

        public void ShowMenuForGuestAcoount()
        {
            int choice = 0;
            while (true)
            {
                Console.WriteLine();
                List<string> menuOptions = new List<string>();
                menuOptions.Add("Preporuči mi termin.");
                menuOptions.Add("Zakaži termin.");
                choice = choiceConsoleParser(menuOptions, true);

                if (choice == 0)
                {
                    break;
                }
                if (choice == 1)
                {
                    recommendAppointment();
                }
                if (choice == 2)
                {
                    makeAnAppointmentConsole();
                }
            }
        }

        private void FullRegistration()
        {
            string name = "", surname = "", phoneNumber = "", emailAddress = "", username = "",
                password = "", identificationNumber = "";
            while (name.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite ime: ");
                    name = Console.ReadLine();
                    validate.IsNameValid(name);
                }
                catch (IncorrectNameFormat e)
                {
                    name = "";
                    Console.WriteLine("Nevalidan format imena!");
                }
            }
            while (surname.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite prezime: ");
                    surname = Console.ReadLine();
                    validate.IsSurnameValid(surname);
                }
                catch (IncorrectSurnameFormat e)
                {
                    surname = "";
                    Console.WriteLine("Nevalidan format prezimena!");
                }
            }
            while (identificationNumber.Equals(""))
            {
                try
                {
                    Console.WriteLine("Unesite JMBG: ");
                    identificationNumber = Console.ReadLine();
                    validate.IsIdentificationNumberValid(identificationNumber);
                }
                catch (IncorrectIdentificationNumberFormat e)
                {
                    Console.WriteLine("Nevalidan format identifikacionog broja!");
                }

            }
            Console.WriteLine("Uneste broj telefona: ");
            phoneNumber = Console.ReadLine();
            while (emailAddress.Equals(""))
            {
                try 
                {
                    Console.WriteLine("Uneste email adresu: ");
                    emailAddress = Console.ReadLine();
                    validate.IsEmailValid(emailAddress);
                }
                catch (IncorrectEmailAddress e)
                {
                    emailAddress = "";
                    Console.WriteLine("Nevalidan format email adrese!");
                }
            }
            while (username.Equals(""))
            {
                try
                {
                    Console.WriteLine("Uneste korisničko ime: ");
                    username = Console.ReadLine();
                    validate.IsUsernameValid(username);
                }
                catch (IncorrectUsernameFormat e)
                {
                    username = "";
                    Console.WriteLine("Nevalidan format korisničkog imena!");
                }
            }
            while (password.Equals(""))
            {
                try
                {
                    Console.WriteLine("Uneste lozinku: ");
                    password = Console.ReadLine();
                    validate.IsPasswrodValid(password);
                }
                catch (IncorrectPasswordFormat e)
                {
                    password = "";
                    Console.WriteLine("Nevalidan format lozinke!");
                }
            }
                Console.WriteLine("Unesite svoje zanimanje: ");
                string profession = Console.ReadLine();

                Console.WriteLine("Unesite grad i državu u kojoj živite oblika:[Grad,Država]: ");
                string input = Console.ReadLine();
                input = input.Trim();
                string[] elem = input.Split(',');

                State state = new State(elem[1]);
                if (!stateController.CheckIfExists(state))
                {
                    stateController.CreateState(state);
                }

                City city = new City(elem[0], state);
                if (!cityController.CheckIfExists(city))
                {
                    city = cityController.CreateCity(city);
                }
                else
                {
                    city = cityController.GetCityByName(city);
                }

                Console.WriteLine("Unesite adresu prebivališta oblika:[Ulica,Broj,Stan,Sprat]: ");
                string input2 = Console.ReadLine();
                input2 = input2.Trim();
                string[] elem2 = input2.Split(',');

                Address address = new Address(elem2[0], int.Parse(elem2[1]), int.Parse(elem2[2]), int.Parse(elem2[3]), city);
                if (!addressController.CheckIfExists(address))
                {
                    address = addressController.CreateAddress(address);
                }
                else
                {
                    address = addressController.GetExistentAddress(address);
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
                userController.RegisterUser(patientBuilder.BuildPatient());
                loggedInPatient = (Patient)userController.GetUser(username);
                ShowMenu();
            } catch (Exception)
            {
                Console.WriteLine("Došlo je do neke greške!");
            }

        }
        
    }
}
