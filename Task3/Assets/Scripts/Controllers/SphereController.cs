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

		private float _timer;
		private const float TimeOut = 30f;

		public void Start()
		{
			Direction = new Vector2 (0f, 0f);		
			_timer = TimeOut;
		}

		public void Update()
		{
			if (GameStarted)
			{
				_timer -= Time.deltaTime;
				if (_timer < 0f) 
				{
					speed += speed*0.05f;
					_timer = TimeOut;
				}
			}
		}

		public void FixedUpdate()
		{
			if (GameStarted) 
			{
				float delta = speed/speedLimit;
				delta = delta < 1f ? delta : 1f;
				float newXPosition = rigidbody.position.x + delta * Direction.x;
				if (newXPosition > Club.StartXPosition + MainClass.ScreenWidth/2f || 
				    newXPosition < Club.StartXPosition - MainClass.ScreenWidth/2f)
				{
					Direction = new Vector2(-Direction.x, Direction.y);
					newXPosition = rigidbody.position.x + delta * Direction.x;
				}

				float newZPosition = rigidbody.position.z + delta * Direction.y;
				if (newZPosition > Club.StartYPosition + MainClass.ScreenHeight*0.95f)
				{
					Direction = new Vector2(Direction.x, -Direction.y);
					newZPosition = rigidbody.position.z + delta * Direction.y;
				}

				rigidbody.MovePosition(
					new Vector3 (
						newXPosition,
						rigidbody.position.y,
						newZPosition));
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

