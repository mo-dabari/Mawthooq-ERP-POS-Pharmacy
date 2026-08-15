# Mawthooq Business Architecture

## Purpose

This document defines the architectural foundation of the Mawthooq Pharmacy ERP system.

It explains the business architecture principles that govern:

* Module boundaries.
* Aggregate design.
* Ownership.
* Source of truth.
* Historical consistency.
* Cross-module communication.

This document is the primary architectural reference for future development decisions.

It does not describe implementation details.

Implementation details belong to their respective layers and modules.


# Architecture Philosophy

## Business Responsibility First

Mawthooq is designed around business responsibility.

Every architectural decision starts with one question:

> Who owns this business decision?

The answer determines:

* Module boundaries.
* Aggregate boundaries.
* Data ownership.
* Event ownership.

---

## Business Before Technology

The domain model is discovered from business concepts and workflows.

Technical structures such as:

* Database tables.
* APIs.
* Frameworks.
* Persistence mechanisms.

do not define the business model.

---

## Domain Before Database

The database represents the domain model.

The domain model is not created by mapping database structures.

Business concepts exist because they have business meaning, not because they require storage.

---

## Business Language Is the Source of Truth

Domain concepts use business terminology.

The model should represent concepts understood by the business, not technical abstractions.

# Business Capability Model

## Overview

Mawthooq is organized around business capabilities.

A module exists because it represents a distinct business responsibility.

Modules are not separated by:

* Database structure.
* User interface screens.
* Technical services.
* CRUD operations.

Each module owns a specific business capability and protects its own rules.

---

# Module Classification

Mawthooq modules are classified into five business categories.

---

# 01 — Operational Modules

Operational modules execute the main pharmacy business workflows.

They represent activities that change the operational state of the business.

Characteristics:

* Own operational decisions.
* Apply business rules.
* Produce business events.
* Manage evolving business state.

Modules:

* Product
* Medicine
* Inventory
* Purchase
* Sales

---

# 02 — Business Partners

Business Partner modules represent external entities that the business interacts with.

They answer:

> Who does the business deal with?

Modules:

* Customer
* Supplier

Characteristics:

* Maintain current partner information.
* Do not own operational transactions.
* Historical documents preserve their own snapshots.

---

# 03 — Organizational Modules

Organizational modules represent internal business structure.

They answer:

> Where did the operation happen?

Module:

* Branch

Characteristics:

* Represents operational context.
* Does not represent transaction ownership.
* Documents keep Branch identity for historical context.

---

# 04 — Reference Modules

Reference modules provide shared business identities used by operational modules.

Modules:

* Active Ingredient
* Manufacturer
* Therapeutic Class

Characteristics:

* Provide reference information.
* Do not execute operational workflows.
* Are referenced through identity.

---

# 05 — Financial

Financial is a separate business capability.

It represents the Financial Recording Layer of the ERP.

Responsibilities:

* Receive completed business facts.
* Record financial consequences.
* Preserve financial history.

It is not a complete accounting system.

# Domain Modeling Approach

## Aggregate Boundaries

Aggregates are designed according to business responsibility.

An Aggregate exists where the business requires:

* Consistency.
* Invariant protection.
* Controlled state changes.

Aggregate boundaries are not determined by:

* Database relationships.
* Foreign keys.
* Object navigation.
* Table grouping.

---

## Aggregate Ownership

Each Aggregate has clear ownership over its internal consistency.

The Aggregate Root controls:

* Creation.
* Modification.
* Validation.
* Lifecycle of internal entities.

Example:

```
Medicine
    |
    └── MedicineIngredient
```

Medicine owns the lifecycle and rules of MedicineIngredient.

---

## Aggregate Lifecycle

Aggregates are classified according to how they evolve.

### Mutable Aggregates

Represent business objects that change over time.

Characteristics:

* Stable identity.
* Controlled state transitions.
* Business rules protect changes.

Examples:

* Product
* Medicine
* Inventory
* Customer
* Supplier
* Branch

---

### Immutable Aggregates

Represent completed business facts.

Characteristics:

* Created once.
* Never rewritten.
* Corrections create new facts.

Example:

* FinancialRecord

---

## Lifecycle Decision Rule

The lifecycle of an Aggregate is a business decision.

It is not chosen because of technical convenience.


# Ownership and Source of Truth

## Ownership Principle

Every business concept has one clear owner.

Ownership determines:

* Who creates the information.
* Who validates its rules.
* Who controls its lifecycle.
* Who is responsible for maintaining consistency.

Ownership is based on business responsibility, not technical structure.

---

# Ownership Types

## Aggregate Ownership

An Aggregate Root owns its internal entities and protects their consistency.

Example:

```text
Medicine
    |
    └── MedicineIngredient
```

Medicine owns the lifecycle and business rules of MedicineIngredient.

Another example:

```text
Inventory
    |
    └── Batch
```

Inventory owns batch availability and FEFO decisions.

---

## Relationship Ownership

A relationship belongs to the Aggregate responsible for the business meaning of that relationship.

Example:

```text
Medicine

ManufacturerId
```

Medicine owns the relationship because it defines the context in which the manufacturer is used.

The Manufacturer module only owns manufacturer information.

---

## Snapshot Ownership

Historical snapshots belong to historical documents.

They preserve the state of external information at the time of the transaction.

Examples:

```text
SalesOrder

CustomerSnapshot
```

```text
PurchaseOrder

SupplierSnapshot
```

The Customer and Supplier Aggregates own current information only.

They do not own historical snapshots.

---

# Source of Truth

Every business concept has one authoritative source.

Other representations are:

* References.
* Snapshots.
* Read Models.
* Derived information.

They do not replace the original source.

---

# Source of Truth Examples

| Business Concept      | Source of Truth |
| --------------------- | --------------- |
| Product Definition    | Product         |
| Medicine Composition  | Medicine        |
| Inventory State       | Inventory       |
| Purchase Agreement    | PurchaseOrder   |
| Received Goods        | GoodsReceipt    |
| Sales Transaction     | SalesOrder      |
| Financial Impact      | FinancialRecord |
| Current Customer Data | Customer        |
| Current Supplier Data | Supplier        |
| Branch Information    | Branch          |

---

# Reference Rule

References transfer knowledge, not ownership.

Example:

```text
Medicine
    |
    └── ActiveIngredientId
```

Medicine knows which Active Ingredient is used.

However:

* ActiveIngredient owns its own definition.
* Medicine owns the composition relationship.

---

# Ownership Validation Rule

Before adding any new concept, answer:

1. Who owns this business decision?
2. Who validates its rules?
3. Who controls its lifecycle?
4. Where is the source of truth?

If these questions cannot be answered, the design boundary is not clear yet.

# Historical Consistency

## Historical Truth Principle

Mawthooq treats historical information as a business fact.

The system must preserve what was true at the time a business operation occurred.

Current data changes must never invalidate historical records.

---

# Current Truth vs Historical Truth

## Current Truth

Represents the current state of a business concept.

Examples:

* Current Customer information.
* Current Supplier information.
* Current Product definition.
* Current Branch information.

These values may evolve over time.

---

## Historical Truth

Represents the state of information when a business event occurred.

Examples:

* Customer information on a Sales Order.
* Supplier information on a Purchase Order.
* Financial impact of a completed operation.

Historical information belongs to the document or fact that records the event.

---

# Historical Preservation Strategies

## Snapshots

Snapshots are used when a transaction requires preserving external information at a specific point in time.

Examples:

```text
SalesOrder

CustomerSnapshot
```

```text
PurchaseOrder

SupplierSnapshot
```

Changing the original Customer or Supplier later does not modify historical documents.

---

## Immutable Business Facts

Some information represents completed business facts.

These records are never rewritten.

Examples:

* FinancialRecord.
* Inventory historical movements.

If a correction is required, a new fact is created instead of modifying the old one.

---

## Identity Preservation

Some historical context requires preserving identity rather than copying full information.

Example:

```text
SalesOrder

BranchId
```

The purpose is to answer:

> Where did the operation happen?

Not:

> What was the branch name at that time?

---

# Historical Rules

## Rule 1

The past is a business fact, not a database state.

---

## Rule 2

Current information and historical information serve different purposes.

---

## Rule 3

Historical documents own their historical representation.

---

## Rule 4

Reference data changes must not rewrite operational history.

---

# Avoided Approach

Mawthooq does not reconstruct history using current master data.

Example:

Incorrect:

```text
Old Sales Order

+

Current Customer

+

Current Product

=

Historical Record
```

The current state may no longer represent the reality at the time of the transaction.

---

# Historical Consistency Goal

The system must always be able to answer:

> What exactly happened at that moment?

without depending on future changes.

# Event-Driven Communication

## Event Principle

Events represent completed business facts.

An Event describes something that has already happened.

It is not a command and does not request an action.

---

# Command vs Event

## Command

Represents business intent.

Example:

```text id="c1v8qz"
Complete Sales Order
```

Meaning:

> The system should perform this action.

---

## Event

Represents a completed business fact.

Example:

```text id="k2r4sm"
SalesOrderCompleted
```

Meaning:

> This action has already happened.

---

# Event Types in Mawthooq

Mawthooq uses different event concepts depending on their responsibility.

---

# Domain Events

Domain Events are created by Aggregates when an important business action occurs.

Characteristics:

* Created inside the Domain.
* Represent meaningful business facts.
* Do not contain technical implementation details.

Examples:

* SalesOrderCompleted.
* GoodsReceiptCompleted.
* ProductDiscontinued.

---

# Integration Events

Integration Events communicate business facts between modules.

They allow modules to remain independent.

Example:

```text id="f9n3p1"
Sales

    ↓

SalesOrderCompleted

    ↓

Inventory
Financial
```

Sales does not directly control Inventory or Financial behavior.

It only publishes the fact that a sale was completed.

---

# Persistence Events

Persistence Events belong to the Infrastructure layer.

They are used to support persistence strategies such as event-based persistence.

Example:

```text id="h8m5x2"
ProductPackagingChanged

        ↓

Repository Dispatcher

        ↓

Execute Database Operation
```

They are not Domain Events and must not affect business modeling.

---

# Event Ownership

The Aggregate that owns a business decision creates the related Domain Event.

Examples:

SalesOrder creates:

```text id="b6z9kf"
SalesOrderCompleted
```

Inventory does not create this event.

Inventory reacts because the business fact affects inventory.

---

# Event Communication Rule

Modules communicate through business facts, not through internal objects.

Incorrect:

```text id="q7s4kd"
SalesService
    calls
InventoryService
```

Correct:

```text id="z2m8wp"
SalesOrderCompleted

        ↓

Inventory reacts
```

---

# Event Sourcing Decision

Mawthooq is not an Event Sourcing system.

Events are used for:

* Communication.
* Coordination.
* Triggering reactions.

They are not the primary source of persisted state.

The source of truth remains:

* Aggregate state.
* Historical business records where required.

---

# Event Flow Model

The general flow is:

```text id="r4m8cz"
Business Intent

        ↓

Application Command

        ↓

Aggregate Behavior

        ↓

Domain Event

        ↓

Module Reaction

        ↓

New Business Fact
```

---

# Event Architecture Goal

Events allow modules to evolve independently while preserving clear ownership boundaries.

The producer owns the business decision.

The consumer decides how to react.

# Application Interaction Model

## Application Layer Responsibility

The Application Layer coordinates business workflows.

It acts as the entry point between external requests and the Domain Model.

Its responsibility is to:

* Receive business intent.
* Execute application workflows.
* Load required Aggregates.
* Invoke domain behavior.
* Manage transaction boundaries.
* Publish resulting events.

---

# Application Does Not Own Business Rules

Business decisions belong to the Domain.

The Application Layer should not contain:

* Business validations.
* Domain policies.
* State transition rules.
* Ownership decisions.

Example:

Incorrect:

```text id="a6q9wk"
if (Batch.Quantity > 0)
{
    SellProduct();
}
```

The decision of selecting and validating available stock belongs to Inventory.

---

# Domain Responsibility

The Domain Model owns:

* Business invariants.
* Business rules.
* State transitions.
* Aggregate consistency.

The Application Layer only coordinates the execution.

---

# Workflow Coordination

A business workflow may involve multiple business capabilities.

Example:

Completing a Sale:

```text id="b8n4mv"
SalesOrder

        ↓

SalesOrderCompleted Event

        ↓

Inventory Reaction

        ↓

Financial Recording
```

Each module remains responsible for its own decisions.

---

# Module Interaction Rule

Modules should not depend on internal implementation details of other modules.

Incorrect:

```text id="p3w8zy"
Sales

directly modifies

Inventory
```

Correct:

```text id="x5k9qs"
Sales

publishes business fact

↓

Inventory decides its own reaction
```

---

# Transaction Boundaries

Transaction boundaries follow business workflows.

They are not determined by:

* Database tables.
* Repository methods.
* Technical implementation.

---

# Aggregate Transaction

An Aggregate protects consistency inside its own boundary.

Example:

```text id="m7f2kc"
Medicine

    └── MedicineIngredient
```

Changes inside this boundary are handled by the Aggregate itself.

---

# Application Transaction

An Application Transaction is required when a workflow involves multiple persistence operations that must succeed or fail together.

Example:

```text id="r9h3vw"
Complete Sale

    ↓

Save SalesOrder

    ↓

Update Inventory

    ↓

Create FinancialRecord
```

---

# Unit Of Work Rule

Unit Of Work is an application-level coordination mechanism.

It is used when the workflow requires multiple persistence operations within one consistency boundary.

It is not required simply because a Repository exists.

---

# Consistency Model

Mawthooq uses two consistency approaches:

## Immediate Consistency

Used inside an Aggregate.

Example:

```text id="c8m5nd"
Medicine

MedicineIngredient
```

---

## Eventual Consistency

Used between independent Modules.

Example:

```text id="z6p1tr"
Sales

↓

Inventory

↓

Financial
```

Each module maintains its own business consistency.

---

# Application Interaction Principle

The final rule:

> Application coordinates, Domain decides.

The Application Layer organizes the workflow.

The Domain Layer owns the business meaning.


# Financial Recording Layer

## Purpose

Mawthooq contains a Financial Recording Layer that records the financial consequences of completed business operations.

It supports ERP operations while allowing future integration with a complete accounting system.

---

# Financial Boundary

Financial is not responsible for executing business operations.

Operational modules decide:

* What happened.
* When it happened.
* Why it happened.

Financial records:

* The financial impact of what happened.

---

# Operational Truth vs Financial Truth

Operational modules represent business execution.

Examples:

* Sales completed a transaction.
* Purchase received goods.
* Inventory changed stock.

Financial records the resulting financial facts.

---

# FinancialRecord Role

FinancialRecord represents:

> A financial fact generated from a completed business operation.

It is not:

* A Sales Order.
* A Purchase Document.
* A complete accounting entry.
* A General Ledger replacement.

---

# Financial Ownership

Financial owns:

* Financial facts.
* Financial history.
* Financial recording rules.

It does not own:

* Product decisions.
* Sales decisions.
* Inventory decisions.
* Purchase decisions.

---

# Future Accounting Integration

Mawthooq can integrate with a complete accounting system through financial facts.

The future flow:

```text id="k4s8nd"
Operational Modules

        ↓

Financial Recording Layer

        ↓

Accounting System
```

The ERP remains independent from accounting implementation details.

---

# Financial Architecture Principle

- Financial records business consequences; it does not control business operations.


# Architecture Evolution Rules

## Purpose

These rules define how future features and modules should evolve without violating the established architecture.

---

# Rule 1 — Respect Business Ownership

Any new capability must have a clear answer to:

> Who owns this business decision?

If ownership is unclear, the design boundary is incomplete.

---

# Rule 2 — Protect Aggregate Boundaries

New features must not bypass Aggregate rules.

External components should not directly modify another Aggregate's internal state.

---

# Rule 3 — Maintain Source of Truth

New data should have one authoritative owner.

Duplicated information must have a clear purpose:

* Snapshot.
* Reference.
* Projection.
* Cache.

---

# Rule 4 — Preserve Historical Truth

New changes must not rewrite historical business facts.

Historical records must remain valid regardless of future master data changes.

---

# Rule 5 — Prefer Business Events for Module Communication

Modules should communicate through business facts rather than direct dependencies.

---

# Rule 6 — Avoid Premature Generalization

New abstractions should be introduced only when a real business capability requires them.

The domain model evolves from business needs, not technical assumptions.

---

# Rule 7 — Technology Must Serve the Domain

Infrastructure decisions may change.

Examples:

* Database technology.
* Persistence mechanism.
* Frameworks.

The business model should remain stable.

---

# Final Architecture Statement

Mawthooq is built around clear business ownership.

Modules represent business capabilities.

Aggregates protect business consistency.

Events communicate completed business facts.

Historical records preserve business truth.

Financial recording captures business consequences.

Future evolution must preserve these principles.
