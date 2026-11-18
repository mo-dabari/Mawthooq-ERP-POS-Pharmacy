using FluentAssertions;
using Mawthooq.Domain.Categories.ValueObjects;
using Mawthooq.Domain.LocalAgents.ValueObjects;
using Mawthooq.Domain.Manufacturers.ValueObjects;
using Mawthooq.Domain.Products.ValueObjects;
using Mawthooq.Domain.SharedKernel.Primitives;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.Primitives;

public class TypedIdTests
{
    public static IEnumerable<object[]> Ids() =>
    [
        ["ProductId",          (Func<Guid, DomainObjectId>)(g => ProductId.From(g)),          (Func<DomainObjectId>)(() => ProductId.New())],
        ["PackagingVersionId", (Func<Guid, DomainObjectId>)(g => PackagingVersionId.From(g)), (Func<DomainObjectId>)(() => PackagingVersionId.New())],
        ["CategoryId",         (Func<Guid, DomainObjectId>)(g => CategoryId.From(g)),         (Func<DomainObjectId>)(() => CategoryId.New())],
        ["ManufacturerId",     (Func<Guid, DomainObjectId>)(g => ManufacturerId.From(g)),     (Func<DomainObjectId>)(() => ManufacturerId.New())],
        ["LocalAgentId",       (Func<Guid, DomainObjectId>)(g => LocalAgentId.From(g)),       (Func<DomainObjectId>)(() => LocalAgentId.New())],
    ];

    [Theory]
    [MemberData(nameof(Ids))]
    public void Should_Throw_When_TheGuidIsEmpty(string name, Func<Guid, DomainObjectId> from, Func<DomainObjectId> create)
    {
        _ = create;
        FluentActions.Invoking(() => from(Guid.Empty)).Should().Throw<ArgumentException>(name);
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Should_KeepTheGuid_When_CreatedFromAGuid(string name, Func<Guid, DomainObjectId> from, Func<DomainObjectId> create)
    {
        _ = create;
        Guid guid = Guid.NewGuid();

        from(guid).Value.Should().Be(guid, name);
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Should_CreateDifferentNonEmptyIds_When_NewIsCalledTwice(string name, Func<Guid, DomainObjectId> from, Func<DomainObjectId> create)
    {
        _ = from;
        DomainObjectId first = create(), second = create();

        first.Value.Should().NotBeEmpty(name);
        first.Should().NotBe(second, name);
    }

    [Theory]
    [MemberData(nameof(Ids))]
    public void Should_BeEqual_When_TheTypeAndTheGuidMatch(string name, Func<Guid, DomainObjectId> from, Func<DomainObjectId> create)
    {
        _ = create;
        Guid guid = Guid.NewGuid();

        from(guid).Should().Be(from(guid), name);
        from(guid).GetHashCode().Should().Be(from(guid).GetHashCode(), name);
    }

    [Fact]
    public void Should_NotBeEqual_When_TheGuidIsTheSameButTheTypesDiffer()
    {
        Guid guid = Guid.NewGuid();
        object productId = ProductId.From(guid);
        object categoryId = CategoryId.From(guid);

        productId.Equals(categoryId).Should().BeFalse();
    }
}
