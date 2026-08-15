# Goods Receipt Lines

## Purpose

This document defines how received products are represented within a Goods Receipt.

Each Goods Receipt Line represents the physical receipt of a specific Product.

---

# Line Responsibility

A Goods Receipt Line records what has actually been received from the supplier.

It represents the physical receiving process rather than the commercial agreement.

---

# Receiving Information

A Goods Receipt Line includes information such as:

- Product
- Received quantity
- Batch information
- Expiration date

This information reflects the actual inventory delivered to the business.

---

# Independent from Purchase Order

A Goods Receipt Line is independent from a Purchase Order Line.

The Purchase Order defines what was requested.

The Goods Receipt records what was actually delivered.

---

# Relationship with Inventory

Goods Receipt Lines never modify physical inventory.

The Application layer coordinates the inventory operation.

The Inventory aggregate validates and applies the resulting stock changes.

---

# Historical Integrity

Goods Receipt Lines preserve the actual receiving history.

Receiving records are never modified to reflect later inventory changes.

---

# Related Documentation

- goods-receipts.md
- purchase-order-lines.md
