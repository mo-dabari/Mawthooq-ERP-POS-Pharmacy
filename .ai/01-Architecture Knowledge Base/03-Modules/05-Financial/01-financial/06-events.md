# Events

## Purpose

This document describes the business events related to the Financial module.

Unlike operational modules, the Financial module is primarily driven by domain events produced by other aggregates.

---

# Incoming Business Events

The Financial module reacts to completed business events.

Typical examples include:

* Sale Completed
* Sale Returned
* Purchase Completed
* Purchase Returned
* Goods Receipt Completed
* Inventory Adjusted

These events trigger the creation of a Financial Record.

---

# Financial Module Events

After successfully recording the financial impact, the module may publish:

* Financial Record Created

This event represents the successful persistence of a financial fact.

It may be consumed by:

* Reporting
* Audit
* Notification
* External accounting integration

---

# Event Principles

Financial events never initiate business workflows.

Operational modules remain the source of business truth.

The Financial module records and communicates financial consequences only.

---

# Design Principle

Operational modules publish business events.

The Financial module consumes those events, records their financial impact, and may publish new events describing completed financial facts.

This preserves a clear upstream/downstream relationship between operational processing and financial recording.
