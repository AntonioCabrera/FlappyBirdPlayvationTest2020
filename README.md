# FlappyBirdPlayvationTest2020
A project for an interview test. Consist in optimize and improve the code and the game's playability in 3-5 hours.  
Original asset can be found on https://assetstore.unity.com/packages/templates/flappy-bird-style-example-game-80330


## Main changes:  
  * Main menu and Gameover menu with leaderboards added.  
  * Input name menu for the first execution with basic controls to filter url friendly characters and simple checks to show a notice if that name has been already used.  
  * Leaderboard records using a http://dreamlo.com/ webservice.  
  * Locally stored highscore.  
  * New obstacle.  
  * Modified columns to make the gameplay a bit more ease, wider spots and softer jump to help the user's experience.  
  * Revamped UI updated to use TextMeshPro and optimized for responsivity on all devices and from all unused properties like RaycastTarget and RichText.  
  * GPU instanced materials to improve performance.  
  * Camera properties and unused components optimized (Depth only, deprecated components...).  
  * Removed redundant loop function on animations.    
  * Project hyerarchy order.  
  * Bird can't fly to the sky without limits anymore.  
  * Backend switched from mono to IL2CPP and architecture to ARMx64.  
  * Visual feedback added for jump and add score.  
  * Soft UI animations to help UX.   
  * Removed Android games and Android Tv from build to save some build space.  
  * Code optimization:  
     * Regularized to Microsoft standards for C# (UCC for public properties lCC for private properties).  
     * Memory allocated properties.  
     * UIManager added to replace in-script injections to the UI for calls to the UIManager.  
     * Forced framerate to 60 for uniform experience across all devices.  
     * Column script deleted to improve memory instances of on scene.  
     * Column pool improved to be more precise and performance friendly replacing checks on update to a coroutine.   
     * Columns are moved to a single parent now to remove redundant scripts and optimize scene.  
     * Bird script now checks and addforce on a FixedUpdate for a more uniform execution.  
         
 ## Comments:  
 I decided to avoid AWS as I used it on the past but only from the FrontEnd, although I know a little about its APIS and flows, not enought to spend a lot of time preparing the service. I decided to use Dreamlo, a simple and small service to provide what I needed in this test.  
 I spent a bit more than 6 hours in the exercise because I found some troubles when testing on Android related to the Unit Test assemblies so I had to remove them because I really wanted it to be mobile ready and it is! It only needs to sign the apk and its prepared for the store. 
   
 * Now I'm out of time, but I want to add some things I would have liked to add:  
   * Optimization on background textures and revamp of the scrolling system.    
   * Sprite packing the project.  
   * Think off the possibility of using Tween animations.    
   * Add some features like a power up to make the bird grow and destroy colliding columns with an animation.   
   * More obstacle variety.  
   * Add unit testing again and solve that trouble I had (altought the size of the project and features don't give much space to need testing).   
   * Improve control on WebRequest calls, like a cache for your best score to post it the next time you have connectivity and/or check internet availability.   
   * Reuse the scene so every time you die you don't need to load the same scene again.   
   * Improve Bird animations.   
