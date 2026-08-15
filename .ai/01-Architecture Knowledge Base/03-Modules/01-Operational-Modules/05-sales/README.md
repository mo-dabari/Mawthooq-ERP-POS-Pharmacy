# Sales Module

## Purpose

The Sales module manages the sale of products to customers.

It records commercial sales transactions and produces the information required to complete the selling process.

---

# Responsibilities

The Sales module owns:

- Sales Orders
- Sales Returns
- Payment information
- Selling prices used at the time of sale
- Sold quantities
- Sales history

---

# Out of Scope

The Sales module does not manage:

- Product definition
- Medicine information
- Physical inventory
- Inventory batch selection rules
- Supplier management
- Financial accounting

Those responsibilities belong to their respective modules.

---

# Relationship with Product

The Product module defines what can be sold.

The Sales module records the sale of those products.

---

# Relationship with Medicine

The Medicine module provides pharmaceutical information required by the business.

The Sales module does not own or modify medicine definitions.

---

# Relationship with Inventory

The Inventory module owns physical stock.

The Sales module never modifies inventory directly.

The Application layer coordinates inventory operations after a sale is completed.

---

# Relationship with Financial

The Sales module records payment information required to complete the sale.

Financial processing, cash movement, and accounting belong to the Financial module.

---

# Business Philosophy

A sale represents a completed commercial transaction.

A Sales Order remains editable while it is in the Draft state.

Once completed, the Sales Order becomes immutable.

Any correction after completion is performed through a separate Sales Return.

---

# Documentation

- aggregate.md
- business-rules.md
- sales-orders.md
- sales-order-lines.md
- sales-returns.md
- sales-return-lines.md
- payment.md
- batch-allocation.md
- decision-rules.md
- commands.md
- queries.md
- events.md
