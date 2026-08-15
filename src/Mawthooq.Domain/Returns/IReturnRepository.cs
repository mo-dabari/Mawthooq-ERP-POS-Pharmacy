using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Returns
{
    public interface IReturnRepository
    {
        Return AddReturn(Return returnEntity);
        Return? GetReturnById(int returnId);
        Return? GetReturnBySaleId(int saleId);
        void Update(Return returnEntity);
    }
}
