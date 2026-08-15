# Supplier Aggregate

## Purpose

This document defines the aggregate boundary within the Supplier module.

The Supplier module consists of a single aggregate:

- Supplier

The Supplier aggregate owns the business identity of a supplier.

---

# Supplier

## Purpose

The Supplier aggregate represents a business partner that supplies products to the pharmacy.

It manages supplier identity and business information required by procurement operations.

---

## Responsibilities

The Supplier aggregate owns:

- Supplier identity
- Supplier code
- Supplier name
- Contact information
- Business information
- Supplier status

---

## Does Not Own

The Supplier aggregate does not own:

- Purchase Orders
- Goods Receipts
- Inventory
- Financial settlements
- Supplier balances

Those responsibilities belong to their respective modules.

---

## Consistency Boundary

The Supplier aggregate guarantees the consistency of supplier information.

Business operations performed by other modules never modify supplier identity.

---

# Aggregate Independence

The Supplier aggregate is independent.

Procurement documents reference suppliers without becoming part of the Supplier aggregate.

---

# Related Documentation

- business-rules.md
- decision-rules.md
