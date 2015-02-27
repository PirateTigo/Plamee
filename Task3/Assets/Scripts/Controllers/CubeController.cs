using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CubeController : MonoBehaviour
	{
		private int _liveCount;
		private bool _immortal;


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
					if (gameObject.name != "SilverCube")
					{
						_liveCount--;
						if (_liveCount == 0)
							gameObject.SetActive(false);
					}
				}
			}
		}
	}
}

