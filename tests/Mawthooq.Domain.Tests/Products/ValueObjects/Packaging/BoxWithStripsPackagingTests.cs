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
    public class BoxWithStripsPackagingTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_When_Is_Valid()
            {
                var sut = CreateBoxWithStripsPackaging(2, 20, UnitOfMeasure.Disc);

                sut.StripsPerBox.Should().Be(2);
                sut.TabletsPerStrip.Should().Be(20);
                sut.SmallUnit.Should().Be(UnitOfMeasure.Disc);
            }
            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_Exception_When_StripsPerBox_Is_Invalid(int stripsPerBox)
            {
                Action sut = () => CreateBoxWithStripsPackaging(stripsPerBox, 10, UnitOfMeasure.Disc);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_Exception_When_UnitsPerStrip_Is_Invalid(int unitsPerStrip)
            {
                Action sut = () => CreateBoxWithStripsPackaging(3, unitsPerStrip, UnitOfMeasure.Disc);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_Throw_Exception_When_SmallUnit_Invalid()
            {
                Action sut = () => CreateBoxWithStripsPackaging(2, 2, (UnitOfMeasure)999);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }
        }

        public class Behavior
        {
            [Fact]
            public void Should_Return_Correct_Unit_Hierarchy()
            {
                var sut = CreateBoxWithStripsPackaging(2, 20, UnitOfMeasure.Disc);

                var result = sut.GetUnitDefinitions().ToList();

                result.Should().BeEquivalentTo(
                    [
                         ProductUnitDefinition.Create(UnitOfMeasure.Box, 40, UnitRank.Primary),
                         ProductUnitDefinition.Create(UnitOfMeasure.Strip, 20, UnitRank.Secondary),
                         ProductUnitDefinition.Create(UnitOfMeasure.Disc, 1, UnitRank.Tertiary)
                    ]);
            }
        }
        private static BoxWithStripsPackaging CreateBoxWithStripsPackaging(int stripsPerBox, int unitsPerStrip, UnitOfMeasure smallUnit)
        {
            return BoxWithStripsPackaging.Create(stripsPerBox, unitsPerStrip, smallUnit);
        }
    }
}
