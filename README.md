# Horror-game
Overview of the game:
My game concept is inspired by Amnesia: The Dark Descent. The player begins in the entrance of a mysterious house with locked doors. It's a first-person exploration game where the player must collect notes to uncover the story, along with keys to unlock doors required for progression. The goal is to gather all the notes, all the keys, and a magical item hidden in the attic to escape the house. Upon leaving the house, the player enters a forest and must survive a chase from a ghost while finding the way out. The player can temporarily force the ghost to respawn farther away by shining their flashlight on it.

Main parts:
-	Player Controls: The game uses standard first-person controls. The player moves with WASD, sprints with Shift, jumps with Space, and the cursor is locked for smooth camera movement.
-	Keys: Keys are hidden throughout the house and must be collected to unlock certain doors. Players can view collected keys in the “Key Inventory,” which also provides backstory about the corresponding rooms.
-	Notes: Notes function like keys. They can be collected and viewed in the “Note Inventory,” where the player can read the story fragments they contain.
-	Other Collectibles: Players can gather batteries to recharge the flashlight and pills or med kits to maintain their sanity. These items are stored in the inventory, where they can be viewed, dragged, and used as needed.
-	Doors: Doors can be interacted with by opening, closing, or unlocking them. Locked doors will rattle and make a sound when interacted with them.
-	Inventory System: Items collected during the game are placed in an inventory. Inventory slots are visible at the bottom of the screen, and pressing I opens the full inventory, allowing players to drag and drop items. Slots are selected with the number keys (1-9), and items can be used by right-clicking when the slot with the item is selected.
-	Monster: The monster appears in the forest and pursues the player. Pointing the flashlight at the monster forces it to respawn farther away, giving the player temporary relief.
-	UI: Above the inventory slots, a sanity bar slowly decreases over time. At low sanity levels, players will hear unsettling sounds like voices and heartbeats. The flashlight operates similarly, with a battery bar that drains to 0%. When the battery is critically low, the flashlight begins to flicker.

How requirements for the project were implemented:
Requirement:	Implementation
An interactive camera that the user can move using input from keyboard, mouse, controller, touch, etc. or connected to an object or character that the user moves.
	The first-person controller, that uses inputs from WASD, Spacebar and Shift together with input from mouse for camera movement.

A character or objects that the user can move and interact with directly through input	The player movement
Objects or characters in the 3D world that the user can interact indirectly with – collect or push by colliding with, 
turn, shoot, grab, etc.	the collectibles (notes, keys, med kit, pills), the magic ball, doors, cabinet and the lever mechanic.
Objects or characters in the 3D world that are controlled by script to move, follow/chase the player, change their shape, size, or behavior	The monster that haunts you in the forest, door, cabinet, lever.
Materials with different properties, directional/point/spotlights and global illumination or baked lights	Ceiling lights, flashlight, candle lights, glowing cauldron, post processing bloom, the material on the cauldron and the magic ball.
Physics interactions with the 3D world by the user through rigidbodies, raycasting, forces, collisions, etc.	Ray casting with flashlight and interaction system
A GUI which is either used for interacting with the application with buttons, sliders, text fields, etc. or displays results 
from user’s interactions like scores, dialog, descriptions, etc.	The inventory system, dragging and dropping items, battery and sprinting text, sanity bar.
Object or player navigating through NavMesh path finding	The monster haunting you uses NavMesh to navigate and respawn position.
Level created through ProBuilder or Terrain Tools	Terrains made for forest environment in both scenes.
Animations on world objects or characters. Ragdolls or Inverse kinematics	The monster is animated, though it’s a prefab from asset store, so I have NOT made the animation myself!
Using the particle system or utilizing noise and randomness for procedural generation	Used particle system to create fog/smoke coming out of the cauldron.


Project Parts:
Scripts:
-	Barrier: Used to handle the barrier in the entrance and calling methods to display the correct message based on the player’s progress.
-	Battery: Manages the functionality of the battery item, inherits from PickUpItems.
-	Cabinet: Handles logic for the pushing of the cabinet in the dining room
-	Door: Handles logic for doors, opening/closing, locking, banging and such.
-	Flashlight: Handles flashlight's logic for toggling it on/off, draining energy, triggering flickering at low energy, and detecting monsters within the flashlight's range to respawn them.
-	FlickeringLights: Handles the ceiling lights flickering
-	HUDController: Controls all UI elements on the screen (battery, sprint text, sanity bar and interaction text)
-	Interactables: Placed on all interactable, making sure they get their outline, and the correct methods are called and text displayed when interacted with the objects
InventoryItem: Manages the behavior and appearance of items in the player's inventory, what gets instantiate as a prefab when something is added to the player's inventory
-	InventoryManager: Handle the player's inventory system in a game, adding items, using items, toggling inventory UI, slot and space management
-	InventorySlot: Handles the functionality for when a slot is selected and deselected and how dropped items are handled.
-	Item: Defines a custom scriptable object for items used in the game.
-	Key: Holds key Name and Number to be used in KeyInventory and has logic for collecting a key.
-	KeyInventoryManager: Manages the KeyInventory, adding them and updating the inventory panel to show the correct keys and their content.
-	Lever: Inherits from PickUpItems, handles the logic for picking up the lever.
-	LeverBase: Handles inserting the lever to the lever base and opening the gate.
-	MagicBall: Inherits from PickUpItems, makes the magic ball rotate.
-	Medkit: Inherits from PickUpItems. Logic for using Medkit and adding sanity.
-	MonsterAI: Handles Monster behavior like chasing the player, respawn at random locations, and affects the sanity drain when getting hit.
-	MouseLook: Handles Mouse Input and locks the cursor to the screen and rotates camera.
-	MusicManager: Singleton that controls music
-	Note: Handles information about name and note number and method for adding to note inventory.
-	NoteInventoryManager: Manages the Note Inventory, adding notes and updating the inventory panel to show the correct notes and their content, keeping track of them.

-	PickUpItems: Parent class to the items to pick up. Has common functionality for them add, including adding an object to inventory and using the item.
-	PlayerInteraction: Handles the player interactions with interactable items in the scene through raycasting
-	PlayerMovement: Handles the player’s movement.
-	TriggerCollider

Scenes:
-	Forest: Second scene in the game, where you have escaped the house and are chased by a monster.
-	House: First scene where you spawn.
-	
Models and Prefabs (link to asset store packages)
-	Big Furniture Pack: https://assetstore.unity.com/packages/3d/props/furniture/big-furniture-pack-7717
-	Furniture Free Pack: https://assetstore.unity.com/packages/3d/props/furniture/furniture-free-pack-192628 
-	Old Bathroom Objects: https://assetstore.unity.com/packages/3d/props/interior/old-bathroom-objects-120069 
-	Flooded Grounds: https://assetstore.unity.com/packages/3d/environments/flooded-grounds-48529 
-	Terrain Sample Asset pack: https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808 
-	Grass and Flowers Pack 1: https://assetstore.unity.com/packages/2d/textures-materials/nature/grass-and-flowers-pack-1-17100 
-	Conifers (BOTD): https://assetstore.unity.com/packages/3d/vegetation/trees/conifers-botd-142076 
-	Outdoor Ground Textures: https://assetstore.unity.com/packages/2d/textures-materials/floors/outdoor-ground-textures-12555 
-	Ultimate Low Poly Dungeon: https://assetstore.unity.com/packages/3d/environments/dungeons/ultimate-low-poly-dungeon-143535 
-	Survival Game Tools: https://assetstore.unity.com/packages/3d/props/tools/survival-game-tools-139872 
-	Quick Outline: https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488 
-	Rusty Flashlight: https://assetstore.unity.com/packages/3d/props/tools/rusty-flashlight-122403 
-	MN D: https://assetstore.unity.com/packages/3d/characters/creatures/mn-d-253748#reviews 

Resources used:
- How to make a HEALTH BAR in Unity by Brackeys: https://www.youtube.com/watch?v=BLfNP4Sc_iA&t=50s 

- FIRST PERSON MOVEMENT in Unity - FPS Controller by Brackeys https://www.youtube.com/watch?v=_QajrabyTJc&t=10s 

- Unity INVENTORY: A Definitive Tutorial by Coco Code https://www.youtube.com/watch?v=oJAE6CbsQQA&t=233s 

- Drag and drop in Unity UI - create your own inventory UI! By Coco Code https://www.youtube.com/watch?v=kWRyZ3hb1Vc

- How To Make A HORROR Game In Unity | Flickering Lights | Horror Series Part 010 by User1 Productions 
https://www.youtube.com/watch?v=oOmAB0rs518&t=75s  
 
