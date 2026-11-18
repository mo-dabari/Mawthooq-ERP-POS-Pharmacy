using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mawthooq.Domain.Products.Enums;
using Mawthooq.Domain.Products.ValueObjects.Packaging;

namespace Mawthooq.Domain.Tests.Products.ValueObjects.Packaging
{
    public class ProductPackagingFactoryTests
    {
        private static ProductPackaging Create(
            PackagingTemplate template,
            int? total = null,
            UnitOfMeasure? direct = null,
            int? strips = null,
            int? perStrip = null,
            int? inner = null,
            int? pieces = null,
            UnitOfMeasure? small = null)
        {
            return ProductPackagingFactory.Create(template, total, direct, strips, perStrip, inner, pieces, small);
        }

        private static IEnumerable<(UnitOfMeasure Unit, int Rate, UnitRank Rank)> Definitions(ProductPackaging packaging)
        {
            return packaging.GetUnitDefinitions().Select(d => (d.Unit, d.ConversionRate, d.Rank));
        }

        [Fact]
        public void Should_BuildASingleUnit()
        {
            ProductPackaging packaging = Create(PackagingTemplate.SingleUnit, small: UnitOfMeasure.Bottle);

            packaging.Template.Should().Be(PackagingTemplate.SingleUnit);

            Definitions(packaging).Should().BeEquivalentTo(
                [
                    (UnitOfMeasure.Bottle, 1, UnitRank.Primary)
                ]);
        }

        [Fact]
        public void Should_BuildABoxWithDirectUnits()
        {
            ProductPackaging packaging = Create(PackagingTemplate.BoxWithDirectUnits, total: 10, direct: UnitOfMeasure.Ampoule);

            packaging.Template.Should().Be(PackagingTemplate.BoxWithDirectUnits);

            Definitions(packaging).Should().BeEquivalentTo(
                [
                    (UnitOfMeasure.Box, 10, UnitRank.Primary),
                    (UnitOfMeasure.Ampoule, 1, UnitRank.Tertiary)
                ]);
        }

        [Fact]
        public void Should_BuildABoxWithStrips()
        {
            ProductPackaging packaging = Create(
                PackagingTemplate.BoxWithStripsAndUnits,
                strips: 3,
                perStrip: 10,
                small: UnitOfMeasure.Disc);

            packaging.Template.Should().Be(PackagingTemplate.BoxWithStripsAndUnits);

            Definitions(packaging).Should().BeEquivalentTo(
            [
                (UnitOfMeasure.Box, 30, UnitRank.Primary),
                (UnitOfMeasure.Strip, 10, UnitRank.Secondary),
                (UnitOfMeasure.Disc, 1, UnitRank.Tertiary)
            ]);
        }

        [Fact]
        public void Should_BuildAnOuterBox()
        {
            ProductPackaging packaging = Create(
                PackagingTemplate.OuterBoxWithInnerBoxesAndPieces,
                inner: 4,
                pieces: 5,
                small: UnitOfMeasure.Piece);

            packaging.Template.Should().Be(PackagingTemplate.OuterBoxWithInnerBoxesAndPieces);

            Definitions(packaging).Should().BeEquivalentTo(
            [
                (UnitOfMeasure.OuterBox, 20, UnitRank.Primary),
                (UnitOfMeasure.Box, 5, UnitRank.Secondary),
                (UnitOfMeasure.Piece, 1, UnitRank.Tertiary)
            ]);
        }

        [Theory]
        [InlineData(PackagingTemplate.SingleUnit)]
        [InlineData(PackagingTemplate.BoxWithDirectUnits)]
        [InlineData(PackagingTemplate.BoxWithStripsAndUnits)]
        [InlineData(PackagingTemplate.OuterBoxWithInnerBoxesAndPieces)]
        public void Should_ThrowArgumentException_When_ARequiredArgumentIsMissing(PackagingTemplate template)
        {
            var act = () => Create(template);

            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Throw_When_TheTemplateIsNotSupported()
        {
            var act = () => Create((PackagingTemplate)99);

            act.Should().Throw<NotSupportedException>();
        }
    }
}
