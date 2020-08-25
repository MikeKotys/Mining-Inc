using GreenPandaAssets.Scripts.Services;
using System.IO;
using UnityEngine;
using UnityEngine.Playables;
using GreenPandaAssets.Scripts.SaveSystem;


namespace GreenPandaAssets.Scripts.Factory
{
	/// <summary>Handles factory's truck unloading animation.</summary>
	public class UnloadTruck : MonoBehaviour, ISavable
	{
		[Tooltip("The 'Rock' GameObject attached to the dummy Dump Truck model, stored inside Factory for animation purposes.")]
		public Transform InternalRockModel;
		PlayableDirector PlayableDirector;

		void Awake()
		{
			PlayableDirector = GetComponent<PlayableDirector>();

#if UNITY_EDITOR
			ServiceLocator.CheckForUniqueness<UnloadTruck>(gameObject);
#endif
		}

		private void OnEnable()
		{
			ServiceLocator.SubscribeToLocationEvent(ArrivedAtLocation);
		}

		private void OnDisable()
		{
			ServiceLocator.UnsubscribeFromLocationEvent(ArrivedAtLocation);
		}

		#region TruckGrabbed
		event TruckGrabbedEventHandler TruckGrabbedEvent;

		void RaiseTruckGrabbedEvent()
		{
			TruckGrabbedEvent?.Invoke(this, new TruckGrabbedEventArgs());
		}

		public void SubscribeToTruckGrabbedEvent(TruckGrabbedEventHandler handler)
		{
			TruckGrabbedEvent += handler;
		}

		public void UnsubscribeFromTruckGrabbedEvent(TruckGrabbedEventHandler handler)
		{
			TruckGrabbedEvent -= handler;
		}

		/// <summary>Animation event, caled by Unity during the unload truck animation.</summary>
		public void AE_TruckGrabbed()
		{
			RaiseTruckGrabbedEvent();
		}
		#endregion


		#region TruckLanded
		event TruckLandedEventHandler TruckLandedEvent;

		void RaiseTruckLandedEvent(Transform target)
		{
			TruckLandedEvent?.Invoke(this, new TruckLandedEventArgs(target));
		}

		public void SubscribeToTruckLandedEvent(TruckLandedEventHandler handler)
		{
			TruckLandedEvent += handler;
		}

		public void UnsubscribeFromTruckLandedEvent(TruckLandedEventHandler handler)
		{
			TruckLandedEvent -= handler;
		}

		/// <summary>Animation event, caled by Unity during the unload truck animation.</summary>
		public void AE_TruckLanded(Transform target)
		{
			RaiseTruckLandedEvent(target);
		}
		#endregion


		#region TruckUnloaded
		event TruckUnLoadedEventHandler TruckUnLoadedEvent;

		void RaiseTruckUnloadedEvent()
		{
			TruckUnLoadedEvent?.Invoke(this, new TruckUnLoadedEventArgs());
		}

		public void SubscribeToTruckUnLoadedEvent(TruckUnLoadedEventHandler handler)
		{
			TruckUnLoadedEvent += handler;
		}

		public void UnsubscribeFromTruckUnLoadedEvent(TruckUnLoadedEventHandler handler)
		{
			TruckUnLoadedEvent -= handler;
		}

		/// <summary>Animation event, caled by Unity during the unload truck animation.</summary>
		public void AE_TruckUnloaded()
		{
			RaiseTruckUnloadedEvent();
		}
		#endregion

		void ArrivedAtLocation(object sender, LocationEventArgs args)
		{
			// Play the unload animation once the player's car has arrived at the Factory.
			if (args.LocationType == LocationType.Factory)
			{
				InternalRockModel.localScale = ServiceLocator.GetRockScale();
				ServiceLocator.GetVoiceoverService().PlaySound(SoundType.TruckUnload, 0);
				PlayableDirector.Play();
			}
		}

		public void Save(ref string file)
		{
			file += PlayableDirector.time.ToString() + "\n";
		}

		public bool Load(StreamReader reader)
		{
			float outFloat;

			if (!float.TryParse(reader.ReadLine(), out outFloat))
				return false;

			if (outFloat > 0)
			{
				InternalRockModel.localScale = ServiceLocator.GetRockScale();
				PlayableDirector.Play();
				PlayableDirector.time = outFloat;
			}
			else
			{
				PlayableDirector.time = 0;
				PlayableDirector.Stop();
			}

			return true;
		}

		public void LateLoad() { }
	}
}
