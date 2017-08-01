## NGUIEffects.cs
![Image](/Screenshots/Components//NGUIEffects1_Info.png)

Adds several very useful effects to NGUI's UISprite component that were previously exclusive to UILabel's (for whatever reason) + additional options and effects.
They provide the benefit of modifying or adding effects to any sprite at runtime, but are more performance intensive than sprites 
with prebakes effects (wich is pretty negligible if you don't overdo it, but that also applies to everything, generally speaking).
This Effect-Addon also doesn't slow down any existing sprites since it only gets called when an effect is selected,
so you can apply any effect on several sprites and the rest of them won't get bogged down.

The Implementation takes just 3 easy steps, wich was important to me as I didn't want to change much or loose anything when I update NGUI:
1.	 In UISprite.cs add "ApplyEffect(verts, uvs, cols, offset);" to the end of OnFill(),
2.	 change the inheritance in UIBasicSpriteEditor.cs from 'UIWidgetInspector' to 'NGUIEffectsInspector'
3.	 and in UISprite.cs from 'UIBasicSprite' to 'NGUIEffects'
So it just plants itself between UIBasicSprite and UISprite, so that it can be placed between the Gradient- and Color-Tint-Options

Currently included:
- Shadow: Drops a shadow of the Sprite under it with any color, at the specified offset and with the specified scale
- Shadow 3D: Drops a shadow according to the distance of the source point (the screen center by default) and the current position 
to give an 3D feel even with an orthograph camera (wich is recommended for UICamera's anyway);
also actively updates when you move your GameObject in Edit-Mode (there seems to be a little bug that if you 
move the GameObject too much / too long, the Editor seems to freeze until you select the Project-Window or an Item in it; weird and rare behaviour)
- Outline & Outline8: Just like in UILabel's (I adopted them from the UILabel-Effects if someone needs them, 
I certainly never did and you can get a better, more seemless outline by using Shadow with a slightly bigger Scale anyway)

Maybe planning to add (if at all possible with UISprite's):
- Any kind of Blur or several of them (Box Blur, Gaussian Blur, Motion Blur, Median Filter Blur), all in different steps or with a float slider from not blurry till too blurry
- More Effects that could look cool or could be useful (for example: Black/White (discarding color information), emboss, bezel, morph)
- a gif preview or several in here that show their behaviour