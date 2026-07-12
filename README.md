# Proof by Climbing Game – Game Design Document

---

# 1. Projektübersicht

## 1.1 Elevator Pitch

Das Projekt ist ein VR-basiertes Climbing-Spiel, bei dem der Spieler eine vertikale Strecke so schnell wie möglich durchquert. Der Fokus liegt auf physikbasiertem Klettern und einem zeitbasierten Gameplay-Loop.

---

## 1.2 Kernfantasie

Der Spieler erlebt sich als VR-Kletterer, der durch präzise Handbewegungen eine Strecke effizient bewältigt und seine Zeit optimiert.

---

## 1.3 Zielplattform

- VR-Headsets (Roomscale / 3DoF)
- Unity Engine
- XR Interaction Toolkit

---

# 2. High Concept

## 2.1 Kernidee

Der Spieler startet einen Run, klettert durch eine Low-Poly-Umgebung und versucht, das Ziel in möglichst kurzer Zeit zu erreichen. Das Gameplay basiert auf physikalischem Greifen und Gravitation.

---

## 2.2 USPs

- Physikbasiertes VR-Klettern
- Kurze, wiederholbare Runs
- Skill-basiertes Movement durch Handinteraktion
- Direkte VR-Immersion ohne klassische Locomotion

---

# 3. Core Gameplay

## 3.1 Core Loop

1. Spieler startet im Main Menu
2. Countdown (Ready – Set – Go)
3. Timer startet
4. Spieler klettert durch das Level
5. Ziel wird erreicht
6. Zeit wird angezeigt
7. Run wird neu gestartet

---

## 3.2 Success State

- Ziel erreicht
- Zeit wird gestoppt und angezeigt

---

## 3.3 Match Flow

- Countdown Phase
- Active Run
- Finish State
- Restart Loop

---

# 4. Game Rules

## 4.1 Grundregeln

- Movement nur durch Klettern
- Gravitation ist aktiv
- Ziel ist die schnellste Zeit
- Keine klassischen Movement-Systeme vorhanden

---

## 4.2 Interaktionen

- Greifen von Oberflächen
- Physikalisches Ziehen des Körpers
- Kletterbare Levelgeometrie

---

# 5. Player System

## 5.1 Fähigkeiten

- Greifen und Klettern
- Physikalisches Movement durch Handpositionen
- Kontrolle über vertikale Bewegung

---

## 5.2 Input

- Grip Button: Klettern / Festhalten
- VR Controller: Handtracking

---

## 5.3 Spielerwerte

- Kein Stamina-System implementiert
- Gravitation aktiv
- Movement abhängig von Handbewegung

---

## 5.4 Feedback

- Timer UI
- Countdown UI
- Finish Anzeige
- Audio Feedback über Musikstart beim Run

---

# 6. VR Movement System

## 6.1 Climbing System

- Hände greifen Oberflächen
- Körper bewegt sich entgegengesetzt zur Handbewegung
- Bewegung basiert auf Hand-Delta

---

## 6.2 Gravity System

- Permanente Gravitation aktiv
- Spieler fällt ohne Grip
- Kombination aus Klettern und Fallen möglich

---

## 6.3 Wall Interaction

- Kanten können gelegentlich zu Hängenbleiben führen
- Kein aktives Push-Off System implementiert
- Level ist für Klettern optimiert

---

# 7. Level Design

## 7.1 Struktur

- Vertikales Leveldesign
- Fokus auf Aufstieg
- Ein klarer Zielpunkt

---

## 7.2 Environment

- Felsen
- Plattformen
- natürliche Kletterstrukturen

---

# 8. UI / UX

## 8.1 HUD

- Timer (laufend)
- Countdown (Ready / Set / Go)
- Finish Text mit Zeit

---

# 9. Audio Design

- Ein Musiktrack startet beim Run Start
- Musik endet/stoppt beim Finish

---

# 10. Technik

## 10.1 Kernsysteme

- VR Climbing System
- Gravity System
- Timer System
- Scene Management
- Finish Detection
- UI System

---

## 10.2 Architektur

- GameManager steuert Spielzustand
- FinishDetection triggert Run-Ende
- UI ist an XR Camera gebunden
- Scoreboard im Main Menu zeigt Bestzeiten

---

# 11. Game Flow

 - Start → Countdown → Run → Finish → Time Display → Restart

---

# 12. Evaluation
Das Spiel wurde getestet mit Fokus auf:

- Usability (Steuerbarkeit)
- Immersion (Presence)
- Motion Comfort
- Gameplay Flow

---
# 13. Final Statement

 - Das Projekt ist ein funktionaler VR Climbing Speedrun Prototyp. Alle Kernmechaniken (Movement, Gravity, Timer, Finish Loop und Scoreboard) sind implementiert und ermöglichen ein wiederholbares, zeitbasiertes Spielerlebnis.

---
