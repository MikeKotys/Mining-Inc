using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.IO;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.Bulldozer
{
	/// <summary>Handles bulldozer's animations.</summary>
	public class AnimationManager : MonoBehaviour, ISavable
	{
		Animator Animator;

		event TruckLoadedEventHandler TruckLoadedEvent;

		const string A_LoadAnimSpeedMult = "LoadAnimSpeedMult";
		const string A_IsLoading = "IsLoading";
		
		private void Awake()
		{
			Animator = GetComponent<Animator>();
		}

		int IdleAnimationHash = Animator.StringToHash("IdleAnimation");

		private void OnEnable()
		{
			ServiceLocator.SubscribeToLocationEvent(ArrivedAtLocation);
		}

		private void OnDestroy()
		{
			ServiceLocator.UnsubscribeFromLocationEvent(ArrivedAtLocation);
		}

		void RaiseTruckLoadedEvent()
		{
			TruckLoadedEvent?.Invoke(this, new TruckLoadedEventArgs());
		}

		public void SubscribeToTruckLoadedEvent(TruckLoadedEventHandler handler)
		{
			TruckLoadedEvent += handler;
		}

		public void UnsubscribeFromTruckLoadedEvent(TruckLoadedEventHandler handler)
		{
			TruckLoadedEvent -= handler;
		}


		public void ChangeAnimParameters(float animSpeed)
		{
			Animator.SetFloat(A_LoadAnimSpeedMult, animSpeed);
		}

		/// <summary>Animation event - called by an animation in Unity.</summary>
		void AE_FinishedDigging()
		{
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.BulldozerBodyRotating, 0);
		}

		/// <summary>Animation event - called by an animation in Unity.</summary>
		void AE_BodyFinishedRotating()
		{
			ServiceLocator.GetVoiceoverService().FadeOutSound(SoundType.BulldozerBodyRotating);
			ServiceLocator.GetVoiceoverService().PlaySound(SoundType.BulldozerHandMoving, 0);
		}

		/// <summary>Animation event - called by an animation in Unity.</summary>
		void AE_TruckLoaded()
		{
			ServiceLocator.GetVoiceoverService().FadeOutSound(SoundType.BulldozerHandMoving);
			RaiseTruckLoadedEvent();
			Animator.SetBool(A_IsLoading, false);
		}

		void ArrivedAtLocation(object sender, LocationEventArgs args)
		{
			if (args.LocationType == LocationType.Quary)
				Animator.SetBool(A_IsLoading, true);	// Play animation
		}

		public void Save(ref string file)
		{
			file += Animator.GetBool(A_IsLoading).ToString() + "\n";
			file += Animator.GetFloat(A_LoadAnimSpeedMult).ToString() + "\n";
		}

		public bool Load(StreamReader reader)
		{
			Animator.Play(IdleAnimationHash, 0, 0);
			bool outBool;
			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			Animator.SetBool(A_IsLoading, outBool);

			float outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			Animator.SetFloat(A_LoadAnimSpeedMult, outFloat);

			return true;
		}
		public void LateLoad() { }
	}
}

