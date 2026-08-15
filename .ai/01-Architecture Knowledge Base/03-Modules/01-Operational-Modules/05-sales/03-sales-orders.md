# Sales Orders

## Purpose

This document defines the Sales Order aggregate.

A Sales Order represents a completed commercial sale between the pharmacy and a customer.

It records exactly what was sold, how it was sold, and how the customer paid.

---

# Responsibilities

The Sales Order aggregate is responsible for:

- Recording sold products
- Recording sold quantities
- Recording selling units
- Recording selling prices
- Recording discounts
- Recording payment information
- Managing the sale lifecycle

---

# Lifecycle

A Sales Order is created in the Draft state.

While in Draft, the sale may be modified.

Once completed, the Sales Order becomes immutable.

Any business correction after completion is performed through a Sales Return.

---

# Sales Lines

A Sales Order consists of one or more Sales Order Lines.

Each line represents a single product included in the sale.

Sales Order Lines belong exclusively to their Sales Order.

---

# Payment

Payment information is recorded as part of the Sales Order.

The Sales module records how the customer paid.

Financial accounting is handled by the Financial module.

---

# Selling Price

Each Sales Order records the selling price agreed with the customer at the time of sale.

Completed Sales Orders preserve their original prices regardless of future pricing changes.

---

# Inventory

The Sales Order never owns inventory.

Inventory validation and stock deduction belong to the Inventory module.

The Application layer coordinates inventory operations after the sale is completed.

---

# Business History

A completed Sales Order represents a historical business fact.

Completed Sales Orders are never modified.

The historical record remains unchanged even if:

- Product information changes
- Medicine information changes
- Inventory changes
- Selling prices change

---

# Related Documentation

- sales-order-lines.md
- sales-returns.md
- payment.md
- batch-allocation.md
