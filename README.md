Creating a insaniquarium inspired game.

![](gameplay.GIF)

Raise fish to collect their coins as they mature. Occasionally enemies will spawn into the tank to eat your fish.
Shoot them down to keep them from eating your fish. 







Documentation

Prefabs:

Guppy--------------------------------------------------------------------------------------------------
    - Guppy are the player's starter fish. Cheapest and fastest for the player to raise. 
    -Guppy mechanics:
        + drop coins for player: silver(teen) -> gold(adult)
        + eat food pellets dropped by player
        
    -Guppy scripts:
        + g_Age         :reference for the guppy's age and life stage
        + g_Collision   :when a collision happens, references another attached script
        + g_Money       :how this guppy creates money
        + g_Movement    :how the guppy moves within the tank, given State
        + g_SM          :SM = State Machine, holds reference to what state the gubby is in
        + g_Stats       :holds variables for health and hunger 


Coins---------------------------------------------------------------------------------------------------


Foods---------------------------------------------------------------------------------------------------



Enemies:
Starfish------------------------------------------------------------------------------------------------
LargeMouthBass------------------------------------------------------------------------------------------
Catfish-------------------------------------------------------------------------------------------------



