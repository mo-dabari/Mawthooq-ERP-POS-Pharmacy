# Sales Decision Rules

## Purpose

This document defines how the Sales module evaluates sales operations.

Decision-making is distributed across the Sales Order and Sales Return aggregates.

Each aggregate validates and applies its own business rules.

---

# Sales Order Decisions

The Sales Order aggregate evaluates decisions related to the commercial selling process.

Typical decisions include:

- Creating a sales order
- Adding products
- Removing products
- Updating quantities
- Applying discounts
- Updating payment information
- Completing the sale
- Cancelling the sale while in the Draft state

These decisions affect only the Sales Order aggregate.

---

# Sales Return Decisions

The Sales Return aggregate evaluates decisions related to returning previously sold products.

Typical decisions include:

- Creating a sales return
- Recording returned products
- Recording return reasons
- Completing the return
- Cancelling the return before completion

These decisions affect only the Sales Return aggregate.

---

# Aggregate Independence

Sales Order decisions never modify Sales Returns.

Sales Return decisions never modify Sales Orders.

Each aggregate protects its own consistency boundary.

---

# Cross-Module Coordination

The Sales module never modifies Inventory or Financial data directly.

Business workflows involving multiple modules are coordinated by the Application layer.

---

# Business Validation

Each aggregate validates its own business rules before applying any operation.

Invalid operations are rejected before persistence.

---

# Historical Integrity

Completed sales and completed returns are preserved as historical business records.

Corrections are recorded through new business transactions rather than modifying existing ones.

---

# Related Documentation

- aggregate.md
- business-rules.md
- sales-orders.md
- sales-returns.md
