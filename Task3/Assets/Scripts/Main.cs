using UnityEngine;
using System.Collections;

namespace AssemblyCSharp {

	public class Main : MonoBehaviour {
		public static SphereController Sphere;
		public ClubController Club;
		public float ScreenWidth;

		private FieldManager _fieldManager;
		private bool _gameStarted;

		void Awake() {
		}

		void Start () {
			SphereInit ();
			_fieldManager = new FieldManager (10, 5, -23.5f, -26.5f, 3.5f, 1.34f);
		}
		
		void Update () {
			if (!_gameStarted) 
			{
				if (Input.GetMouseButtonDown(0))
				{
					_gameStarted = true;
					Sphere.Direction = new Vector2(UnityEngine.Random.Range(-1f, 1f),1f);
					Sphere.Direction.Normalize();
					Club.GameStarted = true;
					Sphere.GameStarted = true;
				}
			}
		}

		static void SphereInit()
		{
			Sphere = GameObject.Find("Sphere").GetComponent<SphereController>();
		}
	}
}
