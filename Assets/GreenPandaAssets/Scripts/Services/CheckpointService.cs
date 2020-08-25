using UnityEngine;
using System.Linq;
using System;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Tracks checkpoints placed by designers. Feeds them into the truck (trucks?) on demand.</summary>
	public sealed class CheckpointService : MonoBehaviour, ICheckpointDispatcher
	{
		CheckpointData[] AllCheckpoints;

		void Awake()
		{
			if (AllCheckpoints == null)
				FindCheckpoints();

#if UNITY_EDITOR
			ServiceLocator.CheckForUniqueness<CheckpointService>(gameObject);
#endif
		}

		void FindCheckpoints()
		{
			// garbage generation - only once per game.
			var checkpoints = GameObject.FindObjectsOfType<Checkpoint>().OrderBy(x => x.OrderNumber).ToArray();
			if (checkpoints != null)
			{
				AllCheckpoints = new CheckpointData[checkpoints.Length];
				for (int i = 0; i < AllCheckpoints.Length; i++)
				{
					AllCheckpoints[i].Pos = checkpoints[i].transform.position;
					Destroy(checkpoints[i]);
				}
			}
		}

		public CheckpointData GetNextCheckpoint(ref int checkpointNum)
		{
			if (AllCheckpoints == null)
				FindCheckpoints();

			checkpointNum = (checkpointNum + 1) % AllCheckpoints.Length;
			return AllCheckpoints[checkpointNum];
		}
	}

	public class NullCheckpointService : ICheckpointDispatcher
	{
		public CheckpointData GetNextCheckpoint(ref int PreviousCheckpointNum)
		{
			CheckpointData point = new CheckpointData();

#if UNITY_EDITOR
			Debug.LogError(nameof(NullCheckpointService) + " is in use! Please make sure that A SINGLE '"
				+ nameof(CheckpointService) + "' component exists in the scene!");
#endif
			return point;
		}
	}

	public interface ICheckpointDispatcher
	{
		CheckpointData GetNextCheckpoint(ref int PreviousCheckpointNum);
	}

	[Serializable]
	public struct CheckpointData
	{
		public Vector3 Pos;
	}
}
