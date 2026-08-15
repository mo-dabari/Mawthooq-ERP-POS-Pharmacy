# Batch Allocation

## Purpose

This document defines how inventory batches are associated with a completed sale.

Batch allocation records which inventory batches were used to fulfill each Sales Order Line.

---

# Responsibility

The Sales module records the batches used during the completed sale.

The Inventory module is responsible for selecting and validating those batches.

---

# Batch Selection

The Sales module does not select inventory batches.

Batch selection follows the business rules owned by the Inventory module.

The Application layer requests batch selection from the Inventory module before completing the sale.

---

# Manual Selection

When permitted by business rules, a user may manually select a different batch.

The selected batch must still satisfy all inventory validation rules.

Manual selection never bypasses inventory validation.

---

# Batch Validation

Inventory is responsible for validating:

- Batch availability
- Available quantity
- Batch status
- Expiration rules

The Sales module never performs these validations.

---

# Recording Allocations

Each completed Sales Order Line records the batches used to fulfill the sale.

These allocations become part of the historical sales record.

---

# Historical Integrity

Batch allocations preserve the exact inventory batches consumed during the sale.

Completed allocations are never modified.

---

# Related Documentation

- sales-order-lines.md
- sales-returns.md
- inventory/README.md
