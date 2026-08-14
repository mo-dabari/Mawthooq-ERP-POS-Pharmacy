using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Entities;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Packaging;
using Xunit;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.Packaging
{
    public class BoxWithDirectUnitsPackagingTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_When_Is_Valid()
            {
                var sut = CreateBoxWithDirectUnitsPackaging(20, UnitOfMeasure.Sachet);

                sut.TotalUnitsPerBox.Should().Be(20);
                sut.DirectUnit.Should().Be(UnitOfMeasure.Sachet);
            }
            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_Exception_When_TotalUnitsPerBox_Is_Invalid(int totalUnitsPerBox)
            {
                Action sut = () => CreateBoxWithDirectUnitsPackaging(totalUnitsPerBox, UnitOfMeasure.Sachet);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_Throw_Exception_When_DirectUnit_Invalid()
            {
                Action sut = () => CreateBoxWithDirectUnitsPackaging(20, (UnitOfMeasure)999);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }
        }


        public class Behavior
        {
            [Fact]
            public void Should_Return_Correct_Unit_Hierarchy()
            {
                var sut = CreateBoxWithDirectUnitsPackaging(20, UnitOfMeasure.Sachet);

                var result = sut.GetUnitDefinitions();

                result.Should().BeEquivalentTo(
                    [
                         ProductUnitDefinition.Create(UnitOfMeasure.Box, 20, UnitRank.Primary),
                         ProductUnitDefinition.Create(UnitOfMeasure.Sachet, 1, UnitRank.Tertiary)
                    ]);
            }
        }
        private static BoxWithDirectUnitsPackaging CreateBoxWithDirectUnitsPackaging(int totalUnitsPerBox, UnitOfMeasure directUnit)
        {
            return BoxWithDirectUnitsPackaging.Create(totalUnitsPerBox, directUnit);
        }
    }
}
