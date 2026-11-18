using FluentAssertions;
using Mawthooq.Domain.SharedKernel.Abstractions;
using Mawthooq.Domain.SharedKernel.Primitives;
using Xunit;

namespace Mawthooq.Domain.Tests.SharedKernel.Primitives;

public class AggregateRootTests
{
    private sealed record TestEvent(DateTime OccurredOn) : DomainEvent(OccurredOn);

    private sealed class TestAggregate : AggregateRoot
    {
        public void Raise(IDomainEvent domainEvent) => RaiseDomainEvent(domainEvent);
    }

    private static TestEvent NewEvent() => new(DateTime.UtcNow);

    [Fact]
    public void Should_StartWithoutEvents()
    {
        new TestAggregate().DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Should_KeepTheRaisedEventsInOrder()
    {
        var aggregate = new TestAggregate();
        TestEvent first = NewEvent(), second = NewEvent();

        aggregate.Raise(first);
        aggregate.Raise(second);

        aggregate.DomainEvents.Should().Equal(first, second);
    }

    [Fact]
    public void Should_Throw_When_TheEventIsNull()
    {
        var aggregate = new TestAggregate();

        FluentActions.Invoking(() => aggregate.Raise(null!)).Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Should_RemoveEverything_When_EventsAreCleared()
    {
        var aggregate = new TestAggregate();
        aggregate.Raise(NewEvent());

        aggregate.ClearDomainEvents();

        aggregate.DomainEvents.Should().BeEmpty();
    }

    [Fact]
    public void Should_NotExposeAModifiableList()
    {
        var aggregate = new TestAggregate();

        aggregate.DomainEvents.Should().NotBeAssignableTo<List<IDomainEvent>>();
        (aggregate.DomainEvents as IList<IDomainEvent>)?.IsReadOnly.Should().BeTrue();
    }
}
