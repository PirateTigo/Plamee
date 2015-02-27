using System;
using System.Collections;
using UnityEngine;

namespace AssemblyCSharp
{
	public class FieldManager
	{
		private CubeFactory _cubeFactory;
		private CubeController[,] _cubes;
		private int _width;
		private int _height;
		private float _startx;
		private float _starty;
		private float _cubeSize;
		private float _cubeStrath;

		public FieldManager (int width, int height, float x, float y, float cubeSize = 1.0f, float cubeStratch = 1.0f)
		{ 
			_cubeFactory = new CubeFactory (
				GameObject.Find ("SilverCube"),
				GameObject.Find ("OrangeCube"),
				GameObject.Find ("GreenCube"),
				GameObject.Find ("RedCube"));
			_width = width;
			_height = height;
			_startx = x;
			_starty = y;
			_cubeSize = cubeSize;
			_cubeStrath = cubeStratch;
			_cubes = new CubeController[width, height];

			for (int i = 0; i < width; i++) 			
				for (int j = 0; j < height; j++) 
					_cubes [i, j] = 
						_cubeFactory.GetCube (i, j, x, y, cubeSize, cubeStratch, GetCubeName (i, j));						
		}

		public IEnumerator UpdateField()
		{
			for (int i = 0; i < _width; i++)
				_cubes [i, 0].Kill ();

			yield return new WaitForSeconds(1f);

			for (int i = 0; i < _width; i++)
				for (int j = 1; j < _height; j++)
				{
					_cubes[i, j-1] = _cubes[i, j];
					_cubes[i, j-1].Forward(_cubeSize);
				}

			for (int i = 0; i < _width; i++)
				_cubes [i, _height - 1] = null;

			for (int i = 0; i < _width; i++)
				_cubes[i, _height - 1] =
					_cubeFactory.GetCube(
						i,_height - 1, _startx, _starty, _cubeSize, _cubeStrath, GetCubeName(i, _height - 1));
		}

		private string GetCubeName(int x, int y)
		{
			bool haveSilverNeighbor = false;
			for (int i = x - 1; i < x + 1; i++) 
			{
				for (int j = y - 1; j < y + 1; j++)
				{
					if ((i == x && j == y) || i < 0 || j < 0 || i == _width || j == _height) continue;
					if (_cubes[i, j] != null)
					{
						if (_cubes[i, j].gameObject.name == "SilverCube")
						{
							haveSilverNeighbor = true;
							break;
						}
					}

				}
			}

			float randValue = UnityEngine.Random.Range (0, 1.0f);

			if (haveSilverNeighbor) 
			{
				return randValue > 1.0f/3.0f ? 
					(randValue > 2.0f/3.0f ? "OrangeCube" : "GreenCube") : "RedCube";
			} else 
			{
				return randValue > 1.0f/4.0f ? 
						(randValue > 2.0f/4.0f ? 
					 		(randValue > 3.0f/4.0f ? 
					 "OrangeCube" : 
					 	"GreenCube") : 
					 		"RedCube") : 
								"SilverCube";
			}
		}
	}
}

