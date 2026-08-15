# Commands

## Purpose

This document describes the write operations supported by the Product module.

Commands express business intentions that modify the commercial definition of a product.

---

# Supported Operations

The Product module supports operations such as:

- Creating a Product
- Updating commercial information
- Creating a new Packaging Version
- Managing selling units
- Managing product barcodes
- Changing the product lifecycle

Each command is responsible for enforcing the business rules owned by the Product aggregate.

Commands never modify Inventory, Medicine, Purchase, or Sales directly.

Cross-module workflows are coordinated by the Application layer.
