# Game Engines Unity Project

## Overview

- top down 2D battle game
- two teams fight against each other on the same map (screen/device)
- teams are equal in size and can consist of 1 to 3 players
- a game is played several rounds on a map
- each player can choose a role

### Controls

- players can walk and fire attacks around them (rotate along the z-axis)
- ps4/xbox controllers

### Win Conditions

- win more rounds than the enemy team in a game
- win rounds by achieving map objective

## Game Mechanics

- health points
- respawn after each death
- death penalty (longer respawn time)
- fighting enemies to kill them
- map objective
- roles have unique skills

## Game Art

- blocky and simple
- low resolution pixel game

## Roadmap

### Core Gameplay

This is the initial plan to have a playable game and a foundation on which more can be built and experimented.

1. Finish Basic Game (Loop)
   - [ ] scene transitions
   - [ ] start menu with play button
   - [x] shooter vs. shooter
   - [ ] round ends after a player dies
   - [ ] display ending screen after finishing 1 round
   - [ ] ending screen shows score
2. Initial Role System
   - [ ] add fighter role
   - [ ] add role selection to start menu
   - [ ] add one special skill to shooter and fighter
   - [ ] add cooldowns for basic attacks and skills
3. Map Objectives
   - [ ] create new map with different goal (flag capturing?)
   - [ ] add map selection to start menu
   - [ ] experiment with map environment objects (e.g. walls, powerups) interactions
4. Teams
   - [ ] 2 teams
   - [ ] differentiate skills and basic attack between allies and enemies
   - [ ] add team selection to start menu
   - [ ] adjust player and camera movement
