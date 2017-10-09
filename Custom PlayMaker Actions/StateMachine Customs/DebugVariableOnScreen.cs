
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(ActionCategory.Debug)]
    [Tooltip("Display the value of any FSM Variable on screen at runtime at the specified position. Optionally constrain the label size. NOTE: Uses OnGUI so you need a PlayMakerGUI component in the scene.")]
    public class DebugVariableOnScreen : FsmStateAction
    {
        [HideTypeFilter]
        [UIHint(UIHint.Variable)]
        [Tooltip("The variable to debug.")]
        public FsmVar variable;

        [Tooltip("Specify the width and height of the label. If left to 'None' uses the screen width & height (doesn't constrain the label size).")]
        public FsmVector2 labelSize;

        [Tooltip("Choose the position of the label, starting from the top left. Positive X goes to the right and positive Y goes down (just experiment during runtime to grasp how it works).")]
        public FsmVector2 labelPosition;

        public override void Reset()
        {
            variable = null;
            labelPosition = Vector2.zero;
            labelSize = new FsmVector2() { UseVariable = true };
        }

        public override void OnGUI()
        {
            variable.UpdateValue();
            GUI.Label(new Rect(labelPosition.Value.x,
                               labelPosition.Value.y,
                               labelSize.Value.x,
                               labelSize.Value.y),
                      variable.DebugString());
        }

        public override void OnEnter()
        {
            if (labelSize.IsNone)
            {
                labelSize.Value = new Vector2(Screen.width, Screen.height);
            }
        }
    }
}
