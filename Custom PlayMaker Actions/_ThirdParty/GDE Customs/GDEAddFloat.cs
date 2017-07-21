using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;


#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(GDMConstants.ActionCategory)]
    [Tooltip("Adds the specified float to the prevalent one. Optionally saves afterwards.")]
    public class GDEAddFloat : GDEActionBase
    {
        [Tooltip("The float value to add to the one found at the Item- & Field-Name.")]
        public FsmFloat floatToAdd;
        public FsmBool save;

        private FsmFloat prevalentFloat;

        public override void Reset()
        {
            base.Reset();
            floatToAdd = 1;
            save = true;
            prevalentFloat = null;
        }

        public override void OnEnter()
        {
            try
            {
              Dictionary<string, object> data;
              if (GDEDataManager.Get(ItemName.Value, out data))
              {
                  float val;
                  data.TryGetFloat(FieldName.Value, out val);
                  prevalentFloat.Value = val;
              }
    				prevalentFloat.Value = GDEDataManager.GetFloat(ItemName.Value, FieldName.Value, prevalentFloat.Value) + floatToAdd.Value;
            GDEDataManager.SetFloat(ItemName.Value, FieldName.Value, prevalentFloat.Value);
            if (save.Value)
            {
              GDEDataManager.Save();
            }

            }
            catch(UnityException ex)
            {
                LogError(ex.ToString());
            }
            finally
            {
                Finish();
            }
        }
    }
}

#endif
