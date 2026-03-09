using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mawthooq.Domain.Users.Enums;
using Mawthooq.Domain.ValueObjects;
using Mawthooq.Domain.ValueObjects.NationalID;

namespace Mawthooq.Domain.Users
{
    public class User
    {
        private static readonly DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        public int UserId { get; }
        public NationalId NationalId { get; }
        public string FirstName { get; }
        public string MiddleName { get; }
        public string LastName { get; }
        public Phone Phone { get; private set; }
        private List<Phone> _contactNumbers = new();
        public IReadOnlyList<Phone> contactNumbers => _contactNumbers.AsReadOnly();
        public DateOnly DateOfBirth { get; }
        public Role Role { get; private set; }
        public byte Age
        {
            get
            {
                byte age = (byte)(today.Year - DateOfBirth.Year);

                if (DateOfBirth > today.AddYears(-age))
                    age--;

                return age;
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public User(
            int userId, NationalId nationalId,string firstName, string middleName, string lastName,Phone phone, DateOnly dateOfBirth, Role role)
            : this(nationalId, firstName, middleName, lastName, phone, dateOfBirth, role)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(userId, nameof(userId));

            UserId = userId;
        }

        private User(
            NationalId nationalId,
            string firstName, string middleName, string lastName,
            Phone phone, DateOnly dateOfBirth, Role role)
        {
            ArgumentNullException.ThrowIfNull(nationalId, nameof(nationalId));

            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(middleName, nameof(middleName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            if (today.AddYears(-18) < dateOfBirth)
                throw new InvalidOperationException("The date of birth must not be earlier than 18.");

            if (!Enum.IsDefined(typeof(Role), role))
                throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(Role));

            NationalId = nationalId;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Phone = phone;
            DateOfBirth = dateOfBirth;
            Role = role;

            _contactNumbers.Add(phone);
        }

        public static User Create(
            NationalId nationalId,
            string firstName, string middleName, string lastName,
            Phone phone, DateOnly dateOfBirth, Role role)
        {
            return new User(nationalId, firstName, middleName, lastName, phone, dateOfBirth, role);
        }

        public void AddPhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            Phone? existing = _contactNumbers.FirstOrDefault(p => p.Equals(phone));

            if (_contactNumbers.Count < 3 && existing is null)
                _contactNumbers.Add(phone);
        }

        public void ChangePhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            if (!Phone.Equals(phone))
            {
                Phone = phone;
                if (_contactNumbers.Count > 0)
                    _contactNumbers[0] = phone;
                else
                    _contactNumbers.Add(phone);
            }
        }

        public void ChangeRole(Role role)
        {
            if (!Enum.IsDefined(typeof(Role), role))
                throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(Role));

            if (Role == role)
                return;

            Role = role;
        }
    }
}
