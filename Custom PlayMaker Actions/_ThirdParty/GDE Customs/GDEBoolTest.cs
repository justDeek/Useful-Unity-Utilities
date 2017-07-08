using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(GDMConstants.ActionCategory)]
    [Tooltip("Sends Events based  on the value of a bool variable from a GDE Item.")]
    public class GDEBoolTest : GDEActionBase
    {
        [Tooltip("Event to send if the bool variable is True.")]
				public FsmEvent isTrueEvent;

        [Tooltip("Event to send if the bool variable is False.")]
        public FsmEvent isFalseEvent;

        [Tooltip("Event to send if the Item or Field couldn't have been found.")]
        public FsmEvent notFoundEvent;

				[UIHint(UIHint.Variable)]
				[Tooltip("Optionally store the bool variable to test.")]
				public FsmBool storeBoolResult;

        [Tooltip("Repeat every frame while the state is active.")]
				public bool everyFrame;

        public override void Reset()
        {
            base.Reset();
            storeBoolResult = null;
						isTrueEvent = null;
            isFalseEvent = null;
            notFoundEvent = null;
						everyFrame = false;
        }

        public override void OnEnter()
        {
            try
            {
                Dictionary<string, object> data;
                if (GDEDataManager.Get(ItemName.Value, out data))
                {
                    bool val;

                    data.TryGetBool(FieldName.Value, out val);
                    storeBoolResult.Value = val;
                }
                else
                {
                  Fsm.Event(notFoundEvent);
                  // storeBoolResult.Value = false;
                  // Finish();
              }

  						// Override from saved data if it exists
  						storeBoolResult.Value = GDEDataManager.GetBool(ItemName.Value, FieldName.Value, storeBoolResult.Value);

            }
            catch(UnityException ex)
            {
                LogError(ex.ToString());
            }
            finally
            {
              Fsm.Event(storeBoolResult.Value ? isTrueEvent : isFalseEvent);

              if (!everyFrame)
              {
                  Finish();
              }
            }
        }

				public override void OnUpdate()
				{
					Fsm.Event(storeBoolResult.Value ? isTrueEvent : isFalseEvent);
				}
    }
}

#endif
