# Medical Identity

## Purpose

This document defines how Mawthooq identifies medicines from a pharmaceutical perspective.

Medical identity is independent from commercial identity.

---

# Philosophy

A medicine has two different identities.

The commercial identity answers:

> What product does the pharmacy sell?

The medical identity answers:

> What medicine is this?

These identities complement each other but represent different business concepts.

---

# Medical Identity

The medical identity of a medicine is defined by its pharmaceutical characteristics.

These include:

- Medicine Name
- Dosage Form
- Active Ingredients
- Ingredient Amounts
- Therapeutic Classification
- Manufacturer
- Reference Quantity

Together these characteristics describe the medicine independently from how it is packaged or sold.

---

# Commercial Identity

Commercial identity belongs to the Product module.

It includes concepts such as:

- Trade Name
- Packaging
- Selling Units
- Commercial Barcodes

Commercial identity does not replace medical identity.

---

# Relationship

Every Medicine is associated with exactly one Product.

The Product represents the commercial view.

The Medicine represents the pharmaceutical view.

Business operations may require one or both identities depending on the use case.

---

# Business Examples

Different Products may represent the same medicine.

Examples include:

- Panadol Extra
- Panadol Advance
- Generic alternatives

Although these Products differ commercially, they may share pharmaceutical characteristics.

Likewise, a single medicine may be marketed under different trade names by different manufacturers.

---

# Aggregate Ownership

Medical identity is owned entirely by the Medicine aggregate.

Commercial identity is owned entirely by the Product aggregate.

Neither aggregate modifies the identity owned by the other.

---

# Related Documentation

- aggregate.md
- drug-compositions.md
