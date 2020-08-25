using UnityEngine;
using System.Linq;
using System.Collections;
using Random = UnityEngine.Random;

namespace GreenPandaAssets.Scripts.Services
{
	/// <summary>Handles the prop animation logic.</summary>
	public class AnimatePropsService : MonoBehaviour, IAnimatedProps
	{
		/// <summary>All props to animate.</summary>
		Transform[] Trees;
		/// <summary>Provides support for custom designer scale values.</summary>
		Vector3[] OriginalScale;

		void Awake()
		{
			var trees = FindObjectsOfType<GreenPandaAssets.Scripts.Other.EnvProp>().ToList();

			for (int i = trees.Count - 1; i >= 0; i--)
				if (!trees[i].name.Contains("Tree") && !trees[i].name.Contains("Rock")
					&& !trees[i].name.Contains("Grass") && !trees[i].name.Contains("Bush") && !trees[i].name.Contains("Branch"))
					trees.Remove(trees[i]);

			Trees = trees.Select(x => x.transform).ToArray();

			OriginalScale = new Vector3[Trees.Length];

			for (int i = 0; i < Trees.Length; i++)
				OriginalScale[i] = Trees[i].localScale;

#if UNITY_EDITOR
			ServiceLocator.CheckForUniqueness<AnimatePropsService>(gameObject);
#endif
		}

		public void AnimateProps()
		{
			if (!IsAnimatingProps)
			{
				IsAnimatingProps = true;
				StartCoroutine(Animate());
			}
		}

		// Ensure that there is only one animation co-routine.
		bool IsAnimatingProps = false;

		private IEnumerator Animate()
		{
			bool isScaleDownFinished = false;
			while (!isScaleDownFinished)
			{
				isScaleDownFinished = true;
				for (int i = 0; i < Trees.Length; i++)
				{
					var item = Trees[i];

					float speed = Random.Range(3f, 8f);
					var scale = item.localScale;
					scale.x -= speed * Time.deltaTime * OriginalScale[i].x;
					if (scale.x < .3f) scale.x = .3f;
					scale.y -= speed * Time.deltaTime * OriginalScale[i].y;
					if (scale.y < .3f) scale.y = .3f;
					scale.z -= speed * Time.deltaTime * OriginalScale[i].z;
					if (scale.z < .3f) scale.z = .3f;

					item.localScale = scale;

					if (isScaleDownFinished && item.localScale != new Vector3(.3f, .3f, .3f))
						isScaleDownFinished = false;
				}
				yield return null;
			}

			bool isScaleUpFinished = false;
			while (!isScaleUpFinished)
			{
				isScaleUpFinished = true;

				for (int i = 0; i < Trees.Length; i++)
				{
					var item = Trees[i];

					float speed = Random.Range(3f, 8f);
					var scale = item.localScale;
					scale.x += speed * Time.deltaTime * OriginalScale[i].x;
					if (scale.x > OriginalScale[i].x) scale.x = OriginalScale[i].x;
					scale.y += speed * Time.deltaTime * OriginalScale[i].y;
					if (scale.y > OriginalScale[i].y) scale.y = OriginalScale[i].y;
					scale.z += speed * Time.deltaTime * OriginalScale[i].z;
					if (scale.z > OriginalScale[i].z) scale.z = OriginalScale[i].z;

					item.localScale = scale;

					if (isScaleUpFinished && item.localScale != OriginalScale[i])
						isScaleUpFinished = false;
				}
				yield return null;
			}

			IsAnimatingProps = false;
		}
	}

	public interface IAnimatedProps
	{
		void AnimateProps();
	}
}
