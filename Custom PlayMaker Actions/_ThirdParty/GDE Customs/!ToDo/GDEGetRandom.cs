using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;
using System.Text;
using System;

#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(GDMConstants.ActionCategory)]
    [Tooltip("Store a random Item in a String.")]
    public class GDEGetRandom : GDEActionBase
    {
        [UIHint(UIHint.FsmColor)]
        public FsmString StoreResult;

        public FsmString Schema;

        public override void Reset()
        {
            base.Reset();
            Schema = null;
            StoreResult = null;
        }

        public override void OnEnter()
        {
            try
            {
                Dictionary<string, object> data;
                if (GDEDataManager.Get(ItemName.Value, out data))
                {
                    string val;
                    data.TryGetString(FieldName.Value, out val);
                    StoreResult.Value = val;
				        }
            //var tmp = string.Concat("GDE", Schema.Value);
				    //StoreResult.Value = GDEDataManager.GetRandom<GDEArmorData>(); //GDEDataManager.GetColor(ItemName.Value, FieldName.Value, StoreResult.Value);
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
