# Inventory Aggregate

## Purpose

The Inventory aggregate represents the physical stock of a single Product within a single Branch.

It protects the business rules that govern stock availability and inventory operations.

The Inventory aggregate answers one question:

> What is the current physical state of this product in this branch?

---

# Responsibilities

The Inventory aggregate is responsible for maintaining:

- Physical stock
- Inventory batches
- Available quantities
- Reserved quantities
- Selling prices
- Batch selection

Every stock modification is validated and applied by the Inventory aggregate.

---

# Business Boundary

The Inventory aggregate owns every rule that changes physical stock.

No other module is allowed to modify stock directly.

---

# Owns

The Inventory aggregate owns:

- Inventory
- Batches
- Stock quantities
- Reservations
- Selling prices

All changes to these concepts must be performed through the Inventory aggregate.

---

# Does Not Own

The Inventory aggregate does not own:

- Product definition
- Medical information
- Purchase documents
- Sales documents
- Financial records

These concepts are referenced when required but are owned by their respective modules.

---

# Relationship with Product

Every Inventory belongs to one Product within one Branch.

The Product defines the commercial identity.

The Inventory defines the physical state of that Product.

---

# Relationship with Medicine

When the Product represents a medicine, the Inventory aggregate may use pharmaceutical information provided by the Medicine module.

Medical information is never owned or modified by Inventory.

---

# Consistency Boundary

The Inventory aggregate guarantees that stock remains internally consistent after every successful business operation.

Invalid inventory states are never persisted.

---

# Stock Authority

The Inventory aggregate is the only module allowed to change physical stock.

Business modules request inventory operations.

The Inventory aggregate decides whether those operations are valid and applies the resulting stock changes.

---

# Historical Integrity

Inventory maintains the current operational state of stock.

Historical business documents remain owned by the modules that created them.
