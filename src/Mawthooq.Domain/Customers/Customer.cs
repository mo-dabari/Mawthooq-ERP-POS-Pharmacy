using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Customers
{
    public class Customer
    {
        public int CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        private List<Phone> _contactNumbers = new();
        public bool IsActive { get; private set; }
        public IReadOnlyList<Phone> contactNumbers => _contactNumbers.AsReadOnly();
        public string FullName => $"{FirstName} {LastName}";
        public DateTime CreatedAt { get; private set; }
        public Customer(int customerId, string firstName, string lastName)
        : this(firstName, lastName)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));

            CustomerId = customerId;
        }

        private Customer(string firstName, string lastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));


            FirstName = firstName;
            LastName = lastName;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static Customer Create(string firstName, string lastName)
        {
            return new Customer(firstName, lastName);
        }
        public void AddPhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            Phone? existing = _contactNumbers.FirstOrDefault(p => p.Equals(phone));

            if (_contactNumbers.Count < 3 && existing is null)
                _contactNumbers.Add(phone);
        }

        public void SetPhones(List<Phone> phones)
        {
            ArgumentNullException.ThrowIfNull(phones, nameof(phones));

            if (phones.Count > 3)
                throw new InvalidOperationException("A user cannot have more than 3 contact numbers.");

            _contactNumbers = phones;
        }

        public void ChangePhone(Phone oldphone, Phone newphone)
        {
            ArgumentNullException.ThrowIfNull(oldphone, nameof(oldphone));
            ArgumentNullException.ThrowIfNull(newphone, nameof(newphone));

            for (int i = 0; i < _contactNumbers.Count; i++)
            {
                if (_contactNumbers[i].Equals(oldphone))
                {
                    _contactNumbers[i] = newphone;
                    return;
                }
            }
        }

        public void RemovePhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            _contactNumbers.Remove(phone);
        }
        public void Activate()
        {
            if (IsActive)
                return;
            IsActive = true;
        }
        public void Deactivate()
        {
            if (!IsActive)
                return;
            IsActive = false;
        }

        public void SetCreatedAt(DateTime createdAt)
        {
            //check if default value
            if (createdAt == default)
                throw new ArgumentException("CreatedAt cannot be the default value.", nameof(createdAt));

            //check if createdAt is in the past
            if (createdAt > DateTime.UtcNow)
                throw new ArgumentException("CreatedAt cannot be in the future.", nameof(createdAt));

            CreatedAt = createdAt;
        }
    }
}
