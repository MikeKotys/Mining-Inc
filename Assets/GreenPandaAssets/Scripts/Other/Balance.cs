#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


namespace GreenPandaAssets.Scripts.Other
{
	// Due to the fact that this is a test task, the Balance class combines functionality of balancing the game parameters
	//	as well as locating components that hold those parameters. In a propper game scenario, the special BalanceServiceLocator
	//	should be implemented to take away the functionality of locating components and reduce coupling.

	/// <summary>Changes the game's primary variable.</summary>
	[ExecuteInEditMode]
	public class Balance : MonoBehaviour
	{
#if UNITY_EDITOR
		public float DumpTruckStartPrice = -1;
		public float DumpTruckPriceMult = -1;
		public float QuaryStartPrice = -1;
		public float QuaryPriceMult = -1;
		public float BulldozerStartPrice = -1;
		public float BulldozerPriceMult = -1;
		public float FactoryStartPrice = -1;
		public float FactoryPriceMult = -1;

		private void OnEnable()
		{
			GetCurrentValues();
		}

		public void GetCurrentValues()
		{
			var dumpTruckUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.DumpTruck.DumpTruckUpgradable>();
			if (dumpTruckUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.DumpTruck.DumpTruckUpgradable) + "' component!");
			else
			{
				DumpTruckStartPrice = dumpTruckUpgrade.GetPrice();
				DumpTruckPriceMult = dumpTruckUpgrade.GetStepFactor();
			}

			var quarryUpgrade = FindObjectOfType<QuaryUpgradable>();
			if (quarryUpgrade == null)
				Debug.LogError("Could not find '" + typeof(QuaryUpgradable) + "' component!");
			else
			{
				QuaryStartPrice = quarryUpgrade.GetPrice();
				QuaryPriceMult = quarryUpgrade.GetStepFactor();
			}

			var bulldozerUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.Bulldozer.BulldozerUpgradable>();
			if (bulldozerUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.Bulldozer.BulldozerUpgradable) + "' component!");
			else
			{
				BulldozerStartPrice = bulldozerUpgrade.GetPrice();
				BulldozerPriceMult = bulldozerUpgrade.GetStepFactor();
			}

			var factoryUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.Factory.FactoryUpgradable>();
			if (factoryUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.Factory.FactoryUpgradable) + "' component!");
			else
			{
				FactoryStartPrice = factoryUpgrade.GetPrice();
				FactoryPriceMult = factoryUpgrade.GetStepFactor();
			}
		}

		public void Apply()
		{
			var dumpTruckUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.DumpTruck.DumpTruckUpgradable>();
			if (dumpTruckUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.DumpTruck.DumpTruckUpgradable) + "' component!");
			else
			{
				var otherSerializedObj = new SerializedObject(dumpTruckUpgrade);
				if (otherSerializedObj != null)
				{
					Undo.RecordObject(dumpTruckUpgrade, "Change Parameters");
					otherSerializedObj.FindProperty("_startPrice").floatValue = DumpTruckStartPrice;
					otherSerializedObj.FindProperty("_priceStepFactor").floatValue = DumpTruckPriceMult;
					otherSerializedObj.ApplyModifiedProperties();
				}
			}

			var quarryUpgrade = FindObjectOfType<QuaryUpgradable>();
			if (quarryUpgrade == null)
				Debug.LogError("Could not find '" + typeof(QuaryUpgradable) + "' component!");
			else
			{
				var otherSerializedObj = new SerializedObject(quarryUpgrade);
				if (otherSerializedObj != null)
				{
					Undo.RecordObject(quarryUpgrade, "Change Parameters");
					otherSerializedObj.FindProperty("_startPrice").floatValue = QuaryStartPrice;
					otherSerializedObj.FindProperty("_priceStepFactor").floatValue = QuaryPriceMult;
					otherSerializedObj.ApplyModifiedProperties();
				}
			}

			var bulldozerUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.Bulldozer.BulldozerUpgradable>();
			if (bulldozerUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.Bulldozer.BulldozerUpgradable) + "' component!");
			else
			{
				var otherSerializedObj = new SerializedObject(bulldozerUpgrade);
				if (otherSerializedObj != null)
				{
					Undo.RecordObject(bulldozerUpgrade, "Change Parameters");
					otherSerializedObj.FindProperty("_startPrice").floatValue = BulldozerStartPrice;
					otherSerializedObj.FindProperty("_priceStepFactor").floatValue = BulldozerPriceMult;
					otherSerializedObj.ApplyModifiedProperties();
				}
			}

			var factoryUpgrade = FindObjectOfType<GreenPandaAssets.Scripts.Factory.FactoryUpgradable>();
			if (factoryUpgrade == null)
				Debug.LogError("Could not find '" + typeof(GreenPandaAssets.Scripts.Factory.FactoryUpgradable) + "' component!");
			else
			{
				var otherSerializedObj = new SerializedObject(factoryUpgrade);
				if (otherSerializedObj != null)
				{
					Undo.RecordObject(factoryUpgrade, "Change Parameters");
					otherSerializedObj.FindProperty("_startPrice").floatValue = FactoryStartPrice;
					otherSerializedObj.FindProperty("_priceStepFactor").floatValue = FactoryPriceMult;
					otherSerializedObj.ApplyModifiedProperties();
				}
			}
		}
#endif
	}
}
