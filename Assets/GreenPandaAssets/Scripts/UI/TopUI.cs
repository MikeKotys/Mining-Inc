using GreenPandaAssets.Scripts.Services;
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using GreenPandaAssets.Scripts.SaveSystem;
using System.IO;

namespace GreenPandaAssets.Scripts.UI
{
	// MVC suggests this should be split into at least two classes (money counting logic and money displaying logic)
	//	but I believe this is excessive because the class is already quite small.

	/// <summary>Handles the player's bank logic and the display of all the coins the player has as well as coin income per minute.</summary>
    public class TopUI : MonoBehaviour, ICoinCounter, ISavable
	{
        public TextMeshProUGUI CoinsText;
        public TextMeshProUGUI CoinsPreMinText;

		const float StartingCoins = 10000;

		/// <summary>When was the last moment we received money?</summary>
		[SerializeField][HideInInspector]
		float LastIncomeTime = 0;

		/// <summary>How much money we received last time?</summary>
		[SerializeField][HideInInspector]
		float LastCoinIncome = 0;

		/// <summary>How much time passed between two last incomes?</summary>
		[SerializeField][HideInInspector]
		float LastIncomeInterval = 0;

		void RecomputeCoinTexts(float coinsEarnedThisTime)
		{
			CoinsText.text = "x" + _coins.ToString("###0", CultureInfo.GetCultureInfo("en-US"));
			if (coinsEarnedThisTime > 0)
			{
				CoinsPreMinText.text = (coinsEarnedThisTime / Mathf.Max(0.0001f, LastIncomeInterval)
						* 60).ToString("###0", CultureInfo.GetCultureInfo("en-US")) + "/min";
			}
		}

		[SerializeField][HideInInspector]
		private float _coins = StartingCoins;

		public float Coins
        {
            get { return _coins; }
            set
			{
				float difference = value - _coins;
				_coins = value;
				if (difference > 0)
				{
					LastCoinIncome = difference;
					LastIncomeInterval = Time.timeSinceLevelLoad - LastIncomeTime;
				}
				RecomputeCoinTexts(difference);
				if (difference > 0)
					LastIncomeTime = Time.timeSinceLevelLoad;
			}
		}


        private void Awake()
        {
			RecomputeCoinTexts(0);

#if UNITY_EDITOR
			ServiceLocator.CheckForUniqueness<TopUI>(gameObject);
#endif
		}

		public void Save(ref string file)
		{
			file += JsonUtility.ToJson(this) + "\n";
		}

		public bool Load(StreamReader reader)
		{
			JsonUtility.FromJsonOverwrite(reader.ReadLine(), this);
			RecomputeCoinTexts(LastCoinIncome);

			return true;
		}
		public void LateLoad() { }
	}

	public class NullCoinCounter : ICoinCounter
	{
		public float Coins
		{
			get
			{
#if UNITY_EDITOR
				Debug.LogError(nameof(NullCoinCounter) + " is in use! Please make sure that A SINGLE '"
					+ nameof(ICoinCounter) + "' component exists in the scene!");
#endif
				return 0;
			}
			set
			{
#if UNITY_EDITOR
				Debug.LogError(nameof(NullCoinCounter) + " is in use! Please make sure that A SINGLE '"
					+ nameof(ICoinCounter) + "' component exists in the scene!");
#endif
			}
		}
	}

	public interface ICoinCounter
	{
		float Coins { get; set; }
	}
}