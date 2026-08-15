using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Branches
{
    public class Branch
    {
        public int BranchID { get; }
        public string Name { get; private set; }
        public Address Address { get; private set; }
        public bool IsActive { get; private set; }

        public Branch(int branchID, string name, Address address) : this(name, address)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(branchID, nameof(branchID));
            BranchID = branchID;
        }

        private Branch(string name, Address address)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));

            Name = name;
            Address = address;
            IsActive = true;
        }

        public static Branch Create(string name, Address address)
        {
            return new Branch(name, address);
        }

        public void ChangeBranchName(string newBranchName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newBranchName, nameof(newBranchName));

            if (Name != newBranchName)
                Name = newBranchName;
        }

        public void SetBranchAddress(Address newAddress)
        {
            ArgumentNullException.ThrowIfNull(newAddress, nameof(newAddress));

            if (Address == null || !Address.Equals(newAddress))
                Address = newAddress;
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
