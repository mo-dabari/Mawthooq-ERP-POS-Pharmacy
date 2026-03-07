using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.ValueObjects;

namespace Mawthooq.Domain.Customers
{
    public class Customer
    {
        public int CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public Phone Phone { get; private set; }
        private List<Phone> _contactNumbers = new();
        public bool IsActive { get; private set; }
        public IReadOnlyList<Phone> contactNumbers => _contactNumbers.AsReadOnly();
        public DateTime CreatedAt { get; }
        public Customer(int customerId, string firstName, string lastName, Phone phone)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));

            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            CustomerId = customerId;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;

            _contactNumbers.Add(phone);
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
        public void ClearContactNumbers(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));

            _contactNumbers.Clear();

            Phone = phone;

            _contactNumbers.Add(Phone);
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
    }
}
