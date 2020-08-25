using GreenPandaAssets.Scripts.Services;

namespace GreenPandaAssets.Scripts.Bulldozer
{
	/// <summary>Handles bulldozer's upgrade logic.</summary>
	public class BulldozerUpgradable : AUpgradable
	{
		BScriptManager BScriptManager;

		private void Awake()
		{
			BScriptManager = GetComponent<BScriptManager>();
		}

		public override void Upgrade()
		{
			base.Upgrade();

			BScriptManager.ChangeAnimParameters(_level * 0.2f + 1);
		}
	}
}
