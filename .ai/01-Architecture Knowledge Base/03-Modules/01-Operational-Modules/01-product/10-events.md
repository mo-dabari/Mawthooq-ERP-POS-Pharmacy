# Product Events

## Purpose

This document describes the business events produced by the Product aggregate.

Events represent completed business actions.

They describe what happened from the business perspective.

---

# Event Philosophy

Events are business facts.

Once raised, an event represents something that has already occurred.

Events describe business outcomes rather than technical operations.

---

# Event Categories

The Product aggregate produces events related to:

- Product creation
- Commercial information updates
- Packaging evolution
- Selling unit changes
- Barcode management
- Lifecycle changes

Each event represents a completed business action.

---

# Persistence

Product events are translated by the Infrastructure layer into the SQL operations required to persist the Product.

The Product aggregate never generates SQL.

The Product aggregate only records business events.

---

# Aggregate Responsibility

Only the Product aggregate produces Product business events.

Other modules never generate Product events on its behalf.

---

# Related Documentation

- aggregate.md
- evolution.md
- decision-rules.md
