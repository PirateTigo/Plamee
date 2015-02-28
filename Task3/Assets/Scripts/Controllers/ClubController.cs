using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ClubController : MonoBehaviour
	{
		public float Density;
		public float Diapason;
		public SphereController Sphere;
		public bool GameStarted;
		public float StartXPosition { get; private set; }
		public float StartYPosition { get; private set; }

		private float _lastMousePosition;
		private Vector3 _startScale;

		public void Start()
		{
			_lastMousePosition = Input.mousePosition.x;
			StartXPosition = transform.position.x;
			StartYPosition = transform.position.z;
			_startScale = transform.localScale;
		}

		public void Update()
		{

		}

		public void FixedUpdate()
		{
			if (_lastMousePosition != Input.mousePosition.x)
			{
				if (!Sphere.NeedResetPosition)
				{
					float newMousePosition = Input.mousePosition.x;
					float delta = newMousePosition - _lastMousePosition;
					_lastMousePosition = newMousePosition;
					float newClubPosition = transform.position.x + delta / Density;
					newClubPosition = newClubPosition > Diapason/2f ? Diapason/2f : 
						(newClubPosition < -Diapason/2f ? -Diapason/2f :
						 newClubPosition);
					rigidbody.MovePosition(
						new Vector3 (newClubPosition, transform.position.y, transform.position.z));
				}
			}
		}

		public void ResetAfterDeath()
		{
			GameStarted = false;
			transform.position = new Vector3 (StartXPosition, transform.position.y, StartYPosition);
		}

		public void ResetAfterGameOver()
		{
			ResetAfterDeath ();
			transform.localScale = _startScale;
		}

		public void OnClubLonger()
		{
			Vector3 currentScale = transform.localScale;
			if (currentScale.y * 1.1f > _startScale.y * 1.5f) return;
			transform.localScale = new Vector3 (currentScale.x, currentScale.y * 1.1f, currentScale.z);

		}

		public void OnClubShorter()
		{
			Vector3 currentScale = transform.localScale;
			if (currentScale.y * 0.95f < _startScale.y * 0.5f) return;
			transform.localScale = new Vector3 (currentScale.x, currentScale.y * 0.95f, currentScale.z);
		}
	}
}

