using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BonusController : MonoBehaviour
	{
		public delegate void OnClubLongerDelegate();
		public event OnClubLongerDelegate OnClubLonger;

		public delegate void OnClubShorterDelegate();
		public event OnClubShorterDelegate OnClubShorter;

		public delegate void OnSphereFasterDelegate();
		public event OnSphereFasterDelegate OnSphereFaster;

		public delegate void OnSphereSlowerDelegate();
		public event OnSphereSlowerDelegate OnSphereSlower;

		public float rotationSpeed;
		public float fallSpeed;
		public bool IsCaught;
		private float _timer;
		private float _fallTimer;
		private float _angle;

		public void Start()
		{
			_timer = 1f / rotationSpeed;
		}

		public void Update()
		{
			if (!IsCaught)
			{
				_timer -= Time.deltaTime;
				if (_timer > 0f)
				{
					_angle = 90 + (1f - _timer * rotationSpeed) * 360f;
					transform.localRotation = 
						Quaternion.Euler(new Vector3(_angle - transform.rotation.x, 90f, 0f));
				}
				else
				{
					_timer = 1/rotationSpeed;
					transform.localRotation = 
						Quaternion.Euler(new Vector3(90f - transform.rotation.x, 90f, 0f));
				}
			}
		}

		public void FixedUpdate()
		{
			if (!IsCaught)
				rigidbody.MovePosition(
					new Vector3(
						rigidbody.position.x, 
						rigidbody.position.y,
						rigidbody.position.z - fallSpeed/Time.deltaTime));
		}

		public void Surprise()
		{
			float rand = UnityEngine.Random.Range (0f, 1f);
			if (rand < 0.25f)
			{
				if (OnClubLonger != null)
					OnClubLonger.Invoke();
			}
			else if (rand < 0.5f)
			{
				if (OnClubShorter != null)
					OnClubShorter.Invoke();
			}
			else if (rand < 0.75f)
			{
				if (OnSphereFaster != null)
					OnSphereFaster.Invoke();
			}
			else
				if (OnSphereSlower != null)
					OnSphereSlower.Invoke();

			IsCaught = true;
			gameObject.SetActive(false);
			GameObject.Destroy(this.gameObject);
		}

		public void OnTriggerEnter(Collider collider)
		{
			if (collider.gameObject == Main.Club.gameObject)
				Surprise ();
		}
	}
}

