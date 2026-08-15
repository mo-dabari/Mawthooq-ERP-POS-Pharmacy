# Events

## Purpose

This document defines the business events produced by the Inventory aggregate.

Inventory events represent completed changes to the physical state of inventory.

---

# Event Ownership

Only the Inventory aggregate may produce Inventory events.

Other modules never produce Inventory events on its behalf.

---

# Event Categories

Inventory events are produced when the Inventory aggregate successfully applies operations such as:

- Inventory receipt
- Inventory consumption
- Inventory adjustment
- Selling price changes

---

# Persistence

Inventory business events are translated by the Infrastructure layer into persistence operations.

The Inventory aggregate never performs persistence directly.

---

# Integration

Inventory events may participate in larger business workflows coordinated by the Application layer.

The aggregate remains responsible only for inventory-related business events.

---

# Related Documentation

- aggregate.md
- decision-rules.md
