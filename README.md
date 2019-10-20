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

This is the initial plan to have a foundation on which more can be build and experiment.

1. Finish Basic Map
   - 1 Round
   - shooter vs. shooter
   - play until the first death
   - show a victory/defeat screen
2. Initial Role System
   - add fighter role
   - add role selection at the beginning
   - add one special skill to shooter and fighter
3. Map Objectives
   - create new map with different goal (flag capturing?)
   - add map selection
   - experiment with map environment (e.g. walls, powerups)
4. Player Input
   - polish player movement
   - revaluate camera (zoom, movement)
   - test for 4 players
