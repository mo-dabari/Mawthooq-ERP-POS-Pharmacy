# Sales Aggregates

## Purpose

This document defines the aggregate boundaries within the Sales module.

The Sales module consists of two independent aggregates:

- Sales Order
- Sales Return

Each aggregate owns a distinct business responsibility and maintains its own consistency boundary.

---

# Sales Order

## Purpose

The Sales Order aggregate represents a commercial sale between the pharmacy and a customer.

It records the products sold, the selling prices applied, the payment information, and the completed commercial transaction.

---

## Responsibilities

The Sales Order aggregate owns:

- Sales order information
- Sales order lines
- Selling prices at the time of sale
- Payment information
- Sales status

---

## Does Not Own

The Sales Order aggregate does not own:

- Product definitions
- Medicine information
- Physical inventory
- Batch availability
- Financial accounting

Those responsibilities belong to their respective modules.

---

## Consistency Boundary

The Sales Order aggregate guarantees the consistency of the commercial sale.

While the order is in the Draft state, it may be modified.

Once completed, it becomes immutable.

---

# Sales Return

## Purpose

The Sales Return aggregate represents the return of products from a previously completed sale.

It records the business transaction required to reverse part or all of a completed sale.

---

## Responsibilities

The Sales Return aggregate owns:

- Returned products
- Returned quantities
- Return reason
- Reference to the original Sales Order
- Return status

---

## Does Not Own

The Sales Return aggregate does not own:

- Original sales information
- Product definitions
- Physical inventory
- Financial accounting

Those responsibilities belong to their respective modules.

---

## Consistency Boundary

The Sales Return aggregate guarantees the consistency of the return transaction.

It never modifies the original Sales Order.

---

# Aggregate Relationship

A Sales Order records the original commercial transaction.

A Sales Return records a separate transaction that references the completed Sales Order.

Returns never modify historical sales records.

---

# Aggregate Independence

The Sales Order and Sales Return aggregates are independent.

Neither aggregate modifies the state of the other directly.

Cross-aggregate workflows are coordinated by the Application layer.
