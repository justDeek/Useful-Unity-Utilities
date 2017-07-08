
using UnityEngine;

/// <summary>
/// Sets the sprite value of multiple UISprites
/// </summary>
namespace HutongGames.PlayMaker.Actions
{
  [ActionCategory("NGUI")]
  [HutongGames.PlayMaker.Tooltip("Sets the sprite value of multiple UISprites.")]
  public class NguiSetSpriteMultiple : FsmStateAction
  {
      [RequiredField]
      [Tooltip("NGUI Sprite to set")]
      public FsmGameObject[] NGUISpriteAmount;

      [RequiredField]
      [Tooltip("The name of the new sprites")]
      public FsmString NewSpriteName;

      [Tooltip("Optionally enable or disable all UISprite Components.")]
      public FsmBool enableSpriteComponents;

      [Tooltip("Optionally enable or disable all specified GameObjects.")]
      public FsmBool enableGameObjects;

      public override void Reset()
      {
  		NGUISpriteAmount = new FsmGameObject[3];
      NewSpriteName = null;
      enableSpriteComponents = new FsmBool() {UseVariable = true};
      enableGameObjects = new FsmBool() {UseVariable = true};
      }

  	  public override void OnLateUpdate()
      {
          // set the sprite
          DoSetSprite();
          Finish();
      }

      private void DoSetSprite()
      {

      if ((NewSpriteName == null) || (string.IsNullOrEmpty(NewSpriteName.Value)))
      {
        Debug.LogWarning("\"New Sprite Name\" is empty. Please specify.");
        return;
      }
  		foreach (var gonum in NGUISpriteAmount)
  		{
  			// exit if objects are null
  			if (gonum == null)
        {
          Debug.LogWarning("One of the Elements is null / not defined.");
          return;
        }

  			// get the UISprite component
  			UISprite sprite = gonum.Value.GetComponent<UISprite>();

  			// exit if no sprite found
  			if (sprite == null)
        {
          Debug.LogWarning("No UISprite Component found on " + gonum.Value.name);
          return;
        }

        if (!enableGameObjects.IsNone)
        {
          if (enableGameObjects.Value == true)
          {
            gonum.Value.SetActive(true);
          }
          else
          {
            gonum.Value.SetActive(false);
          }
        }

        if (!enableSpriteComponents.IsNone)
        {
          if (enableSpriteComponents.Value == true)
          {
            sprite.enabled = true;
          }
          else
          {
            sprite.enabled = false;
          }
        }
  			// set new sprite name
  			sprite.spriteName = NewSpriteName.Value;

  		}

    }
  }
}
