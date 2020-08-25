using System.Collections;
using UnityEngine;
using GreenPandaAssets.Scripts.Services;
using System.IO;
using GreenPandaAssets.Scripts.SaveSystem;

namespace GreenPandaAssets.Scripts.Factory
{
	/// <summary>Handles the skin changing logic and animation for factory upgrades.</summary>
	public class FactoryView : MonoBehaviour, ISavable
	{
		[Tooltip("All skins for the factory.")]
		public GameObject[] Skins;

		/// <summary>Total time in seconds that the upgrade animation takes.</summary>
		const float AnimDuration = .5f;
		private Animator Animator;
		/// <summary>What is the current skin used by the factory.</summary>
		private int CurrentSkinLevel = 0;

		void Awake()
		{
			Animator = GetComponent<Animator>();
			UpdateSkin(CurrentSkinLevel);
		}

		private void OnEnable()
		{
			ServiceLocator.SubscribeToControllerEvent(ChangeFactoryLevel);
		}

		private void OnDisable()
		{
			ServiceLocator.UnsubscribeFromControllerEvent(ChangeFactoryLevel);
		}

		public void SetSkinLevel(int skinLevel)
		{
			Animator.SetBool("isUpgrading", true);

			CurrentSkinLevel = skinLevel;

			StartCoroutine(WaitForSkinUpdate());
		}

		private IEnumerator WaitForSkinUpdate()
		{
			yield return new WaitForSeconds(AnimDuration / 2);
			Animator.SetBool("isUpgrading", false);
			UpdateSkin(CurrentSkinLevel);
		}

		void ChangeFactoryLevel(object sender, ControllerEventArgs args)
		{
			if (args.ControllType == ControllType.Space)
			{
				var skin = CurrentSkinLevel + 1;
				SetSkinLevel(skin);
			}
		}

		private void UpdateSkin(int skinLevel)
		{
			for (int i = 0; i < Skins.Length; i++)
				Skins[i].SetActive(skinLevel == i);
		}

		public void Save(ref string file)
		{
			file += CurrentSkinLevel.ToString() + "\n";
		}

		public bool Load(StreamReader reader)
		{
			int outInt;

			if (!int.TryParse(reader.ReadLine(), out outInt))
				return false;

			CurrentSkinLevel = outInt;
			UpdateSkin(CurrentSkinLevel);

			return true;
		}
		public void LateLoad() { }
	}
}
