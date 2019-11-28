# Game Engines Unity Project

## Please use the classes branch and not the master

## Overview

- top down 2D battle game
- two teams fight against each other on the same map (screen/device)
- teams are equal in size and can consist of 1 to 3 players
- as placeholder bots can be added to the game
- each player can choose a role (normal shooter, sniper, melee, bouncer)
- there are two game modes: death match and defense
- there are three maps available (large, medium and small)

### Menu

- by clicking any button on the controller you can join
- by clicking on an empty slot you can add a bot
- by clicking on a your character you can change your team (color)
- by clicking on the arrows you can change your class
- by clicking on the picture of the map you can change the map
- click on the game mode to toggle the game mode, suitable maps will automatically be selected

### Controls

- players can walk and fire attacks around them
- controllers are used to play
- use left stick to move, right stick to look around / aim
- use B to navigate in menu
- use RB to shoot
- use RT to use your special shooting skill (if available in class)
- use LB to use your special movement skill (if available in class)

### Game Modes

- in deatchmatch mode you have to kill as much enemies as possible
- the team that has reached a certain amount of kills, wins the round
- in defense mode you have to defend your base which is a block near your spawn area
- at the same time you have to destroy the block of the enemy team
- the team that destroys the other team's block first wins
- the block actually crumbles uppon taking damage

### Classes

- Normal shooter: shooting skill with short cooldown, medium range, damage and speed of projectiles
- Sniper: shooting skill with short range, bad accuracy, low damage + snipe skill with high speed, high damage, long range, the longer the shot the higher the damage
- Melee: a tough and slightly faster class with a melee attack that knockbacks back others (and does additional damage on pushing into walls)
- (beta ;) Bouncer: shooting skill with a shorter range but bouncing bullets, going further each bounce

## Game Mechanics

-

## Game Art

- blocky and simple
- low resolution pixel game
- various shaders on screen while game is running
- emulate an arcade game / old monitor (mostly analog TV effects)
- all kinds of effects for good game feel / juice, such as muzzle flash, screenshake, and more
