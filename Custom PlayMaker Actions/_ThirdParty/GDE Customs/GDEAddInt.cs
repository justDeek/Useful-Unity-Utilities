using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;


#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(GDMConstants.ActionCategory)]
    [Tooltip("Adds the specified int to the prevalent one to quickly increment or add up. Optionally saves afterwards.")]
    public class GDEAddInt : GDEActionBase
    {
        [Tooltip("The int value to add to the one found at the Item- & Field-Name.")]
        public FsmInt intToAdd;
        public FsmBool save;

        private FsmInt prevalentInt;

        public override void Reset()
        {
            base.Reset();
            intToAdd = 1;
            save = true;
            prevalentInt = null;
        }

        public override void OnEnter()
        {
            try
            {
                Dictionary<string, object> data;
                if (GDEDataManager.Get(ItemName.Value, out data))
                {
                    int val;
                    data.TryGetInt(FieldName.Value, out val);
                    prevalentInt.Value = val;
    				    }
    				prevalentInt.Value = GDEDataManager.GetInt(ItemName.Value, FieldName.Value, prevalentInt.Value) + intToAdd.Value;
            GDEDataManager.SetInt(ItemName.Value, FieldName.Value, prevalentInt.Value);
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
