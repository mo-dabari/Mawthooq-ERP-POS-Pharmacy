# Customer Commands

## Purpose

This document defines the write operations supported by the Customer module.

Commands represent business intentions that modify customer information.

Each command targets the Customer aggregate.

---

# Customer Commands

Examples:

- Create Customer
- Update Customer Information
- Update Contact Information
- Activate Customer
- Deactivate Customer

---

# Aggregate Responsibility

Commands express business intentions.

The Customer aggregate validates and applies its own business rules.

Commands do not contain business rules.

---

# Module Boundary

Customer commands never modify:

- Sales
- Purchase
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
