# Shredders MonoInjectionTool
 
* A MelonMod that will take your DLL that contains Monobehaviours and register the types with Il2CPP
* You may also include an AssetBundle such as UI built in Unity that is made for your DLL and this tool will load and instantiate everything in the bundle
* You may also add Harmony patches to your DLL and this tool will patch the appropriate methods in Shredders

## Getting Started

### Dependencies

* Must own the Shredders game
* MelonLoader must be installed

### Installing

* Build the project
* Drag the MonoInjectionTool.dll into Shredders\Mods folder
* Create a folder in Shredders\UserLibs called Inject
* Drag and drop your dll and AssetBundle (if you have one) in the Shredders\UserLibs\Inject folder you created
* Start the game and MonoInjectionTool will tell you if it loaded your library / bundle in the MelonLoader console window
