
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory(ActionCategory.GUI)]
	[Tooltip("Fills the screen with a Color (as long as in current State). NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene. Added functionality to toggle visibility and disable the preview (filling the Game-Window). Automatically updates every Frame.")]
	public class DrawFullscreenColorAdvanced : FsmStateAction
	{
		[RequiredField]
		[Tooltip("Color. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
		public FsmColor color;

		[Tooltip("Toggles visibility. If false, sets the alpha to 0, if true applies the previous alpha Value (or full alpha if not every frame).")]
		public FsmBool isVisible;

		[Tooltip("If active, fills the Game-Window when this state is selected with set color.")]
		public FsmBool preview;

		private float storePrevAlpha;

		public override void Reset()
		{
			color = Color.white;
			isVisible = true;
			preview = true;
			storePrevAlpha = color.Value.a;
		}

		public override void OnGUI()
		{
			var guiColor = GUI.color;


			if (isVisible.Value)
			{
				guiColor.a = storePrevAlpha;
				GUI.color = guiColor;
				GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), ActionHelpers.WhiteTexture);

				if (preview.Value)
					GUI.color = color.Value;
				
			}
			else
			{
				guiColor.a = 0;
				GUI.color = guiColor;
				GUI.DrawTexture(new Rect(0,0,0,0), ActionHelpers.WhiteTexture);
			}

			storePrevAlpha = guiColor.a;
		}
	}
}