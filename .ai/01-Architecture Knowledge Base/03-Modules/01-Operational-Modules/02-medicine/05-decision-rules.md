# Medicine Decision Rules

## Purpose

This document defines how the Medicine aggregate classifies incoming business changes.

Every requested change is evaluated before it is applied.

---

# Decision Categories

Every change belongs to one of the following categories:

- Medical Information Correction
- Pharmaceutical Composition Change
- Lifecycle Change

---

# Medical Information Correction

Medical Information Corrections update the existing pharmaceutical definition without changing the medicine itself.

Typical examples include:

- Correcting the medicine name
- Correcting the dosage form
- Correcting reference information

---

# Pharmaceutical Composition Change

Changes affecting active ingredients or ingredient amounts modify the pharmaceutical identity of the medicine.

These changes must follow the business rules defined by the Medicine aggregate.

---

# Lifecycle Change

Lifecycle changes affect the availability of the Medicine record.

They do not change its pharmaceutical identity.

---

# Aggregate Decision

The Medicine aggregate is solely responsible for classifying and validating incoming changes.

Other modules never decide how pharmaceutical information is managed.

---

# Related Documentation

- medical-identity.md
- drug-compositions.md
- lifecycle.md
