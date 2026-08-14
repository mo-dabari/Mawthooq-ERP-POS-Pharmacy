using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Xunit;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.Packaging
{
    public class SingleUnitPackagingTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_SingleUnitPackaging_When_Is_Valid()
            {
                var sut = CreateSingleUnitPackagingPackaging(UnitOfMeasure.Sachet);

                sut.SingleUnit.Should().Be(UnitOfMeasure.Sachet);
            }

            [Fact]
            public void Should_Throw_Exception_When_SingleUnit_Invalid()
            {
                Action sut = () => CreateSingleUnitPackagingPackaging((UnitOfMeasure)999);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }
        }


        public class GenerateUnitDetails
        {
            [Fact]
            public void Should_Return_Correct_Unit_Hierarchy()
            {
                var sut = CreateSingleUnitPackagingPackaging(UnitOfMeasure.Sachet);

                var result = sut.GetUnitDefinitions().ToList();

                result.Should().HaveCount(1);

                result.Should().BeEquivalentTo(
                    [
                         ProductUnitDefinition.Create(UnitOfMeasure.Sachet, 1, UnitRank.Primary)
                    ]);

            }
        }
        private static SingleUnitPackaging CreateSingleUnitPackagingPackaging(UnitOfMeasure directUnit)

        {
            return  SingleUnitPackaging.Create(directUnit);
        }
    }
}
