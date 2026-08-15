using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Customers;

namespace Mawthooq.Application.Customers.UseCases.GetCustomerByID
{
    public class GetCustomerByIdUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        public GetCustomerByIdUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public GetCustomerByIdResponse Execute(int customerId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(customerId, nameof(customerId));

            Customer? customer = _customerRepository.GetById(customerId);

            if (customer is null)
                throw new InvalidOperationException("Customer not found");

            return new GetCustomerByIdResponse(
                customer.CustomerId,
                customer.FullName,
                customer.IsActive,
                customer.CreatedAt);
        }
    }
}
