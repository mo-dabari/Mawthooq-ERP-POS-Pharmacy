# Customer Business Rules

## Purpose

This document defines the business rules enforced by the Customer module.

The Customer module is responsible for maintaining accurate customer information used throughout the system.

---

# Customer Identity

Each Customer represents a single business entity.

Customer identity must remain unique within the system.

---

# Customer Information

A Customer maintains business information required for commercial operations.

Customer information may be updated whenever business requirements change.

---

# Customer Status

Only active customers may participate in new business operations.

Inactive customers remain available for historical business records.

Historical transactions are never affected by customer status changes.

---

# Commercial Responsibility

The Customer module does not record:

- Sales
- Returns
- Payments
- Financial balances

Commercial operations belong to their respective modules.

---

# Historical Integrity

Updating customer information never modifies historical business documents.

Completed business transactions preserve the information recorded at the time they occurred.

---

# Related Documentation

- aggregate.md
- decision-rules.md
- commands.md
- queries.md
