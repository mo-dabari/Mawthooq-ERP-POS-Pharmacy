# Supplier Events

## Purpose

This document defines the business events produced by the Supplier module.

Supplier events represent completed changes to supplier information.

---

# Event Ownership

The Supplier aggregate produces all Supplier domain events.

Supplier events represent business facts that occurred within the Supplier module.

---

# Supplier Events

Examples:

- Supplier Created
- Supplier Information Updated
- Supplier Contact Information Updated
- Supplier Activated
- Supplier Deactivated

---

# Cross-Module Coordination

Supplier events may be consumed by other modules when supplier information is relevant.

Supplier events never modify other modules directly.

Business workflows involving multiple modules are coordinated by the Application layer.

---

# Persistence

Supplier domain events are translated by the Infrastructure layer into persistence operations.

The Supplier aggregate never performs persistence directly.

---

# Historical Integrity

Supplier events preserve the history of changes made to supplier information.

They never modify historical procurement documents.

---

# Related Documentation

- aggregate.md
- commands.md
- queries.md
