# Domain Model

## Purpose

This document describes how Mawthooq models the pharmacy business.

It defines the major business concepts and the relationships between them without describing implementation details.

---

# Business Perspective

Mawthooq models the pharmacy as a collection of independent business areas.

Each area is responsible for a specific part of the business and owns its own rules.

No business concept exists without a clear owner.

---

# Core Business Concepts

## Product

Defines what can be bought, stocked, and sold.

A Product exists independently from stock.

---

## Inventory

Represents the physical existence of products inside a branch.

Inventory tracks quantities and availability.

---

## Purchase

Represents bringing products into the pharmacy.

Purchasing increases inventory after successful receiving.

---

## Sale

Represents delivering products to customers.

Sales consume inventory and generate financial records.

---

## Supplier

Represents organizations that provide products.

Suppliers participate in purchasing operations.

---

## Customer

Represents people or organizations purchasing products.

Customers participate in sales operations.

---

## Branch

Represents a physical pharmacy location.

Business operations occur within a branch.

---

## Financial Records

Represent the financial impact created by business operations.

Financial information reflects business events rather than replacing them.

---

# Business Relationships

Product defines what exists.

Inventory stores products.

Purchase adds inventory.

Sale consumes inventory.

Customers participate in sales.

Suppliers participate in purchases.

Branches host inventory and business operations.

Financial records are produced by completed business operations.

---

# Business Ownership

Each business concept owns its own rules.

Business concepts collaborate but do not own each other's data.

Changes affecting multiple business concepts are coordinated by the Application layer.

---

# Business Events

Every significant business change represents a real business event.

Business events describe what happened in the pharmacy rather than technical operations.

These events become the basis for updating the persistence layer.
