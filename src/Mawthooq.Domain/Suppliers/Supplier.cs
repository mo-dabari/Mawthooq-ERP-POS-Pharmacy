using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Suppliers.ValueObjects;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Domain.Suppliers
{
    public class Supplier
    {
        public int SupplierId { get; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public Address Address { get; private set; }
        public PaymentTerms PaymentTerms { get; }
        public Money CreditLimit { get; private set; }
        public Money CurrentBalance { get; private set; }

        public bool IsActive { get; private set; }
        private List<Phone> _phones = new List<Phone>();
        public IReadOnlyList<Phone> Phones => _phones.AsReadOnly();
        public string FullName => $"{FirstName} {LastName}";


        public Supplier(int supplierId, string firstName, string lastName, Email email, Address address, PaymentTerms paymentTerms, Money creditLimit)
        : this(firstName, lastName, email, address, paymentTerms, creditLimit)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(supplierId, nameof(supplierId));
            SupplierId = supplierId;
        }

        private Supplier(string firstName, string lastName, Email email, Address address, PaymentTerms paymentTerms, Money creditLimit)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName, nameof(firstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName, nameof(lastName));

            ArgumentNullException.ThrowIfNull(email, nameof(email));
            ArgumentNullException.ThrowIfNull(address, nameof(address));
            ArgumentNullException.ThrowIfNull(paymentTerms, nameof(paymentTerms));
            ArgumentNullException.ThrowIfNull(creditLimit, nameof(creditLimit));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
            PaymentTerms = paymentTerms;
            CreditLimit = creditLimit;
            IsActive = true;
            CurrentBalance = Money.Zero(creditLimit.Currency);
        }

        public static Supplier Create(string firstName, string lastName, Email email, Address address, PaymentTerms paymentTerms, Money creditLimit)
        {
            return new Supplier(firstName, lastName, email, address, paymentTerms, creditLimit);
        }

        public void AddPhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            if (_phones.Any(p => p.Equals(phone)))
                throw new InvalidOperationException("Already Phone exist");

            _phones.Add(phone);
        }

        public void UpdateBalance(Money amount)
        {
            ArgumentNullException.ThrowIfNull(amount, nameof(amount));
            if (amount.Currency != CreditLimit.Currency)
                throw new ArgumentException($"Amount currency {amount.Currency} does not match supplier currency {CreditLimit.Currency}.");

            if ((CurrentBalance.Amount + amount.Amount) > CreditLimit.Amount)
                throw new InvalidOperationException("Balance update exceeds credit limit");

            CurrentBalance += amount;
        }

        public void DeductBalance(Money amount)
        {
            ArgumentNullException.ThrowIfNull(amount, nameof(amount));
            if (amount.Currency != CreditLimit.Currency)
                throw new ArgumentException($"Amount currency {amount.Currency} does not match supplier currency {CreditLimit.Currency}.");

            if (CurrentBalance.Amount < amount.Amount)
                throw new InvalidOperationException("Insufficient balance to deduct the specified amount.");

            CurrentBalance -= amount;
        }

        public void UpdateCreditLimit(Money newCreditLimit)
        {
            ArgumentNullException.ThrowIfNull(newCreditLimit, nameof(newCreditLimit));
            if (newCreditLimit.Currency != CreditLimit.Currency)
                throw new ArgumentException($"New credit limit currency {newCreditLimit.Currency} does not match supplier currency {CreditLimit.Currency}.");

            CreditLimit = newCreditLimit;
        }

        public void UpdateEmail(Email newEmail)
        {
            ArgumentNullException.ThrowIfNull(newEmail, nameof(newEmail));
            Email = newEmail;
        }

        public void UpdateAddress(Address newAddress)
        {
            ArgumentNullException.ThrowIfNull(newAddress, nameof(newAddress));
            Address = newAddress;
        }

        public void UpdateName(string newFirstName, string newLastName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(newFirstName, nameof(newFirstName));
            ArgumentException.ThrowIfNullOrWhiteSpace(newLastName, nameof(newLastName));

            FirstName = newFirstName;
            LastName = newLastName;
        }

        public void RemovePhone(Phone phone)
        {
            ArgumentNullException.ThrowIfNull(phone, nameof(phone));
            _phones.RemoveAll(p => p.Equals(phone));
        }

        public void ClearPhones()
        {
            _phones.Clear();
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
        public override string ToString()
        {
            return $"Supplier: {FullName}, Email: {Email}, Address: {Address}, Payment Terms: {PaymentTerms}, Credit Limit: {CreditLimit}, Current Balance: {CurrentBalance}";
        }
    }
}
