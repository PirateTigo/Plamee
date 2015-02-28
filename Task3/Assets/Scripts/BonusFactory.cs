using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class BonusFactory
	{
		private GameObject _exampleBonus;
		public BonusFactory ()
		{
			_exampleBonus = GameObject.Find ("Bonus");
		}

		public BonusController GetBonus(Vector3 position)
		{
			GameObject newBonus = 
				(GameObject.Instantiate (_exampleBonus) as GameObject);
			BonusController bonusController = newBonus.AddComponent<BonusController> ();
			bonusController.OnClubLonger += Main.Club.OnClubLonger;
			bonusController.OnClubShorter += Main.Club.OnClubShorter;
			bonusController.OnSphereFaster += Main.Sphere.OnSphereFaster;
			bonusController.OnSphereSlower += Main.Sphere.OnSphereSlower;
			Rigidbody bonusRigidbody = newBonus.AddComponent<Rigidbody> ();
			bonusRigidbody.isKinematic = true;
			newBonus.transform.position = position;
			bonusController.rotationSpeed = 1f;
			bonusController.fallSpeed = 0.005f;
			bonusController.IsCaught = false;
			return bonusController;
		}
	}
}