using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mawthooq.Domain.Product
{
    public class Product
    {
        public int ProductId { get; }
        public int CategoryId { get; private set; }
        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public string SKU { get; private set; }
        public bool IsActive { get; private set; }
        public string Barcode { get; private set; }

        public Product(int productId, string name, string sku, string barcode, string imageUrl, int categoryId, bool isActive = true)
        : this(name, sku, barcode, imageUrl, categoryId, isActive)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(productId, nameof(productId));

            ValidateArguments(name, sku, imageUrl, barcode, categoryId);

            ProductId = productId;
        }

        private Product(string name, string sku, string barcode, string imageUrl, int categoryId, bool isActive = true)
        {
            ValidateArguments(name, sku, imageUrl, barcode, categoryId);

            Name = name;
            SKU = sku;
            Barcode = barcode;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
            IsActive = isActive;

        }

        public static Product Create(string name, string sku, string barcode, string imageUrl, int categoryId, bool isActive = true)
        {
            return new Product(name, sku, barcode, imageUrl, categoryId, isActive);
        }

        public void ChangeProductName(string name)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            if (Name == name) return;
            Name = name;
        }

        public void ChangeSKU(string sKU)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(sKU, nameof(sKU));
            if (SKU == sKU) return;
            SKU = sKU;
        }

        public void SetBarcode(string barcode)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(barcode, nameof(barcode));
            if (Barcode == barcode) return;
            Barcode = barcode;
        }

        public void ChangeImageUrl(string imageUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl, nameof(imageUrl));
            if (ImageUrl == imageUrl) return;
            ImageUrl = imageUrl;
        }

        public void ChangeCategory(int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));
            if (CategoryId == categoryId) return;
            CategoryId = categoryId;
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
        private void ValidateArguments(string name, string sku, string imageUrl, string barcode, int categoryId)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(categoryId, nameof(categoryId));

            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(imageUrl, nameof(imageUrl));
            ArgumentException.ThrowIfNullOrWhiteSpace(sku, nameof(sku));
            ArgumentException.ThrowIfNullOrWhiteSpace(barcode, nameof(barcode));
        }
    }
}
