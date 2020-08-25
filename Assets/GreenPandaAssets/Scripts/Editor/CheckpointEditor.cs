using UnityEditor;
using UnityEngine;
using GreenPandaAssets.Scripts.Services;

namespace GreenPandaAssets.Scripts.Other
{
	/// <summary>Ensures every checkpoint has a unique ID</summary>
	[CustomEditor(typeof(Checkpoint))]
	[CanEditMultipleObjects]
	public class CheckpointEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			Checkpoint checkpoint = target as Checkpoint;
			GUILayout.BeginHorizontal();
			GUIContent content = new GUIContent();
			content.text = "Order number";
			content.tooltip = "The truck will drive from checkpoints with lower order numbers to checkpoints with higher order numbers."
			+ " The checkpoints are always looped in a circle (the truck will drive from the last checkpoint to the first one).";
			GUILayout.Label(content, GUILayout.Width(115), GUILayout.Height(25));
			int result = EditorGUILayout.IntField(checkpoint.OrderNumber);
			GUILayout.EndHorizontal();
			if (result < 0)
				result = checkpoint.OrderNumber;


			// Ensure that each checkpoint has a unique order number.
			var allCheckpoints = Resources.FindObjectsOfTypeAll<Checkpoint>();
			if (allCheckpoints != null)
			{
				for (int i = 0; i < allCheckpoints.Length; i++)
				{
					if (allCheckpoints[i].OrderNumber == result)
					{
						allCheckpoints[i].OrderNumber = checkpoint.OrderNumber;
						break;
					}
				}
			}

			checkpoint.OrderNumber = result;
		}
	}
}
