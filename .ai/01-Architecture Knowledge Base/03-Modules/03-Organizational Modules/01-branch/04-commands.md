# Branch Commands

## Purpose

This document defines the write operations supported by the Branch module.

Commands represent business intentions that modify branch information.

Each command targets the Branch aggregate.

---

# Branch Commands

Examples:

- Create Branch
- Update Branch Information
- Update Contact Information
- Activate Branch
- Deactivate Branch

---

# Aggregate Responsibility

Commands express business intentions.

The Branch aggregate validates and applies its own business rules.

Commands do not contain business rules.

---

# Module Boundary

Branch commands never modify:

- Inventory
- Sales
- Purchase Orders
- Goods Receipts
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
