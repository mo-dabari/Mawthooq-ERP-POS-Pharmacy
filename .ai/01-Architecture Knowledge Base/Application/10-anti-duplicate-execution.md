# Idempotency

## Purpose

Idempotency prevents the same business operation from being executed multiple times when duplicate requests are received.

It protects the system from unintended duplicate business transactions caused by retries, network failures, or client-side resubmissions.

---

# Scope

Idempotency is applied only to operations where duplicate execution would produce incorrect business results.

It is not required for every use case.

---

# Typical Scenarios

Duplicate requests may occur because of:

- Network retries.
- Client retries.
- Connection timeouts.
- User refreshing the application.
- Multiple submissions of the same request.

The system should prevent duplicate execution when required by the business.

---

# Business Motivation

Without idempotency, duplicate execution may lead to:

- Duplicate sales.
- Duplicate purchase receipts.
- Duplicate payments.
- Duplicate financial transactions.
- Incorrect inventory movements.

These situations violate business consistency.

---

# Applicability

Typical candidates include:

- Completing a sale.
- Receiving a purchase.
- Posting financial transactions.
- Creating payments.

Read operations do not require idempotency.

---

# Responsibility

The decision to require idempotency belongs to the business use case.

The Application Layer coordinates the execution.

The Infrastructure Layer provides the technical implementation when required.

---

# Design Principles

Idempotency follows these principles:

- Prevent duplicate business execution.
- Preserve business consistency.
- Be applied only where necessary.
- Remain independent from infrastructure technology.

---

# Infrastructure Independence

The Application Layer defines that a use case requires idempotent execution.

How duplicate detection is implemented is an infrastructure concern.

Possible implementation strategies are intentionally outside the scope of this document.

---

# Summary

Idempotency is an application capability used to protect critical business operations from duplicate execution.

It is applied selectively based on business requirements and remains independent from its technical implementation.
