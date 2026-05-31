# 🚀 Cosmic Navigation System

An automated, console-based C# application designed to simulate and execute space rescue missions. The system calculates the most efficient routes for stranded astronauts to reach extraction points while navigating treacherous cosmic hazards, and securely transmits mission reports via email.

---

## 📖 Table of Contents
* [About the Project](#about-the-project)
* [Features](#features)
* [Map Legend](#map-legend)
* [Getting Started](#getting-started)
* [Usage](#usage)
* [Tech Stack](#tech-stack)

---

## 🌌 About the Project
**Cosmic Navigation System** utilizes **Dijkstra's Algorithm** to determine the shortest path through a cosmic grid based on varying terrain costs. It handles dynamic map generation, calculates safe routes around impassable obstacles, visualizes the journey in the console, and integrates secure SMTP protocols to email the final mission debrief to Mission Control.

---

## ✨ Features
* **Advanced Pathfinding:** Intelligently navigates around impassable obstacles and calculates movement costs based on the environment using Dijkstra's Algorithm.
* **Dynamic Map Generation:** * **Manual Mode:** Input custom map dimensions and layout directly.
  * **Random Mode:** Procedurally generates a cosmic grid with randomized distributions of open space, debris, asteroids, targets, and up to 3 astronauts.
* **Visual Mission Reports:** Generates an ASCII-based visual map of the executed mission, plotting the exact route taken by the astronauts.
* **Live Gmail Transmission:** Automatically compiles a mission report and sends it securely to a designated email address directly from the console.
* **Secure Input:** Features a custom password-masking interface that hides your 16-character App Password during input to ensure security during live demonstrations.

---

## 🗺️ Map Legend

The cosmic grid uses specific characters to represent the environment. Here is how to read the map:

| Symbol | Environment | Description | Movement Cost |
| :---: | :--- | :--- | :---: |
| **`0`** | **Open Space** | Clear vacuum; safe for normal movement. | 1 Step |
| **`D`** | **Space Debris** | Difficult terrain; slows down movement. | 2 Steps |
| **`X`** | **Asteroid** | Impassable obstacle; route must go around. | *Blocked* |
| **`S#`** | **Astronaut** | Starting position of a stranded astronaut (e.g., `S1`, `S2`). | - |
| **`F`** | **Finish/Target** | The extraction point / mission destination. | - |
| **`*`** | **Travel Path** | The visual trail left by the astronaut in the final report. | - |

---

## ⚙️ Getting Started

To run this project locally, you will need to set up your development environment.

### Prerequisites
1. **.NET SDK** (Version 6.0 or higher recommended).
2. **Gmail App Password:** To use the email transmission feature, you must have 2-Step Verification enabled on your Google account and generate a **16-character App Password**. Standard Google account passwords will not work for SMTP authentication.

### Installation
1. Clone the repository or download the source code.
2. Open your terminal in the project directory.
3. Install the required NuGet packages for email handling:
   ```bash
   dotnet add package MailKit
   dotnet add package MimeKit
