# LabDiner (In Progress)

LabDiner is a modular Unity restaurant-management idle tycoon game built around data-driven progression, reusable gameplay systems, and editor tooling for fast content production.

## Overview

The project is structured to keep gameplay code, shared foundation systems, and editor workflows separated into clear modules. This reduces coupling, avoids circular dependencies, and makes it easier to extend the game with new levels, stations, upgrades, and mission types.

## Key Features

- Modular assembly-definition layout to separate shared systems, gameplay modules, editor tools, and support packages.
- ScriptableObject-based event architecture for decoupled communication between gameplay, UI, and runtime state.
- Data-driven upgrade, mission, and progression systems that are configured through assets instead of hard-coded logic.
- Reusable UI transition effects powered by DOTween.
- MVC-inspired UI separation using base panel abstractions, a panel event layer, and a centralized UI manager.
- Custom editor utilities for creating stations, missions, upgrades, and level content more efficiently.
- Navigation, input, rendering, and UI dependencies configured through modern Unity packages.

## Architecture

### Core Design Principles

- Data-first gameplay: stations, upgrades, missions, rewards, and level configuration are represented as assets.
- Event-driven communication: ScriptableObject events carry gameplay and UI signals without forcing direct component references.
- Runtime state isolation: progress, task state, upgrade state, and tutorial state are tracked separately from static content.
- Extensibility by composition: new mission, upgrade, or task types can be added by introducing new assets and small specialized classes.
- UI decoupling: panels self-register through events, while the UI manager controls panel lookup, history, and visibility state.

## Key Systems

### ScriptableObject Event System

The project uses a reusable event layer built on ScriptableObject assets. Typed and non-typed events support prioritized listeners, which helps keep feature logic independent while still allowing deterministic event ordering when needed.

### UI Layer

UI is organized around a base panel abstraction, a centralized UI manager, and reusable transition effects. This keeps animation, presentation, and panel lifecycle concerns separated from gameplay logic.

### Editor Tooling

Custom editor windows support authoring stations, missions, upgrades, and level configurations. The tools also work with a level registry to help keep generated content consistent and easy to browse.
