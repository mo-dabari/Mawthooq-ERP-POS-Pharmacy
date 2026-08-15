# Inventory Decision Rules

## Purpose

This document defines how the Inventory aggregate classifies incoming inventory operations.

Every inventory request is evaluated before physical stock is modified.

---

# Decision Categories

Every inventory operation belongs to one of the following categories:

- Inventory Increase
- Inventory Decrease
- Inventory Adjustment
- Pricing Change

Each category follows its own business rules.

---

# Business Rules

- Inventory operations are restricted to the current branch.

---

# Inventory Increase

Inventory Increase adds physical stock to the Inventory.

Typical examples include:

- Receiving inventory
- Positive inventory adjustments

The Inventory aggregate validates the operation before increasing stock.

---

# Inventory Decrease

Inventory Decrease removes physical stock from the Inventory.

Typical examples include:

- Selling inventory
- Negative inventory adjustments

The Inventory aggregate determines the required batch allocation before decreasing stock.

---

# Inventory Adjustment

Inventory Adjustments correct the physical inventory to match the actual stock.

Adjustments represent inventory corrections rather than commercial operations.

---

# Selling Price Update

Selling price changes affect future inventory operations.

Price changes never modify historical business records.

---

# Aggregate Decision

The Inventory aggregate is solely responsible for validating inventory operations.

External modules request inventory changes but never decide how those changes are applied.

---

# Related Documentation

- stock.md
- batches.md
- fefo.md
