using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines;
using Mawthooq.Domain.Medicines.ValueObjects;

namespace Mawthooq.Application.Medicines.Commands.RenameMedicine
{
    public class RenameMedicineUseCase
    {
        private IMedicineRepository _medicineRepository;

        public RenameMedicineUseCase(IMedicineRepository medicineRepository)
        {
            ArgumentNullException.ThrowIfNull(medicineRepository);
            _medicineRepository = medicineRepository;
        }

        public RenameMedicineResponse Execute(RenameMedicineRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);

            Medicine? medicine = _medicineRepository.GetById(request.MedicineId);

            if (medicine is null)
                throw new InvalidOperationException("Not Found Medicine");

            medicine.Rename(new MedicineName(request.MedicineName));

            _medicineRepository.Update(medicine);

            return new RenameMedicineResponse(medicine.MedicineId, medicine.MedicineName.Value);
        }
    }
}
