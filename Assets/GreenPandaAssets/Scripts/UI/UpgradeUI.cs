using TMPro;
using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.Globalization;
using GreenPandaAssets.Scripts.SaveSystem;
using System.IO;

namespace GreenPandaAssets.Scripts.UI
{
	/// <summary>Handles display logic for upgrade buttons in the game.</summary>
	public class UpgradeUI : MonoBehaviour, ISavable
	{
		[Tooltip("The Upgradable script associated with this upgrade.")]
		public AUpgradable Upgradable;

		public TextMeshProUGUI PriceText;
		public TextMeshProUGUI CurrentLevelText;
		public TextMeshProUGUI NextLevelText;

		public Animation Animation;
		[Tooltip("What is the name of the animation that should be played on click?")]
		public string ClickAnimName;

		private void Awake()
		{
			PriceText.text = Upgradable.GetPrice().ToString();
			CurrentLevelText.text = Upgradable.Level.ToString();
			NextLevelText.text = (Upgradable.Level + 1).ToString();
		}

		public void UpdateButtonTexts()
		{
			PriceText.text = Upgradable.GetPrice().ToString("###0", CultureInfo.GetCultureInfo("en-US"));
			CurrentLevelText.text = Upgradable.Level.ToString();
			NextLevelText.text = (Upgradable.Level + 1).ToString();
		}

		public void Upgrade()
		{
			var price = Upgradable.GetPrice();
			if (price > ServiceLocator.GetCoinCounter().Coins || Upgradable.IsMax())
			{
				ServiceLocator.GetVoiceoverService().PlaySound(SoundType.InsufficientFunds, 0);
				return;
			}

			ServiceLocator.GetCoinCounter().Coins -= price;
			Upgradable.Upgrade();
			UpdateButtonTexts();
			ServiceLocator.AnimateProps();

			Animation.Play(ClickAnimName);
		}

		public void Save(ref string file)
		{
		}

		public bool Load(StreamReader reader)
		{
			return true;
		}
		public void LateLoad()
		{
			UpdateButtonTexts();
		}
	}
}
