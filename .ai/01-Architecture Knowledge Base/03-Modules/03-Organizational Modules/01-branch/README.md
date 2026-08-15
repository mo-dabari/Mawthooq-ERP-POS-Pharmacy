# Branch Module

## Purpose

The Branch module defines the organizational units through which the pharmacy operates.

A Branch represents the operational boundary where business activities are performed.

---

# Responsibilities

The Branch module owns:

- Branch identity
- Branch organizational information
- Branch contact information
- Branch operational status

---

# Out of Scope

The Branch module does not manage:

- Inventory
- Sales
- Purchases
- Financial transactions
- Employees
- Cash drawers

Those responsibilities belong to their respective modules.

---

# Relationship with Operational Modules

Operational modules execute business activities within a specific branch.

A Branch provides the organizational context in which those operations occur.

---

# Relationship with Financial

The Financial module records financial activities performed by each branch.

The Branch module defines the organizational unit where those activities originate.

---

# Business Philosophy

A Branch is an organizational unit of the pharmacy.

It defines where business operations take place but does not execute those operations.

---

# Documentation

- aggregate.md
- business-rules.md
- decision-rules.md
- commands.md
- queries.md
- events.md
