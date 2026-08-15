# Purchase Business Rules

## Purpose

This document defines the business rules that apply to the Purchase module as a whole.

Detailed rules for Purchase Orders and Goods Receipts are documented separately.

---

# Commercial Responsibility

The Purchase module is responsible for documenting the business process of purchasing inventory.

It does not represent the physical state of inventory.

---

# Separation of Responsibilities

Commercial purchasing and physical receiving are separate business processes.

Purchase Orders define the purchasing agreement.

Goods Receipts record the actual inventory received.

---

# Inventory Ownership

The Purchase module never modifies physical inventory directly.

Inventory changes are performed exclusively by the Inventory aggregate.

---

# Receiving Requirement

Inventory cannot be increased without a valid Goods Receipt.

A Purchase Order alone never changes physical stock.

---

# Business Integrity

The Purchase module preserves the integrity of purchasing documents.

Historical purchasing records are never modified to reflect future inventory changes.

---

# Aggregate Independence

Purchase Orders and Goods Receipts maintain independent consistency boundaries.

Business workflows involving both aggregates are coordinated by the Application layer.

---

# Related Documentation

- purchase-orders.md
- purchase-lines.md
- goods-receipts.md
