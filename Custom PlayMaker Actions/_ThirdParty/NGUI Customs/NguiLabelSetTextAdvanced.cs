//License: Attribution 4.0 International (CC BY 4.0)
//Author: Deek

/*--- __ECO__ __PLAYMAKER__ __ACTION__
EcoMetaStart
{
"script dependancies":[
						"Assets/PlayMaker Custom Actions/__Internal/FsmStateActionAdvanced.cs"
					  ]
}
EcoMetaEnd
---*/

using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
	[ActionCategory("NGUI")]
	[HelpUrl("http://hutonggames.com/playmakerforum/index.php?topic=15458.0")]
	[Tooltip("Sets the text of multiple NGUI UILabel component by converting and concatenating the given " +
			 "variables to a string and optionally adds additional strings to the start and end.")]
	public class NguiLabelSetTextAdvanced : FsmStateActionAdvanced
	{
		[RequiredField]
		[CheckForComponent(typeof(UILabel))]
		[Tooltip("The GameObject on which there is a UILabel")]
		public FsmGameObject[] gameObjects;

		[RequiredField]
		[HideTypeFilter]
		[UIHint(UIHint.Variable)]
		[Tooltip("The variable to convert to a string and set as the label text.")]
		public FsmVar[] stringParts;

		[Tooltip("Optionally add a string to the start of the String Parts.")]
		public FsmString addToFront;

		[Tooltip("Optionally add a string to the end of the String Parts.")]
		public FsmString addToBack;

		private GameObject go;
		private UILabel label;
		private string text;
		private char[] m_chars = null;
		private string m_stringGenerated = "";
		private bool m_isStringGenerated = false;
		private int m_charsCount = 0;
		private int m_charsCapacity = 0;
		private int prevStrPartsLength = 0;

		public override void Reset()
		{
			base.Reset();

			gameObjects = new FsmGameObject[1];
			stringParts = new FsmVar[1];
			addToFront = null;
			addToBack = null;
		}

		public override void OnEnter()
		{
			SetText();

			if(!everyFrame) Finish();
		}

		public override void OnActionUpdate()
		{
			SetText();
		}

		void ConvertText()
		{
			text = addToFront.Value;

			m_chars = new char[m_charsCapacity = 32];
			Clear();

			for(var i = 0; i < stringParts.Length - 1; i++)
			{
				stringParts[i].UpdateValue();
				Append(stringParts[i].GetValue().ToString());
			}

			ToString();

			stringParts[stringParts.Length - 1].UpdateValue();
			Append(stringParts[stringParts.Length - 1].GetValue().ToString());

			text += ToString();
			text += addToBack.Value;
		}

		void SetText()
		{
			ConvertText();

			foreach(var go in gameObjects)
			{
				if(!go.Value)
				{
					LogError("GameObject in " + Owner.name + " (" + Fsm.Name + ") is null!");
					return;
				}

				label = go.Value.GetComponent<UILabel>();

				label.text = text;
			}
		}

		public void Append(string value)
		{
			ReallocateIFN(value.Length);
			int n = value.Length;

			for(int i = 0; i < n; i++)
				m_chars[m_charsCount + i] = value[i];

			m_charsCount += n;
			m_isStringGenerated = false;
		}

		private void ReallocateIFN(int nbCharsToAdd)
		{
			if(m_charsCount + nbCharsToAdd > m_charsCapacity)
			{
				m_charsCapacity = System.Math.Max(m_charsCapacity + nbCharsToAdd, m_charsCapacity * 2);
				char[] newChars = new char[m_charsCapacity];
				m_chars.CopyTo(newChars, 0);
				m_chars = newChars;
			}
		}

		public bool IsEmpty()
		{
			return (m_isStringGenerated ? (m_stringGenerated == null) : (m_charsCount == 0));
		}

		public void Clear()
		{
			m_charsCount = 0;
			m_isStringGenerated = false;
		}

		public override string ToString()
		{
			if(!m_isStringGenerated)
			{
				m_stringGenerated = new string(m_chars, 0, m_charsCount);
				m_isStringGenerated = true;
			}
			return m_stringGenerated;
		}

	}
}