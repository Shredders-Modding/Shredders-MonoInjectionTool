# Shredders MonoInjectionTool
 
* A MelonMod that will take your DLL that contains Monobehaviours and register the types with Il2CPP. 
* You may also add Harmony patches to your DLL and this tool will patch the appropriate methods in Shredders

## Getting Started

### Dependencies

* Must own the Shredders game
* MelonLoader must be installed

### Installing

* Build the project
* Drag the MonoInjectionTool.dll into Shredders/Mods folder
* Create a folder in Shredders\UserLibs called Inject
* Drag and drop your dll in the Shredders\UserLibs\Inject folder you created
* Start the game and MonoInjectionTool will tell you if it loaded your library in the MelonLoader console window
