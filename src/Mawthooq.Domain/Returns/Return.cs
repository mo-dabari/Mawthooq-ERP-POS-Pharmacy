using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Returns.Entity;

namespace Mawthooq.Domain.Returns
{
    public class Return
    {
        public int ReturnId { get; }
        public int SaleId { get; }
        public DateTime ReturnDate { get; }
        private List<Refund> _refunds = new List<Refund>();
        public IReadOnlyList<Refund> Refunds => _refunds.AsReadOnly();
        private List<ReturnLine> _returnedProducts = new List<ReturnLine>();
        public IReadOnlyList<ReturnLine> ReturnedProducts => _returnedProducts.AsReadOnly();

        public Return(int returnId, int saleId) :this(saleId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(returnId, nameof(returnId));
            ReturnId = returnId;
        }

        private Return(int saleId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(saleId, nameof(saleId));

            SaleId = saleId;
            ReturnDate = DateTime.UtcNow;
        }

        public static Return Create(int saleId)
        {
            return new Return(saleId);
        }
        public void AddReturnedProduct(ReturnLine returnedProduct)
        {
            ArgumentNullException.ThrowIfNull(returnedProduct, nameof(returnedProduct));

            ReturnLine? existingProduct = _returnedProducts.FirstOrDefault(rp => rp.ProductId == returnedProduct.ProductId);

            if (existingProduct is not null)
            {
                existingProduct.IncreaseQuantity(returnedProduct.Quantity);
                return;
            }
            _returnedProducts.Add(returnedProduct);
        }
        public void AddRefund(Refund? refund)
        {
            ArgumentNullException.ThrowIfNull(refund, nameof(refund));
            Refund? existingRefund = _refunds.FirstOrDefault(r => r.PaymentMethod == refund.PaymentMethod);

            if (existingRefund is not null)
            {
                existingRefund.IncreaseAmount(refund.Amount);
                return;
            }
            _refunds.Add(refund);
        }
    }
}
