# VR Game Design Document (Unity / Team)

> **Projektname:** ZVIT-Project

> **Teammitglieder:** Tim, Kürsat, Alex, Philipp

> **Status:** Idee / Pre-Production

---

## 1. Projektübersicht

### 1.1 Elevator Pitch

ZVIT-Project ist ein kompetitives VR-Kletterspiel als 1v1-Rennen. <br>
Zwei Spieler starten auf gegenüberliegenden Seiten einer Kletterstrecke und versuchen als Erste das Ziel zu erreichen. <br>
Zusätzlich ist ein Singleplayer-Modus mit Timer geplant.

- Genre: Jump and Run (VR)

### 1.2 Kernfantasie

- Ich bin ein schneller, taktischer VR-Kletterer.
- Ich nutze Umgebung, Timing und Items, um den Gegner auszustechen.

### 1.3 Zielplattform

- VR-Headsets mit 6 Freiheitsgraden (6DoF)
- Steuerung über VR-Controller (Hände + Joystick)

---

## 2. High Concept

### 2.1 Kernidee

Der Spieler verbringt den Großteil seiner Zeit mit Klettern, Greifen, Ausweichen und dem klugen Einsatz von Items.

### 2.2 USPs

- Fokus auf direktes VR-Klettern statt klassischem Shooter-Gameplay
- Kurze, intensive 1v1-Matches mit hoher Wiederholbarkeit
- Skill trifft Risiko durch Physik, Ausdauer und Item-Einsatz

### 2.3 Accessibility / Komfort

- Bewegungsoptionen: Smooth Locomotion (Teleport optional als Komfortmodus)
- Komfortfeatures: Snap Turn, Vignette, einstellbare Drehgeschwindigkeit [TODO]
- Klettern über Hände (Grip/Festhalten)

---

## 3. Core Gameplay

### 3.1 Core Loop

1. Spieler spawnen auf gegenüberliegenden Seiten der Strecke.
2. Spieler klettern nach oben, interagieren mit der Umgebung und nutzen Items.
3. Ein Spieler erreicht zuerst das Ziel und gewinnt.
4. Ergebnisanzeige, dann Rematch oder neues Match.

### 3.2 Success / Failure States

- Sieg: Als erster im Ziel ankommen.
- Niederlage: Gegner erreicht zuerst das Ziel.
- Sturzverhalten: Weiterspielen ab Landepunkt oder Checkpoint.
- Checkpoints: Ja.

### 3.3 Match-Fluss

- Lobby / Ready-Phase
- Startsignal: "Ready - Set - Go"
- Laufende Runde
- End-Screen mit Sieger und Zeit

---

## 4. Game Rules

### 4.1 Grundregeln

- Spieler dürfen: Klettern, bewegen, greifen, werfen, Items verwenden.
- Spieler dürfen nicht: Aus der Map gehen, Exploits nutzen.
- Zeitdruck: Modusabhängig (z.B. Timer im Singleplayer).
- Ressourcenlimit: Ausdauer (Stamina).

### 4.2 Interaktionsregeln

- Greifbare Objekte: Kletterkanten, Seile, Lianen, Kletterhaken, Kisten [TODO].
- Dekorative Objekte: Hintergrund, entfernte Berge/Bäume, nicht markierte Props.
- Aktivierung: Collider + Grip bzw. Trigger.
- Physik: Gravitation ist aktiv, Objekte können bewegt/geworfen werden.

### 4.3 PvP-Regeln

- Indirektes Stören über Items und Umgebungsinteraktionen ist erlaubt.
- Direkter Nahkampf ist aktuell nicht vorgesehen.

---

## 5. Player

### 5.1 Fähigkeiten

- Laufen / Locomotion
- Greifen / Festhalten
- Werfen
- Klettern
- Spezialfähigkeit über Item

### 5.2 Eingaben / Controls

- Linker Controller: linke Hand
- Rechter Controller: rechte Hand
- Grip: Greifen/Festhalten [TODO]
- Trigger: Nutzen/Werfen [TODO]
- Joystick: Bewegung
- Menü-Taste: Pause / Einstellungen [TODO]
- Gesten: optional [TODO]

### 5.3 Spielerwerte

- Ausdauer (Stamina): [TODO]
- Inventarslots: [TODO]
- Bewegungsgeschwindigkeit: [TODO]
- Gravitationseinfluss: [TODO]

### 5.4 Spieler-Feedback

- Audio: Schnaufen, Fallen, Landen, Treffer
- Visuell: Treffer-Feedback, Stamina-Status, Fortschritt zum Ziel
- UI: Item-Slot + Stamina-Anzeige
- Haptik: Controller-Vibration bei relevanten Aktionen

### 5.5 Spawn-Logik

Faire, symmetrische Spawnpositionen für beide Spieler.

---

## 6. Items und Inventar

### 6.1 Item-Liste

| Name           | Definition                                              | Interaktion                            |
| -------------- | ------------------------------------------------------- | -------------------------------------- |
| Energy         | Füllt Ausdauer auf / beschleunigt Regeneration          | Aufheben + Aktivieren                  |
| Gravity        | Verändert eigene oder gegnerische Gravitation           | Aktivieren mit begrenzter Dauer [TODO] |
| Biene          | Stört Gegner akustisch und visuell                      | Kann Gegner bedrängen/stechen          |
| Stein          | Bringt Gegner ins Fallen, falls nicht festgehalten wird | Interaktion per Greifen/Werfen [TODO]  |
| Fels           | Fels von oben (Donkey-Kong-ähnlich)                     | Event-/Triggerbasiert [TODO]           |
| Grappling Hook | Zieht Spieler an einen Zielpunkt                        | Zielen + Auslösen                      |

### 6.2 Tools

- Stamina-Monitor (z.B. Uhr/HUD)

### 6.3 Verbrauchsitems

- Spezialitem pro Runde oder Pickup-basiert [TODO]

### 6.4 Inventarsystem

- Quick Access für aktives Spezialitem
- Kleine Slot-Anzahl für VR-Übersichtlichkeit

---

## 7. VR-spezifisches Design

### 7.1 Interaktionen

- Direct Grab
- Two-Handed Handling
- Physikobjekte

### 7.2 Immersion

- Räumlicher Sound (3D-Audio)
- Glaubwürdige Handpräsenz
- Klare visuelle Rückmeldungen bei Interaktion

---

## 8. Level / World Design

### 8.1 Weltstruktur

- Grundsätzlich linearer Aufstieg
- Optionale Mikro-Routen für Risiko/Belohnung

### 8.2 Interaktive Umgebung

- Kisten
- Bäume
- Felsvorsprünge
- Spalten
- Seile
- Felsen
- Lianen
- Kletterhaken

### 8.3 Story Delivery

- Leichter Einstieg über Start-Ansage oder Monitor ("Ready - Set - Go")

---

## 9. Art Direction

### 9.1 Stil

- Nature
- Low Poly

### 9.2 Asset-Liste

- [TODO] Kletter-Assets
- [TODO] Umwelt-Props
- [TODO] UI-Elemente

---

## 10. Audio Design

### 10.1 Sound-Kategorien

- Interaktionssounds
- Treffer-/Impactsounds
- Gegnergeräusche / Störsounds
- Ambient Sounds
- Musik

### 10.2 Audio-Ziele

- Mehr Immersion durch klare Raumwahrnehmung
- Relevante Gameplay-Events müssen eindeutig hörbar sein
- 3D-Audio-Priorisierung für wichtige Quellen

### 10.3 Musik

- Ruhiger/chilliger Grundton
- Dynamik in spannenden Match-Phasen [TODO]

---

## 11. UI / UX

### 11.1 Menüs

- Main Menu
- Pause Menu
- Settings

### 11.2 UX-Prinzipien

- Große Buttons
- Sofortiges Feedback auf jede Interaktion
- Gute Lesbarkeit und Kontrast in VR

---

## 12. Technik

### 12.1 Kernsysteme

- Player Controller
- Interaction System
- Inventory System
- Audio Manager
- Scene Management

### 12.2 Multiplayer

- Modus: Multiplayer
- Spieleranzahl: Min 2 / Max 2

### 12.3 Technische Fragen

- Host/Client oder Dedicated Server?
- Welche Objekte müssen synchronisiert werden?
- Ownership-Regeln für Items/Interaktionen?
- Welche Elemente sind besonders latenzkritisch?

---

## 13. Entwicklungsphasen

### Phase 1: Vision fixieren

- Vision
- Zielplattform
- Core Loop
- Player
- Enemy
- Game Rules
- MVP

### Phase 2: Prototyp bauen

- VR Movement
- Greifen / Interaktion
- 1 Gegner oder Störelement
- 1 Waffen- oder Tool-System
- 1 Testarena

### Phase 3: Erweitern und polieren

- Story
- UI-Polish
- Zusätzliche Gegner
- Zusätzliche Level
- Effekte / Audio / Balancing

---

## 14. MVP Scope

### Must-have

- Spielbarer 1v1-Prototyp in VR
- Kletter-Loop mit Start, Ziel und Checkpoints
- Funktionierende Ausdauer-Logik
- 2 funktionierende Items
- Basic UI (Start, Pause, Ergebnis)

### Should-have

- Singleplayer-Timer-Modus
- Drittes Item zur Gegnerstörung
- Verbesserte Audio-Hinweise

### Won't-have (im MVP)

- Große Story-Kampagne
- Mehrere komplett unterschiedliche Maps
- Komplexe Progression/Crafting-Systeme
