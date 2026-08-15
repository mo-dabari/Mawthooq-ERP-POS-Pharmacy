using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Application.Categories.Commands.Mappers;
using Mawthooq.Application.Medicines.Commands.CreateMedicine.Mappers;
using Mawthooq.Domain.Categories;
using Mawthooq.Domain.Medicines;
using Mawthooq.Domain.Products;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Barcodes;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

// namespace Mawthooq.Application.Medicines.Commands.CreateMedicine
// {
//     public class CreateMedicineUseCase
//     {
//         private readonly ICategoryRepository _categoryRepository;
//         private readonly IProductRepository _productRepository;
//         private readonly IMedicineRepository _medicineRepository;

//         public CreateMedicineUseCase(ICategoryRepository categoryRepository, IProductRepository productRepository, IMedicineRepository medicineRepository, IUnitOfWork unitOfWork)
//         {
//             ArgumentNullException.ThrowIfNull(categoryRepository);
//             ArgumentNullException.ThrowIfNull(productRepository);
//             ArgumentNullException.ThrowIfNull(medicineRepository);
//             ArgumentNullException.ThrowIfNull(unitOfWork);

//             _categoryRepository = categoryRepository;
//             _productRepository = productRepository;
//             _medicineRepository = medicineRepository;
//         }

//         public CreateMedicineResponse Execute(CreateMedicineRequest request)
//         {
//             ArgumentNullException.ThrowIfNull(request);

//             Category? category = _categoryRepository.GetById(request.ProductRequest.CategoryId.ToDomain());

//             if (category == null)
//                 throw new InvalidOperationException($"Category with id {request.ProductRequest.CategoryId} not found");

//             SKU sKU = new SKU(request.ProductRequest.SKU);

//             if (_productRepository.ExistsBySku(sKU))
//                 throw new InvalidOperationException("SKU already exists.");


//             if (request.ProductRequest.Barcode is not null)
//             {
//                 var barcode =  Barcode(request.ProductRequest.Barcode.Value, request.ProductRequest.Barcode.BarcodeType);

//                 if (_productRepository.ExistsByBarcode(barcode))
//                     throw new InvalidOperationException("Barcode already exists.");
//             }

//             Product product = Product.Create
//                     (
//                         request.ProductRequest.Name,
//                         sKU,
//                         request.ProductRequest.CategoryId,
//                         ProductPackagingFactory.Create
//                         (
//                             request.PackagingRequest.Template,
//                             request.PackagingRequest.TotalUnitsPerBox,

//                             (request.PackagingRequest.DirectUnit is not null)
//                                 ? request.PackagingRequest.DirectUnit
//                                 : null,
//                             request.PackagingRequest.StripsPerBox,
//                             request.PackagingRequest.UnitsPerStrip,
//                             request.PackagingRequest.InnerBoxesPerOuterBox,
//                             request.PackagingRequest.PiecesPerInnerBox,
//                             (request.PackagingRequest.SmallUnit is not null)
//                                 ? request.PackagingRequest.SmallUnit
//                                 : null
//                         ),
//                         request.ProductRequest.BaseUnit
//                     );

//             try
//             {
//                 _unitOfWork.Begin();

//                 Product? savedProduct = _productRepository.Add(product);

//                 if (savedProduct is null)
//                     throw new InvalidOperationException("Failed to create product");

//                 Medicine? saveMedicine = _medicineRepository.Add(request.ToDomain(product.ProductId));

//                 if (saveMedicine is null)
//                     throw new InvalidOperationException("Failed to create Medicine");

//                 _unitOfWork.Commit();

//                 return saveMedicine.ToDto(product);
//             }

//             catch (Exception)
//             {
//                 _unitOfWork.Rollback();
//                 throw;
//             }
//         }
//     }
// }
