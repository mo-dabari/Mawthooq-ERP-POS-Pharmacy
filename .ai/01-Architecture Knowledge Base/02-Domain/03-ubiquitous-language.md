# Ubiquitous Language

## Purpose

This document defines the official business vocabulary used throughout the Mawthooq project.

Every business term has a single meaning and must be used consistently across the Domain Model, Application Layer, Infrastructure, API, documentation, and AI prompts.

# Product

Represents the business definition of an item that can be purchased, stocked, and sold.

A Product defines business information such as identity, packaging, and selling units.

A Product never represents physical stock.

# Inventory

Represents the physical stock of a Product within a specific Branch.

Inventory is the source of truth for product availability.

# Inventory Batch

Represents a physical batch of inventory received from a supplier.

A Batch contains its own quantity, cost, selling price, batch number, and expiration date.

All inventory movements occur at the Batch level.

# Packaging

Defines how a Product is organized into sellable units.

Examples include:

- Box
- Strip
- Tablet
- Bottle
- Piece

Packaging belongs to the Product definition.

# Selling Unit

A unit that can be sold independently.

A Product may expose one or more selling units according to its packaging hierarchy.

# Conversion Rule

Defines the relationship between packaging units.

Example:

- 1 Box = 10 Strips
- 1 Strip = 10 Tablets

Conversion rules belong to the Product.

# Branch

A physical pharmacy location.

Inventory always belongs to a specific Branch.

# Purchase

The business process of acquiring inventory from suppliers.

Purchases create new inventory batches.

# Sale

The business process of delivering products to customers.

Sales consume inventory.

# Stock Movement

Any business operation that changes inventory quantities.

Examples include:

- Purchase
- Sale
- Return
- Inventory Adjustment

# FEFO

First Expired, First Out.

The inventory allocation strategy used when selecting batches for sale.

FEFO is an Inventory business rule.

# Domain Event

A business event recorded by an Aggregate after a successful business operation.

Domain Events describe what has happened inside the Domain.

They are used internally by Mawthooq to drive Event-Based Persistence.

# Aggregate

The consistency boundary of the Domain Model.

An Aggregate protects business rules and is the only component allowed to modify its own business state.

# Business Rule

A rule that defines valid business behavior.

Business Rules are enforced only by the Domain.

# Invariant

A business condition that must always remain true.

Aggregates are responsible for protecting their invariants.

# Unit of Work

The transaction boundary for a write operation.

It coordinates repositories and guarantees that all persistence operations succeed or fail together.

# RowVersion

The concurrency token used to detect conflicting updates during persistence.

RowVersion is an Infrastructure concern and is not part of the Domain Model.

# Design Goal

Every business concept in Mawthooq has a single, well-defined meaning.

Using a shared language reduces ambiguity, improves communication, and keeps the implementation aligned with the business domain.
