using UnityEngine;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Reads player inputs.</summary>
	public class ControllerService : MonoBehaviour
	{
		event ControllerEventHandler ControllerEvent;

#if UNITY_EDITOR
		private void Awake()
		{
			ServiceLocator.CheckForUniqueness<ControllerService>(gameObject);
		}
#endif

		void RaiseControllerEvent(ControllType controllType)
		{
			ControllerEvent?.Invoke(this, new ControllerEventArgs(controllType));
		}

		public void SubscribeForControllerEvent(ControllerEventHandler handler)
		{
			ControllerEvent += handler;
		}

		public void UnsubscribeFromControllerEvent(ControllerEventHandler handler)
		{
			ControllerEvent -= handler;
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
				RaiseControllerEvent(ControllType.Space);
		}
	}
}
