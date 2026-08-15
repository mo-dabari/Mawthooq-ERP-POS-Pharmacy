# Decision Rules

## Purpose

This document explains the architectural decisions behind the TherapeuticClass aggregate.

It focuses on *why* the aggregate is designed this way rather than *how* it is implemented.

---

# Why Is Therapeutic Class a Separate Aggregate?

A therapeutic class has its own business identity.

Many medicines may belong to the same therapeutic class.

Keeping therapeutic classes in a dedicated aggregate establishes a single source of truth and eliminates duplicated classifications across medicines.

---

# Why Does Medicine Reference Only TherapeuticClassId?

Aggregate references are made only through identifiers.

This keeps aggregate boundaries independent and avoids direct object references between aggregates.

The Medicine aggregate only needs to know the primary therapeutic classification assigned to the medicine.

---

# Why Does Therapeutic Class Not Know Which Medicines Belong to It?

Relationships are intentionally one-way.

Medicine references TherapeuticClass.

TherapeuticClass has no knowledge of the medicines that reference it.

Queries requiring this information belong to the read model.

---

# Why Does Each Medicine Have Only One Therapeutic Class?

The system adopts a **Primary Therapeutic Classification** model.

The therapeutic class is used as a business classification for:

* Search
* Filtering
* Reporting
* Analytics
* Data organization

It is not intended to represent every possible clinical indication or medical use of a medicine.

This keeps the domain model simple while satisfying the business requirements of the ERP system.

---

# Why Doesn't Therapeutic Class Store Clinical Information?

The aggregate identifies a business classification.

It does not model:

* Diseases
* Treatment protocols
* Medical indications
* Clinical guidelines
* Drug interactions

These concerns belong to specialized medical knowledge, not to the ERP domain.

---

# Why Is Deactivation Preferred Over Deletion?

Medicines may already reference a therapeutic class.

Deleting it would break historical consistency and invalidate existing references.

Marking the class as inactive preserves historical data while preventing future use.

---

# Why Is This Aggregate Intentionally Small?

TherapeuticClass is a reference catalog.

Its responsibility is to provide a stable business classification—not to execute business workflows or manage medicine behavior.

Keeping the aggregate small improves maintainability, scalability, and transactional consistency.

---

# Design Summary

The TherapeuticClass aggregate exists to provide a stable primary business classification for medicines.

All operational behavior remains inside the Medicine aggregate and other operational modules.
