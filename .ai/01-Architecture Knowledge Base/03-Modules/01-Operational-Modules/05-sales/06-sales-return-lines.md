# Sales Return Lines

## Purpose

This document defines how returned products are represented within a Sales Return.

Each Sales Return Line represents a single product returned from a previously completed sale.

---

# Line Responsibility

A Sales Return Line records exactly what was returned.

Each line preserves the return transaction independently from the original sale.

---

# Return Information

A Sales Return Line includes information such as:

- Product
- Returned unit
- Returned quantity
- Return reason
- Batch allocations

These values represent the return exactly as it occurred.

---

# Relationship with Sales Order

A Sales Return Line references the product originally sold.

It never modifies the corresponding Sales Order Line.

The sale and the return remain separate business records.

---

# Batch Allocation

Each Sales Return Line records the inventory batches associated with the returned products.

Batch validation belongs to the Inventory module.

The Application layer coordinates inventory operations after the return is completed.

---

# Aggregate Ownership

Sales Return Lines belong exclusively to a single Sales Return.

They cannot exist independently.

---

# Historical Integrity

Completed Sales Return Lines are never modified.

They preserve the historical record of the return transaction.

---

# Related Documentation

- sales-returns.md
- sales-order-lines.md
- batch-allocation.md
