using System.Collections;
using UnityEditor;
using UnityEngine;

namespace GreenPandaAssets.Scripts.Other
{
	/// <summary>Introduces 2 buttons.</summary>
	[CustomEditor(typeof(Balance))]
	public class BalanceEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			Balance balance = target as Balance;

			DrawDefaultInspector();

			if (GUILayout.Button("Get current values"))
				balance.GetCurrentValues();

			if (GUILayout.Button("Apply"))
				balance.Apply();
		}
	}
}
