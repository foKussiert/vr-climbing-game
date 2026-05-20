# VR Game Design Document (Unity / Team)

> **Projektname:** ZVIT-Project

> **Teammitglieder:** Tim, Kürsat, Alex, Philipp

> **Status:** Idee / Pre-Production

---

## 1. Projektübersicht

### 1.1 Elevator Pitch

ZVIT-Project ist ein VR-Kletterspiel im Singleplayer-Timer-Modus. <br>
Der Spieler startet eine Kletterstrecke und versucht, die Strecke in möglichst kurzer Zeit zu absolvieren.

- Genre: Jump and Run (VR)

### 1.2 Kernfantasie

- Ich bin ein schneller, taktischer VR-Kletterer.
- Ich nutze Umgebung, Timing und Items, um meine Zeit zu verbessern.

### 1.3 Zielplattform

- VR-Headsets mit 3 Freiheitsgraden (3DoF)
- Steuerung über VR-Controller (Hände + Joystick)

---

## 2. High Concept

### 2.1 Kernidee

Der Spieler verbringt den Großteil seiner Zeit mit Klettern, Greifen, Ausweichen und dem klugen Einsatz von Items.

### 2.2 USPs

- Fokus auf direktes VR-Klettern statt klassischem Shooter-Gameplay
- Kurze, intensive Runs mit hoher Wiederholbarkeit
- Skill trifft Risiko durch Physik, Ausdauer und Item-Einsatz

### 2.3 Accessibility / Komfort

- Bewegungsoptionen: Smooth Locomotion (Teleport optional als Komfortmodus)
- Komfortfeatures: Snap Turn, Vignette, einstellbare Drehgeschwindigkeit [TODO]
- Klettern über Hände (Grip/Festhalten)

---

## 3. Core Gameplay

### 3.1 Core Loop

1. Spieler spawnt am Startpunkt der Strecke.
2. Der Spieler klettert nach oben, interagiert mit der Umgebung und nutzt Items.
3. Ziel ist es, die Strecke in möglichst kurzer Zeit zu absolvieren (Timer).
4. Ergebnisanzeige mit Zeit, dann Neustart oder neues Level.

### 3.2 Success / Failure States

- Erfolg: Level abgeschlossen / Bestzeit verbessert.
- Misserfolg: Sturz oder Zeitverlust, Weiterspielen ab Checkpoint.
- Sturzverhalten: Weiterspielen ab Landepunkt oder Checkpoint.
- Checkpoints: Ja.

### 3.3 Match-Fluss

- Ready-Phase / Start
- Laufende Runde (Einzelspieler, Timer läuft)
- End-Screen mit Bestzeit und Timer-Statistiken

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

### 4.3 Interaktionen

- Interaktionen beziehen sich auf die Umwelt und das eigene Movement; das Design fokussiert Singleplayer-Spielabläufe.

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

- Startposition und Checkpoint-Placement für den Spieler.

---

## 6. Items und Inventar

### 6.1 Item-Liste

| Name           | Definition                                                | Interaktion                            |
| -------------- | --------------------------------------------------------- | -------------------------------------- |
| Energy         | Füllt Ausdauer auf / beschleunigt Regeneration            | Aufheben + Aktivieren                  |
| Gravity        | Verändert temporär die Gravitation (z. B. langsamer Fall) | Aktivieren mit begrenzter Dauer [TODO] |
| Biene          | Erzeugt lokale Ablenkung/Partikeleffekt                   | Aufheben + Aktivieren                  |
| Stein          | Fügt dem Level physikalische Hindernisse hinzu            | Interaktion per Greifen/Werfen [TODO]  |
| Fels           | Fels von oben (Donkey-Kong-ähnlich)                       | Event-/Triggerbasiert [TODO]           |
| Grappling Hook | Zieht Spieler an einen Zielpunkt                          | Zielen + Auslösen                      |

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
- Interaktionssounds / Störsounds
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

### 12.2 Singleplayer

- Modus: Singleplayer (Timer-Modus)
- Spieleranzahl: 1

### 12.3 Technische Fragen

- Benötigen wir Netzwerkcode? Nein, Fokus auf lokalem Singleplayer.
- Welche Objekte müssen persistent gespeichert werden (Bestzeiten, Einstellungen)?
- Ownership-Regeln entfallen, da keine netzwerkbasierten Interaktionen geplant sind.

---

## 13. Entwicklungsphasen

### Phase 1: Vision fixieren

- Vision
- Zielplattform
- Core Loop
- Player
- Game Rules
- MVP

### Phase 2: Prototyp bauen

- VR Movement
- Greifen / Interaktion
- Timer-basiertes Level und Checkpoints
- 1 Toolsystem (z.B. Grappling Hook)
- 1 Testarena

### Phase 3: Erweitern und polieren

- Story
- UI-Polish
- Zusätzliche Level
- Effekte / Audio / Balancing

---

## 14. MVP Scope

### Must-have

- Spielbarer Singleplayer-Timer-Prototyp in VR
- Kletter-Loop mit Start, Ziel und Checkpoints
- Funktionierende Ausdauer-Logik
- 2 funktionierende Items
- Basic UI (Start, Pause, Ergebnis, Bestzeiten-Anzeige)

### Should-have

- Singleplayer-Timer-Modus
- Drittes Item zur zusätzlichen Herausforderung
- Verbesserte Audio-Hinweise

### Won't-have (im MVP)

- Große Story-Kampagne
- Mehrere komplett unterschiedliche Maps
- Komplexe Progression/Crafting-Systeme
