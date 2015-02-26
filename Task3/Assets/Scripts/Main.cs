using UnityEngine;
using System.Collections;

namespace AssemblyCSharp {

	public class Main : MonoBehaviour {

		private FieldManager _fieldManager;

		void Awake() {
		}

		void Start () {
			_fieldManager = new FieldManager (10, 5, -23.5f, -26.5f, 3.5f, 1.34f);
		}
		
		void Update () {
		}
	}
}
