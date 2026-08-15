# CQRS in Mawthooq

## Purpose

This document describes how CQRS is implemented in the Mawthooq project.

It defines the project's conventions rather than explaining CQRS as a general architectural pattern.

# Core Principle

Mawthooq separates write operations from read operations.

Commands modify the system.

Queries read the system.

A request is never both.

# Write Side

The write side is responsible for changing business state.

Typical flow:

Client
│
▼
API
│
▼
Command
│
▼
Command Handler
│
▼
Load Aggregate
│
▼
Execute Domain Behavior
│
▼
Repository.Update()
│
▼
Persistence

# Command Rules

Commands:

- represent an intention
- modify state
- execute business behavior through the Aggregate
- never contain business rules
- never execute SQL directly
- never return domain objects

A Command Handler coordinates the use case.

The Aggregate makes the business decision.

# Read Side

The read side exists only to retrieve information.

Typical flow:

Client
│
▼
API
│
▼
Query
│
▼
Query Handler
│
▼
Read Repository
│
▼
DTO

# Query Rules

Queries:

- never modify state
- never load Aggregates unless absolutely required
- return DTOs or read models
- may use SQL optimized for reading
- are independent from the write model

# Separation

Write and read models have different responsibilities.

The write model protects business rules.

The read model optimizes data retrieval.

They are allowed to evolve independently.

# Business Rules

Business rules always belong to the Domain.

They never belong to:

- API
- Command Handlers
- Query Handlers
- Infrastructure

Handlers coordinate.

Aggregates decide.

# Repositories

Write operations use Aggregate Repositories.

Read operations use Read Repositories.

The read side must never reuse the write repository simply for convenience.

# Transactions

Transactions belong to the write side.

Queries should not start transactions unless explicitly required by infrastructure concerns.

# Domain Events

Aggregates may produce Domain Events while executing business behavior.

Persistence of those events is handled by the Infrastructure according to the project's persistence strategy.

# Goal

CQRS in Mawthooq exists to achieve:

- clear separation of responsibilities
- explicit business behavior
- simpler maintenance
- independent optimization of reads and writes
- a rich and protected domain model
