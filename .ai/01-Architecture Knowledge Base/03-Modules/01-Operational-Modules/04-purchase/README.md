# Purchase Module

## Purpose

The Purchase module manages the procurement of products from suppliers.

It records the business process of purchasing inventory and serves as the source of truth for purchase transactions.

The Purchase module answers one question:

> What inventory has the business purchased?

---

# Responsibilities

The Purchase module owns:

- Purchase documents
- Purchase lines
- Supplier purchases
- Purchase lifecycle
- Purchase history

---

# Out of Scope

The Purchase module does not manage:

- Product definition
- Medicine information
- Physical inventory
- Selling prices
- Sales
- Financial accounting

Those responsibilities belong to their respective modules.

---

# Relationship with Supplier

Every Purchase is associated with a Supplier.

The Supplier provides the purchased products.

Supplier management belongs to the Supplier module.

---

# Relationship with Inventory

Purchase records what has been purchased.

Inventory determines how purchased inventory becomes physical stock.

Purchase never modifies inventory directly.

---

# Business Philosophy

Purchasing and inventory are separate business concepts.

A purchase documents a commercial transaction.

Inventory represents the physical stock resulting from that transaction.

---

# Documentation

- aggregate.md
- business-rules.md
- purchase-lifecycle.md
- purchase-lines.md
- receiving.md
- decision-rules.md
- commands.md
- queries.md
- events.md
