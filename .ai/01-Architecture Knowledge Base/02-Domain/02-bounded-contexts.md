# Bounded Contexts

## Purpose

This document defines the business boundaries of the Mawthooq domain.

Each Bounded Context owns a specific business capability and is solely responsible for its business rules, language, and model.

A business concept must have one owner.

# Product Context

## Responsibility

The Product Context defines what can be sold.

It owns:

- Product identity
- Product information
- Packaging hierarchy
- Selling units
- Unit conversion rules
- Product barcodes
- Product lifecycle

The Product Context never owns stock quantities, inventory batches, or selling prices.

# Inventory Context

## Responsibility

The Inventory Context represents the physical stock inside a pharmacy branch.

It owns:

- Inventory quantities
- Inventory batches
- Batch expiration dates
- Batch cost
- Batch selling prices
- FEFO allocation
- Stock movements

Inventory is the source of truth for product availability.

# Purchase Context

## Responsibility

The Purchase Context manages inventory acquisition.

It owns:

- Purchase Orders
- Purchase Invoices
- Goods Receipts
- Supplier deliveries
- Bonus quantities
- Inventory replenishment

Purchases increase inventory through approved business operations.

# Sales Context

## Responsibility

The Sales Context manages product sales.

It owns:

- Sales transactions
- Sales returns
- Discounts
- Payment processing
- POS shifts
- Sales completion

Sales consume inventory according to business rules defined by the Inventory Context.

# Supplier Context

## Responsibility

The Supplier Context manages supplier information.

It owns:

- Supplier profiles
- Supplier contacts
- Commercial information

It does not manage purchasing operations.

# Customer Context

## Responsibility

The Customer Context manages customer information when required.

It owns:

- Customer profiles
- Customer accounts
- Customer balances
- Customer history

It does not participate in inventory management.

# Branch Context

## Responsibility

The Branch Context represents the pharmacy branches.

It owns:

- Branch information
- Branch configuration
- Branch operational status

Each Inventory belongs to exactly one Branch.

# Context Relationships

The contexts collaborate through clearly defined responsibilities.

Examples:

- Sales retrieves Product information but consumes Inventory.
- Purchase creates Inventory through received goods.
- Inventory references Products but never modifies Product information.
- Product defines packaging but never tracks quantities.

Each context protects its own business rules.

# Ownership Rules

Business ownership is never shared.

If a business rule belongs to a context, no other context is allowed to implement or duplicate it.

Cross-context collaboration must respect these boundaries.

# Design Goal

The domain is divided into autonomous business contexts.

Each context models one part of the pharmacy business, making the system easier to understand, maintain, and evolve without spreading business rules across multiple areas.
