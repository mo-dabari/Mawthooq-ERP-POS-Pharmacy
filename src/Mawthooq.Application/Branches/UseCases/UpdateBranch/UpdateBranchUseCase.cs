using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Branches;

namespace Mawthooq.Application.Branches.UseCases.UpdateBranch
{
    public class UpdateBranchUseCase
    {
        private readonly IBranchRepository _branchRepository;

        public UpdateBranchUseCase(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public UpdateBranchResponse Execute(UpdateBranchRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Branch? branch = _branchRepository.GetById(request.BranchId);

            if (branch == null)
                throw new InvalidOperationException("Branch not found");

            branch.ChangeBranchName(request.Name);
            branch.ChangeBranchAddress(request.Location.ToAddressDomain());

            if (request.IsActive)
                branch.Activate();
            else
                branch.Deactivate();

            _branchRepository.Update(branch);

            return new UpdateBranchResponse(branch.BranchID, branch.Name, branch.Location.ToAddressDto(), branch.IsActive);
        }
    }
}
