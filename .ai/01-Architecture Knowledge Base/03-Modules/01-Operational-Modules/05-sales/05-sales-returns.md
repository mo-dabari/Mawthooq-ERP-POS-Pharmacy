# Sales Returns

## Purpose

This document defines the Sales Return aggregate.

A Sales Return represents the return of products from a previously completed sale.

It records a new business transaction rather than modifying historical sales records.

---

# Responsibilities

The Sales Return aggregate is responsible for:

- Recording returned products
- Recording returned quantities
- Recording return reasons
- Referencing the original Sales Order
- Managing the return lifecycle

---

# Relationship with Sales Order

Every Sales Return references an existing completed Sales Order.

The original Sales Order remains unchanged.

A Sales Return records an independent business transaction linked to the original sale.

---

# Return Lines

A Sales Return consists of one or more Sales Return Lines.

Each line represents a product being returned.

Sales Return Lines belong exclusively to their Sales Return.

---

# Inventory

The Sales Return aggregate never modifies inventory directly.

Inventory validation and stock restoration belong to the Inventory module.

The Application layer coordinates inventory operations after the return is completed.

---

# Financial Processing

The Sales Return records the commercial return.

Refund processing and financial accounting belong to the Financial module.

---

# Historical Integrity

Sales Returns preserve the history of returned products.

The original sale and the return remain separate business records.

---

# Business Philosophy

Returns never rewrite business history.

The original sale represents what happened.

The Sales Return represents what happened afterward.

Both records remain part of the business history.

---

# Related Documentation

- sales-return-lines.md
- sales-orders.md
- events.md
