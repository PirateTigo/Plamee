using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CubeController : MonoBehaviour
	{
		public delegate void OnScoreDelegate();
		public event OnScoreDelegate OnScore;

		public delegate void OnKillDelegate(int x, int y, GameObject gameObject);
		public event OnKillDelegate OnKill;

		private int _liveCount;
		private bool _immortal;
		private bool _mustKilled;
		private float _timer = 1f;
		private int _x;
		private int _y;
		private float _cubeSize;
		private float _cubeStratch;

		public void Update()
		{
			if (_mustKilled) 
			{
				_timer -= Time.deltaTime;
				if (_timer < 0f)
				{
					_mustKilled = false;
					gameObject.SetActive(false);
					GameObject.Destroy(gameObject, 2f);
				}
			}
		}

		public void Init(int x, int y, float startx, float starty, float cubeSize, float cubeStratch)
		{
			_x = x;
			_y = y;
			_cubeSize = cubeSize;
			_cubeStratch = cubeStratch;
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
							if (UnityEngine.Random.Range(0f, 1f) < 0.1f)
								Main.SurpriseFactory.GetBonus(
									new Vector3(rigidbody.position.x, rigidbody.position.y, rigidbody.position.z));
							if (OnScore != null)
								OnScore.Invoke();
							if (OnKill != null)
								OnKill.Invoke(_x, _y, gameObject);
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
			rigidbody.MovePosition(
				new Vector3 (rigidbody.position.x, rigidbody.position.y, rigidbody.position.z - cubeSize));
			Rigidbody sphereRigidbody = Main.Sphere.rigidbody;
			if (Mathf.Abs(sphereRigidbody.position.x - rigidbody.position.x) < _cubeStratch && 
			    Mathf.Abs(sphereRigidbody.position.z - rigidbody.position.z) < _cubeSize)
			{
				sphereRigidbody.rigidbody.MovePosition(
					new Vector3(
						sphereRigidbody.position.x, 
						sphereRigidbody.position.y, 
						sphereRigidbody.position.z - _cubeSize));
			}
		}
	}
}

