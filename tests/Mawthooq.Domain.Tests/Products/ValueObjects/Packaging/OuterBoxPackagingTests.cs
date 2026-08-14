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
    public class OuterBoxPackagingTests
    {
        public class Constructor
        {
            [Fact]
            public void Should_Create_OuterBoxPackaging_When_Is_Valid()
            {
                var sut = CreateOuterBoxPackaging(10, 5, UnitOfMeasure.Piece);

                sut.InnerBoxesPerOuterBox.Should().Be(10);
                sut.PiecesPerInnerBox.Should().Be(5);
                sut.SmallUnit.Should().Be(UnitOfMeasure.Piece);
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_Exception_When_InnerBoxesPerOuterBox_Is_Invalid(int innerBoxesPerOuterBox)
            {
                Action sut = () => CreateOuterBoxPackaging(innerBoxesPerOuterBox, 10, UnitOfMeasure.Piece);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Theory]
            [InlineData(-1)]
            [InlineData(0)]
            public void Should_Throw_Exception_When_PiecesPerInnerBox_Is_Invalid(int piecesPerInnerBox)
            {
                Action sut = () => CreateOuterBoxPackaging(5, piecesPerInnerBox, UnitOfMeasure.Piece);

                sut.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Fact]
            public void Should_Throw_Exception_When_SmallUnit_Invalid()
            {
                Action sut = () => CreateOuterBoxPackaging(10, 5, (UnitOfMeasure)999);

                sut.Should().Throw<InvalidEnumArgumentException>();
            }
        }

        public class Behavior
        {
            [Fact]
            public void Should_Return_Correct_Unit_Hierarchy()
            {
                var sut = CreateOuterBoxPackaging(10, 5, UnitOfMeasure.Piece);

                var result = sut.GetUnitDefinitions().ToList();

                result.Should().BeEquivalentTo(
                    [
                         ProductUnitDefinition.Create(UnitOfMeasure.OuterBox, 50, UnitRank.Primary),
                         ProductUnitDefinition.Create(UnitOfMeasure.Box, 5, UnitRank.Secondary),
                         ProductUnitDefinition.Create(UnitOfMeasure.Piece, 1, UnitRank.Tertiary)
                    ]);
            }
        }
        private static OuterBoxPackaging CreateOuterBoxPackaging(int innerBoxesPerOuterBox, int piecesPerInnerBox, UnitOfMeasure smallUnit)
        {
            return new OuterBoxPackaging(innerBoxesPerOuterBox, piecesPerInnerBox, smallUnit);
        }
    }
}
