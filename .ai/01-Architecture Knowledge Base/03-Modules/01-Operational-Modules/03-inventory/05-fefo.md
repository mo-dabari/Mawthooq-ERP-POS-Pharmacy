# FEFO

## Purpose

This document defines how Mawthooq allocates inventory batches for business operations.

Batch allocation is performed exclusively by the Inventory aggregate.

---

# Allocation Philosophy

Mawthooq does not allow external modules to select inventory batches.

Business operations request a quantity.

The Inventory aggregate determines how that quantity is fulfilled.

---

# FEFO Strategy

Inventory batches are allocated using the First Expired, First Out (FEFO) strategy.

Batches with the earliest expiration date are allocated before later-expiring batches.

This minimizes product expiration and reduces inventory waste.

---

# Batch Selection

Batch selection is an internal responsibility of the Inventory aggregate.

External modules never specify which batch should be consumed.

---

# Partial Allocation

A requested quantity may be fulfilled from multiple batches.

The Inventory aggregate determines the required allocation automatically.

---

# Business Validation

Before allocating inventory, the Inventory aggregate validates:

- Stock availability
- Batch availability
- Requested quantity

Allocation is rejected if the requested quantity cannot be satisfied.

---

# Aggregate Ownership

The FEFO algorithm belongs entirely to the Inventory aggregate.

Other modules request inventory allocation but never participate in batch selection.

---

# Related Documentation

- stock.md
- batches.md
- aggregate.md
