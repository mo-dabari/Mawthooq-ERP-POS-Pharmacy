# Product Lifecycle

## Purpose

This document defines the lifecycle states of a Product.

Lifecycle states control whether a Product can participate in business operations.

They do not describe how the Product evolves commercially.

---

# Lifecycle Philosophy

A Product remains part of the business even when it is no longer actively used.

Historical business records must always remain valid.

For this reason, Products are never physically removed from the system.

---

# Lifecycle States

## Active

The Product is available for normal business operations.

It may be:

- Purchased
- Stocked
- Sold
- Referenced by new business documents

---

## Discontinued

The Product is no longer available for future business activities.

Historical records remain unchanged.

Existing business history continues referencing the Product.

---

# State Transitions

The Product lifecycle supports the following transition:

Active

↓

Discontinued

Once discontinued, the Product is preserved for historical purposes.

---

# Historical Integrity

Changing the lifecycle state never modifies historical business documents.

Business history always reflects the state that existed when the business event occurred.

---

# Aggregate Responsibility

Lifecycle state changes are controlled exclusively by the Product aggregate.

No external module may change the lifecycle directly.

---

# Related Documentation

- aggregate.md
- evolution.md
