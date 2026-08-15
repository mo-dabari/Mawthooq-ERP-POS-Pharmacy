using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Common.DTOs.Mappers;
using Mawthooq.Application.Returns.DTOs;
using Mawthooq.Domain.Inventories;
using Mawthooq.Domain.Inventories.ValueObjects;
using Mawthooq.Domain.Payments;
using Mawthooq.Domain.Payments.Entity;
using Mawthooq.Domain.Returns;
using Mawthooq.Domain.Returns.Entity;
using Mawthooq.Domain.Returns.Enums;
using Mawthooq.Domain.Sales;
using Mawthooq.Domain.Sales.Entity;
using Mawthooq.Domain.Sales.Enums;
using Mawthooq.Domain.SharedKernel.ValueObjects;

namespace Mawthooq.Application.Returns.UseCase
{
    public class CreateReturnUseCase
    {
        private readonly IReturnRepository _returnRepository;
        private readonly ISaleRepository _saleRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IPaymentRepository _paymentRepository;
        public CreateReturnUseCase(
            IReturnRepository returnRepository,
            ISaleRepository saleRepository,
            IInventoryRepository inventoryRepository,
            IPaymentRepository paymentRepository
            )
        {
            _returnRepository = returnRepository;
            _saleRepository = saleRepository;
            _inventoryRepository = inventoryRepository;
            _paymentRepository = paymentRepository;
        }


        public CreateReturnResponse Execute(CreateReturnRequest request)
        {
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            Sale sale = ValidateSale(request.SaleId, request.BranchId);

            Payment? payment = GetPaymentOrThrow(sale.SaleId);

            Return createdReturn = Return.Create(sale.SaleId);

            Money refundTotal = Money.Zero(sale.Currency);
            foreach (ReturnedProductDto returnedProduct in request.ReturnedProducts)
            {
                SaleLine saleLine = ValidateReturnedProduct(returnedProduct, sale);

                Inventory? inventory = GetInventoryOrThrow(returnedProduct.ProductId, sale.BranchId);

                ReturnLine returnLine = new ReturnLine(
                    returnedProduct.ProductId,
                    returnedProduct.Quantity,
                    returnedProduct.Reason,
                    returnedProduct.Status,
                    returnedProduct.ExpiryDate);

                createdReturn.AddReturnedProduct(returnLine);

                // inventory.ReceiveStock(returnedProduct.Quantity, inventory.Batches.Last().PurchaseCost, DateTime.UtcNow, returnedProduct.ExpiryDate);

                _inventoryRepository.Update(inventory);

                refundTotal += new Money(saleLine.SellingPrice.Amount * returnedProduct.Quantity, sale.Currency);
            }

            foreach (PaymentLine paymentLine in payment.PaymentLines)
            {
                Money refundMoney = CalculateRefundTotal(payment, paymentLine, refundTotal, sale);

                CreateRefunds(createdReturn, refundMoney, paymentLine);
            }

            Return savedReturn = _returnRepository.AddReturn(createdReturn);

            return MapToResponse(savedReturn, sale);
        }
        private Sale ValidateSale(int saleId, int branchId)
        {
            Sale? sale = _saleRepository.GetById(saleId);
            if (sale == null)
                throw new InvalidOperationException("Sale not found");

            if (sale.Status != SaleStatus.Confirmed)
                throw new InvalidOperationException("Cannot return items for a non-confirmed sale.");

            if (sale.BranchId != branchId)
                throw new InvalidOperationException("Sale does not belong to the specified branch");

            return sale;
        }
        private SaleLine ValidateReturnedProduct(ReturnedProductDto product, Sale sale)
        {
            SaleLine? saleLine = sale.Lines.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (saleLine == null)
                throw new InvalidOperationException($"Product with ID {product.ProductId} was not part of the original sale");

            if (product.Quantity <= 0)
                throw new InvalidOperationException("Returned quantity must be greater than zero");

            if (product.Quantity > saleLine.Quantity)
                throw new InvalidOperationException($"Returned quantity for product ID {product.ProductId} cannot exceed the quantity sold");

            if (product.Status != ProductReturnStatus.Defective &&
                product.Reason != ReturnReason.ExpiredProduct)
                throw new InvalidOperationException($"Invalid return status for product ID {product.ProductId}");

            return saleLine;
        }
        private Payment GetPaymentOrThrow(int saleId)
        {
            Payment? payment = _paymentRepository.GetBySaleId(saleId);
            if (payment == null)
                throw new InvalidOperationException("Payment not found for the original sale");

            return payment;
        }
        private Inventory GetInventoryOrThrow(int productId, int branchId)
        {
            Inventory? inventory = _inventoryRepository.GetByInventoryKey(new InventoryKey(branchId, productId));
            if (inventory == null)
                throw new InvalidOperationException($"Inventory not found for product ID {productId} in branch ID {branchId}");
            return inventory;
        }
        private Money CalculateRefundTotal(Payment payment, PaymentLine paymentLine, Money refundTotal, Sale sale)
        {
            decimal ratio = paymentLine.Amount.Amount / payment.TotalAmount.Amount;
            decimal refundAmount = Math.Round(refundTotal.Amount * ratio, 2);
            Money refundMoney = new Money(refundAmount, sale.Currency);
            return refundMoney;
        }
        private void CreateRefunds(Return createdReturn, Money refundTotal, PaymentLine paymentLine)
        {
            Refund createRefund = Refund.Create(refundTotal, paymentLine.PaymentMethod);
            createdReturn.AddRefund(createRefund);
        }
        private CreateReturnResponse MapToResponse(Return savedReturn, Sale sale)
        {
            return new CreateReturnResponse(
                  savedReturn.ReturnId,
                  savedReturn.SaleId,
                  savedReturn.ReturnDate,
                  sale.BranchId,
                  savedReturn.ReturnedProducts.Select(rp => new ReturnedProductDto(
                      rp.ProductId,
                      rp.Quantity,
                      rp.Reason,
                      rp.Status,
                      rp.ExpiryDate)).ToImmutableList(),
                  savedReturn.Refunds.Select(r => new RefundDto(
                    r.RefundId,
                    r.Amount.ToDto(),
                    r.Status,
                    r.PaymentMethod
                )).ToImmutableList()
            );
        }

    }
}
