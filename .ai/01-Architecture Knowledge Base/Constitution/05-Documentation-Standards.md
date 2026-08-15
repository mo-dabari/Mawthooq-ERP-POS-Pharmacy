# Constitution 005 - Documentation Standards

**Status:** Active
**Version:** 1.0
**Priority:** Highest

# Purpose

This constitution defines the official standards for creating, maintaining, and evolving documentation within the Mawthooq AI Knowledge Base.

Every document in the repository must comply with these standards.

The objective is to ensure documentation remains consistent, authoritative, maintainable, and understandable by both humans and AI systems.

# Documentation as an Architectural Asset

Documentation is part of the system architecture.

It is not an afterthought.

Every document should improve the long-term understanding of the project.

# Single Source of Truth

Every piece of authoritative knowledge must have exactly one owning document.

Other documents may reference that knowledge but must not redefine it.

Duplicate authoritative documentation is prohibited.

# One Primary Purpose

Each document must have one clearly defined primary purpose.

A document should not attempt to solve multiple unrelated problems.

# Clear Ownership

Every document should clearly communicate:

- what it defines
- what it does not define
- when it should be consulted
- its relationship to other documents

Readers should immediately understand the document's responsibility.

# Normative vs Informative Content

Documentation is classified into two categories.

## Normative

Normative documentation defines mandatory project behavior.

Normative language includes:

- MUST
- MUST NOT
- REQUIRED
- SHALL

Normative documents establish project rules.

## Informative

Informative documentation explains, teaches, illustrates, or provides examples.

Informative language includes:

- SHOULD
- MAY
- EXAMPLE
- NOTE

Informative documents never override normative rules.

# Explicit Definitions

Definitions should be:

- precise
- concise
- unambiguous
- self-contained

Definitions must avoid circular explanations.

# Avoid Redundancy

Documentation should reference existing knowledge whenever possible.

Repeating large sections of another document should be avoided.

Updates should occur at the authoritative source rather than in every reference.

# Consistent Terminology

Official project terminology must be used consistently.

The same concept should always use the same official term.

Alternative wording should not introduce ambiguity.

# Explicit Scope

Every document should clearly state its scope.

Readers should understand:

- what is included
- what is excluded
- where responsibility ends

## Aggregate Documentation

The `aggregate.md` document defines the aggregate boundaries of a module.

If the module contains a single aggregate, the document describes that aggregate.

If the module contains multiple aggregates, the document describes each aggregate, its responsibility, its consistency boundary, and the relationships between the aggregates.

## Module Documentation

Documents inside a module describe the module as a whole rather than a single aggregate.

When a module contains a single aggregate, the documentation naturally describes that aggregate.

When a module contains multiple aggregates, the documentation explains:

- The responsibility of each aggregate.
- The consistency boundary of each aggregate.
- The relationship between the aggregates.
- How the aggregates collectively implement the module's business capability.

The documentation must preserve the independence of each aggregate and avoid presenting multiple aggregates as a single consistency boundary.

# Structured Documents

Documents should follow a predictable structure whenever practical.

Typical sections include:

- Purpose
- Scope
- Definitions
- Rules
- Examples
- References

Not every document requires every section, but structure should remain consistent.

# Examples

Examples exist to clarify understanding.

Examples are informative.

Examples must never redefine project rules.

If an example conflicts with a rule, the rule always takes precedence.

# Traceability

Important statements should be traceable to one or more of:

- constitutional documents
- architectural decisions
- requirements
- standards
- official references

Documentation should make reasoning discoverable.

# Document Relationships

Documents should reference related documents instead of duplicating their content.

Cross-references should improve navigation and understanding.

# Living Documentation

Documentation should evolve together with the project.

Outdated documentation should be updated, deprecated, or archived.

Documentation must never knowingly contradict the implementation or approved architectural decisions.

# Version Awareness

Significant documentation changes should preserve historical understanding.

Major revisions should document why the change occurred.

Historical context should not be silently removed.

# Clarity Before Brevity

Documentation should prioritize clarity over minimizing word count.

Concise writing is encouraged, but not at the expense of understanding.

# AI Readability

Documentation should be optimized for deterministic interpretation by AI systems.

Authors should minimize:

- ambiguity
- implicit assumptions
- context-dependent wording
- undefined terminology

Well-structured documents improve both human comprehension and AI reasoning.

# Internal Consistency

A document must remain internally consistent.

Contradictory statements within the same document are considered documentation defects.

# External Consistency

Documentation must remain consistent with:

- constitutional documents
- approved decisions
- project terminology
- architectural principles
- repository organization

Conflicts must be resolved explicitly.

# Update Strategy

When new knowledge belongs to an existing document, the existing document should be updated.

A new document should only be created when:

- responsibility changes,
- scope expands significantly,
- or a separate authoritative source is required.

# Deletion Policy

Documentation should rarely be permanently deleted.

When documentation becomes obsolete, it should normally be:

- Deprecated
- Archived

Historical knowledge remains valuable for future reasoning.

# Quality Characteristics

High-quality documentation is:

- accurate
- complete
- consistent
- explicit
- discoverable
- maintainable
- traceable
- reusable

These qualities should guide every documentation effort.

# Authority

This constitution defines the official documentation standards for the Mawthooq AI Knowledge Base.

All documentation, templates, generated content, contributor guides, and AI-generated artifacts must comply with these standards unless explicitly superseded by a newer constitutional document.
