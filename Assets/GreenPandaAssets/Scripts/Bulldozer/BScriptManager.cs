using UnityEngine;

namespace GreenPandaAssets.Scripts.Bulldozer
{
	/// <summary>Mediator, fulfills the MVC pattern.</summary>
	public class BScriptManager : MonoBehaviour
	{
		AnimationManager AnimationManager;

		private void Awake()
		{
			AnimationManager = GetComponent<AnimationManager>();
		}

		public void ChangeAnimParameters(float animSpeed)
		{
			AnimationManager.ChangeAnimParameters(animSpeed);
		}
	}
}
