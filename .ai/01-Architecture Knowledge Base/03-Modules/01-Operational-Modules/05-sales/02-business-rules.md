# Sales Business Rules

## Purpose

This document defines the business rules enforced by the Sales module.

The Sales module is responsible for ensuring that every completed sale accurately represents the commercial transaction performed by the pharmacy.

---

# Sales Order

## Draft State

A Sales Order is created in the Draft state.

While in Draft, it may be updated by:

- Adding products
- Removing products
- Changing quantities
- Changing selling units
- Applying or removing discounts
- Updating payment information

No inventory changes occur while the Sales Order remains in the Draft state.

---

# Preconditions

- Active Shift is required.
- Cashier must belong to the current branch.

---

## Completing a Sale

A sale is completed only after all business validations succeed.

Once completed:

- The Sales Order becomes immutable.
- Inventory operations are coordinated by the Application layer.
- Payment information becomes part of the historical sale record.

---

## Historical Integrity

Completed sales are never modified.

The Sales module preserves the exact commercial transaction that occurred.

Changes after completion are recorded as separate business transactions.

---

# Sales Return

A Sales Return always references an existing completed Sales Order.

Returns are recorded as independent business transactions.

A Sales Return never modifies the original Sales Order.

---

# Payment

The Sales module records payment information required to complete the sale.

Payment information is part of the Sales Order.

Financial processing belongs to the Financial module.

---

# Inventory Responsibility

The Sales module never owns physical inventory.

Inventory availability, batch validation, and stock deduction belong to the Inventory module.

The Application layer coordinates inventory operations after a successful sale.

---

# Product Pricing

The selling price recorded in the Sales Order represents the price agreed with the customer at the time of sale.

Later price changes never affect completed sales.

---

# Business History

Sales records represent historical business facts.

Business history is preserved even if:

- Product information changes
- Medicine information changes
- Inventory changes
- Selling prices change

---

# Related Documentation

- sales-orders.md
- sales-returns.md
- payment.md
- batch-allocation.md
