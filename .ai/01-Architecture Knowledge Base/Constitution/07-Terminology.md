# Constitution 007 - Terminology

**Status:** Active
**Version:** 1.0
**Priority:** Highest

# Purpose

This constitution establishes the official vocabulary of the Mawthooq AI Knowledge Base.

Its purpose is to ensure that every human contributor and every AI system uses the same terminology with the same meaning throughout the repository.

Consistent language is essential for architectural integrity, deterministic AI reasoning, and long-term maintainability.

# Terminology Principles

The official terminology of the project follows these principles:

- Every concept has one preferred name.
- Every preferred name has one official meaning.
- Different concepts must not share the same term.
- The same concept must not be referred to by multiple interchangeable names unless explicitly declared as aliases.
- Definitions should remain stable over time.

# Official Terms

## Constitution

A constitutional document that defines permanent project-wide rules, principles, or governance.

Constitutions represent the highest level of authority within the repository.

## Knowledge

Information that contributes to understanding, designing, implementing, operating, or evolving the project.

Knowledge is the primary asset of the repository.

## Knowledge Unit

The smallest independently meaningful piece of knowledge.

A Knowledge Unit represents one primary concept.

## Principle

A long-lived architectural or organizational truth that guides decision-making.

Principles are intentionally stable.

## Rule

A mandatory requirement that defines required or prohibited behavior.

Rules are normative.

Violating a rule is considered non-compliant.

## Standard

A documented specification describing how work should be performed consistently.

Standards promote uniformity across the project.

## Guideline

A recommended practice.

Guidelines allow justified exceptions.

They do not establish mandatory behavior.

## Decision

A documented and approved project choice.

Only accepted decisions are considered authoritative.

## Architecture

The intentional organization of the project, including its structure, relationships, boundaries, and governing principles.

Architecture is independent of implementation technology.

## Repository

The complete collection of project artifacts maintained under version control.

The repository contains both implementation and knowledge assets.

## Source of Truth

The single authoritative location that owns a specific piece of knowledge.

Only one Source of Truth may exist for any authoritative definition.

## Traceability

The ability to understand why something exists by following documented relationships between knowledge, decisions, requirements, and implementation.

## Constraint

A limitation that restricts available choices.

Constraints describe boundaries rather than preferences.

## Requirement

A capability or condition that the project must satisfy.

Requirements define expected outcomes.

## Pattern

A reusable solution to a recurring problem within a defined context.

Patterns describe proven approaches rather than mandatory rules.

## Anti-Pattern

A commonly recurring approach that consistently produces undesirable outcomes.

Anti-Patterns should explain both the problem and the preferred alternative.

## AI Agent

Any artificial intelligence system that analyzes, generates, modifies, reviews, or reasons about project artifacts.

The term is independent of vendor, implementation, or deployment model.

## Contributor

Any person or AI Agent that creates or modifies repository content.

All contributors are expected to follow the constitutional rules.

## Artifact

Any version-controlled project asset.

Examples include:

- source code
- documentation
- architectural decisions
- diagrams
- templates
- configuration
- metadata

## Document

A structured artifact whose primary purpose is to communicate knowledge.

Documents may be normative or informative.

## ADR

Architecture Decision Record.

A structured document that preserves an important architectural decision together with its context, rationale, alternatives, consequences, and status.

## Metadata

Structured descriptive information about an artifact.

Metadata improves discoverability, organization, validation, and automation.

## Relationship

An explicitly documented connection between two or more Knowledge Units or artifacts.

Relationships express semantic meaning rather than simple navigation.

## Lifecycle

The sequence of states through which an artifact progresses during its existence.

Examples include Draft, Approved, Deprecated, and Archived.

## Authority

The level of governance assigned to a document or artifact.

Higher-authority documents take precedence over lower-authority documents in case of conflict.

# Normative Language

The following keywords have precise meanings throughout the repository.

## MUST

Indicates an absolute requirement.

No exceptions are permitted unless explicitly documented.

## MUST NOT

Indicates an absolute prohibition.

## REQUIRED

Equivalent in intent to MUST.

## SHOULD

Indicates a strong recommendation.

Exceptions are allowed only with documented justification.

## SHOULD NOT

Indicates a strong recommendation against a practice.

Exceptions require justification.

## MAY

Indicates an optional capability or behavior.

## OPTIONAL

Indicates that implementation is left to project discretion.

# Aliases

Aliases should be avoided whenever possible.

If an official alias is introduced, it must:

- be explicitly documented,
- preserve the original meaning,
- never introduce ambiguity.

# Deprecated Terminology

Obsolete terms should not be removed immediately.

Deprecated terminology should:

- remain documented,
- identify the preferred replacement,
- preserve historical understanding.

# Terminology Evolution

New terms may be introduced only when:

- an existing term is insufficient,
- the concept is genuinely new,
- introducing the term improves clarity.

New terminology should minimize overlap with existing vocabulary.

# Consistency Requirement

All repository artifacts must use the official terminology defined in this constitution.

Alternative wording must not redefine official meanings.

# Authority

This constitution defines the official terminology of the Mawthooq AI Knowledge Base.

All constitutional documents, standards, templates, documentation, implementation guidance, and AI-generated artifacts must use these definitions unless a newer constitutional document explicitly supersedes them.
