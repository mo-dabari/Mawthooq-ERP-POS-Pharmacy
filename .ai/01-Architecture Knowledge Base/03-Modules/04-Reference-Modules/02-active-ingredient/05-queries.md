# Queries

## Purpose

This document describes the information that can be retrieved from the Active Ingredient module.

Queries never modify the aggregate.

They are read operations used by the application and reporting layers.

---

# Get Active Ingredient

## Description

Retrieves the details of a single Active Ingredient.

---

## Typical Usage

* View ingredient details.
* Edit ingredient information.
* Display ingredient metadata.

---

# List Active Ingredients

## Description

Returns all Active Ingredients.

---

## Typical Usage

* Administration screens.
* Reference catalog management.
* Internal maintenance.

---

# List Active Active Ingredients

## Description

Returns only ingredients that are currently active.

---

## Typical Usage

* Medicine creation.
* Medicine editing.
* Ingredient selection dialogs.

Only active ingredients should normally be available for new medicine definitions.

---

# Search Active Ingredients

## Description

Searches ingredients by name.

---

## Typical Usage

* Autocomplete.
* Lookup dialogs.
* Fast ingredient selection.

---

# Check Ingredient Usage

## Description

Determines whether an Active Ingredient is referenced by one or more medicines.

---

## Typical Usage

* Validate deactivation decisions.
* Administrative analysis.
* Dependency reporting.

This query belongs to the read model and does not create a relationship inside the Active Ingredient aggregate.

---

# Query Principles

Queries:

* Never modify the aggregate.
* May join data from multiple modules.
* May use optimized read models.
* Are independent from aggregate boundaries.
* May expose projections instead of domain objects.
