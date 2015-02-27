using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp
{
	public class CubeFactory
	{
		private Dictionary<string, GameObject> _cubeInstances;

		public CubeFactory (GameObject silver, GameObject orange, GameObject green, GameObject red)
		{
			_cubeInstances = new Dictionary<string, GameObject> ();
			_cubeInstances.Add(silver.name, silver);
			_cubeInstances.Add(orange.name, orange);
			_cubeInstances.Add(green.name, green);
			_cubeInstances.Add(red.name, red);
		}

		public CubeController GetCube(int x, int y, float startx, float starty, float cubeSize, float cubeStratch, string cubeName)
		{			
			GameObject gameObject = (GameObject)GameObject.Instantiate (_cubeInstances[cubeName]);
			gameObject.name = cubeName;
			CubeController cubeController = gameObject.AddComponent<CubeController> ();
			cubeController.Init (x, y, startx, starty, cubeSize, cubeStratch);
			return cubeController;
		}
	}
}