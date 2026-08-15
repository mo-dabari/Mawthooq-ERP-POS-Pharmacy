# Purchase Commands

## Purpose

This document defines the write operations supported by the Purchase module.

Commands represent business intentions that modify purchasing state.

Each command targets a specific aggregate responsible for validating and applying the operation.

---

# Purchase Order Commands

Purchase Order commands manage the commercial purchasing agreement.

Examples:

- Create Purchase Order
- Update Purchase Order
- Approve Purchase Order
- Cancel Purchase Order
- Close Purchase Order

These commands affect only the Purchase Order aggregate.

---

# Goods Receipt Commands

Goods Receipt commands manage the physical receiving process.

Examples:

- Create Goods Receipt
- Add Received Product
- Complete Goods Receipt
- Cancel Goods Receipt

These commands affect only the Goods Receipt aggregate.

---

# Inventory Boundary

Purchase commands never modify Inventory directly.

When a Goods Receipt requires inventory changes, the Application layer coordinates the operation with the Inventory aggregate.

---

# Aggregate Responsibility

Commands do not contain business rules.

Each aggregate validates its own rules before applying the requested operation.

---

# Transaction Boundary

When a business workflow involves multiple aggregates, the Application layer coordinates the transaction boundary.

Repositories never coordinate business operations.

---

# Related Documentation

- aggregate.md
- decision-rules.md
- events.md
