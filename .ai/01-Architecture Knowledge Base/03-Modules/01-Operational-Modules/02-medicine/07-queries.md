# Queries

## Purpose

This document describes the information exposed by the Medicine module.

Queries provide pharmaceutical information without modifying business state.

---

# Available Information

The Medicine module provides information such as:

- Medicine identity
- Medicine name
- Dosage form
- Reference quantity
- Therapeutic classification
- Manufacturer
- Drug compositions

---

# Query Characteristics

Queries are read-only.

They never modify pharmaceutical information.

---

# Module Boundary

Medicine queries expose only pharmaceutical information.

Commercial information is obtained from the Product module when required.

Inventory information is obtained from the Inventory module.

---

# Historical Consistency

Queries always return pharmaceutical information consistent with the current business definition of the Medicine.
