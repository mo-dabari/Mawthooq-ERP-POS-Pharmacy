# Business Rules

## Purpose

This document defines the business rules governing the FinancialRecord aggregate.

These rules apply to both the FinancialRecord aggregate and its FinancialRecordLine entities.

---

# Record Creation

A Financial Record is created only as the result of a completed business operation.

Financial records are never created manually.

---

# Business Origin

Every Financial Record must reference exactly one originating business operation.

Examples include:

* Sales Order
* Purchase Order
* Goods Receipt
* Inventory Adjustment

The originating business operation remains the source of business truth.

---

# Record Lines

A Financial Record contains one or more Financial Record Lines.

Each line represents a financial component of the recorded business event.

Examples include:

* Sale Amount
* Discount
* Tax
* Shipping
* Payment
* Refund
* Adjustment

---

# Immutability

Once finalized, a Financial Record becomes immutable.

Historical financial records are never modified.

Corrections are represented by creating new Financial Records.

---

# Lifecycle

Typical lifecycle:

Created

↓

Recorded

↓

Finalized

Only finalized records are considered part of the permanent financial history.

---

# Ownership Rules

FinancialRecord owns all FinancialRecordLine entities.

FinancialRecordLine cannot exist independently.

Its lifecycle is completely controlled by the aggregate.

---

# Financial Scope

The Financial module records financial consequences.

It does not validate business operations.

Business validation always belongs to the originating operational aggregate.

---

# Access Control

Profit reports are visible only to authorized roles.

---

# Business Invariants

The following invariants must always hold:

* Every Financial Record has one originating business operation.
* Every Financial Record contains at least one Financial Record Line.
* FinancialRecordLine cannot exist outside its aggregate.
* Finalized financial records are immutable.
* Historical financial records are preserved.
