using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CubeController : MonoBehaviour
	{
		public delegate void OnDestroyDelegate();
		public event OnDestroyDelegate OnDestroy;

		private int _liveCount;
		private bool _immortal;
		private bool _mustKilled;
		private float _timer = 5f;

		public void Update()
		{
			if (_mustKilled) 
			{
				_timer -= Time.deltaTime;
				if (_timer < 0f)
				{
					_mustKilled = false;
					gameObject.SetActive(false);
				}
			}
		}

		public void Init(int x, int y, float startx, float starty, float cubeSize, float cubeStratch)
		{
			transform.position = 
				new Vector3 (
					startx + cubeSize * cubeStratch * (x + 0.5f), 
					cubeSize / 2.0f, 
					starty + cubeSize * (y + 0.5f));
			transform.localScale = new Vector3 (cubeSize*cubeStratch, cubeSize, cubeSize);
			switch(gameObject.name)
			{
			case "SilverCube" : 
				_immortal = true;
				break;
			case "OrangeCube" : 
				_liveCount = 2;
				break;
			case "GreenCube" : 
				_liveCount = 1;
				break;
			case "RedCube" : 
				_liveCount = 4;
				break;
			default :
					break;
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			if (Main.Sphere != null) 
			{
				if (collision.gameObject == Main.Sphere.gameObject) 
				{
					if (!_immortal)
					{
						_liveCount--;
						if (_liveCount == 0)
						{
							if (OnDestroy != null)
								OnDestroy.Invoke();
							gameObject.SetActive(false);
						}
					}
				}
			}
		}

		public void Kill()
		{
			GetComponent<Collider> ().isTrigger = true;
			GetComponent<Rigidbody> ().isKinematic = false;
			_mustKilled = true;
		}

		public void Forward(float cubeSize)
		{
			rigidbody.position = 
				new Vector3 (rigidbody.position.x, rigidbody.position.y, rigidbody.position.z - cubeSize);
		}
	}
}

