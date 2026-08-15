# Commands

## Purpose

This document describes the write operations supported by the Medicine module.

Commands express business intentions that modify the pharmaceutical definition of a medicine.

The Medicine module owns only pharmaceutical information.

Commercial information is managed by the Product module.

---

# Supported Operations

The Medicine module supports operations such as:

- Creating a Medicine
- Updating pharmaceutical information
- Managing drug compositions

Each command enforces the business rules owned by the Medicine aggregate.

---

# Module Boundary

Medicine commands never modify:

- Product commercial information
- Packaging
- Selling units
- Barcodes
- Inventory
- Purchase
- Sales

Cross-module workflows are coordinated by the Application layer.

---

# Business Validation

Every command validates the pharmaceutical consistency of the Medicine aggregate before changes are accepted.

Invalid pharmaceutical states are rejected before persistence.
