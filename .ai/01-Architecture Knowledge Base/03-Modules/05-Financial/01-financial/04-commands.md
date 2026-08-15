# Commands

## Purpose

This document describes the business operations that may modify the FinancialRecord aggregate.

Unlike operational modules, these commands are reactions to completed business events rather than user-initiated business workflows.

---

# Record Financial Impact

## Description

Creates a Financial Record representing the financial consequence of a completed business operation.

The operation is triggered internally after the originating business transaction has successfully completed.

---

## Business Rules

* The originating business operation must be completed.
* Exactly one originating business operation is required.
* At least one Financial Record Line must be created.
* The record is created in a valid initial state.

---

# Finalize Financial Record

## Description

Marks a Financial Record as finalized.

A finalized record becomes part of the permanent financial history.

---

## Business Rules

* Only recorded financial records may be finalized.
* Finalized records become immutable.
* Historical integrity must be preserved.

---

# Unsupported Operations

The following operations are intentionally not supported:

* Update Financial Record
* Delete Financial Record
* Edit Financial Record Lines
* Merge Financial Records
* Split Financial Records
* Execute Sales
* Execute Purchases
* Execute Inventory Operations

These responsibilities belong to operational modules.
