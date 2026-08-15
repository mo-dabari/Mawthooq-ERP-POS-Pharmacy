# Supplier Queries

## Purpose

This document defines the information exposed by the Supplier module.

Queries provide read access to supplier information without modifying business state.

---

# Supplier Queries

The Supplier module provides information such as:

- Supplier identity
- Supplier code
- Supplier name
- Contact information
- Business information
- Supplier status

---

# Query Characteristics

Queries are read-only.

They never modify:

- Supplier information
- Purchase Orders
- Goods Receipts
- Inventory
- Financial data

---

# Module Boundary

The Supplier module exposes supplier information only.

Procurement transactions are provided by the Purchase module.

Financial information is provided by the Financial module.

---

# Historical Information

Queries always return the current supplier information.

Historical procurement documents preserve the supplier information recorded at the time of each transaction.

---

# Related Documentation

- aggregate.md
- business-rules.md
- events.md
