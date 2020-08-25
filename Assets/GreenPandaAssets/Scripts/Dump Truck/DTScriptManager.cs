using UnityEngine;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Mediator, fulfills the MVC pattern. Facilitates the communication between the components of the Dump Truck.</summary>
	public class DTScriptManager : MonoBehaviour
	{
		// Tightly coupled with other components by design.
		DumpTruckView DisplayController;
		MoveTruck MoveTruck;

		private void Awake()
		{
			DisplayController = GetComponent<DumpTruckView>();
			MoveTruck = GetComponent<MoveTruck>();
		}

		public void ToggleModelVisibility(bool isVisible)
		{
			DisplayController.ToggleTruckVisibility(isVisible);
		}

		public void ChageTruckParameters(float level)
		{
			MoveTruck.ChageTruckParameters(level);
		}
	}
}
