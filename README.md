# Game Overview
#### classic slot machine game with initial player balance of 100$. The player can increase or decrease his bet using the UI symbols. The player also has a balance UI element denoting his current balance. on spinning using the slot_handle, one of 3 outcomes can happen:
#####  1. Jackpot: all 3 symbols are matching, the player wins and the payout is multiplied.
#####  2. Return Bet: If any 2 symbols are matching, the bet is returned to player balance.
#####  3. Lose Bet: if all 3 symbols are distinct, player loses bet & player balance is subtracted.

## WebGL build
#### to play the game simply go to: https://follicallychallenged.itch.io/slot-machine

### bonus-feature
#### win window has a particle system that plays win particle effect & background-music

## Approach
##### to design the game i used layer-sort to layer different components above each other. To spin the symbols from top to bottom i used RandomPick script which will be called when the slot_handle has been clicked. This RandomPick then calls the SlotSymbol script and spins the slot-spaces. After a duration, the first spinning slot-space stops, then after some time the other slot-spaces stop. The final result of the middle row it given back to RandomPick which decide if the player wins or loses based on the symbols (if they are matching or not) To place a bet I created different UI elements to increase/decrease bet & UI elements to display messages such you lose the bet or Pair and the bet is returned. Additionally if you win a JACKPOT window pops up with particle system in the background that signifies a win. If your balance goes to 0, a GAME OVER window pops up where you can either restart or quit.

###### Note: the no button on the game over screen does not work with WebGL builds and instead player is redirected to the game again. 
