# Inventory Module

## Purpose

The Inventory module represents the physical stock of products within pharmacy branches.

It is responsible for maintaining accurate stock information and ensuring that every stock change reflects a valid business operation.

The Inventory module answers one question:

> What stock is currently available for this product in this branch?

---

# Responsibilities

The Inventory module owns:

- Physical stock
- Inventory batches
- Available quantities
- Reserved quantities
- Selling prices
- Batch selection
- Stock allocation

---

# Out of Scope

The Inventory module does not manage:

- Product definition
- Medicine information
- Purchase documents
- Sales documents
- Supplier information
- Customer information
- Financial records

Those responsibilities belong to their respective modules.

---

# Relationship with Product

Every Inventory belongs to a single Product within a specific Branch.

The Product module defines the commercial identity of the item.

The Inventory module defines the physical availability of that item.

---

# Relationship with Medicine

When the Product represents a medicine, the Inventory module references its pharmaceutical information through the Medicine module.

The Inventory module never owns or modifies medical information.

---

# Business Philosophy

Inventory is the operational source of truth for stock.

Business operations never add, remove, or reserve quantities directly.

Instead, they request stock operations through the Inventory module.

The Inventory module validates the request and applies the appropriate business rules before changing stock.

---

# Documentation

- aggregate.md
- business-rules.md
- stock.md
- batches.md
- reservations.md
- fefo.md
- decision-rules.md
- commands.md
- queries.md
- events.md
