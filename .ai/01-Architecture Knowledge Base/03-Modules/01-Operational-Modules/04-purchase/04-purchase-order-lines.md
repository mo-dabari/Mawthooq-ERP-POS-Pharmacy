# Purchase Order Lines

## Purpose

This document defines how products are represented within a Purchase Order.

Each Purchase Order Line represents a single commercial agreement for purchasing a specific Product.

---

# Line Responsibility

A Purchase Order Line records the commercial details agreed with the supplier.

It defines what the business intends to purchase.

---

# Commercial Information

A Purchase Order Line includes information such as:

- Product
- Ordered quantity
- Agreed purchase price
- Line totals

This information belongs exclusively to the Purchase Order.

---

# Independent from Receiving

A Purchase Order Line never represents received inventory.

Receiving information is recorded by Goods Receipt Lines.

The commercial agreement and the physical receiving process remain independent.

---

# Relationship with Goods Receipt

One Purchase Order Line may be fulfilled by one or more Goods Receipt Lines.

The total received quantity is determined by the completed Goods Receipts rather than the Purchase Order itself.

---

# Inventory Independence

Purchase Order Lines never modify Inventory.

Inventory changes occur only after successful inventory operations coordinated by the Application layer.

---

# Historical Integrity

Purchase Order Lines preserve the original purchasing agreement.

Subsequent receiving activities never modify the agreed commercial terms.

---

# Related Documentation

- purchase-orders.md
- goods-receipts.md
