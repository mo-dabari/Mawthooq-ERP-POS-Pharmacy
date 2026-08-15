# Queries

## Purpose

This document describes the information that can be retrieved from the Manufacturer module.

Queries never modify the aggregate.

They are read operations used by the application and reporting layers.

---

# Get Manufacturer

## Description

Retrieves the details of a single Manufacturer.

---

## Typical Usage

* View manufacturer details.
* Edit manufacturer information.
* Display manufacturer metadata.

---

# List Manufacturers

## Description

Returns all manufacturers.

---

## Typical Usage

* Administration screens.
* Reference catalog management.
* Internal maintenance.

---

# List Active Manufacturers

## Description

Returns only manufacturers that are currently active.

---

## Typical Usage

* Medicine creation.
* Medicine editing.
* Manufacturer selection dialogs.

Only active manufacturers should normally be available for new medicine definitions.

---

# Search Manufacturers

## Description

Searches manufacturers by name.

---

## Typical Usage

* Autocomplete.
* Lookup dialogs.
* Fast manufacturer selection.

---

# Check Manufacturer Usage

## Description

Determines whether a Manufacturer is referenced by one or more medicines.

---

## Typical Usage

* Validate deactivation decisions.
* Administrative analysis.
* Dependency reporting.

This query belongs to the read model and does not create a relationship inside the Manufacturer aggregate.

---

# Query Principles

Queries:

* Never modify the aggregate.
* May join data from multiple modules.
* May use optimized read models.
* Are independent from aggregate boundaries.
* May expose projections instead of domain objects.
