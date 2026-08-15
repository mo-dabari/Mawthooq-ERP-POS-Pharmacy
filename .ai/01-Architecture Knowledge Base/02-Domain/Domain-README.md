# Domain

## Purpose

This directory documents the business domain of the Mawthooq project.

Its purpose is to describe the business model, business rules, and domain concepts that drive the system.

This documentation focuses on **how Mawthooq models its business**, not on explaining Domain-Driven Design (DDD) theory.

# Objectives

The Domain documentation helps contributors and AI agents:

- Understand the business model.
- Understand why Aggregates are designed the way they are.
- Follow the project's business rules.
- Maintain domain consistency.
- Extend the model without breaking existing behavior.

# Documents

## 01-business-overview

Introduces Mawthooq from a business perspective.

---

## 02-bounded-contexts

Defines the major business areas of the system.

---

## 03-ubiquitous-language

Defines the shared business terminology used throughout Mawthooq.

---

## 04-aggregates

Defines the business consistency boundaries and responsibilities of each aggregate.

---

## 05-domain-model

Describes the major business concepts and how they relate to one another.

# Scope

This directory documents:

- Business overview
- Bounded Contexts
- Ubiquitous Language
- Aggregates
- Entities
- Value Objects
- Domain Services
- Domain Events
- Business Rules
- Invariants
- Domain modeling guidelines

Implementation details belong outside this directory.

# Reading Order

Read the documents in the following order:

- Business Overview
- Bounded Contexts
- Ubiquitous Language
- Aggregates
- Entities
- Value Objects
- Domain Services
- Domain Events
- Business Rules
- Invariants
- Domain Modeling Guidelines

Each document builds upon the previous one.

# Guiding Principles

The Mawthooq Domain Model follows these principles:

- Business behavior comes before data.
- Aggregates protect business consistency.
- Business rules live inside the Domain.
- Every model should reflect the language used by the business.
- Technical concerns must not influence domain design.

# Audience

This documentation is intended for:

- Backend developers
- Domain experts
- Software architects
- AI agents working on the project

# Keeping This Documentation Updated

Whenever the business model changes, the corresponding document in this directory must be updated.

The documentation should always represent the current business model of Mawthooq.
