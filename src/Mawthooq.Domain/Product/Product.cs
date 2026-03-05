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

        public Product(int productId, string name, string sku, int categoryId, bool isActive = true, string? barcode = null)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, nameof(sku));

            ProductId = productId;
            CategoryId = categoryId;
            Name = name;
            SKU = sku;
            IsActive = isActive;
            Barcode = barcode;
        }

        private Product(string name, string sku, int categoryId, bool isActive = true, string? barcode = null)
        {

            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, nameof(sku));

            Name = name;
            SKU = sku;
            CategoryId = categoryId;
            IsActive = isActive;
            Barcode = barcode;

        }

        public static Product Create(string name, string sku, int categoryId, bool isActive = true, string? barcode = null)
        {
            return new Product(name, sku, categoryId, isActive, barcode);
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

        public void SetBarcode(string? barcode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(barcode, nameof(barcode));
            if (Barcode == barcode) return;
            Barcode = barcode;
        }
    }
}
