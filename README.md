# VR Game Design Document (Unity / Team)

> **Projektname:**
> ZVIT-Project
> **Teammitglieder:**
> Tim, Kürsat, Alex, Philipp
> **Status:** Idee

---

## 1. Projektübersicht

### 1.1 Elevator Pitch

**Beschreibt das Spiel in 2–4 Sätzen.**

**Vorlage:**
„Kletter Game in VR. 1 Gegen 1 gegen Mitspieler, wer als ersten am Ziel ist. | Single mit Timer“

* Genre: Jump and Run


## 2. High Concept

### 2.1 Kernidee

* Was tut der Spieler die meiste Zeit?
* Klettern


### 3.3 Accessibility / Komfort

* Teleport Movement / Smooth Locomotion?
* Hände fürs Klettern, VR Controller Movement
* Freiheitsgrade: 6


## 4. Core Gameplay

### 4.1 Core Loop

1. Spieler Spawnen entgegensetzen Seiten des zu erklimmenden Objektes
2. Spieler interagiert mit Objekten  / Hochkletterm
3. Spieler Gewinnt/Verliert
4. New Game



### 4.3 Failure / Success States

* Wann gewinnt der Spieler?
* erster der Oben ist
* Was passiert beim Fallen?
* Da weiter machen wo man landet
* Gibt es Checkpoints?
* Ja

---

## 5. Game Rules

### 5.1 Grundregeln

* Was darf der Spieler jederzeit tun?
  Klettern, Interagieren, Bewegen
* Was darf der Spieler nicht?
 Mitspieler interagieren - Aus der Map gehen - 
* Gibt es Zeitdruck?
  Modusabhängig
* Gibt es Ressourcenlimits?
 Stamina


### 5.2 Interaktionsregeln

* Welche Objekte sind greifbar?
  TODO
* Welche Objekte sind nur dekorativ?
  Hintergrund und Bäume am Berg z.b
* Wie funktioniert Benutzung / Aktivierung?
  Testen ob Collider klappt - dann festhalten (Klettern)
* Gibt es physikbasierte Interaktion?
* Gravitation

### 5.3 Kampfrelevante Regeln
Wenn Multiplayer: Gegenseitiges Trollen der Spieler


## 6. Player

### 6.2 Fähigkeiten

* Laufen 
* Greifen
* Werfen
* Klettern
* Spezialfähigkeit
* Inventar für Spezialfähigkeit

### 6.3 Eingaben / Controls

* Linker Controller: Linke Hand
* Rechter Controller: Rechte Hand
* Grip:
* Trigger:
* Joystick: Bewegen
* Menü-Taste:
* Gesten:

### 6.4 Spielerwerte

* Ausdauer:
* Inventarslots:
* Bewegungsgeschwindigkeit:
* Gravitation:

### 6.6 Spielerfeedback

* Soundfeedback: Schnaufen, Fallen, Landen
* Visuelles Feedback
* UI-Feedback: Item,Stamina
* Controller-Vibration bei Treffern von Gegner - Items

---

### 6.7 Spawn-Logik

Faires verteiltes Spawnen der Player auf der Map


### 8.1 Items

| Name           | Definition                                                   | Interaktion                                     |
| -------------- | ------------------------------------------------------------ | ----------------------------------------------- |
| Energy         | Stamina auffüllen/regeneration                               |                                                 |
| Gravity        | Gravitation des Gegenspielers - Oder eigene                  |                                                 |
| Biene          | Nervendes Monster das den Gegner nervt Audio und visuell nur | Kann stechen, falls weggeschubst                |
| Stein          | Wegschubsen, falls nicht festgehalten wird                   | NOCH ANSCHAUEN WEIL MAN SIEHT SICH JA EIG NICHT |
| Fels           | Fels von Oben wie Donkey Kong                                | NOCH ANSCHAUEN WEIL MAN SIEHT SICH JA EIG NICHT |
| Grappling Hook | Ranziehen an Stelle                                          |                                                 |

### 8.2 Tools

* Stamina Monitor(Uhr)

### 8.3 Verbrauchsitems

* Spezialitem

### 8.4 Inventarsystem

* Quick Access?
  Spezialitem

## 9. VR-Spezifisches Design

### 9.3 Interaktionen

* Direct Grab
* Two-Handed Handling
* Physikobjekte

### 9.4 Immersion

* Räumlicher Sound
* Handpräsenz



## 10. Level / World Design

### 10.1 Weltstruktur

* Linear


### 10.4 Interaktive Umgebung

* Kisten
* Baum
* Felsvorsprung
* Spalten
* Seil
* Felsen
* Lianen
* Kletterhacken
### 11.4 Story Delivery

  Anfangs, Für das Ready Set Go ein Monitor oder so.


## 12. Art Direction

### 12.1 Stil
* Nature
* Low Poly

### 12.3 Asset-Liste

LEER


## 13. Audio Design

### 13.1 Sound-Kategorien

* Interaktionssounds
* Waffen / Treffer
* Gegnergeräusche
* Ambient Sounds
* Musik

### 13.2 Audio-Ziele

* Welche Sounds erhöhen Immersion?
* Gibt es 3D-Audio-Prioritäten?

### 13.3 Musik
Chillig


### 14.2 Menüs

* Main Menu
* Pause Menu
* Settings


### 14.3 UX-Prinzipien

* Große Buttons
* Feedback bei jeder Interaktion



### 15.4 Kernsysteme

* Player Controller
* Interaction System
* Inventory System
* Audio Manager
* Scene Management

## 16. Multiplayer

### 16.1 Modus
* Multiplayer

### 16.2 Spieleranzahl

* Min:2
* Max:2

### 16.4 Technische Fragen

* Host / Client oder Dedicated Server?
* Synchronisierte Objekte:
* Ownership:
* Latenzkritische Elemente:


### Phase 1: Erst festhalten - Siehe dieses Dokument oben

* Vision
* Zielplattform
* Core Loop
* Player
* Enemy
* Game Rules
* MVP

### Phase 2: Dann prototypen

* VR Movement
* Greifen / Interaktion
* 1 Gegner
* 1 Waffen- oder Tool-System
* 1 Testarena

### Phase 3: Erst danach erweitern

* Story
* UI Polish
* Zusätzliche Gegner
* Zusätzliche Level
* Effekte / Audio / Balancing

---
