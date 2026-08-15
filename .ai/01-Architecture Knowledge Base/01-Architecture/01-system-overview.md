# System Overview

## Purpose

This document provides a high-level overview of the Mawthooq project.

Its goal is to help both humans and AI systems quickly understand how the project is organized before diving into implementation details.

# Project Goal

Mawthooq is an AI-first knowledge platform designed to organize software engineering knowledge in a structured, traceable, and maintainable way.

The repository is optimized for both human understanding and AI reasoning.

# High-Level Architecture

The project follows a layered architecture.

API
│
Application
│
Domain
│
Infrastructure
│
SQL / External Services

Each layer has a single responsibility and communicates only through well-defined boundaries.

# Core Principles

- Domain drives the design.
- Knowledge is the primary asset.
- Architecture is documented before implementation.
- AI should understand the project with minimal context.
- Every important decision is documented.

# Main Components

- **Constitution** - Permanent project rules.
- **Architecture** - System design and structure.
- **Domain** - Business knowledge and domain model.
- **Application** - Use cases and application workflows.
- **Infrastructure** - Technical implementation.
- **API** - External interfaces.
- **SQL** - Database design.
- **ADR** - Architectural decisions.
- **Templates** - Reusable document templates.
- **Checklists** - Review and validation guides.

# Design Philosophy

The project favors:

- Simplicity
- Explicit design
- Traceability
- Consistency
- Incremental evolution

Complexity should only be introduced when it solves a real problem.

# Reading Order

For new contributors or AI agents:

- README
- Constitution
- Architecture
- Domain
- Application
- Infrastructure
- ADRs

This order provides the necessary context before implementation.
