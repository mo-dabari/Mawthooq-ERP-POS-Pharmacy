# Supplier Commands

## Purpose

This document defines the write operations supported by the Supplier module.

Commands represent business intentions that modify supplier information.

Each command targets the Supplier aggregate.

---

# Supplier Commands

Examples:

- Create Supplier
- Update Supplier Information
- Update Contact Information
- Activate Supplier
- Deactivate Supplier

---

# Aggregate Responsibility

Commands express business intentions.

The Supplier aggregate validates and applies its own business rules.

Commands do not contain business rules.

---

# Module Boundary

Supplier commands never modify:

- Purchase Orders
- Goods Receipts
- Inventory
- Financial data

Those responsibilities belong to their respective modules.

---

# Transaction Boundary

Business workflows involving multiple modules are coordinated by the Application layer.

Repositories remain responsible only for persistence.

---

# Related Documentation

- aggregate.md
- decision-rules.md
- events.md
