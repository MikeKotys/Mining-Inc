# Mining-Inc

Calculate and display coins income per min:

see TopUI.cs


Add 4th upgrade for Career Development - Bulldozed loads higher amount to the Truck:
see QuaryUpgradable.cs

Make FactoryView to support unlimited amount of skins:
see FactoryView.cs. Keep in mind, that although all the functionality is in place to have unlimited skins, the code in FactoryUpgradable.cs still only supports three factory skins due to the fact that there are only three factory models preset. Once more models are provided code would need to be changed.

Implement progress saving between the sessions:
see SaveSystem.cs and other classes in the "Scripts/Save System" folder.

Implement game loop:
done.

Implement game balance configs:
see Balance.cs

Implement MVC or MVC based pattern (or explain why not):
reading player input was transfered to Controller.cs class, all the model drawing/visibility change code is handled in ...View.cs classes. The logic is handeled in the remaining classes.

Use IoC principle (or explain why not):
see ServiceLocator.cs

Stick to the SOLID approach (or any other):
for your consideration

Decrease UI draw calls:
UI textures were combined into two atlases, reducing the overall draw call.

Optimize UI rendering:
all "Raycast Target" checkboxes were unchecked.

Optimize scene draw calls:
all models in the scene were moved to blender to have their textures baked into vertex colors. Now all models are rendered with a single material - VertexColorStandard. This resulted in slight discoloration of the final image due to the lighting models slightly differing between the Standard shader and my shader. This can be fixed for a propper project.
Moreover, I very much understand that baking the vertex colors manualy is inefficient - for the propper project I can write an editor extension that can automate thsi process.
Also, for the objects that still use textures (Dump Truck, bulldozer, etc...) I reduced the size of their textures in the texture importer.
Finally all Directional Lights in the scene but one were turned off and their intensity merged into the remaining Directional Light. This drastically improves draw call limit, but the double shadow effect was lost in the process. If the double shadow effect is desirable, the intensity of the main Directional Light must be reduced to .25 and the Secondary Camera directional light should be turned on. However, it is advised to either use Deferred rendering or URP for this scenario as builtin render pipeline with forward rendering is very inefficient in scenarios with multiple lights.

Optimize upgrade animation code:
see AnimatePropsService.cs. The code was not only optimized, but I also introduced support for custom designer scale for models. Without it large rocks dissapeared after the first animation cycle. Also the code was added to prevent several animation coroutines running at the same time, which fixed a bug that led to endless prop flickering if you clicked upgrade when the props were scaling up.

Text doesn’t overlaps other elements:
for your consideration. I can't see anything overlapping.

Adaptive UI for Portrait orientation:
for your consideration. The game can be comfortably played in both Portrait and Landscape modes.

Add popup window for every new object skin:
unclear task - I did not understand what is required here.

Animated UI interactions (of your choice):
button clicks are now animated.

“Not enough coins” feedback:
audio feedback was added.

Floating coins amount when Truck delivers the rock:
added, see FactoryView.cs

Visual feedback on each upgrade:
there is no immediate feedback but Truck moves faster with each Truck upgrade, rock is larger and gives more gold with each Quarry upgrade, bulldozer operates faster with each bulldozer upgrade and factory gives more gold per trip with each upgrade.

Upgrade level feedback - next level transition, max level reached etc:
did not do - out of time

Rock loaded feedback:
implemented - see ReceiveRockAnim.cs

Sounds (of your choice):
implemented

Truck VFX (of your choice):
did not do - out of time

Factory VFX (of your choice):
did not do - out of time

Environment (of your choice):
did not do - out of time
