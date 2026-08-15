# Read Model

## Purpose

This document describes how the read side is implemented in the Mawthooq project.

The goal of the read model is to retrieve data efficiently without executing business behavior.

# Core Principle

The read side exists only to answer queries.

It never changes business state.

Performance and simplicity are prioritized over domain object reconstruction.

# Read Flow

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
SQL
│
▼
DTO

# Query Handlers

A Query Handler is responsible for:

- Receiving a Query.
- Calling the appropriate Read Repository.
- Returning the requested data.

A Query Handler must not:

- Modify business state.
- Execute domain behavior.
- Raise Domain Events.
- Call Write Repositories.

# Read Repositories

Read Repositories are optimized for data retrieval.

They:

- Execute SQL optimized for reading.
- Return DTOs or read models.
- Do not construct Aggregates.
- Do not execute business rules.

# DTOs

Queries always return DTOs.

DTOs are designed for consumers of the API.

They are not part of the Domain Model.

# SQL Strategy

Read queries may:

- Use JOINs.
- Select only required columns.
- Use database-specific optimizations.
- Shape data specifically for the requested response.

Read-side SQL is optimized for retrieval, not for preserving the domain model.

# Aggregate Usage

Queries should not load Aggregates.

Aggregates exist to protect business rules during state changes.

Reading data does not require Aggregate reconstruction.

# Design Goal

The Read Model is intentionally independent from the Write Model.

This allows read performance to improve without affecting business behavior or domain integrity.
