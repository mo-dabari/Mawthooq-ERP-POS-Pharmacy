# Sales Order Lines

## Purpose

This document defines how sold products are represented within a Sales Order.

Each Sales Order Line represents a single product sold during the commercial transaction.

---

# Line Responsibility

A Sales Order Line records exactly what was sold to the customer.

Each line preserves the commercial agreement made at the time of sale.

---

# Sales Information

A Sales Order Line includes information such as:

- Product
- Selling unit
- Sold quantity
- Unit selling price
- Applied discount
- Batch allocations

These values represent the sale exactly as it occurred.

---

# Historical Snapshot

A Sales Order Line preserves the historical values used during the sale.

Future changes to:

- Product information
- Inventory
- Selling prices

never modify completed Sales Orders.

---

# Batch Allocation

Each Sales Order Line records the inventory batches consumed to fulfill the sale.

Batch allocations become part of the historical sales record after the sale is completed.

Batch selection rules are owned by the Inventory module.

---

# Aggregate Ownership

Sales Order Lines belong exclusively to a single Sales Order.

They cannot exist independently.

---

# Historical Integrity

Completed Sales Order Lines are never modified.

Business corrections are recorded through a separate Sales Return.

---

# Related Documentation

- sales-orders.md
- batch-allocation.md
- sales-returns.md
