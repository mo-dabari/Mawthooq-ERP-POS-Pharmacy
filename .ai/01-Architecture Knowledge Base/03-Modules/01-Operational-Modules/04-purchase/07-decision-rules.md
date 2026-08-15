# Purchase Decision Rules

## Purpose

This document defines how the Purchase module evaluates purchasing operations.

Decision-making is distributed across the Purchase Order and Goods Receipt aggregates.

Each aggregate is responsible for validating and applying its own business rules.

---

# Purchase Order Decisions

The Purchase Order aggregate evaluates decisions related to the commercial purchasing agreement.

Typical decisions include:

- Creating a purchase order
- Updating the purchasing agreement
- Approving the purchase order
- Closing the purchase order
- Cancelling the purchase order

These decisions affect only the commercial purchasing process.

---

# Goods Receipt Decisions

The Goods Receipt aggregate evaluates decisions related to the physical receiving process.

Typical decisions include:

- Recording received products
- Validating received quantities
- Recording batch information
- Recording expiration dates
- Completing the receiving process

These decisions affect only the receiving process.

---

# Aggregate Independence

Purchase Order decisions never modify physical inventory.

Goods Receipt decisions never modify physical inventory.

Inventory remains responsible for all stock modifications.

---

# Cross-Aggregate Coordination

Business workflows involving both aggregates are coordinated by the Application layer.

Aggregates never modify each other's state directly.

---

# Business Validation

Each aggregate validates its own consistency boundary before applying any business operation.

Invalid operations are rejected before persistence.

---

# Related Documentation

- aggregate.md
- purchase-orders.md
- goods-receipts.md
