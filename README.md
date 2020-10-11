# Hallow's Hunt

### By Claudia Lapalme
#### ID :40058454

##### How to read the code
* separated into 4 scripts
    * GameOver.cs : game asks you if you want to replay
       * If yes, restarts game
       * If no, brings you back to the main menu
    * Ghost.cd : Handles the enemies
       * Enemy lifespan
       * Enemy random movement
       * Enemy speed (increased every level)
    * MainMenu.cs : everything related to the main menu
       * starts the game
       * quits the game
       * sfx when you press play and quit
    * Player.cs : gameplay mechanics
       * handles player lives
       * handles level timer
       * increases player level
       * spawns enemies
       * spawns dog when the player misses
       * etc.
 
 ##### Compile
You can just press play in the Unity editor if you want
You can also build the unity project and then run the .exe that was built
The game is available here: https://claudiabowie.itch.io/hallows-hunt

##### How to play
###### Regular Mode
* left click to shoot bullet
* kill 10 ghosts and encounter at least two witches to change level
* you win 3 points per killed ghost (one shot required)
   * 5 bonus points if you kill overlapping targets
* you get 5 points per killed witch (three shots required)

###### Special Mode
* Activated by pressing shift
* Lasts 3 seconds
* 5 second downtime between each special mode
* **hold** left mouse click to shoot infinitely
   * 1 point per kill
   * -2 per miss
   * no lives lost in this mode
 
