# Inventory Batches

## Purpose

This document defines how Mawthooq represents inventory using batches.

A batch represents a specific quantity of a Product that shares the same inventory characteristics.

The Inventory aggregate manages stock through batches rather than a single quantity.

---

# Batch Philosophy

Physical stock is not treated as one continuous quantity.

Different portions of stock may have different business characteristics.

For this reason, Inventory maintains stock as a collection of batches.

---

# Batch Identity

Each batch represents a distinct inventory entry.

A batch maintains its own business information independently from other batches.

---

# Batch Information

A batch may include information such as:

- Batch number
- Expiration date
- Available quantity
- Reserved quantity
- Cost information
- Receiving information

The exact business data is owned by the Inventory aggregate.

---

# Independent Lifecycle

Each batch changes independently as inventory operations occur.

Business operations may consume, reserve, release, or adjust one batch without affecting others.

---

# Batch Selection

The existence of batches does not determine which batch should be used.

Batch selection follows the inventory allocation rules defined elsewhere.

---

# Aggregate Ownership

Inventory owns every batch.

Other modules never modify batches directly.

---

# Related Documentation

- stock.md
- fefo.md
- reservations.md
