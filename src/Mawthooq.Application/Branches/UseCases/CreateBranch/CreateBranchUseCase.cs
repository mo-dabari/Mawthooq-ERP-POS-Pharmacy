using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Branches;
using Mawthooq.Domain.ValueObjects;

namespace Mawthooq.Application.Branches.UseCases.CreateBranch
{
    public class CreateBranchUseCase
    {
        private readonly IBranchRepository _branchRepository;

        public CreateBranchUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }


        public CreateBranchResponse Execute(CreateBranchRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Branch branch = Branch.Create(request.Name, request.Location.ToAddressDomain());

            Branch? saveBranch = _branchRepository.Add(branch);

            if (saveBranch is null)
                throw new InvalidOperationException("Failed to create branch.");

            return new CreateBranchResponse(saveBranch.BranchID, saveBranch.Name, saveBranch.Location.ToAddressDto(), saveBranch.IsActive);
        }
    }
}
