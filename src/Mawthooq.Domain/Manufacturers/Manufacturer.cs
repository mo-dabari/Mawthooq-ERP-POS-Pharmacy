using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Manufacturers.Enums;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.SharedKernel.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Manufacturers
{
    public class Manufacturer
    {
        public ManufacturerId ManufacturerId { get; private set; }
        public ManufacturerName Name { get; private set; }
        public ManufacturerType Type { get; private set; }
        public TaxNumber TaxNumber { get; private set; }
        public CommercialRegister CommercialRegister { get; private set; }

        public Address HeadquartersAddress { get; private set; }
        public Phone Phone { get; private set; }
        public Email Email { get; private set; }

        public Status Status { get; private set; }

        private Manufacturer(
            ManufacturerName name,
            ManufacturerType type,
            TaxNumber tyaxNumber,
            CommercialRegister commercialRegister,
            Address headquartersAddress,
            Phone phone,
            Email email
        )
        {
            ArgumentNullException.ThrowIfNull(name, nameof(name));
            ArgumentNullException.ThrowIfNull(type, nameof(type));
            ArgumentNullException.ThrowIfNull(tyaxNumber, nameof(tyaxNumber));
            ArgumentNullException.ThrowIfNull(commercialRegister, nameof(commercialRegister));
            ArgumentNullException.ThrowIfNull(headquartersAddress, nameof(headquartersAddress));
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            ArgumentNullException.ThrowIfNull(email, nameof(email));

            ManufacturerId = ManufacturerId.New();
            Name = name;
            Type = type;
            TaxNumber = tyaxNumber;
            CommercialRegister = commercialRegister;
            HeadquartersAddress = headquartersAddress;
            Phone = phone;
            Email = email;
            Status = Status.Active;
        }

        public static Manufacturer Create(
            ManufacturerName name,
            ManufacturerType type,
            TaxNumber taxNumber,
            CommercialRegister commercialRegister,
            Address headquartersAddress,
            Phone phone,
            Email email
        )
        {
            return new(name, type, taxNumber, commercialRegister, headquartersAddress, phone, email);
        }

        public void Rename(ManufacturerName manufacturerName)
        {
            ArgumentNullException.ThrowIfNull(manufacturerName, nameof(manufacturerName));

            if (Name.Equals(manufacturerName))
                return;

            Name = manufacturerName;
        }

        public void ChangeType(ManufacturerType type)
        {
            if (!Enum.IsDefined(typeof(ManufacturerType), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(ManufacturerType));

            if (type.Equals(type))
                return;

            Type = type;
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
