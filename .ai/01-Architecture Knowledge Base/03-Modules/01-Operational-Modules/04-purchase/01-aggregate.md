# Purchase Aggregates

## Purpose

This document defines the aggregate boundaries within the Purchase module.

The Purchase module consists of two independent aggregates:

- Purchase Order
- Goods Receipt

Each aggregate owns a distinct business responsibility and maintains its own consistency boundary.

---

# Purchase Order

## Purpose

The Purchase Order aggregate represents the commercial agreement between the pharmacy and a supplier.

It defines what the business intends to purchase and under which commercial terms.

---

## Responsibilities

The Purchase Order aggregate owns:

- Supplier
- Purchase lines
- Agreed quantities
- Agreed purchase prices
- Purchase status

---

## Does Not Own

The Purchase Order aggregate does not own:

- Physical inventory
- Received quantities
- Batch information
- Expiration dates

Those responsibilities belong to the Goods Receipt and Inventory aggregates.

---

## Consistency Boundary

The Purchase Order aggregate guarantees the consistency of the purchasing agreement.

It never represents the physical state of inventory.

---

# Goods Receipt

## Purpose

The Goods Receipt aggregate represents the physical receipt of inventory from a supplier.

It records what was actually received by the business.

---

## Responsibilities

The Goods Receipt aggregate owns:

- Received quantities
- Received batches
- Expiration dates
- Receiving status
- Reference to the Purchase Order

---

## Does Not Own

The Goods Receipt aggregate does not own:

- Purchase agreement
- Supplier pricing
- Physical inventory

Those responsibilities belong to their respective aggregates.

---

## Consistency Boundary

The Goods Receipt aggregate guarantees the correctness of the physical receiving process.

It validates received inventory before it becomes available to the Inventory module.

---

# Aggregate Relationship

The Purchase Order defines the commercial agreement.

The Goods Receipt records the physical receiving of that agreement.

After a Goods Receipt is successfully completed, the Application layer coordinates the corresponding inventory operation.

The Inventory aggregate remains solely responsible for modifying physical stock.

---

# Aggregate Independence

The Purchase Order and Goods Receipt aggregates are independent.

Neither aggregate modifies the state of the other directly.

Cross-aggregate workflows are coordinated by the Application layer.
