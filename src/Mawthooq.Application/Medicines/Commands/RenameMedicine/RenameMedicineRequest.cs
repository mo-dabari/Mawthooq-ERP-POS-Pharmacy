using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Application.Medicines.Commands.RenameMedicine
{
    public sealed record class RenameMedicineRequest(int MedicineId, string MedicineName);

}
