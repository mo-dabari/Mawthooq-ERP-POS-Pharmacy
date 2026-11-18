using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.Packaging
{
    public class ProductUnitDefinitionTests
    {
        [Fact]
        public void Should_KeepTheParts_When_Created()
        {
            var definition = ProductUnitDefinition.Create(UnitOfMeasure.Box, 120, UnitRank.Primary);

            definition.Unit.Should().Be(UnitOfMeasure.Box);
            definition.ConversionRate.Should().Be(120);
            definition.Rank.Should().Be(UnitRank.Primary);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Should_Throw_When_TheConversionRateIsNotPositive(int rate)
        {
            var act = () => ProductUnitDefinition.Create(UnitOfMeasure.Box, rate, UnitRank.Primary);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Should_Throw_When_TheUnitTheRankIsUndefined()
        {
            var act = () => ProductUnitDefinition.Create(UnitOfMeasure.Box, 1, (UnitRank)99);

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_TheUnitOfMeasureIsUndefined()
        {
            var act = () => ProductUnitDefinition.Create((UnitOfMeasure)999, 1, UnitRank.Primary);

            act.Should().Throw<InvalidEnumArgumentException>();
        }

        [Fact]
        public void Should_BeEqual_When_AllThePartsMatch()
        {
            var sut = ProductUnitDefinition.Create(UnitOfMeasure.Box, 12, UnitRank.Primary);
            
            sut.Should().Be(ProductUnitDefinition.Create(UnitOfMeasure.Box, 12, UnitRank.Primary));
        }
    }
}
