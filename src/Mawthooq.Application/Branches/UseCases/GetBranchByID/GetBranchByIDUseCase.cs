using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Branches;

namespace Mawthooq.Application.Branches.UseCases.GetBranchByID
{
    public class GetBranchByIDUseCase
    {
        private readonly IBranchRepository _branchRepository;

        public GetBranchByIDUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public GetBranchByIDResponse Execute(int branchId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchId, nameof(branchId));
            Branch? branch = _branchRepository.GetById(branchId);

            if (branch is null)
                throw new InvalidOperationException($"Branch with ID {branchId} not found.");

            return new GetBranchByIDResponse(branch.BranchID, branch.Name, branch.Address?.ToAddressDto(), branch.IsActive);
        }
    }
}
