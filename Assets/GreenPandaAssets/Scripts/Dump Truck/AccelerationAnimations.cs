using UnityEngine;

namespace GreenPandaAssets.Scripts.DumpTruck
{
	/// <summary>Animates the X rotation of the <see cref="CarBody"/>
	/// depending on the differences in <see cref="MoveTruck.CurrentSpeed"/>.</summary>
	public class AccelerationAnimations : MonoBehaviour
	{
		[Tooltip("Car body without wheels.")]
		public Transform CarBody;
		[Tooltip("Calibrate the rotation.")]
		public float MaxRotation = 15;
		[Tooltip("Calibrate the rotation sensitivity to changes in the truck's speed.")]
		public float Sensitivity = 1;
		[Tooltip("Calibrate how fast would the car revert back to the normal state.")]
		[Range(0.01f,1)]
		public float AnimationSpeed = .1f;

		float OriginalZRotation;

		//The current component is a parasite component and thus can have direct access to the class it latches onto.
		MoveTruck MoveTruck;

		private void Awake()
		{
			MoveTruck = GetComponent<MoveTruck>();

#if UNITY_EDITOR
			if (CarBody == null)
			{
				Debug.LogError("WARNING! The '" + nameof(CarBody) + "' variable of the '" + nameof(AccelerationAnimations) + "'"
					+ " component has not been assigned! Component will be disabled!", gameObject);
				enabled = false;
			}

			if (MoveTruck == null)
			{
				Debug.LogError("WARNING! The '" + nameof(DumpTruck.MoveTruck) + "' component could not"
					+ " be found by the '" + nameof(AccelerationAnimations) + "'"
					+ " component! The '" + nameof(AccelerationAnimations) + "' component will be disabled!", gameObject);
				enabled = false;
			}
#endif

			OriginalZRotation = CarBody.transform.eulerAngles.z;
		}

		float LastFrameMovementSpeed;
		float LastFrameDelta;

		private void Update()
		{
			float currentMovementSpeed = MoveTruck.GetCurrentSpeed();
			float delta = currentMovementSpeed - LastFrameMovementSpeed;
			delta = Mathf.Lerp(LastFrameDelta, delta, AnimationSpeed);

			CarBody.transform.eulerAngles = new Vector3(CarBody.transform.eulerAngles.x,
				CarBody.transform.eulerAngles.y, Mathf.LerpUnclamped(OriginalZRotation, MaxRotation, delta * Sensitivity));

			LastFrameMovementSpeed = currentMovementSpeed;
			LastFrameDelta = delta;
		}
	}
}
