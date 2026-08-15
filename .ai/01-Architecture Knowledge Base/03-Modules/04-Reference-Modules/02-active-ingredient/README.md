# Active Ingredient Module

## Purpose

The Active Ingredient module manages pharmaceutical reference data describing the active substances used in medicines.

It provides standardized pharmaceutical information shared across the system.

---

# Responsibilities

The Active Ingredient module owns:

- Active ingredient identity
- Active ingredient name
- Pharmaceutical reference information
- Active ingredient status

---

# Out of Scope

The Active Ingredient module does not manage:

- Medicines
- Products
- Inventory
- Sales
- Purchases

Those responsibilities belong to their respective modules.

---

# Relationship with Medicine

The Medicine module references one or more active ingredients when defining a medicine.

The Active Ingredient module never owns medicines.

---

# Business Philosophy

An Active Ingredient represents a pharmaceutical substance.

It provides standardized pharmaceutical reference data used throughout the system.

---

# Documentation

- aggregate.md
- business-rules.md
- decision-rules.md
- commands.md
- queries.md
- events.md
