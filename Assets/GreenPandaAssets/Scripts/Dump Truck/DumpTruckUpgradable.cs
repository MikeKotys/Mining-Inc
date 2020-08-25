using GreenPandaAssets.Scripts.Services;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Handles the Dump Truck upgrade logic.</summary>
	public class DumpTruckUpgradable : AUpgradable
	{
		DTScriptManager DTScriptManager;

		private void Awake()
		{
			DTScriptManager = GetComponent<DTScriptManager>();
		}

		public override void Upgrade()
		{
			base.Upgrade();

			DTScriptManager.ChageTruckParameters(_level);
		}
	}
}
