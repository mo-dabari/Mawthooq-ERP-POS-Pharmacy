using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.Returns.Enums;

namespace Mawthooq.Domain.Returns.Entity
{
    public class ReturnLine
    {
        public int ProductId { get; }
        public byte Quantity { get; private set; }
        public ReturnReason Reason { get; }
        public ProductReturnStatus Status { get; }
        public DateOnly ExpiryDate { get; }

        public ReturnLine(int productId, byte quantity, ReturnReason reason, ProductReturnStatus status, DateOnly expiryDate)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));

            if (!Enum.IsDefined(typeof(ReturnReason), reason))
                throw new InvalidEnumArgumentException(nameof(reason), (int)reason, typeof(ReturnReason));

            if (!Enum.IsDefined(typeof(ProductReturnStatus), status))
                throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(ProductReturnStatus));

            if (expiryDate == default)
                throw new ArgumentException("Expiry date cannot be the default value.", nameof(expiryDate));

            ProductId = productId;
            Quantity = quantity;
            Reason = reason;
            Status = status;
            ExpiryDate = expiryDate;
        }

        public void IncreaseQuantity(byte quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            Quantity += quantity;
        }

        public void DecreaseQuantity(byte quantity)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity, nameof(quantity));
            if (quantity > Quantity)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Cannot decrease quantity below zero.");

            Quantity -= quantity;
        }
    }
}
