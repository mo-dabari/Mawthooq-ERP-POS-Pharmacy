# Purchase Queries

## Purpose

This document defines the information exposed by the Purchase module.

Queries provide read access to purchasing information without modifying business state.

---

# Purchase Order Queries

The Purchase module provides information related to purchase agreements, including:

- Purchase Orders
- Ordered products
- Agreed quantities
- Agreed purchase prices
- Purchase Order status

---

# Goods Receipt Queries

The Purchase module provides information related to receiving processes, including:

- Goods Receipts
- Received products
- Received quantities
- Batch information
- Expiration dates
- Receiving status

---

# Query Characteristics

Queries are read-only.

They never modify:

- Purchase Orders
- Goods Receipts
- Inventory

---

# Module Boundary

Purchase queries expose purchasing information only.

Inventory availability is provided by the Inventory module.

Product definition is provided by the Product module.

Medical information is provided by the Medicine module.

---

# Historical Information

Purchase queries preserve the historical view of purchasing activities.

Changes in current inventory state do not alter purchase records.

---

# Related Documentation

- purchase-orders.md
- goods-receipts.md
- events.md
