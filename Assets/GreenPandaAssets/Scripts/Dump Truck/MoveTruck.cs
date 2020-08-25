using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.IO;
using GreenPandaAssets.Scripts.SaveSystem;
using System;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Handles all the logic of the Dump Truck moving.</summary>
	public class MoveTruck : MonoBehaviour, ISavable
	{
		/// <summary>How fast can the truck move?</summary>
		[NonSerialized][HideInInspector]
		public float MaxMovementSpeed = 8;
		/// <summary>How fast can the truck accelerate?</summary>
		[NonSerialized][HideInInspector]
		public float AccelerationSpeed = 5;

		/// <summary>How fast can the truck break?</summary>
		[NonSerialized][HideInInspector]
		public float DecelerationMultiplier = 5;

		/// <summary>How fast can the truck rotate?</summary>
		[NonSerialized][HideInInspector]
		public float MaxRotationSpeed = 5;

		/// <summary>How close to the checkpoint the truck should be to trigger the checkpoint change?</summary>
		[NonSerialized][HideInInspector]
		public float CheckpointProximityThreshold = .8f;

		/// <summary>Current checkpoint number.</summary>
		[SerializeField][HideInInspector]
		int CheckpointNumber = -1;
		/// <summary>Current checkpoint coordinates.</summary>
		[SerializeField][HideInInspector]
		CheckpointData CurrentCheckpointPos;

		[SerializeField][HideInInspector]
		float CurrentMovementSpeed = 0;

		[SerializeField][HideInInspector]
		float CurrentDecelerationSpeed = 0;

		DTScriptManager DTScriptManager;

		private void Awake()
		{
			CurrentCheckpointPos = ServiceLocator.GetCheckpointService().GetNextCheckpoint(ref CheckpointNumber);

			DTScriptManager = GetComponent<DTScriptManager>();
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckMovingNormal, 0);
		}

		private void OnEnable()
		{
			ServiceLocator.SubscribeToTruckGrabbedEvent(TruckGrabbed);
			ServiceLocator.SubscribeToTruckLandedEvent(TruckLanded);
			ServiceLocator.SubscribeToTruckUnLoadedEvent(TruckUnloaded);
			ServiceLocator.SubscribeToLocationEvent(ArrivedAtLocation);
			ServiceLocator.SubscribeToTruckLoadedEvent(WasLoaded);
		}

		private void OnDisable()
		{
			ServiceLocator.UnsubscribeFromTruckGrabbedEvent(TruckGrabbed);
			ServiceLocator.UnsubscribeFromTruckLandedEvent(TruckLanded);
			ServiceLocator.UnsubscribeFromTruckUnLoadedEvent(TruckUnloaded);
			ServiceLocator.UnsubscribeFromLocationEvent(ArrivedAtLocation);
			ServiceLocator.UnsubscribeFromTruckLoadedEvent(WasLoaded);
		}

		public float GetCurrentSpeed()
		{
			return CurrentMovementSpeed;
		}

		void ArrivedAtLocation(object sender, LocationEventArgs args)
		{
			CurrentDecelerationSpeed = AccelerationSpeed * DecelerationMultiplier;

			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckSlowdown, 0);
			ServiceLocator.GetVoiceoverService().FadeOutSound(SoundType.TruckMovementSounds);
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckEngineNeutral, .8f);
		}

		void WasLoaded(object sender, TruckLoadedEventArgs args)
		{
			CurrentDecelerationSpeed = 0;
			ServiceLocator.GetVoiceoverService().FadeOutSound(SoundType.TruckEngineNeutral);
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckMovingNormal, .2f);
		}

		public void ChageTruckParameters(float level)
		{
			MaxMovementSpeed = level + 8;
			MaxRotationSpeed = level * .3f + 5;
			AccelerationSpeed = level * .3f + 5;
			DecelerationMultiplier = 5 + (AccelerationSpeed - 5);
		}

		void TruckGrabbed(object sender, TruckGrabbedEventArgs args)
		{
			// Make sure that if we load the game during the Truck Unloading animation, no Location Event can be triggered.
			transform.position = new Vector3(1000, 1000, 1000);
		}

		public void TruckLanded(object sender, TruckLandedEventArgs args)
		{
			transform.position = args.Target.position;
			transform.rotation = args.Target.rotation;
			CheckpointNumber = -1;
			CurrentCheckpointPos = ServiceLocator.GetCheckpointService().GetNextCheckpoint(ref CheckpointNumber);
			DTScriptManager.ToggleModelVisibility(true);
		}


		public void TruckUnloaded(object sender, TruckUnLoadedEventArgs args)
		{
			CurrentDecelerationSpeed = 0;
			ServiceLocator.GetVoiceoverService().FadeOutSound(SoundType.TruckEngineNeutral);
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckMovingBoosted, 0);
			CurrentMovementSpeed = MaxMovementSpeed;
		}

		private void Update()
		{
			// Check if we need to change checkpoints
			if ((transform.position - CurrentCheckpointPos.Pos).sqrMagnitude < CheckpointProximityThreshold)
				CurrentCheckpointPos = ServiceLocator.GetCheckpointService().GetNextCheckpoint(ref CheckpointNumber);

			Vector3 difference = transform.position - CurrentCheckpointPos.Pos;

			// Rotate towards the checkpoint
			Quaternion newQ = Quaternion.LookRotation(CurrentCheckpointPos.Pos - transform.position, Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, newQ, MaxRotationSpeed * Time.deltaTime);

			CurrentMovementSpeed = Mathf.Max(0, Mathf.Min(CurrentMovementSpeed 
				+ (AccelerationSpeed - CurrentDecelerationSpeed) * Time.deltaTime, MaxMovementSpeed));

			// Move forward.
			transform.position += transform.forward * Time.deltaTime * CurrentMovementSpeed;
		}

		public void Save(ref string file)
		{
			file += transform.position.x.ToString() + "\n";
			file += transform.position.y.ToString() + "\n";
			file += transform.position.z.ToString() + "\n";

			file += transform.rotation.x.ToString() + "\n";
			file += transform.rotation.y.ToString() + "\n";
			file += transform.rotation.z.ToString() + "\n";
			file += transform.rotation.w.ToString() + "\n";

			file += JsonUtility.ToJson(this) + "\n";
		}

		public bool Load(StreamReader reader)
		{
			float outFloat;

			Vector3 pos = new Vector3();

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			pos.x = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			pos.y = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			pos.z = outFloat;

			transform.position = pos;

			Quaternion rot = new Quaternion();

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			rot.x = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			rot.y = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			rot.z = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			rot.w = outFloat;

			transform.rotation = rot;

			JsonUtility.FromJsonOverwrite(reader.ReadLine(), this);

			return true;
		}
		public void LateLoad() { }
	}
}
