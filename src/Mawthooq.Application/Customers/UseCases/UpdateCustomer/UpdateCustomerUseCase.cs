using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Domain.Customers;

namespace Mawthooq.Application.Customers.UseCases.UpdateCustomer
{
    public class UpdateCustomerUseCase
    {
        private readonly ICustomerRepository _customerRepository;
        public UpdateCustomerUseCase(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public UpdateCustomerResponse Execute(UpdateCustomerRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Customer? customer = _customerRepository.GetById(request.CustomerId);

            if (customer is null)
                throw new InvalidOperationException("Customer not found");

            customer.ChangePhone(request.PhoneNumber.ToDomain());

            if (request.IsActive)
                customer.Activate();
            else
                customer.Deactivate();


            _customerRepository.Update(customer);

            return new UpdateCustomerResponse(
                customer.CustomerId,
                customer.FullName,
                customer.Phone.ToDto(),
                customer.IsActive);
        }
    }
}
