using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Handles all sound in the game.</summary>
	public class VoiceoverService : MonoBehaviour, IVoiceover, ISavable
	{
		public AudioSource[] InsufficientFunds;
		public AudioSource TruckMovingNormal;
		public AudioSource TruckMovingBoosted;
		public AudioSource TruckSlowdown;
		public AudioSource TruckEngineNeutral;
		public AudioSource BulldozerBodyRotating;
		public AudioSource BulldozerHandMoving;
		public AudioSource TruckUnloadSound;
		public AudioSource KaChingSound;

		public AudioSource[] RockFallsSounds;
		
		int LastInsufficientFundsSound = 0;

#if UNITY_EDITOR
		private void Awake()
		{
			ServiceLocator.CheckForUniqueness<VoiceoverService>(gameObject);
		}
#endif

		public void PlaySound(SoundType soundType, float delay)
		{
			switch (soundType)
			{
				case SoundType.InsufficientFunds:
					int randomSound = 0;
					do
					{
						randomSound = Random.Range(0, InsufficientFunds.Length);
					}
					while (randomSound == LastInsufficientFundsSound);
					LastInsufficientFundsSound = randomSound;

					InsufficientFunds[randomSound].PlayOneShot(InsufficientFunds[randomSound].clip);
					break;
				case SoundType.TruckMovingNormal:
					TruckMovingNormal.volume = 1;
					TruckMovingNormal.time = 0;
					TruckMovingNormal.PlayDelayed(delay);
					break;
				case SoundType.TruckMovingBoosted:
					TruckMovingBoosted.volume = 1;
					TruckMovingBoosted.time = 0;
					TruckMovingBoosted.PlayDelayed(delay);
					break;
				case SoundType.TruckEngineNeutral:
					TruckEngineNeutral.volume = 1;
					TruckEngineNeutral.time = 0;
					TruckEngineNeutral.PlayDelayed(delay);
					break;
				case SoundType.TruckSlowdown:
					TruckSlowdown.volume = 1;
					TruckSlowdown.time = 0;
					TruckSlowdown.PlayDelayed(delay);
					break;
				case SoundType.BulldozerBodyRotating:
					BulldozerBodyRotating.volume = 1;
					BulldozerBodyRotating.time = 0;
					BulldozerBodyRotating.PlayDelayed(delay);
					break;
				case SoundType.BulldozerHandMoving:
					BulldozerHandMoving.volume = 1;
					BulldozerHandMoving.time = 0;
					BulldozerHandMoving.PlayDelayed(delay);
					break;
				case SoundType.TruckUnload:
					TruckUnloadSound.Stop();
					TruckUnloadSound.volume = 1;
					TruckUnloadSound.time = 0;
					TruckUnloadSound.PlayDelayed(delay);
					break;
				case SoundType.KaChing:
					KaChingSound.PlayDelayed(delay);
					break;
			}
		}

		public void FadeOutSound(SoundType soundType)
		{
			switch (soundType)
			{
				case SoundType.TruckMovementSounds:
					StartCoroutine(FadeOutTruckMovementSoundsRoutine());
					break;
				case SoundType.TruckEngineNeutral:
					StartCoroutine(FadeOutTruckEngineNeutralSoundsRoutine());
					break;
				case SoundType.BulldozerBodyRotating:
					StartCoroutine(FadeOutBulldozerBodyRotatingSoundRoutine());
					break;
				case SoundType.BulldozerHandMoving:
					StartCoroutine(FadeOutBulldozerHandMovingSoundRoutine());
					break;
			}
		}

		public void PlayRockFallsSound(float rockLevelRelative)
		{
			RockFallsSounds[(int)Mathf.Floor(Mathf.Clamp(rockLevelRelative * (RockFallsSounds.Length), 0, RockFallsSounds.Length))].Play();
		}

		private IEnumerator FadeOutTruckMovementSoundsRoutine()
		{
			while (TruckMovingNormal.volume > .05f && TruckMovingBoosted.volume > .05f)
			{
				TruckMovingNormal.volume -= Time.deltaTime * 3;
				TruckMovingBoosted.volume -= Time.deltaTime * 3;

				yield return null;
			}

			TruckMovingNormal.Stop();
			TruckMovingBoosted.Stop();
		}

		private IEnumerator FadeOutTruckEngineNeutralSoundsRoutine()
		{
			while (TruckEngineNeutral.volume > .05f)
			{
				TruckEngineNeutral.volume -= Time.deltaTime * 3;
				yield return null;
			}

			TruckEngineNeutral.Stop();
		}

		private IEnumerator FadeOutBulldozerBodyRotatingSoundRoutine()
		{
			while (BulldozerBodyRotating.volume > .05f)
			{
				BulldozerBodyRotating.volume -= Time.deltaTime * 3;
				yield return null;
			}

			BulldozerBodyRotating.Stop();
		}

		private IEnumerator FadeOutBulldozerHandMovingSoundRoutine()
		{
			while (BulldozerHandMoving.volume > .05f)
			{
				BulldozerHandMoving.volume -= Time.deltaTime * 3;
				yield return null;
			}

			BulldozerHandMoving.Stop();
		}

		public void Save(ref string file)
		{
			file += TruckMovingNormal.volume.ToString() + "\n";
			file += TruckMovingNormal.time.ToString() + "\n";
			file += TruckMovingNormal.isPlaying.ToString() + "\n";
			file += TruckMovingBoosted.volume.ToString() + "\n";
			file += TruckMovingBoosted.time.ToString() + "\n";
			file += TruckMovingBoosted.isPlaying.ToString() + "\n";
			file += TruckEngineNeutral.volume.ToString() + "\n";
			file += TruckEngineNeutral.time.ToString() + "\n";
			file += TruckEngineNeutral.isPlaying.ToString() + "\n";
			file += BulldozerBodyRotating.volume.ToString() + "\n";
			file += BulldozerBodyRotating.time.ToString() + "\n";
			file += BulldozerBodyRotating.isPlaying.ToString() + "\n";
			file += BulldozerHandMoving.volume.ToString() + "\n";
			file += BulldozerHandMoving.time.ToString() + "\n";
			file += BulldozerHandMoving.isPlaying.ToString() + "\n";
			file += TruckUnloadSound.volume.ToString() + "\n";
			file += TruckUnloadSound.time.ToString() + "\n";
			file += TruckUnloadSound.isPlaying.ToString() + "\n";
		}

		public bool Load(StreamReader reader)
		{
			var allAudioSources = FindObjectsOfType<AudioSource>();
			for (int a = 0; a < allAudioSources.Length; a++)
				allAudioSources[a].Stop();

			float outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckMovingNormal.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckMovingNormal.time = outFloat;

			bool outBool;
			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				TruckMovingNormal.Play();


			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckMovingBoosted.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckMovingBoosted.time = outFloat;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				TruckMovingBoosted.Play();


			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckEngineNeutral.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckEngineNeutral.time = outFloat;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				TruckEngineNeutral.Play();


			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			BulldozerBodyRotating.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			BulldozerBodyRotating.time = outFloat;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				BulldozerBodyRotating.Play();


			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			BulldozerHandMoving.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			BulldozerHandMoving.time = outFloat;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				BulldozerHandMoving.Play();


			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckUnloadSound.volume = outFloat;
			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			TruckUnloadSound.time = outFloat;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			if (outBool)
				TruckUnloadSound.Play();

			return true;
		}
		public void LateLoad() { }
	}

	public enum SoundType
	{
		InsufficientFunds,
		TruckMovingNormal,
		TruckMovingBoosted,
		TruckEngineNeutral,
		TruckSlowdown,
		BulldozerBodyRotating,
		BulldozerHandMoving,
		RockFalling,
		TruckUnload,
		KaChing,

		TruckMovementSounds
	}

	public interface IVoiceover
	{
		void PlaySound(SoundType soundType, float delay);
		void FadeOutSound(SoundType soundType);
		void PlayRockFallsSound(float rockLevelRelative);
	}

	public class NullVoiceoverService : IVoiceover
	{
#if UNITY_EDITOR
		void ShowError()
		{
			Debug.LogError(nameof(NullVoiceoverService) + " is in use! Please make sure that A SINGLE '"
				+ nameof(VoiceoverService) + "' component exists in the scene!");
		}
#endif

		public void PlaySound(SoundType soundType, float delay)
		{
#if UNITY_EDITOR
			ShowError();
#endif
		}
		public void FadeOutSound(SoundType soundType)
		{
#if UNITY_EDITOR
			ShowError();
#endif
		}
		public void PlayRockFallsSound(float rockLevelRelative)
		{
#if UNITY_EDITOR
			ShowError();
#endif
		}
	}
}
