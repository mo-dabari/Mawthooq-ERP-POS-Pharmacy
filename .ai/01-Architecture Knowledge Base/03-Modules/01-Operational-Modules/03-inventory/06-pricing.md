# Inventory Pricing

## Purpose

This document defines how Mawthooq manages selling prices.

Selling prices belong to the Inventory module because they represent how a Product is sold within a specific Branch.

---

# Pricing Philosophy

A Product does not own its selling price.

The Product defines what is sold.

The Inventory defines how that Product is sold within a Branch.

For this reason, selling prices are part of the Inventory aggregate.

---

# Branch Pricing

Each Inventory maintains its own selling price.

Different branches may sell the same Product at different prices.

Price differences do not affect the Product definition.

---

# Current Selling Price

The Inventory aggregate maintains the current selling price for future business operations.

Business operations always use the current selling price unless another business rule explicitly applies.

---

# Historical Integrity

Changing the selling price never modifies historical business documents.

Completed business transactions always preserve the selling price that was applied when the transaction occurred.

---

# Aggregate Ownership

Only the Inventory aggregate may modify the current selling price.

Other modules may read the selling price but never modify it directly.

---

# Related Documentation

- stock.md
- aggregate.md
