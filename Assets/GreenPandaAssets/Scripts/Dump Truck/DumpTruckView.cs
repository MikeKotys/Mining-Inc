using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.IO;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Handles the Dump Truck display logic.</summary>
	public class DumpTruckView : MonoBehaviour, ISavable
	{
		[Tooltip("The entire truck visual representation.")]
		public GameObject TruckDisplayModel;
		[Tooltip("The model of the Rock inside the truck's dump box.")]
		public Transform RockDisplayModel;

		[Tooltip("The minimal size of the Rock.")]
		public Vector3 MinRockScale;
		[Tooltip("The maximum size of the Rock after all Quary upgrades.")]
		public Vector3 MaxRockScale;


		private void Awake()
		{
			RockDisplayModel.gameObject.SetActive(false);
			RockDisplayModel.localScale = MinRockScale;
			ServiceLocator.SubscribeToTruckGrabbedEvent(TruckGrabbed);
			ServiceLocator.SubscribeToTruckLoadedEvent(TruckLoaded);
			ServiceLocator.SubscribeToTruckUnLoadedEvent(TruckUnloaded);
			ServiceLocator.SubscribeToQuaryUpgradedEvent(QuaryUpgraded);
		}

		private void OnDestroy()
		{
			ServiceLocator.UnsubscribeFromTruckGrabbedEvent(TruckGrabbed);
			ServiceLocator.UnsubscribeFromTruckLoadedEvent(TruckLoaded);
			ServiceLocator.UnsubscribeFromTruckUnLoadedEvent(TruckUnloaded);
			ServiceLocator.UnsubscribeFromQuaryUpgradedEvent(QuaryUpgraded);
		}

		event RockeEnlargedEventHandler RockEnlargedEvent;

		public Vector3 GetRockScale()
		{
			return RockDisplayModel.localScale;
		}

		void RaiseRockEnlargedEvent(int currentVisualRockLevel, int maxLevel)
		{
			RockEnlargedEvent?.Invoke(this, new RockEnlargedEventArgs(currentVisualRockLevel, maxLevel));
		}

		public void SubscribeRockEnlargedEvent(RockeEnlargedEventHandler handler)
		{
			RockEnlargedEvent += handler;
		}

		public void UnsubscribeFromRockEnlargedEvent(RockeEnlargedEventHandler handler)
		{
			RockEnlargedEvent -= handler;
		}

		public void ToggleTruckVisibility(bool isVisible)
		{
			if (isVisible)
				TruckDisplayModel.SetActive(true);
			else
				TruckDisplayModel.SetActive(false);
		}

		void TruckLoaded(object sender, TruckLoadedEventArgs args)
		{
			RockDisplayModel.gameObject.SetActive(true);
		}

		/// <summary>Delays rock enlargement untill next time its invisible.</summary>
		bool Request_ChangeRockScaleOnNextUnload = false;
		QuaryUpgradedEventArgs UpgradedRockArguments;

		void ChangeRockScale()
		{
			RockDisplayModel.localScale = Vector3.Lerp(MinRockScale, MaxRockScale,
				((float)UpgradedRockArguments.Level) / ((float)UpgradedRockArguments.MaxLevel));
			RaiseRockEnlargedEvent(UpgradedRockArguments.Level, UpgradedRockArguments.MaxLevel);
			Request_ChangeRockScaleOnNextUnload = false;
		}

		void QuaryUpgraded(object sender, QuaryUpgradedEventArgs args)
		{
			UpgradedRockArguments = args;
			if (RockDisplayModel.gameObject.activeSelf)
				Request_ChangeRockScaleOnNextUnload = true;
			else
				ChangeRockScale();
		}

		void TruckGrabbed(object sender, TruckGrabbedEventArgs args)
		{
			ToggleTruckVisibility(false);
			RockDisplayModel.gameObject.SetActive(false);
		}

		void TruckUnloaded(object sender, TruckUnLoadedEventArgs args)
		{
			RockDisplayModel.gameObject.SetActive(false);
			// Only enlarge the rock when its invisible to the player.
			if (Request_ChangeRockScaleOnNextUnload)
				ChangeRockScale();
		}

		public void Save(ref string file)
		{
			file += RockDisplayModel.localScale.x.ToString() + "\n";
			file += RockDisplayModel.localScale.y.ToString() + "\n";
			file += RockDisplayModel.localScale.z.ToString() + "\n";
			file += RockDisplayModel.gameObject.activeSelf.ToString() + "\n";
			file += TruckDisplayModel.activeSelf.ToString() + "\n";
		}

		public bool Load(StreamReader reader)
		{
			float outFloat;

			Vector3 scale = new Vector3();

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			scale.x = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			scale.y = outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;
			scale.z = outFloat;

			RockDisplayModel.localScale = scale;

			bool outBool;

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			RockDisplayModel.gameObject.SetActive(outBool);

			if (!bool.TryParse(reader.ReadLine(), out outBool))
				return false;
			TruckDisplayModel.gameObject.SetActive(outBool);

			return true;
		}
		public void LateLoad() { }
	}
}
