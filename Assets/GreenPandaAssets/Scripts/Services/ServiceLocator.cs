using UnityEngine;
using UnityEngine.SceneManagement;
using GreenPandaAssets.Scripts.DumpTruck;
using GreenPandaAssets.Scripts.Bulldozer;
using GreenPandaAssets.Scripts.Factory;
using GreenPandaAssets.Scripts.UI;
using GreenPandaAssets.Scripts.Other;
#if UNITY_EDITOR
using UnityEditor;
#endif

// Service loactor's code sometimes looks like boilerplate code, but keep in mind that the code
//	is designed to be changed in the future for every case where there is boilerplate code.
namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Allows different components in scene to comunicate between themselves.</summary>
	public static class ServiceLocator
	{
		static IAnimatedProps AnimatedProps;
		static ICheckpointDispatcher CheckpointService;
		static NullCheckpointService NullCheckpointService;
		static TopUI CoinCointer;
		static NullCoinCounter NullCoinCounter;
		static IVoiceover VoiceoverService;
		static NullVoiceoverService NullVoiceoverService;

		static Location[] AllLocations;

		static AnimationManager LoadTruck;

		static UnloadTruck UnloadTruck;

		static ControllerService ControllerService;

		static QuaryUpgradable QuaryUpgradable;

		static DumpTruckView DisplayController;

		static bool HideWarnings = false;

		static ServiceLocator()
		{
			SceneManager.sceneLoaded += OnSceneLoaded;
#if UNITY_EDITOR
			EditorApplication.playModeStateChanged += PlayModeStateChanged;
#endif
		}

#if UNITY_EDITOR
		static void PlayModeStateChanged(PlayModeStateChange change)
		{
			if (change == PlayModeStateChange.ExitingPlayMode)
				HideWarnings = true;
		}
#endif

		static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			// Make sure we empty all our static references every time the new scene is loaded.
			AnimatedProps = null;
			CheckpointService = null;
			NullCheckpointService = null;
			CoinCointer = null;
			NullCoinCounter = null;
			VoiceoverService = null;
			NullVoiceoverService = null;
			AllLocations = null;
			LoadTruck = null;
			ControllerService = null;
			UnloadTruck = null;
			QuaryUpgradable = null;
			DisplayController = null;
		}

		public static void AnimateProps()
		{
			if (AnimatedProps == null)
				AnimatedProps = GameObject.FindObjectOfType<AnimatePropsService>();

#if UNITY_EDITOR
			if (!HideWarnings && AnimatedProps == null)
				Debug.LogError("No '" + nameof(AnimatePropsService) + "' component present in the scene!");
#endif

			if (AnimatedProps != null)
				AnimatedProps.AnimateProps();
		}

		public static IVoiceover GetVoiceoverService()
		{
			if (VoiceoverService != null)
				return VoiceoverService;
			else
			{
				var service = GameObject.FindObjectOfType<VoiceoverService>();
				if (service != null)
				{
					VoiceoverService = service;
					return VoiceoverService;
				}
				else
				{
					if (NullVoiceoverService == null)
						NullVoiceoverService = new NullVoiceoverService();
					return NullVoiceoverService;
				}
			}
		}

		public static ICheckpointDispatcher GetCheckpointService()
		{
			if (CheckpointService != null)
				return CheckpointService;
			else
			{
				var service = GameObject.FindObjectOfType<CheckpointService>();
				if (service != null)
				{
					CheckpointService = service;
					return CheckpointService;
				}
				else
				{
					if (NullCheckpointService == null)
						NullCheckpointService = new NullCheckpointService();
					return NullCheckpointService;
				}
			}
		}

		public static ICoinCounter GetCoinCounter()
		{
			if (CoinCointer != null)
				return CoinCointer;
			else
			{
				var service = GameObject.FindObjectOfType<TopUI>();
				if (service != null)
				{
					CoinCointer = service;
					return CoinCointer;
				}
				else
				{
					if (NullCoinCounter == null)
						NullCoinCounter = new NullCoinCounter();
					return NullCoinCounter;
				}
			}
		}

		#region Location event.
		static void FindAllLocations()
		{
			AllLocations = GameObject.FindObjectsOfType<Location>();
#if UNITY_EDITOR
			if (AllLocations == null && !HideWarnings)
				Debug.LogError("No '" + nameof(Location) + "' component present in the scene!");
#endif
		}

		public static void SubscribeToLocationEvent(LocationEventHandler handler)
		{
			if (AllLocations == null)
				FindAllLocations();

			if (AllLocations != null)
			{
				for (int i = 0; i < AllLocations.Length; i++)
					AllLocations[i].SubscribeToLocationEvent(handler);
			}
		}

		public static void UnsubscribeFromLocationEvent(LocationEventHandler handler)
		{
			if (AllLocations == null)
				FindAllLocations();

			if (AllLocations != null)
			{
				for (int i = 0; i < AllLocations.Length; i++)
					AllLocations[i].UnsubscribeFromLocationEvent(handler);
			}
		}
		#endregion

		#region Truck Loaded event.
		public static void SubscribeToTruckLoadedEvent(TruckLoadedEventHandler handler)
		{
			if (LoadTruck == null)
			{
				LoadTruck = GameObject.FindObjectOfType<AnimationManager>();
#if UNITY_EDITOR
				if (LoadTruck == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Bulldozer.AnimationManager) + "' component present in the scene!");
#endif
			}

			if (LoadTruck != null)
				LoadTruck.SubscribeToTruckLoadedEvent(handler);
		}

		public static void UnsubscribeFromTruckLoadedEvent(TruckLoadedEventHandler handler)
		{
			if (LoadTruck == null)
			{
				LoadTruck = GameObject.FindObjectOfType<AnimationManager>();
#if UNITY_EDITOR
				if (LoadTruck == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Bulldozer.AnimationManager) + "' component present in the scene!");
#endif
			}

			if (LoadTruck != null)
				LoadTruck.UnsubscribeFromTruckLoadedEvent(handler);
		}
		#endregion


		#region Controller.
		public static void SubscribeToControllerEvent(ControllerEventHandler handler)
		{
			if (ControllerService == null)
			{
				ControllerService = GameObject.FindObjectOfType<ControllerService>();
#if UNITY_EDITOR
				if (ControllerService == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Services.ControllerService) + "' component present in the scene!");
#endif
			}

			if (ControllerService != null)
				ControllerService.SubscribeForControllerEvent(handler);
		}

		public static void UnsubscribeFromControllerEvent(ControllerEventHandler handler)
		{
			if (ControllerService == null)
			{
				ControllerService = GameObject.FindObjectOfType<ControllerService>();
#if UNITY_EDITOR
				if (ControllerService == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Services.ControllerService) + "' component present in the scene!");
#endif
			}

			if (ControllerService != null)
				ControllerService.UnsubscribeFromControllerEvent(handler);
		}
		#endregion

		#region Unload Truck Animation events.
		static void FindUnloadTruckComponent()
		{
			UnloadTruck = GameObject.FindObjectOfType<UnloadTruck>();
#if UNITY_EDITOR
			if (UnloadTruck == null && !HideWarnings)
				Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Factory.UnloadTruck) + "' component present in the scene!");
#endif
		}

		public static void SubscribeToTruckGrabbedEvent(TruckGrabbedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.SubscribeToTruckGrabbedEvent(handler);
		}

		public static void UnsubscribeFromTruckGrabbedEvent(TruckGrabbedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.UnsubscribeFromTruckGrabbedEvent(handler);
		}

		public static void SubscribeToTruckLandedEvent(TruckLandedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.SubscribeToTruckLandedEvent(handler);
		}

		public static void UnsubscribeFromTruckLandedEvent(TruckLandedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.UnsubscribeFromTruckLandedEvent(handler);
		}

		public static void SubscribeToTruckUnLoadedEvent(TruckUnLoadedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.SubscribeToTruckUnLoadedEvent(handler);
		}

		public static void UnsubscribeFromTruckUnLoadedEvent(TruckUnLoadedEventHandler handler)
		{
			if (UnloadTruck == null)
				FindUnloadTruckComponent();

			if (UnloadTruck != null)
				UnloadTruck.UnsubscribeFromTruckUnLoadedEvent(handler);
		}

#if UNITY_EDITOR
		/// <summary>Check if a generic component T is unique in the scene. If it is not - show error.</summary>
		/// <param name="caller">A caller of this method - is needed to link the error to the GameObject, so that it can be easily
		///		tracked in the scene by clicking on it.</param>
		/// <typeparam name="T">A generic component type to check uniqueness of. Inherits from MonoBehaviour.</typeparam>
		public static void CheckForUniqueness<T>(GameObject caller) where T : MonoBehaviour
		{
			var allClassesInScene = GameObject.FindObjectsOfType<T>();

			if (!HideWarnings && allClassesInScene != null && allClassesInScene.Length > 1)
			{
				Debug.LogError("Component '" + typeof(T) + "' is not unique in the scene! Please make sure there is only ONE '"
					+ typeof(T) + "' component in the scene!", caller);
			}
		}
#endif
		#endregion

		#region Quary Upgraded event.
		public static void SubscribeToQuaryUpgradedEvent(QuaryUpgradedEventHandler handler)
		{
			if (QuaryUpgradable == null)
			{
				QuaryUpgradable = GameObject.FindObjectOfType<QuaryUpgradable>();
#if UNITY_EDITOR
				if (QuaryUpgradable == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Other.QuaryUpgradable) + "' component present in the scene!");
#endif
			}

			if (QuaryUpgradable != null)
				QuaryUpgradable.SubscribeToQuaryUpgradedEvent(handler);
		}

		public static void UnsubscribeFromQuaryUpgradedEvent(QuaryUpgradedEventHandler handler)
		{
			if (QuaryUpgradable == null)
			{
				QuaryUpgradable = GameObject.FindObjectOfType<QuaryUpgradable>();
#if UNITY_EDITOR
				if (QuaryUpgradable == null && !HideWarnings)
					Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.Other.QuaryUpgradable) + "' component present in the scene!");
#endif
			}

			if (QuaryUpgradable != null)
				QuaryUpgradable.UnsubscribeFromQuaryUpgradedEvent(handler);
		}
		#endregion

		#region Rock Enlarged event.
		static void FindDisplayController()
		{
			DisplayController = GameObject.FindObjectOfType<DumpTruckView>();
#if UNITY_EDITOR
			if (DisplayController == null && !HideWarnings)
				Debug.LogError("No '" + nameof(GreenPandaAssets.Scripts.DumpTruck.DumpTruckView) + "' component present in the scene!");
#endif
		}

		public static void SubscribeToRockEnlargedEvent(RockeEnlargedEventHandler handler)
		{
			if (DisplayController == null)
				FindDisplayController();

			if (DisplayController != null)
				DisplayController.SubscribeRockEnlargedEvent(handler);
		}

		public static void UnsubscribeFromRockEnlargedEvent(RockeEnlargedEventHandler handler)
		{
			if (DisplayController == null)
				FindDisplayController();

			if (DisplayController != null)
				DisplayController.UnsubscribeFromRockEnlargedEvent(handler);
		}
		#endregion

		public static Vector3 GetRockScale()
		{
			if (DisplayController == null)
				FindDisplayController();

			if (DisplayController != null)
				return DisplayController.GetRockScale();
			else
				return Vector3.one;
		}
	}

	public class LocationEventArgs
	{
		public LocationType LocationType;

		public LocationEventArgs(LocationType locationType)
		{
			LocationType = locationType;
		}
	}

	public delegate void LocationEventHandler(object sender, LocationEventArgs e);

	public enum LocationType
	{
		Quary,
		Factory
	}

	public class TruckLoadedEventArgs
	{
		public TruckLoadedEventArgs()
		{
		}
	}

	public delegate void TruckLoadedEventHandler(object sender, TruckLoadedEventArgs e);


	public enum ControllType
	{
		Space,
	}
	public class ControllerEventArgs
	{
		public ControllType ControllType;

		public ControllerEventArgs(ControllType controllType)
		{
			ControllType = controllType;
		}
	}

	public delegate void ControllerEventHandler(object sender, ControllerEventArgs e);

	public class TruckGrabbedEventArgs
	{
		public TruckGrabbedEventArgs()
		{
		}
	}

	public delegate void TruckGrabbedEventHandler(object sender, TruckGrabbedEventArgs e);

	public class TruckLandedEventArgs
	{
		public Transform Target;
		public TruckLandedEventArgs(Transform target)
		{
			Target = target;
		}
	}

	public delegate void TruckLandedEventHandler(object sender, TruckLandedEventArgs e);

	public class TruckUnLoadedEventArgs
	{
		public TruckUnLoadedEventArgs()
		{
		}
	}

	public delegate void TruckUnLoadedEventHandler(object sender, TruckUnLoadedEventArgs e);

	public class QuaryUpgradedEventArgs
	{
		public int Level;
		public int MaxLevel;
		public QuaryUpgradedEventArgs(int level, int maxLevel)
		{
			Level = level;
			MaxLevel = maxLevel;
		}
	}

	public delegate void QuaryUpgradedEventHandler(object sender, QuaryUpgradedEventArgs e);

	public class RockEnlargedEventArgs
	{
		public int Level;
		public int MaxLevel;
		public RockEnlargedEventArgs(int level, int maxLevel)
		{
			Level = level;
			MaxLevel = maxLevel;
		}
	}

	public delegate void RockeEnlargedEventHandler(object sender, RockEnlargedEventArgs e);
}
