# Payment

## Purpose

This document defines how payment information is handled within the Sales module.

The Sales module records the payment information required to complete a sale.

It does not perform financial accounting.

---

# Ownership

Payment information belongs to the Sales Order.

It records how the customer paid for the completed sale.

---

# Payment Information

Payment information may include:

- Payment methods
- Paid amounts
- Remaining balance, when applicable

The Sales module records the commercial outcome of the payment.

---

# Financial Responsibility

The Sales module does not manage:

- Cash drawer balances
- Treasury
- Accounting
- Financial settlements

These responsibilities belong to the Financial module.

---

# Historical Record

Payment information becomes part of the completed Sales Order.

Completed payment information is never modified.

---

# Business Philosophy

Payment information exists to complete the commercial sale.

Financial processing is a separate business capability.

---

# Related Documentation

- sales-orders.md
- business-rules.md
- events.md
