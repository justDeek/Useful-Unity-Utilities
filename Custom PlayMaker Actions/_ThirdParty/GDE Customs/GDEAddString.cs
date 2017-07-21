using UnityEngine;
using System.Collections.Generic;
using GameDataEditor;


#if GDE_PLAYMAKER_SUPPORT

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory(GDMConstants.ActionCategory)]
    [Tooltip("Concatenates the specified String with the prevalent one. Optionally choose to add it to the end or beginning and save afterwards.")]
    public class GDEAddString : GDEActionBase
    {
        [Tooltip("The String value to add to the one found at the Item- & Field-Name.")]
        public FsmString stringToAdd;
        [Tooltip("Wheter to add the string to the end (true) or beginning (false).")]
        public FsmBool addToEnd;
        public FsmBool save;

        private FsmString prevalentString;

        public override void Reset()
        {
            base.Reset();
            stringToAdd = "";
            addToEnd = true;
            save = true;
            prevalentString = null;
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
                  prevalentString.Value = val;
              }
            if (addToEnd.Value)
            {
              prevalentString.Value = string.Concat(GDEDataManager.GetString(ItemName.Value, FieldName.Value, prevalentString.Value), stringToAdd.Value);
            }
            else
            {
              prevalentString.Value = string.Concat(stringToAdd.Value, GDEDataManager.GetString(ItemName.Value, FieldName.Value, prevalentString.Value));
            }
            GDEDataManager.SetString(ItemName.Value, FieldName.Value, prevalentString.Value);
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
