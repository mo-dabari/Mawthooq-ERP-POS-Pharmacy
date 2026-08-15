# Queries

## Purpose

This document describes the information that can be retrieved from the Therapeutic Class module.

Queries never modify the aggregate.

They are read operations used by the application and reporting layers.

---

# Get Therapeutic Class

## Description

Retrieves the details of a single Therapeutic Class.

---

## Typical Usage

* View therapeutic class details.
* Edit therapeutic class information.
* Display therapeutic class metadata.

---

# List Therapeutic Classes

## Description

Returns all therapeutic classes.

---

## Typical Usage

* Administration screens.
* Reference catalog management.
* Internal maintenance.

---

# List Active Therapeutic Classes

## Description

Returns only therapeutic classes that are currently active.

---

## Typical Usage

* Medicine creation.
* Medicine editing.
* Therapeutic class selection dialogs.

Only active therapeutic classes should normally be available for new medicine definitions.

---

# Search Therapeutic Classes

## Description

Searches therapeutic classes by name.

---

## Typical Usage

* Autocomplete.
* Lookup dialogs.
* Fast therapeutic class selection.

---

# Check Therapeutic Class Usage

## Description

Determines whether a Therapeutic Class is referenced by one or more medicines.

---

## Typical Usage

* Validate deactivation decisions.
* Administrative analysis.
* Dependency reporting.

This query belongs to the read model and does not create a relationship inside the TherapeuticClass aggregate.

---

# Query Principles

Queries:

* Never modify the aggregate.
* May join data from multiple modules.
* May use optimized read models.
* Are independent from aggregate boundaries.
* May expose projections instead of domain objects.
