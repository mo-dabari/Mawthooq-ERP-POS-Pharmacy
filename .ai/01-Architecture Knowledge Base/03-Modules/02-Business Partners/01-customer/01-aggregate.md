# Customer Aggregate

## Purpose

This document defines the aggregate boundary within the Customer module.

The Customer module consists of a single aggregate:

- Customer

The Customer aggregate owns the business identity of a pharmacy customer.

---

# Customer

## Purpose

The Customer aggregate represents a customer who purchases products from the pharmacy.

It manages customer identity and contact information required by business operations.

---

## Responsibilities

The Customer aggregate owns:

- Customer identity
- Customer code
- Customer name
- Contact information
- Customer status

---

## Does Not Own

The Customer aggregate does not own:

- Sales transactions
- Sales history
- Payment information
- Customer balances
- Loyalty information
- Financial accounting

Those responsibilities belong to their respective modules.

---

## Consistency Boundary

The Customer aggregate guarantees the consistency of customer information.

Business operations performed by other modules never modify customer identity.

---

# Aggregate Independence

The Customer aggregate is independent.

Commercial transactions reference customers but never become part of the Customer aggregate.

---

# Related Documentation

- customers.md
- business-rules.md
- decision-rules.md
