using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SphereController : MonoBehaviour
	{
		public float speed;
		public float speedLimit;
		public Vector2 Direction { get; set; }
		public bool GameStarted;
		public ClubController Club;
		public Main MainClass;

		public void Start()
		{
			Direction = new Vector2 (0f, 0f);		
		}

		public void FixedUpdate()
		{
			if (GameStarted) 
			{
				float delta = speed/speedLimit;
				delta = delta < 1f ? delta : 1f;
				float newXPosition = rigidbody.position.x + delta * Direction.x;
				if (newXPosition > Club.StartPosition + MainClass.ScreenWidth/2f || 
				    newXPosition < Club.StartPosition - MainClass.ScreenWidth/2f)
				{
					Direction = new Vector2(-Direction.x, Direction.y);
					newXPosition = rigidbody.position.x + delta * Direction.x;
				}

				rigidbody.MovePosition(
					new Vector3 (
						newXPosition,
						rigidbody.position.y,
						rigidbody.position.z + delta * Direction.y));
			}
		}

		public void OnCollisionEnter(Collision collision)
		{
			Vector2 normal = 
				new Vector2(
					collision.contacts [0].normal.normalized.x, 
					collision.contacts [0].normal.normalized.z);
			Vector2 basis = new Vector2(-normal.y, normal.x);
			Direction = - Vector2.Dot (normal, Direction) * normal + 
				Vector2.Dot (basis, Direction) * basis;
		}
	}
}

