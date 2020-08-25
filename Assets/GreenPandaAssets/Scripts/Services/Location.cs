using UnityEngine;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Registers the event of the track arriving to the
	/// collision zone of the current GameObject and raises the appropriate eventHandler.</summary>
	public class Location : MonoBehaviour
	{
		[Header("Make sure that the trigger collider", order = 0)]
		[Header("is attached to THIS GameObject", order = 1)]
		[Header("NOT to its children.", order = 2)]

		[Tooltip("What is this location?")]
		public LocationType LocationType;

		event LocationEventHandler LocationEvent;

		void RaiseLocationEvent()
		{
			LocationEvent?.Invoke(this, new LocationEventArgs(LocationType));
		}

		public void SubscribeToLocationEvent(LocationEventHandler handler)
		{
			LocationEvent += handler;
		}

		public void UnsubscribeFromLocationEvent(LocationEventHandler handler)
		{
			LocationEvent -= handler;
		}

		private void OnTriggerEnter(Collider other)
		{
			RaiseLocationEvent();
		}
	}
}
