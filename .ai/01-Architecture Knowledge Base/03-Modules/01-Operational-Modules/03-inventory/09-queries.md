# Queries

## Purpose

This document describes the information exposed by the Inventory module.

Queries provide the current operational state of inventory without modifying business state.

---

# Available Information

The Inventory module provides information such as:

- Current stock
- Inventory batches
- Batch availability
- Current selling price
- Product availability within a branch

---

# Query Characteristics

Queries are read-only.

They never modify physical stock.

Queries always return the current operational state of inventory.

---

# Module Boundary

Inventory queries expose only information owned by the Inventory module.

Commercial information is obtained from the Product module.

Medical information is obtained from the Medicine module.

---

# Historical Consistency

Queries return the current inventory state.

Historical inventory information is obtained from the business documents that produced those changes.
