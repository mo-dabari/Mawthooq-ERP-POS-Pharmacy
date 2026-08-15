# Sales Commands

## Purpose

This document defines the write operations supported by the Sales module.

Commands represent business intentions that modify sales state.

Each command targets a specific aggregate responsible for validating and applying the requested operation.

---

# Sales Order Commands

Sales Order commands manage the commercial selling process.

Examples:

- Create Sales Order
- Add Product
- Remove Product
- Update Quantity
- Update Selling Unit
- Apply Discount
- Remove Discount
- Update Payment Information
- Complete Sale
- Cancel Draft Sale

These commands affect only the Sales Order aggregate.

---

# Sales Return Commands

Sales Return commands manage product return operations.

Examples:

- Create Sales Return
- Add Returned Product
- Record Return Reason
- Complete Sales Return
- Cancel Sales Return

These commands affect only the Sales Return aggregate.

---

# Inventory Boundary

Sales commands never modify Inventory directly.

Inventory operations are coordinated by the Application layer after the successful completion of a sale or return.

---

# Financial Boundary

Sales commands record commercial payment information.

Financial processing belongs to the Financial module.

---

# Aggregate Responsibility

Commands do not contain business rules.

Each aggregate validates and applies its own business rules.

---

# Transaction Boundary

Business workflows involving multiple modules are coordinated by the Application layer.

Repositories remain responsible only for persistence.

---

# Related Documentation

- aggregate.md
- decision-rules.md
- events.md
