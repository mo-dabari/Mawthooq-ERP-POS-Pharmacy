# Queries

## Purpose

This document describes the information that can be retrieved from the Product module.

Queries expose the commercial definition of products without modifying business state.

---

# Available Information

The Product module provides access to information such as:

- Product identity
- Trade name
- Product classification
- Packaging Versions
- Selling units
- Unit hierarchy
- Unit conversion definitions
- Product barcodes
- Product lifecycle state

---

# Query Characteristics

Queries never modify business state.

Queries represent the current commercial definition of a Product.

Historical information is retrieved according to the business requirements of the requesting use case.

---

# Module Boundary

Queries only expose information owned by the Product module.

Information owned by other modules, such as Inventory or Medicine, is retrieved from those modules.

---

# Related Documentation

- aggregate.md
- packaging.md
- barcodes.md
- lifecycle.md
