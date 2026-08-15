# Customer Module

## Purpose

The Customer module manages customer information used throughout the pharmacy system.

It provides the business identity of customers and supports commercial operations such as sales and customer history.

---

# Responsibilities

The Customer module owns:

- Customer identity
- Customer personal information
- Customer contact information
- Customer status

---

# Out of Scope

The Customer module does not manage:

- Sales transactions
- Sales returns
- Payment processing
- Financial accounting
- Customer balances
- Loyalty programs

Those responsibilities belong to their respective modules.

---

# Relationship with Sales

The Sales module records commercial transactions.

The Customer module identifies who those transactions belong to.

---

# Relationship with Financial

The Customer module provides customer identity.

Financial processing and customer account balances belong to the Financial module.

---

# Business Philosophy

A customer represents a business entity that purchases products from the pharmacy.

The Customer module manages customer information only.

Commercial operations remain the responsibility of the Sales module.

---

# Documentation

- aggregate.md
- business-rules.md
- customers.md
- decision-rules.md
- commands.md
- queries.md
- events.md
