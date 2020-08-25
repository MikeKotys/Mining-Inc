using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.IO;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Handles the suspension animation that simulates the car receiving a large rock into its trunk.</summary>
	public class ReceiveRockAnim : MonoBehaviour, ISavable
	{
		Animator Animator;

		const int RockLayer = 1;

		void Awake()
		{
			Animator = GetComponent<Animator>();
			Animator.SetLayerWeight(RockLayer, .07f + .1f);	// The larger the rock - the more the layer weight.
			ServiceLocator.SubscribeToTruckLoadedEvent(WasLoaded);
			ServiceLocator.SubscribeToRockEnlargedEvent(RockEnlarged);
			ServiceLocator.SubscribeToQuaryUpgradedEvent(QuaryUpgraded);

#if UNITY_EDITOR
			ServiceLocator.CheckForUniqueness<ReceiveRockAnim>(gameObject);
#endif
		}

		int LoadedAnimHash = Animator.StringToHash("Loaded");

		private void OnDestroy()
		{
			ServiceLocator.UnsubscribeFromTruckLoadedEvent(WasLoaded);
			ServiceLocator.UnsubscribeFromRockEnlargedEvent(RockEnlarged);
			ServiceLocator.UnsubscribeFromQuaryUpgradedEvent(QuaryUpgraded);
		}

		float CurrentRockRelativeLevel = 0;

		void RockEnlarged(object sender, RockEnlargedEventArgs args)
		{
			CurrentRockRelativeLevel = ((float)args.Level ) / ((float)args.MaxLevel);
		}

		void WasLoaded(object sender, TruckLoadedEventArgs args)
		{
			ServiceLocator.GetVoiceoverService().PlayRockFallsSound(CurrentRockRelativeLevel);
			Animator.Play(LoadedAnimHash, RockLayer, 0);
		}

		void QuaryUpgraded(object sender, QuaryUpgradedEventArgs args)
		{
			Animator.SetLayerWeight(RockLayer, args.Level * .07f + .1f);    // The larger the rock - the more the layer weight.
		}

		public void Save(ref string file)
		{
			file += CurrentRockRelativeLevel.ToString() + "\n";
			file += Animator.GetLayerWeight(RockLayer) + "\n";
		}

		public bool Load(StreamReader reader)
		{
			int outInt;

			if (!int.TryParse(reader.ReadLine(), out outInt))
				return false;
			CurrentRockRelativeLevel = outInt;

			float outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			Animator.SetLayerWeight(RockLayer, outFloat);

			return true;
		}
		public void LateLoad() { }
	}
}
