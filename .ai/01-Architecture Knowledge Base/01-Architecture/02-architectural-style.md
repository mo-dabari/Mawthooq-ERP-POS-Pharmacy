# Architectural Style

## Purpose

This document describes the architectural style used throughout the Mawthooq project.

It explains the design philosophy rather than implementation details.

# Primary Architecture

The project follows **Clean Architecture**.

Responsibilities are separated into independent layers to reduce coupling and improve maintainability.

# Design Philosophy

The architecture is designed around the business domain.

Technology supports the domain-it never dictates it.

Business rules remain independent of frameworks, databases, and external services.

# Core Architectural Principles

- Domain-first design.
- Clear separation of responsibilities.
- Explicit dependencies.
- Stable boundaries.
- High cohesion.
- Low coupling.

# Layer Overview

## Domain

Contains the business model.

The Domain has no knowledge of infrastructure, databases, APIs, or frameworks.

## Application

Coordinates use cases.

It orchestrates business operations but does not contain business rules.

## Infrastructure

Implements technical concerns such as:

- Database access
- File storage
- External services
- Messaging
- Logging

Infrastructure depends on the inner layers-not the reverse.

## API

Acts as the entry point to the system.

Its responsibility is to translate external requests into application use cases.

# Dependency Rule

Dependencies always point inward.

API
↓
Application
↓
Domain
<br/>Infrastructure ─────► Domain
Infrastructure ─────► Application

The Domain must never depend on any outer layer.

# Architectural Goals

The architecture aims to achieve:

- Maintainability
- Testability
- Scalability
- Explicit design
- Long-term evolution

# Out of Scope

This document does not describe:

- Folder structure
- Database schema
- Domain model
- Coding conventions
- Implementation details

These topics are documented elsewhere.
