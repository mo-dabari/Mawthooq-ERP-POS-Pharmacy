# Decision Rules

## Purpose

This document explains the architectural decisions behind the Manufacturer aggregate.

It focuses on *why* the aggregate is designed this way rather than *how* it is implemented.

---

# Why Is Manufacturer a Separate Aggregate?

A Manufacturer has its own business identity.

Many medicines may be produced by the same manufacturer.

Keeping manufacturers in a dedicated aggregate eliminates duplication and establishes a single source of truth.

---

# Why Does Medicine Reference Only ManufacturerId?

Aggregate references are made only through identifiers.

This keeps aggregate boundaries independent and avoids direct object references between aggregates.

The Medicine aggregate only needs to know which manufacturer produced the medicine.

---

# Why Doesn't Manufacturer Know Which Medicines Use It?

Relationships are intentionally one-way.

Medicine references Manufacturer.

Manufacturer has no knowledge of the medicines that reference it.

This prevents unnecessary coupling and keeps the aggregate lightweight.

Queries requiring this information belong to the read model.

---

# Why Doesn't Manufacturer Store Medicine Information?

A Manufacturer describes the organization itself.

It does not own medicines, products, inventory, purchases, or sales.

Keeping those responsibilities separate preserves clear aggregate boundaries.

---

# Why Is Deactivation Preferred Over Deletion?

Medicines may already reference a Manufacturer.

Deleting the manufacturer would break historical consistency and create invalid references.

Marking the manufacturer as inactive preserves existing references while preventing future use.

---

# Why Is This Aggregate Intentionally Small?

Manufacturer is a reference catalog.

Its responsibility is to identify pharmaceutical manufacturers—not to model manufacturing operations or medicine behavior.

Keeping the aggregate small improves maintainability, scalability, and transactional consistency.

---

# Why Is Manufacturer Separate From Supplier?

Although the same organization may act as both a manufacturer and a supplier, the two concepts represent different business roles.

A Manufacturer answers:

Who produced the medicine?

A Supplier answers:

Who supplied the medicine to us?

These responsibilities belong to different business contexts and therefore remain separate aggregates.

---

# Design Summary

The Manufacturer aggregate exists to provide a stable business identity for pharmaceutical manufacturers.

Operational aggregates may reference it, but all business behavior related to medicines, purchasing, inventory, and sales remains outside its boundary.
