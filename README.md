Cosmic Navigation System
Overview
Cosmic Navigation System is a C# console application designed to simulate space rescue missions. It calculates the most efficient route for stranded astronauts to reach a designated target coordinate while navigating through treacherous cosmic hazards. The system utilizes Dijkstra's Algorithm to determine the shortest path based on varying terrain costs and generates an automated mission report that can be securely transmitted via email.

Key Features
Advanced Pathfinding: Utilizes a custom IPathfinderImplementation to calculate the shortest route. It intelligently navigates around impassable obstacles and calculates movement costs based on the environment.

Dynamic Map Generation: * Manual Mode: Input custom map dimensions and grid layouts directly into the console.

Random Mode: Procedurally generates a map with random distributions of open space, space debris, asteroids, a target destination, and up to 3 stranded astronauts.

Visual Mission Reports: Generates an ASCII-based visual map of the executed mission, plotting the exact steps (*) taken by the astronauts to reach the target (F).

Live Gmail Transmission: Integrates MailKit and MimeKit to securely email the final mission report directly from the console using an SMTP connection.

Secure Input: Features a custom password masking system that hides your 16-character App Password during input to ensure security during live demonstrations.

Map Legend
The cosmic grid is built using specific characters to represent the environment:

0 : Open Space (Normal movement, Cost: 1 step)

D : Space Debris (Difficult terrain, Cost: 2 steps)

X : Asteroid (Impassable obstacle)

S# : Astronaut (Starting position, e.g., S1, S2)

F : Target / Finish (The extraction point)

* : Travel Path (Visualized in the final report)

 Prerequisites and Setup
To run this project locally, you will need:

.NET SDK installed on your machine.

MailKit & MimeKit NuGet Packages installed in your project:

Bash

dotnet add package MailKit

dotnet add package MimeKit

Gmail App Password: To use the email transmission feature, you must have 2-Step Verification enabled on your Google account and generate a 16-character App Password. Standard Google account passwords will not work for SMTP authentication.

Usage
Build and run the console application.

Select your map generation preference:

Press 1 to manually type the map dimensions and grid.

Press 2 to specify dimensions and let the system randomly populate the map.

The system will process the pathfinding algorithms and print the success/failure statuses of the astronauts in the console.

When prompted, type Y to send the report via email, or N to exit.

If sending an email, input your sender address, the 16-character App Password (which will be hidden as *), and the receiver address.
