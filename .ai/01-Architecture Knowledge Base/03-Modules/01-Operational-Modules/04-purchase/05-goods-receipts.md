# Goods Receipts

## Purpose

This document defines how Mawthooq records the physical receipt of inventory from suppliers.

A Goods Receipt documents what the business has actually received.

It does not modify physical inventory directly.

---

# Goods Receipt Philosophy

A Goods Receipt represents the physical receiving process.

It confirms the inventory that has been delivered by the supplier.

Receiving inventory is independent from the original purchasing agreement.

---

# Receiving Information

A Goods Receipt records information such as:

- Supplier
- Purchase Order reference
- Received products
- Received quantities
- Batch information
- Expiration dates

This information represents the actual delivery received by the business.

---

# Goods Receipt Lines

Each Goods Receipt consists of one or more Goods Receipt Lines.

Each line represents the physical receipt of a specific Product.

Goods Receipt Lines own the receiving information for that Product.

---

# Relationship with Purchase Order

A Goods Receipt fulfills one or more Purchase Order Lines.

Receiving may be complete or partial.

Multiple Goods Receipts may fulfill the same Purchase Order.

---

# Relationship with Inventory

Completing a Goods Receipt does not modify Inventory directly.

The Application layer coordinates the corresponding inventory operation.

The Inventory aggregate validates and applies the resulting stock changes.

---

# Historical Integrity

Goods Receipts preserve the actual receiving history.

Receiving records are never rewritten to match later inventory changes.

---

# Related Documentation

- purchase-orders.md
- purchase-lines.md
- aggregate.md
