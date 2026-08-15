# Supplier Decision Rules

## Purpose

This document defines how the Supplier aggregate evaluates business operations.

The Supplier aggregate validates and applies all decisions related to supplier information.

---

# Supplier Decisions

The Supplier aggregate evaluates decisions such as:

- Creating a supplier
- Updating supplier information
- Updating supplier contact information
- Activating a supplier
- Deactivating a supplier

These decisions affect only the Supplier aggregate.

---

# Aggregate Independence

The Supplier aggregate never modifies:

- Purchase Orders
- Goods Receipts
- Inventory
- Financial data

Other modules reference suppliers without becoming part of the Supplier aggregate.

---

# Cross-Module Coordination

Business workflows involving multiple modules are coordinated by the Application layer.

The Supplier aggregate manages supplier information only.

---

# Business Validation

The Supplier aggregate validates all business rules before applying any operation.

Invalid operations are rejected before persistence.

---

# Historical Integrity

Supplier updates never modify historical procurement documents.

Completed business transactions remain unchanged.

---

# Related Documentation

- aggregate.md
- business-rules.md
- commands.md
- events.md
