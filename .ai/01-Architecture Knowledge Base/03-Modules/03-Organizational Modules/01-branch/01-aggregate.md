# Branch Aggregate

## Purpose

This document defines the aggregate boundary within the Branch module.

The Branch module consists of a single aggregate:

- Branch

The Branch aggregate owns the organizational identity of a pharmacy branch.

---

# Branch

## Purpose

The Branch aggregate represents an organizational unit through which the pharmacy conducts its business operations.

It defines the operational context for business activities performed within the pharmacy.

---

## Responsibilities

The Branch aggregate owns:

- Branch identity
- Branch code
- Branch name
- Contact information
- Address
- Operational status

---

## Does Not Own

The Branch aggregate does not own:

- Inventory
- Sales
- Purchase Orders
- Goods Receipts
- Financial transactions
- Employees
- Cash drawers

Those responsibilities belong to their respective modules.

---

## Consistency Boundary

The Branch aggregate guarantees the consistency of branch information and organizational status.

Business operations performed within a branch never modify the Branch aggregate.

---

# Aggregate Independence

The Branch aggregate is independent.

Operational modules reference the Branch to establish organizational context without becoming part of the Branch aggregate.

---

# Related Documentation

- business-rules.md
- decision-rules.md
