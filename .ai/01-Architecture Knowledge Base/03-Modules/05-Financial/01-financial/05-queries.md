# Queries

## Purpose

This document describes the information that can be retrieved from the Financial module.

Queries never modify the aggregate.

They provide access to financial records for reporting, auditing, and business analysis.

---

# Get Financial Record

## Description

Retrieves a single Financial Record together with its Financial Record Lines.

---

## Typical Usage

* View financial details of a business operation.
* Audit historical records.
* Investigate financial activity.

---

# List Financial Records

## Description

Returns financial records matching the requested criteria.

---

## Typical Usage

* Financial history.
* Reporting.
* Administrative review.

---

# Search Financial Records

## Description

Searches financial records using business-related filters.

Examples include:

* Source module
* Source identifier
* Date range
* Transaction type
* Currency

---

## Typical Usage

* Reporting
* Investigation
* Auditing

---

# Get Financial Records by Business Operation

## Description

Returns the financial records generated from a specific business operation.

---

## Typical Usage

* View the financial impact of a sale.
* View the financial impact of a purchase.
* View the financial impact of an inventory adjustment.

---

# Query Principles

Queries:

* Never modify financial history.
* May join information from operational modules.
* May use optimized read models.
* Preserve aggregate boundaries.
* May expose reporting projections.
