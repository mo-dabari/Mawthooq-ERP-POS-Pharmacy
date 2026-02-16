# Mawthooq-POS
Advanced Point of Sale (POS) Management System for Pharmacies and Retail.  The "Mawthooq" system is a comprehensive technology solution specifically designed to meet the needs of precision-sensitive environments such as pharmacies and supermarkets. The system is based on a robust, offline-first infrastructure with advanced cloud synchronization.



# 🧾 Retail POS System — Clean Architecture Learning Project

## 📌 Project Purpose

This repository is not just a coding exercise.
It is a **step-by-step construction of a production-ready Retail POS system** designed to:

* Build deep understanding of **Clean Architecture**
* Apply **SOLID principles in real-world scenarios**
* Simulate a **multi-branch retail environment**
* Produce a system that can later evolve into a **commercial product**
* Serve as a strong, explainable project in a professional CV

---

## 🏗️ Development Philosophy

This project is intentionally built with constraints to enforce learning:

* ❌ No ORM (No Entity Framework)
* ❌ No Design Patterns (initially)
* ✅ Pure ADO.NET
* ✅ Manual separation of responsibilities
* ✅ Architecture enforced by discipline, not frameworks
* ✅ Console Application as Presentation Layer (temporary)

The goal is to understand **why architecture exists**, not just use it.

---

## 🧠 System Scope (Phase 1)

We are building a **Retail POS Core Engine** starting with a Supermarket use case.

Supported Concepts:

* Multi-Branch Management (1 warehouse per branch)
* Inventory per Branch
* Sales Transactions
* Multi-Currency Support (EGP, USD, JOD, ILS)
* Extensible for future domains (e.g., Pharmacy)

---

## 🌳 Git Workflow Strategy (MANDATORY)

We are using a **structured GitFlow model** to simulate real team development.

---

## 🔀 Branch Types

### `main`

* Always stable
* Represents production-ready code
* No direct commits allowed

### `develop`

* Integration branch
* All completed features merge here first
* Acts as the current working version of the system

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

* Created from `develop`
* Merged back into `develop`
* Deleted after merge

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

* `build` → new structure
* `add` → new behavior
* `refactor` → improve without changing behavior
* `fix` → correct logic
* `docs` → documentation only

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

* Commit directly to `main`
* Mix multiple features in one branch
* Create "test" commits like `update`, `change`, `fix stuff`
* Push broken architecture

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

```
Project Initialized
GitFlow Established
Ready to Begin Domain Discovery
```

---

## 🚫 Important Rule

If something feels "easy", we probably did it wrong.

This system must be:

* Explicit
* Intentional
* Explainable

---

## ▶ Next Step

Create the first branch:

```
feature/domain-discovery
```

This branch will contain **NO CODE** — only business analysis and domain definition.
