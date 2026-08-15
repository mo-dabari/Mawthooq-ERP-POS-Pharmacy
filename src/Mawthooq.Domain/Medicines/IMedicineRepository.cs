using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Medicines.ValueObjects;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.Medicines
{
    public interface IMedicineRepository : IBaseRepository<Medicine, int>
    {
        bool ExistsByMedicineName(MedicineName medicineName);
    }
}
