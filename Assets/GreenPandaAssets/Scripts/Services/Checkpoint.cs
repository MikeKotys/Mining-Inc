using UnityEngine;
using System.Linq;


namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Allows designers to place checkpoints in the Editor, which the dumping truck will follow on its way.</summary>
	[ExecuteInEditMode]
	public sealed class Checkpoint : MonoBehaviour
	{
		public int OrderNumber;

#if UNITY_EDITOR

		/// <summary>Helps catch duplication events in the editor as well as new check point creation events.</summary>
		[SerializeField][HideInInspector]
		int instanceID = 0;

		private void Awake()
		{
			if (!Application.isPlaying)
			{
				// Catch duplication events and assign the correct value for the newly created checkpoint in the editor.
				bool assignValue = false;

				if (instanceID != GetInstanceID())
				{
					if (instanceID == 0)
					{
						instanceID = GetInstanceID();
						assignValue = true;
					}
					else
					{
						instanceID = GetInstanceID();
						if (instanceID < 0)
							assignValue = true;
					}
				}

				if (assignValue)
				{
					var allCheckpoints = GameObject.FindObjectsOfType<Checkpoint>();
					if (allCheckpoints != null)
						OrderNumber = allCheckpoints.Length - 1;
				}
			}
		}
#endif

		void OnDrawGizmos()
		{
			// Draw checkpoint gizmo.
			Gizmos.color = new Color(1, 0, 0, 0.4f);
			Gizmos.DrawSphere(transform.position + Vector3.up * 3.5f, 1);
			Gizmos.DrawCube(transform.position + Vector3.up * 1.75f, new Vector3(.2f, 3.5f, .2f));
			Gizmos.DrawCube(transform.position, new Vector3(1, .15f, 1));

			// Draw arrows between checkpoints.
			var allCheckpoints = GameObject.FindObjectsOfType<Checkpoint>().OrderBy(x => x.OrderNumber).ToArray();
			for (int i = 0; i < allCheckpoints.Length; i++)
			{
				int next = (i + 1) % allCheckpoints.Length;

				Gizmos.DrawLine(allCheckpoints[i].transform.position, allCheckpoints[next].transform.position);

				Vector3 direction = allCheckpoints[i].transform.position - allCheckpoints[next].transform.position;

				direction = Quaternion.AngleAxis(30, Vector3.up) * direction.normalized;
				Gizmos.DrawLine(allCheckpoints[next].transform.position, allCheckpoints[next].transform.position + direction);
				direction = Quaternion.AngleAxis(-60, Vector3.up) * direction.normalized;
				Gizmos.DrawLine(allCheckpoints[next].transform.position, allCheckpoints[next].transform.position + direction);
			}
		}
	}
}
