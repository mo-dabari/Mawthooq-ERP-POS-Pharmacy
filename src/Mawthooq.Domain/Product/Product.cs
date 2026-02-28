using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Product
{
    public class Product
    {
        public int ProductId { get; }
        public int CategoryId { get; }
        public string Name { get; private set; }
        public string SKU { get; }
        public bool IsActive { get; private set; }
        public string? Barcode { get; private set; }

        public Product(int productId, string name, string sku, int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, nameof(sku));

            ProductId = productId;
            CategoryId = categoryId;
            Name = name;
            SKU = sku;
            IsActive = true;
            Barcode = null;
        }

        public void ChangeProductName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            if (Name == name) return;
            Name = name;
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
