# Customer Events

## Purpose

This document defines the business events produced by the Customer module.

Customer events represent completed changes to customer information.

---

# Event Ownership

The Customer aggregate produces all Customer domain events.

Customer events represent business facts that occurred within the Customer module.

---

# Customer Events

Examples:

- Customer Created
- Customer Information Updated
- Customer Contact Information Updated
- Customer Activated
- Customer Deactivated

---

# Cross-Module Coordination

Customer events may be consumed by other modules when customer information is relevant.

Customer events never modify other modules directly.

Business workflows involving multiple modules are coordinated by the Application layer.

---

# Persistence

Customer domain events are translated by the Infrastructure layer into persistence operations.

The Customer aggregate never performs persistence directly.

---

# Historical Integrity

Customer events preserve the history of changes made to customer information.

They never modify historical business documents.

---

# Related Documentation

- aggregate.md
- commands.md
- queries.md
