# Financial Aggregate

## Purpose

The FinancialRecord aggregate represents the financial impact of a completed business operation.

It acts as the central financial record within the Financial Recording Layer.

The aggregate does not model accounting processes.

It records financial facts produced by completed business events.

---

## Responsibilities

The FinancialRecord aggregate is responsible for:

* Recording financial consequences.
* Maintaining financial record integrity.
* Preserving the business origin of each financial record.
* Providing a reliable source for financial reporting.
* Supporting future accounting integration.

---

## What Does NOT Belong Here

The aggregate does **not** own:

* Sales
* Purchases
* Inventory
* Customers
* Suppliers
* Products
* Accounting journals
* Ledger accounts
* Tax calculations

These responsibilities belong to other modules or future accounting systems.

---

## Aggregate Boundary

FinancialRecord

The aggregate has no child entities.

Each Financial Record represents one completed business fact with financial significance.

---

## Relationships

FinancialRecord references the originating business operation by its business identity.

Examples include:

* SalesOrder
* PurchaseOrder
* GoodsReceipt
* Inventory Adjustment

The originating aggregate remains the source of business truth.

FinancialRecord stores only the information required to represent the financial consequence.

---

## Identity

Each Financial Record has a unique identifier.

The identifier remains stable throughout its lifetime.

---

## Lifecycle

Typical lifecycle:

Created

↓

Recorded

↓

Finalized

Financial records are immutable after finalization.

Corrections should be represented by new financial records rather than modifying historical ones.

---

## Design Principles

* Single Aggregate.
* Event-driven.
* Immutable after finalization.
* Records financial facts.
* Independent from accounting systems.
* Suitable for future integration.
