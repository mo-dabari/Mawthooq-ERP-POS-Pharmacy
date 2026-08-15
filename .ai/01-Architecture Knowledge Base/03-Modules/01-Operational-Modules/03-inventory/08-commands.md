# Commands

## Purpose

This document describes the write operations supported by the Inventory module.

Commands express business intentions that modify the physical state of inventory.

The Inventory module owns all modifications to physical stock.

---

# Supported Operations

The Inventory module supports operations such as:

- Receiving inventory
- Consuming inventory
- Adjusting inventory
- Updating selling prices

Each command validates inventory business rules before applying any changes.

---

# Batch Allocation

Commands never specify which inventory batches should be used.

The Inventory aggregate performs batch allocation according to its allocation strategy.

---

# Module Boundary

Inventory commands never modify:

- Product definition
- Medical information
- Purchase documents
- Sales documents
- Financial information

Cross-module workflows are coordinated by the Application layer.

---

# Business Validation

Every command validates inventory consistency before changing stock.

Invalid inventory operations are rejected before persistence.
