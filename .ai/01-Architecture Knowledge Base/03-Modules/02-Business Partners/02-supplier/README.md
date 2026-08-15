# Supplier Module

## Purpose

The Supplier module manages supplier information used throughout the procurement process.

It provides the business identity of suppliers and supports purchasing operations.

---

# Responsibilities

The Supplier module owns:

- Supplier identity
- Supplier business information
- Supplier contact information
- Supplier status

---

# Out of Scope

The Supplier module does not manage:

- Purchase Orders
- Goods Receipts
- Inventory
- Financial settlements
- Supplier balances

Those responsibilities belong to their respective modules.

---

# Relationship with Purchase

The Purchase module records procurement transactions.

The Supplier module identifies the supplier involved in those transactions.

---

# Relationship with Financial

The Supplier module provides supplier identity.

Financial settlements and supplier accounts belong to the Financial module.

---

# Business Philosophy

A supplier represents a business partner that provides products to the pharmacy.

The Supplier module manages supplier information only.

Procurement operations remain the responsibility of the Purchase module.

---

# Documentation

- aggregate.md
- business-rules.md
- decision-rules.md
- commands.md
- queries.md
- events.md
