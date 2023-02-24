using Model.Rooms;
using Repository.RoomRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Service.RoomService
{
    public class EquipmentTypeService
    {
        public EquipmentTypeRepository equipmentTypeRepository;
        public EquipmentTypeService(EquipmentTypeRepository equipmentTypeRepository)
        {
            this.equipmentTypeRepository = equipmentTypeRepository;
        }

        internal EquipmentType AddNewType(EquipmentType type) => equipmentTypeRepository.Create(type);

        internal IEnumerable<EquipmentType> GetAllTypes() => equipmentTypeRepository.GetAll();

        
    }
}
