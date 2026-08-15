# Product Evolution

## Purpose

This document defines how a Product evolves throughout its lifetime.

Mawthooq distinguishes between correcting existing information and introducing a new commercial definition.

This distinction preserves historical business correctness.

---

# Evolution Philosophy

A Product is expected to evolve as business requirements change.

Not every change has the same business meaning.

Some changes correct existing information.

Others create a new commercial definition.

These two cases are treated differently.

---

# Business Correction

A business correction fixes information that was incorrect.

The business identity of the product does not change.

Examples include:

- Correcting a spelling mistake
- Correcting a product description
- Fixing invalid metadata

Business corrections update the existing Product.

Historical meaning remains unchanged.

---

# Commercial Evolution

Commercial evolution changes how the product is represented by the business.

These changes create a new commercial definition.

Examples include:

- Packaging structure changes
- Selling unit hierarchy changes
- Unit conversion changes

Commercial evolution preserves previous definitions by creating a new Packaging Version.

---

# Historical Preservation

Business records always remain associated with the commercial definition that existed when the business event occurred.

Existing historical definitions are never rewritten.

---

# Immutable History

Historical commercial definitions are preserved.

Future changes never modify previously used definitions.

This guarantees consistent reporting and historical traceability.

---

# Aggregate Responsibility

The Product aggregate decides whether a requested change is:

- A business correction
- A commercial evolution

Other modules never make this decision.

---

# Related Documentation

- aggregate.md
- packaging.md
- lifecycle.md
