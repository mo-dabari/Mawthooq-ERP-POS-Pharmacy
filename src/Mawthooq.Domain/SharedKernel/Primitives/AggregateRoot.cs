using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mawthooq.Domain.SharedKernel.Abstractions;

namespace Mawthooq.Domain.SharedKernel.Primitives
{
    public class AggregateRoot
    {
        private readonly List<IDomainEvent> _domainEvents = new();

        public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

        protected void RaiseDomainEvent(IDomainEvent domainEvent)
        {
            ArgumentNullException.ThrowIfNull(domainEvent, nameof(domainEvent));
            _domainEvents.Add(domainEvent);
        }

        public void ClearDomainEvents()
        {
            if (_domainEvents.Any())
                _domainEvents.Clear();
        }
    }
}
