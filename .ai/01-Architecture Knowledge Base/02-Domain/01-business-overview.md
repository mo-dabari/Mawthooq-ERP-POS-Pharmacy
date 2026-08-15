# Business Overview

## Purpose

This document provides a high-level overview of the Mawthooq business domain.

It explains the business philosophy, the problems the system solves, and the principles that drive every business decision.

This document intentionally focuses on the business, not on the technical implementation.

# What is Mawthooq?

Mawthooq is an Offline-First Pharmacy ERP and Point of Sale (POS) system designed to manage pharmacy operations with a strong emphasis on business correctness, inventory accuracy, and financial integrity.

Rather than acting as a simple data-entry system, Mawthooq models the real-world behavior of a pharmacy and ensures that every business operation reflects what actually happens in daily pharmacy operations.

# Business Vision

Mawthooq is built around a single philosophy:

**The system must reflect the physical and financial reality of the pharmacy as accurately as possible.**

Every business operation should represent a real-world event.

The system exists to prevent inconsistencies before they reach the database.

# What "Offline-First" Means for Mawthooq

Being Offline-First is not the same as being an Offline POS.

An Offline POS simply continues accepting sales when the network is down.

Mawthooq's Offline-First commitment is a broader business guarantee, composed of six distinct capabilities:

```text
Mawthooq Offline-First
        ≠
Offline POS

Mawthooq Offline-First
        =
Branch Operational Autonomy
+
Local Persistence
+
Durable Operations
+
Idempotent Synchronization
+
Conflict Handling
+
Cloud Reconciliation
```

## Branch Operational Autonomy

A branch must be able to complete a full business operation — a sale, a purchase receipt, a shift close — entirely on its own, without depending on the availability of the central system at the moment of execution.

Connectivity is a convenience for synchronization, never a precondition for business execution.

## Local Persistence

Every business operation performed at a branch must be recorded immediately and reliably within that branch's own boundary, using the same business rules and consistency guarantees that would apply if the branch were connected.

## Durable Operations

A business operation that has been completed at a branch must never be lost, regardless of what happens to connectivity, the device, or the application afterward.

Once a business fact is recorded locally, its eventual delivery to the central system is guaranteed — not attempted.

## Idempotent Synchronization

The same business operation may be transmitted to the central system more than once, due to retries or interrupted connectivity.

The system must guarantee that a business fact is recorded exactly once centrally, regardless of how many times it was transmitted.

## Conflict Handling

Two branches, or two devices within the same branch, may record information that appears to conflict once synchronized.

The system must never silently discard or overwrite conflicting information. Every conflict must produce a traceable, well-defined outcome.

## Cloud Reconciliation

Synchronizing data is not the same as confirming its correctness.

After synchronization, the central system must be able to verify that what it has recorded accurately and completely reflects what each branch reported — particularly for inventory and financial totals — and surface any discrepancy for review rather than assuming synchronization alone guarantees correctness.

---

# Business Goals

Mawthooq aims to help pharmacies:

- Maintain accurate inventory.
- Protect financial correctness.
- Process sales efficiently.
- Simplify purchasing and replenishment.
- Reduce inventory waste.
- Support multi-branch operations.
- Continue operating even when internet connectivity is unavailable.

# Core Business Principles

The business model follows these principles:

- Business correctness always takes precedence over implementation convenience.
- Every state change represents a real business event.
- Product definition is independent from physical inventory.
- Inventory is the only module allowed to change physical stock availability.
- Sales consume inventory, not products.
- Business rules are enforced by the Domain Model.
- Financial records must remain traceable.
- Inventory accuracy is non-negotiable.

These principles guide every business decision throughout the system.

# Core Business Concepts

The Mawthooq domain is centered around the following business concepts:

- Products
- Inventory
- Inventory Batches
- Packaging Structures
- Purchases
- Sales
- Branches
- Suppliers
- Customers
- Stock Movements

Each concept has a clearly defined business owner and belongs to a dedicated Bounded Context.

# Business Integrity

Business integrity is the highest priority of the system.

Business operations are never performed by directly modifying database records.

Every business change must be executed through the appropriate Aggregate so that business rules, invariants, and consistency are always preserved.

# Design Goal

Mawthooq is designed to be a trustworthy representation of pharmacy operations.

The Domain Model protects business correctness, inventory consistency, and financial integrity while remaining independent from technical implementation details.

The architecture exists to serve the business-not the other way around.

# Related Documentation

- ADR-011 - Idempotency Strategy for Offline-First Operations
- ADR-012 - Offline-First Synchronization Strategy
