using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Customers;

namespace Mawthooq.Application.Customers.UseCases.CreateCustomer
{
    public class CreateCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        public CreateCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public CreateCustomerResponse Execute(CreateCustomerRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));
            
            Customer customer = Customer.Create(
                request.FirstName,
                request.LastName,
                request.PhoneNumber.ToDomain());

            Customer? saveCustomer = _customerRepository.Add(customer);

            if (saveCustomer is null)
                throw new InvalidOperationException("Failed to create customer");

            return new CreateCustomerResponse(
                saveCustomer.CustomerId,
                saveCustomer.FullName,
                saveCustomer.Phone.ToDto(),
                saveCustomer.IsActive);
        }
    }
}
