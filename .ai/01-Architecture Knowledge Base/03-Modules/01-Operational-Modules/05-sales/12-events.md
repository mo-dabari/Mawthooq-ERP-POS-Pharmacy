# Sales Events

## Purpose

This document defines the business events produced by the Sales module.

Sales events represent completed changes within Sales aggregates.

---

# Event Ownership

Each aggregate produces its own business events.

Sales Order events are produced by the Sales Order aggregate.

Sales Return events are produced by the Sales Return aggregate.

Aggregates never produce events on behalf of other aggregates.

---

# Sales Order Events

Sales Order events represent completed changes to the selling process.

Examples:

- Sales Order Created
- Product Added
- Product Removed
- Quantity Changed
- Selling Unit Changed
- Discount Applied
- Discount Removed
- Payment Information Updated
- Sales Order Completed
- Draft Sale Cancelled

---

# Sales Return Events

Sales Return events represent completed changes to the return process.

Examples:

- Sales Return Created
- Returned Product Added
- Return Reason Recorded
- Sales Return Completed
- Sales Return Cancelled

---

# Cross-Module Coordination

Completing a sale may require inventory and financial operations.

Completing a sales return may require inventory and financial operations.

The Application layer coordinates these workflows.

Sales events never modify other modules directly.

---

# Persistence

Sales domain events are translated by the Infrastructure layer into persistence operations.

Sales aggregates never perform persistence directly.

---

# Historical Integrity

Sales events preserve the history of commercial transactions.

They represent business facts that occurred within the Sales module.

---

# Related Documentation

- aggregate.md
- commands.md
- sales-orders.md
- sales-returns.md
