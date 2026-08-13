# Mawthooq-POS

Advanced Point of Sale (POS) Management System for Pharmacies and Retail. The "Mawthooq" system is a comprehensive technology solution specifically designed to meet the needs of precision-sensitive environments such as pharmacies and supermarkets. The system is based on a robust, offline-first infrastructure with advanced cloud synchronization.

# 🧾 Retail POS System — Clean Architecture Learning Project

## 📌 Project Purpose

This repository is not just a coding exercise.
It is a **step-by-step construction of a production-ready Retail POS system** designed to:

- Build deep understanding of **Clean Architecture**
- Apply **SOLID principles in real-world scenarios**
- Simulate a **multi-branch retail environment**
- Produce a system that can later evolve into a **commercial product**
- Serve as a strong, explainable project in a professional CV

---

## 🏗️ Development Philosophy

This project is intentionally built with constraints to enforce learning:

- ❌ No ORM (No Entity Framework)
- ❌ No Design Patterns (initially)
- ✅ Pure ADO.NET
- ✅ Manual separation of responsibilities
- ✅ Architecture enforced by discipline, not frameworks
- ✅ Console Application as Presentation Layer (temporary)

The goal is to understand **why architecture exists**, not just use it.

---

## 🧠 System Scope (Phase 1)

We are building a **Retail POS Core Engine** starting with a Supermarket use case.

Supported Concepts:

- Multi-Branch Management (1 warehouse per branch)
- Inventory per Branch
- Sales Transactions
- Multi-Currency Support (EGP, USD, JOD, ILS)
- Extensible for future domains (e.g., Pharmacy)

---

## 🌳 Git Workflow Strategy (MANDATORY)

We are using a **structured GitFlow model** to simulate real team development.

---

## 🔀 Branch Types

### `main`

- Always stable
- Represents production-ready code
- No direct commits allowed

### `develop`

- Integration branch
- All completed features merge here first
- Acts as the current working version of the system

### `feature/*`

Used for building individual domain parts.

Example:

```
feature/domain-branch
feature/domain-product
feature/domain-inventory
feature/application-sale-usecase
```

Rules:

- Created from `develop`
- Merged back into `develop`
- Deleted after merge

### `release/*` (Later)

Used when preparing a version to become production-ready.

### `hotfix/*` (Later)

Used only to patch `main` if needed.

---

## 🔁 Development Lifecycle (What You MUST Follow)

### 1️⃣ Start Work

```
git checkout develop
git pull origin develop
git checkout -b feature/domain-product
```

---

### 2️⃣ Develop the Feature

Commit frequently using **meaningful messages**.

Commit Format:

```
<type>: <what was done>
```

Examples:

```
build: create Product entity with invariants
refactor: enforce encapsulation in Branch aggregate
add: initial money value object structure
fix: prevent invalid quantity mutation
```

Allowed Types:

- `build` → new structure
- `add` → new behavior
- `refactor` → improve without changing behavior
- `fix` → correct logic
- `docs` → documentation only

---

### 3️⃣ Finish Feature

```
git checkout develop
git merge feature/domain-product
git branch -d feature/domain-product
git push origin develop
```

---

### 4️⃣ Never Do This ❌

- Commit directly to `main`
- Mix multiple features in one branch
- Create "test" commits like `update`, `change`, `fix stuff`
- Push broken architecture

---

## 📐 Architecture Roadmap (Execution Order)

We will build layers in this strict order:

```
Phase 1 → Domain Modeling
Phase 2 → Application Use Cases
Phase 3 → Infrastructure (ADO.NET)
Phase 4 → Console Presentation
Phase 5 → Hardening for Production
Phase 6 → Future API Refactor
```

Each phase must be completed through Feature Branches only.

---

## 🧪 What This Repository Represents

This is not just code.

It is a **log of architectural decisions**:
Every branch tells a story.
Every commit explains intent.
Every merge reflects a validated concept.

---

## ⚙️ Current Status

### 🏢 Domain Layer — Phase 1: Complete ✅

Core aggregates fully implemented with invariants and repository interfaces:

**Business Domains:**

- 🏪 **Branches** — multi-branch store management with factory methods
- 📦 **Products** — product catalog with categorization support
- 👥 **Customers** — customer entity with factory methods and validation
- 🤝 **Suppliers** — supplier aggregate with balance tracking and factory methods
- 🛒 **Sales** — sales transactions with line item management and discount handling
- 📤 **Returns** — customer and supplier returns with extended reasons and expiry tracking
- 💳 **Payments** — multi-currency payment workflows with explicit lifecycle
- 📊 **Inventory** — stock management with batches, adjustments, and damage tracking
- 🏷️ **Discounts** — product and sale-level discounts with calculation logic and invariants
- 🗂️ **Categories** — product categorization system

**Value Objects:**

- 💰 **Money** — multi-currency support (EGP, USD, JOD, ILS) with safe arithmetic
- 🆔 **NationalId** — country-specific validation for national IDs
- 📱 **Phone** — country-specific phone number validation
- Comprehensive enum definitions (DiscountType, AdjustmentReason, PaymentMethod, ReturnReason, etc.)

---

### 📲 Application Layer — Phase 2: In Progress 🔄

Use cases, DTOs, and mappers for business workflows:

**Implemented Workflows:**

- ✅ Customer management (create, update, retrieve)
- ✅ Sales operations (create sales, manage line items)
- ✅ Payment processing (multi-currency, refunds)
- ✅ Return workflows (customer and supplier returns with refunds)
- ✅ Inventory management (stock adjustments, batch tracking, damage logging)
- ✅ Discount application (product-level and sale-level discount workflows)
- ✅ Employee management (users, permissions, branch assignment)
- ✅ Supplier operations (create, update, balance deduction)
- 🔄 **In Progress:** EmailDto and enhanced supplier workflows

**Mapping Layer:**

- Complete DTOs for all aggregates
- Type-safe mappers (MoneyMapper, entity mappers, etc.)
- Bi-directional conversion between domain and application layers

---

### 🗄️ Infrastructure Layer — Phase 3: Started 🚀

- ADO.NET implementation (no ORM)
- Manual connection and query management
- Repository pattern foundation
- Ready for database schema implementation

---

### Current Git Status

**Branch:** `feature/domain-sale-discount-support`

**In Progress:**

- Refactoring Sale discount handling (moved from ValueObject to aggregate-controlled)
- Enhancing supplier workflows with Email support
- Improving supplier return management

---

## 🚫 Important Rule

If something feels "easy", we probably did it wrong.

This system must be:

- Explicit
- Intentional
- Explainable

---

## 📋 Next Phase

**Phase 3 — Infrastructure Implementation:**

- Implement ADO.NET repository contracts
- Design and create database schema
- Build persistence layer for all aggregates
- Integration testing with real database

**Phase 4 — Presentation Layer:**

- Upgrade from Console to a proper UI (likely console-based with TUI framework or API)
- Implement user workflows end-to-end
- System testing and validation
