# Purchase Events

## Purpose

This document defines the business events produced by the Purchase module.

Purchase events represent completed changes within Purchase aggregates.

---

# Event Ownership

Each aggregate produces its own business events.

Purchase Order events are produced by the Purchase Order aggregate.

Goods Receipt events are produced by the Goods Receipt aggregate.

Aggregates never produce events on behalf of other aggregates.

---

# Purchase Order Events

Purchase Order events represent changes in the commercial purchasing agreement.

Examples:

- Purchase Order Created
- Purchase Order Updated
- Purchase Order Approved
- Purchase Order Cancelled
- Purchase Order Closed

---

# Goods Receipt Events

Goods Receipt events represent changes in the physical receiving process.

Examples:

- Goods Receipt Created
- Goods Receipt Completed
- Goods Receipt Cancelled

---

# Inventory Integration

Completing a Goods Receipt may require an inventory operation.

The Application layer coordinates this workflow with the Inventory aggregate.

Purchase events do not directly modify Inventory.

---

# Persistence

Purchase domain events are translated by the Infrastructure layer into persistence operations.

The Purchase aggregates never perform persistence directly.

---

# Historical Integrity

Purchase events preserve the history of purchasing activities.

They represent business facts that occurred within the Purchase module.

---

# Related Documentation

- aggregate.md
- commands.md
- goods-receipts.md
