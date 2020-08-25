using GreenPandaAssets.Scripts.Services;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

namespace GreenPandaAssets.Scripts.Factory
{
	/// <summary>Handles all the logic of the factory upgrade.</summary>
    public class FactoryUpgradable : AUpgradable
    {
		[Tooltip("A floating text to show when the truck delivers its load to the factory.")]
        public TextMeshPro RewardText;

		float TotalUnloadReward = 100;

		/// <summary>How much money we will get next time the truck arrives at the factory.</summary>
		[SerializeField][HideInInspector]
		float BaseUnloadReward = 100;

		/// <summary>The reward we get from the truck will be multiplied by this value.</summary>
		[SerializeField][HideInInspector]
		float RewardRockSizeMult = 1;

		Vector3 OriginalRewardTextPos;

		FScriptManager FScriptManager;

		private void Awake()
		{
			OriginalRewardTextPos = RewardText.transform.position;
			FScriptManager = GetComponent<FScriptManager>();
		}


		private void OnEnable()
		{
			ServiceLocator.SubscribeToLocationEvent(ArrivedAtLocation);
			ServiceLocator.SubscribeToRockEnlargedEvent(RockEnlarged);
		}

		private void OnDestroy()
		{
			ServiceLocator.UnsubscribeFromLocationEvent(ArrivedAtLocation);
			ServiceLocator.UnsubscribeFromRockEnlargedEvent(RockEnlarged);
		}

		void ArrivedAtLocation(object sender, LocationEventArgs args)
		{
			if (args.LocationType == LocationType.Factory)
			{
				ServiceLocator.GetVoiceoverService().PlaySound(SoundType.KaChing, 0);
				ServiceLocator.GetCoinCounter().Coins += TotalUnloadReward;
				RewardText.text = "+$" + TotalUnloadReward.ToString();
				RewardText.transform.position = OriginalRewardTextPos;
				RewardText.gameObject.SetActive(true);
				StartCoroutine(FloatRewardText());
			}
		}

		void RecomputeReward()
		{
			TotalUnloadReward = BaseUnloadReward * RewardRockSizeMult;
		}

		void RockEnlarged(object sender, RockEnlargedEventArgs args)
		{
			// Recompute the reward we receive if the rock has been enlarged (Quary was upgraded).
			RewardRockSizeMult = args.Level * .4f;
			RecomputeReward();
		}

		public override void Upgrade()
        {
            base.Upgrade();

			int skinLevel = 0;
            
            if (_level <= 5)
				skinLevel = 0;
            else if (_level <= 10)
				skinLevel = 1;
            else
				skinLevel = 2;

			BaseUnloadReward = _level * 100;
			RecomputeReward();

			FScriptManager.GetFactoryView().SetSkinLevel(skinLevel);
		}

		private IEnumerator FloatRewardText()
		{
			while (RewardText.transform.position.y < 30)
			{
				RewardText.transform.position += Vector3.up * Time.deltaTime * 6;
				yield return null;
			}
			RewardText.gameObject.SetActive(false);
		}

		public override void Save(ref string file)
		{
			base.Save(ref file);
			file += JsonUtility.ToJson(this) + "\n";
		}

		public override bool Load(StreamReader reader)
		{
			if (!base.Load(reader))
				return false;
			RewardText.gameObject.SetActive(false);
			JsonUtility.FromJsonOverwrite(reader.ReadLine(), this);
			RecomputeReward();

			return true;
		}
	}
}