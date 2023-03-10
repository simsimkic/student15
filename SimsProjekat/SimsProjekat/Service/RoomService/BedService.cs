// File:    BedService.cs
// Created: Tuesday, May 19, 2020 11:01:37 PM
// Purpose: Definition of Class BedService

using Model.MedicalRecord;
using Model.Rooms;
using Repository.RoomRepository;
using SimsProjekat.Exceptions;
using SimsProjekat.SIMS.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Service.RoomService
{
   public class BedService
   {
        public BedService(IBedRepository bedRepository)
        {
            this.bedRepository = bedRepository;
        }

        public Bed AddBed(Bed bed) => bedRepository.Create(bed);

        public bool DeleteBed(Bed bed) => bedRepository.Delete(bed);


        public Bed GetBed(int bedId) => bedRepository.GetObject(bedId);

        public IEnumerable<Bed> GetAllBeds() => bedRepository.GetAll();

        public IEnumerable<Bed> GetAllFreeBedsInPeriod(DateTime startDate, DateTime endDate)
        {
            var allBeds = bedRepository.GetAll().ToList();
            List<Bed> freeBeds = new List<Bed>();
            foreach (Bed bed in allBeds)
            {
                bool isFree = true;
                foreach (Occupation occupation in bed.Occupations)
                {
                    if (CheckIfDatesOverlap(startDate, endDate, occupation))
                    {
                        isFree = false;
                        break;
                    }
                }
                if (isFree)
                {
                    freeBeds.Add(bed);
                }
            }
            return freeBeds;
        }


        public IEnumerable<Bed> GetBedsByRoomNumber(int roomNumber) => bedRepository.GetBedsByRoomNumber(roomNumber);

        public bool OccupyBed(Bed bed, Occupation occupation)
        {
            if (CheckIfOccupationDatesAreValid(occupation))
            {
                Bed bedForOccupation = bedRepository.GetObject(bed.BedId);
                if (!CheckIfOccupationsOverlap(bedForOccupation, occupation))
                {
                    bedForOccupation.AddOccupation(occupation);
                    bedRepository.Update(bedForOccupation);
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool FreeBed(Bed bed, DateTime startDate, DateTime endDate)
        {
            Bed freeBed = bedRepository.GetObject(bed.BedId);
            List<Occupation> newOccupationList = new List<Occupation>();
            foreach (Occupation occupation in freeBed.Occupations)
            {
                if (!CheckIfDatesOverlap(startDate, endDate, occupation))
                {
                    newOccupationList.Add(occupation);
                }
            }
            freeBed.Occupations = newOccupationList;
            bedRepository.Update(freeBed);
            return true;
        }

        private bool CheckIfOccupationDatesAreValid(Occupation occupation)
        {
            if (DateTime.Compare(occupation.OccupiedFromDate, occupation.OccupiedToDate) > 0)
            {
                throw new InvalidDate(INVALID_DATE);
            }
            else if(DateTime.Compare(DateTime.Now, occupation.OccupiedFromDate) > 0)
            {
                throw new InvalidDate(INVALID_DATE);
            }
            return true;
        }

        private bool CheckIfOccupationsOverlap(Bed bed, Occupation occupation)
        {
            foreach (Occupation occup in bed.Occupations)
            {
                if (CheckIfDatesOverlap(occup.OccupiedFromDate, occup.OccupiedToDate, occupation))
                {
                    throw new AlreadyOccupied(string.Format(ALREADY_OCCUPIED, occupation.OccupiedFromDate.ToString("dd.MM.yyyy."), occupation.OccupiedToDate.ToString("dd.MM.yyyy.")));
                }
            }
            return false;
        }

        private bool CheckIfDatesOverlap(DateTime startDate, DateTime endDate, Occupation occupation)
        {
            if (DateTime.Compare(startDate, occupation.OccupiedToDate) > 0)
            {
                return false;
            }
            else if (DateTime.Compare(occupation.OccupiedFromDate, endDate) > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private const string INVALID_DATE = "Occupation date is invalid!";
        private const string ALREADY_OCCUPIED = "Bed is already occupied from {0} to {1}";


        public IBedRepository bedRepository;

    }
}