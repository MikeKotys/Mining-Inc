using UnityEngine;

namespace GreenPandaAssets.Scripts.Factory
{
	/// <summary>Mediator. Provides communication between components of the Factory GameObject.</summary>
	public class FScriptManager : MonoBehaviour
	{
		FactoryView FactoryView;

		private void Awake()
		{
			FactoryView = GetComponent<FactoryView>();
		}


		public FactoryView GetFactoryView()
		{
			return FactoryView;
		}
	}
}
