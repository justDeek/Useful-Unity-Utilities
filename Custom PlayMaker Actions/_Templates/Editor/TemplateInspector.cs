using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using HutongGames.PlayMakerEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomActionEditor(typeof(Template))]
public class TemplateInspector : CustomActionEditor
{
	//store references to the target script
	private static Template action;
	private PlayMakerFSM thisFSM;

	//set this to true on every change to only update OnGUI when something has differed from the last call
	private bool isDirty = false;

	//use this for expensive initialization. Gets called when the target-fsm is selected.
	public override void OnEnable()
	{
	}

	//gets called when interacting with the action window or if something has changed
	public override bool OnGUI()
	{
		//reset isDirty
		isDirty = false;

		//get action and FSM reference
		action = target as Template;

		if(action.Fsm == null || action.Fsm.FsmComponent == null)
		{
			return false;
		}

		thisFSM = action.Fsm.FsmComponent;

		//draw default inspector (action-content) if you only want to add to it
		isDirty = DrawDefaultInspector();
		//OR only draw single fields (name has to be the exact same as the variable in the base script):
		//EditField("gameObject");

		//-- Your main code goes in here ( or before DrawDefaultInspector() ) --//

		//needs to be at the end and tells OnGUI if something has changed
		return isDirty || GUI.changed;
	}

}
