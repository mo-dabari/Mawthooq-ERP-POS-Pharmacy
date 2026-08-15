# Stock

## Purpose

This document defines how Mawthooq represents physical stock.

Stock represents the quantities of a Product that are physically available within a specific Branch.

It is maintained exclusively by the Inventory aggregate.

---

# Stock Philosophy

Stock is a physical business concept.

It represents the real quantities that exist inside the pharmacy.

Every stock change must correspond to a valid business event.

Inventory never changes stock arbitrarily.

---

# Stock Representation

Stock is represented by the Inventory aggregate through:

- Inventory batches
- Available quantities
- Reserved quantities
- Selling prices

Together these concepts describe the current operational state of stock.

---

# Stock Availability

Available stock represents quantities that may participate in business operations.

Reserved quantities remain part of the physical stock but are temporarily unavailable for allocation.

The Inventory aggregate determines the effective availability of stock.

---

# Stock Changes

Stock changes occur only as the result of valid business operations.

Examples include:

- Receiving inventory
- Selling stock
- Processing returns
- Performing inventory adjustments

The originating business operation requests the change.

The Inventory aggregate validates and applies the resulting stock modification.

---

# Business Ownership

Only the Inventory aggregate may modify physical stock.

Other modules request stock operations but never change stock directly.

---

# Current State

The Inventory aggregate represents the current operational state of stock.

Historical business documents remain owned by the modules that created them.

---

# Related Documentation

- batches.md
- reservations.md
- fefo.md
