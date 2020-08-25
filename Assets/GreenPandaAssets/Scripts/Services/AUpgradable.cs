using System.IO;
using UnityEngine;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Base class for handling upgrade logic.</summary>
	public abstract class AUpgradable : MonoBehaviour, ISavable
	{
		[SerializeField] [HideInInspector] protected int _level = 1;

		public int Level => _level;

		[SerializeField] [HideInInspector] protected int _maxLevel = 15;
		[SerializeField] [HideInInspector] protected float _startPrice = 100;
		[SerializeField] [HideInInspector] protected float _priceStepFactor = 1.5f;

		public virtual void Upgrade()
		{
			_level++;
		}

		public bool IsMax()
		{
			return _level >= _maxLevel;
		}

		public float GetPrice()
		{
			return _startPrice * Mathf.Pow(_priceStepFactor, _level - 1);
		}

		public float GetStepFactor()
		{
			return _priceStepFactor;
		}

		public virtual void Save(ref string file)
		{
			file += JsonUtility.ToJson(this) + "\n";
		}


		public virtual bool Load(StreamReader reader)
		{
			JsonUtility.FromJsonOverwrite(reader.ReadLine(), this);

			return true;
		}
		public void LateLoad() { }
	}
}