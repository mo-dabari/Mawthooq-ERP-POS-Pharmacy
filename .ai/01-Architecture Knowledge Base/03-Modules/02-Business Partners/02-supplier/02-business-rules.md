# Supplier Business Rules

## Purpose

This document defines the business rules enforced by the Supplier module.

The Supplier module is responsible for maintaining accurate supplier information used throughout procurement operations.

---

# Supplier Identity

Each Supplier represents a single business entity.

Supplier identity must remain unique within the system.

---

# Supplier Information

A Supplier maintains business information required for procurement operations.

Supplier information may be updated whenever business requirements change.

---

# Supplier Status

Only active suppliers may participate in new procurement operations.

Inactive suppliers remain available for historical business records.

Historical procurement documents are never affected by supplier status changes.

---

# Procurement Responsibility

The Supplier module does not record:

- Purchase Orders
- Goods Receipts
- Inventory transactions
- Financial settlements

Those responsibilities belong to their respective modules.

---

# Historical Integrity

Updating supplier information never modifies historical procurement documents.

Completed business transactions preserve the information recorded at the time they occurred.

---

# Related Documentation

- aggregate.md
- decision-rules.md
- commands.md
- queries.md
