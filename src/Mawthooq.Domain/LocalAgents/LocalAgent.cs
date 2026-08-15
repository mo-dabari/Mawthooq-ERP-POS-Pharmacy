using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.LocalAgents
{
    public class LocalAgent
    {
        public LocalAgentId LocalAgentId { get; }
        public LocalAgentName Name { get; private set; }
        public TaxNumber TaxNumber { get; private set; }
        public CommercialRegister CommercialRegister { get; private set; }

        public Address HeadquartersAddress { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }

        public Status Status { get; private set; }


        private LocalAgent(
            LocalAgentName name,
            TaxNumber tyaxNumber,
            CommercialRegister commercialRegister,
            Address headquartersAddress,
            Phone phone,
            Email email
        )
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(tyaxNumber, nameof(tyaxNumber));
            ArgumentNullException.ThrowIfNull(commercialRegister, nameof(commercialRegister));
            ArgumentNullException.ThrowIfNull(headquartersAddress, nameof(headquartersAddress));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            LocalAgentId = LocalAgentId.New();
            Name = name;
            TaxNumber = tyaxNumber;
            CommercialRegister = commercialRegister;
            HeadquartersAddress = headquartersAddress;
            Phone = phone;
            Email = email;
            Status = Status.Active;
        }

        public static LocalAgent Create(
            LocalAgentName name,
            TaxNumber taxNumber,
            CommercialRegister commercialRegister,
            Address headquartersAddress,
            Phone phone,
            Email email
        )
        {
            return new(name, taxNumber, commercialRegister, headquartersAddress, phone, email);
        }

        public void Rename(LocalAgentName localAgentName)
        {
            ArgumentNullException.ThrowIfNull(localAgentName, nameof(localAgentName));

            if (Name.Equals(localAgentName))
                return;

            Name = localAgentName;
        }

        public void ChangeTyaxNumber(TaxNumber taxNumber)
        {
            ArgumentNullException.ThrowIfNull(taxNumber, nameof(taxNumber));

            if (TaxNumber.Equals(taxNumber))
                return;

            TaxNumber = taxNumber;
        }

        public void ChangeCommercialRegister(CommercialRegister commercialRegister)
        {
            ArgumentNullException.ThrowIfNull(commercialRegister, nameof(commercialRegister));

            if (CommercialRegister.Equals(commercialRegister))
                return;

            CommercialRegister = commercialRegister;
        }

        public void ChangeAddress(Address headquartersAddress)
        {
            ArgumentNullException.ThrowIfNull(headquartersAddress, nameof(headquartersAddress));

            if (HeadquartersAddress.Equals(headquartersAddress))
                return;

            HeadquartersAddress = headquartersAddress;
        }

        public void ChangePhone(Phone newNumberPhone)
        {
            ArgumentNullException.ThrowIfNull(newNumberPhone, nameof(newNumberPhone));

            if (Phone.Equals(newNumberPhone))
                return;

            Phone = newNumberPhone;
        }

        public void ChangeEmail(Email newEmail)
        {
            ArgumentNullException.ThrowIfNull(newEmail, nameof(newEmail));

            if (Email.Equals(newEmail))
                return;

            Email = newEmail;
        }

        public void AllowActivated()
        {
            if (Status == Status.Active)
                return;

            Status = Status.Active;
        }

        public void Deactivation()
        {
            if (Status == Status.Inactive)
                return;

            Status = Status.Inactive;
        }
    }
}
