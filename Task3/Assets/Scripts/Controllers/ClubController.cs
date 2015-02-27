using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class ClubController : MonoBehaviour
	{
		public float Density;
		public float Diapason;
		public GameObject Sphere;
		public bool GameStarted;
		public float StartXPosition;
		public float StartYPosition;

		private float _lastMousePosition;

		public void Start()
		{
			Reset ();
			_lastMousePosition = Input.mousePosition.x;
			StartXPosition = transform.position.x;
			StartYPosition = transform.position.z;
		}

		public void Update()
		{
			if (_lastMousePosition != Input.mousePosition.x)
			{
				float newMousePosition = Input.mousePosition.x;
				float delta = newMousePosition - _lastMousePosition;
				_lastMousePosition = newMousePosition;
				float newClubPosition = transform.position.x + delta / Density;
				newClubPosition = newClubPosition > Diapason/2f ? Diapason/2f : 
					(newClubPosition < -Diapason/2f ? -Diapason/2f :
					 	newClubPosition);
				transform.position = new Vector3 (newClubPosition, transform.position.y, transform.position.z);
				if (!GameStarted)
				{
					Vector3 spherePosition = Sphere.transform.position;
					newClubPosition = spherePosition.x + delta / Density;
					newClubPosition = newClubPosition > Diapason/2f ? Diapason/2f :
						(newClubPosition < -Diapason/2f ? -Diapason/2f :
						 	newClubPosition);
					Sphere.transform.position = new Vector3(newClubPosition, spherePosition.y, spherePosition.z);
				}
			}
		}

		public void Reset()
		{
			transform.position = new Vector3 (0, 1.5f, -38);
			transform.localScale = new Vector3 (1, 5, 1);
		}
	}
}

