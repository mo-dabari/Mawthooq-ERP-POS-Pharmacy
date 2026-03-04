using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects.Address;

namespace Mawthooq.Domain.Branch
{
    public class Branch
    {
        public int BranchID { get; }
        public string Name { get; private set; }
        public Address Location { get; private set; }
        public bool IsActive { get; private set; }

        public Branch(int branchID, string name, Address location)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchID, nameof(branchID));
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentNullException.ThrowIfNull(location, nameof(location));

            BranchID = branchID;
            Name = name;
            Location = location;
            IsActive = true;
        }

        public void ChangeBranchName(string newBranchName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newBranchName, nameof(newBranchName));

            if (Name != newBranchName)
                Name = newBranchName;
        }

        public void ChangeBranchAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address, nameof(address));

            if (!Location.Equals(address))
                Location = address;
        }
        public void Deactivate()
        {
            if (!IsActive)
                return;
            IsActive = false;
        }
        public void Activate()
        {
            if (IsActive)
                return;
            IsActive = true;
        }
    }
}
