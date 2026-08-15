# Purchase Orders

## Purpose

This document defines how Mawthooq represents purchase agreements with suppliers.

A Purchase Order records the business intention to purchase products under agreed commercial terms.

It does not represent the physical receipt of inventory.

---

# Purchase Order Philosophy

A Purchase Order is a commercial agreement.

It defines what the business expects to receive from a supplier.

Creating or approving a Purchase Order never changes physical inventory.

---

# Commercial Information

A Purchase Order records the agreed purchasing information, including:

- Supplier
- Products
- Ordered quantities
- Agreed purchase prices
- Purchase terms

This information represents the purchasing agreement between both parties.

---

# Purchase Lifecycle

A Purchase Order progresses through its business lifecycle until it is fulfilled or closed.

Changes to the Purchase Order affect the purchasing process only.

They never modify physical stock.

---

# Relationship with Goods Receipt

A Purchase Order may be fulfilled by one or more Goods Receipts.

Each Goods Receipt records what was actually received from the supplier.

Receiving inventory is independent from the purchasing agreement.

---

# Inventory Independence

The Purchase Order never modifies Inventory.

Physical stock changes only after inventory operations are coordinated by the Application layer.

---

# Historical Integrity

Purchase Orders preserve the original purchasing agreement.

Later receiving activities never rewrite the agreed commercial terms.

---

# Related Documentation

- goods-receipts.md
- purchase-lines.md
- aggregate.md
