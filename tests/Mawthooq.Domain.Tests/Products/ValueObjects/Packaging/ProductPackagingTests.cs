using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Xunit;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.Packaging
{
    public class ProductPackagingTests
    {
        private sealed record TestProductPackaging : ProductPackaging
        {
            public TestProductPackaging(PackagingTemplate template)
            : base(template)
            {

            }
            internal override IEnumerable<ProductUnitDefinition> GetUnitDefinitions()
            {
                return new List<ProductUnitDefinition>();
            }
        }

        public class Constructor
        {
            [Fact]
            public void Should_Create_TestProductPackaging_When_Is_Valid()
            {
                var sut = CreateTestProductPackaging(PackagingTemplate.BoxWithStripsAndUnits);

                sut.Template.Should().Be(PackagingTemplate.BoxWithStripsAndUnits);
            }

            [Fact]
            public void Should_Throw_Exception_When_PackagingTemplate_Is_Invalid()
            {
                Action sut = () => CreateTestProductPackaging((PackagingTemplate)999);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }
        }

        private static TestProductPackaging CreateTestProductPackaging(PackagingTemplate template)
        {
            return new TestProductPackaging(template);
        }
    }
}
