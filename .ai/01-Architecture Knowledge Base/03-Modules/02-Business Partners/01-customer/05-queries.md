# Customer Queries

## Purpose

This document defines the information exposed by the Customer module.

Queries provide read access to customer information without modifying business state.

---

# Customer Queries

The Customer module provides information such as:

- Customer identity
- Customer code
- Customer name
- Contact information
- Customer status

---

# Query Characteristics

Queries are read-only.

They never modify:

- Customer information
- Sales
- Purchase
- Inventory
- Financial data

---

# Module Boundary

The Customer module exposes customer information only.

Commercial transactions are provided by the Sales module.

Financial information is provided by the Financial module.

---

# Historical Information

Queries always return the current customer information.

Historical business documents preserve the customer information recorded at the time of each transaction.

---

# Related Documentation

- aggregate.md
- business-rules.md
- events.md
