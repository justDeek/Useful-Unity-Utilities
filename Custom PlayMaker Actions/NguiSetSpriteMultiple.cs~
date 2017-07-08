using HutongGames.PlayMaker;
// using Tooltip = HutongGames.PlayMaker;

/*
 * *************************************************************************************
 * Created by: Rocket Games Mobile  (http://www.rocketgamesmobile.com), 2013-2014
 * For use in Unity 4.5+
 * *************************************************************************************
*/

/// <summary>
/// Sets the sprite value of a UISprite
/// </summary>
[ActionCategory("NGUI")]
[HutongGames.PlayMaker.Tooltip("Sets the sprite value of multiple UISprite's")]
public class NguiSetSpriteMultiple : FsmStateAction
{
    [RequiredField]
    [HutongGames.PlayMaker.Tooltip("NGUI Sprite to set")]
    public FsmOwnerDefault[] NguiSprite;

    [RequiredField]
    [HutongGames.PlayMaker.Tooltip("The name of the new sprite")]
    public FsmString NewSpriteName;

    public override void Reset()
    {
		NguiSprite = new FsmOwnerDefault[3];
        NewSpriteName = null;
    }

	public override void OnLateUpdate() //Default: OnUpdate()
    {
        // set the sprite
        DoSetSprite();
        Finish();
    }

    private void DoSetSprite()
    {

		foreach (var gonum in NguiSprite)
		{

			// exit if objects are null
			if ((gonum == null) || (NewSpriteName == null) || (string.IsNullOrEmpty(NewSpriteName.Value)))
				return;

			// get the sprite
			UISprite sprite = Fsm.GetOwnerDefaultTarget(gonum).GetComponent<UISprite>();

			// exit if no sprite found
			if (sprite == null)
				return;

			// set new sprite name
			sprite.spriteName = NewSpriteName.Value;

		}

    }
}