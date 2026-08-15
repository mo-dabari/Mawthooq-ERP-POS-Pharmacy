# Sales Queries

## Purpose

This document defines the information exposed by the Sales module.

Queries provide read access to sales information without modifying business state.

---

# Sales Order Queries

The Sales module provides information related to completed and in-progress sales, including:

- Sales Orders
- Sold products
- Sold quantities
- Selling units
- Selling prices
- Applied discounts
- Payment information
- Sales status

---

# Sales Return Queries

The Sales module provides information related to product returns, including:

- Sales Returns
- Returned products
- Returned quantities
- Return reasons
- Return status

---

# Query Characteristics

Queries are read-only.

They never modify:

- Sales Orders
- Sales Returns
- Inventory
- Financial data

---

# Module Boundary

Sales queries expose commercial sales information only.

Product definitions are provided by the Product module.

Medicine information is provided by the Medicine module.

Inventory availability is provided by the Inventory module.

Financial information is provided by the Financial module.

---

# Historical Information

Sales queries preserve the historical view of commercial transactions.

Completed sales and completed returns remain unchanged regardless of later business changes.

---

# Related Documentation

- sales-orders.md
- sales-returns.md
- events.md
