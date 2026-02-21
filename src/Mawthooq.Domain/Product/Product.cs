using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Product
{
    public class Product
    {
        public Guid ProductId { get; }
        public Guid CategoryId { get; }
        public string Name { get; private set; }
        public string SKU { get; }
        public bool IsActive { get; private set; }
        public string? Barcode { get; private set; }

        public Product(Guid productId, string name, string sku, Guid categoryId)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, nameof(sku));

            if (Guid.Empty == productId)
                throw new ArgumentException("require product Identity");

            if (Guid.Empty == categoryId)
                throw new ArgumentException("require Category Identity");

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
