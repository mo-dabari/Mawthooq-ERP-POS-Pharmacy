# Events

## Purpose

This document defines the business events produced by the Medicine aggregate.

Business events describe completed pharmaceutical business actions.

They are the output of successful business behavior.

---

# Event Philosophy

The Medicine aggregate records business events after successfully applying business rules.

Events describe what happened from the pharmaceutical perspective.

They never describe technical operations.

---

# Event Ownership

Only the Medicine aggregate may produce Medicine events.

Other modules never produce Medicine events on its behalf.

---

# Persistence

Recorded business events are translated by the Infrastructure layer into persistence operations.

The Medicine aggregate has no knowledge of SQL or database implementation.

---

# Integration

Medicine events may participate in larger business workflows coordinated by the Application layer.

The aggregate remains responsible only for its own pharmaceutical events.

---

# Related Documentation

- aggregate.md
- decision-rules.md

