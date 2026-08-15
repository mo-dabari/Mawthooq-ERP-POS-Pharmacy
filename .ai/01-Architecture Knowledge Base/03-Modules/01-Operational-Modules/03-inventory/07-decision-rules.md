# Inventory Decision Rules

## Purpose

This document defines how the Inventory aggregate evaluates incoming inventory operations.

Every request that affects physical stock is validated before any inventory state changes.

---

# Validation First

The Inventory aggregate never modifies stock immediately.

Every request is validated against the current inventory state.

Only valid operations may change physical stock.

---

# Batch Decision

The Inventory aggregate determines whether an operation:

- Creates a new batch
- Updates an existing batch
- Consumes one batch
- Consumes multiple batches

External modules never participate in this decision.

---

# Allocation Decision

When inventory must be consumed, the Inventory aggregate selects the appropriate batches according to its allocation policy.

Business operations request quantities.

The Inventory aggregate determines how those quantities are fulfilled.

---

# Quantity Validation

The Inventory aggregate validates that sufficient inventory is available before completing an operation.

Operations that would produce invalid stock are rejected.

---

# Pricing Decision

Selling price changes are validated by the Inventory aggregate.

Price updates affect future inventory operations only.

Historical business records remain unchanged.

---

# Aggregate Authority

The Inventory aggregate is solely responsible for deciding how inventory changes are applied.

External modules request inventory operations but never determine their internal execution.

---

# Related Documentation

- stock.md
- batches.md
- fefo.md
- pricing.md
