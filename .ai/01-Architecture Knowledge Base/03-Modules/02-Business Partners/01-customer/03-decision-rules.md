# Customer Decision Rules

## Purpose

This document defines how the Customer aggregate evaluates business operations.

The Customer aggregate validates and applies all decisions related to customer information.

---

# Customer Decisions

The Customer aggregate evaluates decisions such as:

- Creating a customer
- Updating customer information
- Updating customer contact information
- Activating a customer
- Deactivating a customer

These decisions affect only the Customer aggregate.

---

# Aggregate Independence

The Customer aggregate never modifies:

- Sales
- Purchases
- Inventory
- Financial data

Other modules reference customers without becoming part of the Customer aggregate.

---

# Cross-Module Coordination

Business workflows involving multiple modules are coordinated by the Application layer.

The Customer aggregate manages customer information only.

---

# Business Validation

The Customer aggregate validates all business rules before applying any operation.

Invalid operations are rejected before persistence.

---

# Historical Integrity

Customer updates never modify historical business documents.

Completed business transactions remain unchanged.

---

# Related Documentation

- aggregate.md
- business-rules.md
- commands.md
- events.md
