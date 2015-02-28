using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class SphereController : MonoBehaviour
	{
		public float speed;
		public float speedLimit;
		public Vector2 Direction; // { get; set; }
		public bool GameStarted;
		public ClubController Club;
		public Main MainClass;
		public bool NeedResetPosition;

		private float _speedBeforeStart;
		private float _startXPosition;
		private float _startYPosition;
		private float _startZPosition;
		private float _timer;
		private const float TimeOut = 30f;
		private float _lastMousePosition;

		public void Start()
		{
			Direction = new Vector2 (0f, 0f);		
			_timer = TimeOut;
			_startXPosition = transform.position.x;
			_startYPosition = transform.position.y;
			_startZPosition = transform.position.z;
			_speedBeforeStart = speed;
			_lastMousePosition = Input.mousePosition.x;
		}

		public void Update()
		{
			if (GameStarted)
			{
				_timer -= Time.deltaTime;
				if (_timer < 0f) 
				{
					speed *= 1.05f;
					_timer = TimeOut;
				}
				if (Direction == Vector2.zero)
					Direction = new Vector2(0f, -1f);
			}
			else
			{
				float newMousePosition = Input.mousePosition.x;
				float delta = newMousePosition - _lastMousePosition;
				_lastMousePosition = newMousePosition;
				Vector3 spherePosition = transform.position;
				float newClubPosition = spherePosition.x + delta / Club.Density;
				newClubPosition = newClubPosition > Club.Diapason/2f ? Club.Diapason/2f :
					(newClubPosition < -Club.Diapason/2f ? -Club.Diapason/2f :
					 newClubPosition);
				transform.position = new Vector3(newClubPosition, spherePosition.y, spherePosition.z);			
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
					Direction.Normalize();
					newXPosition = rigidbody.position.x + delta * Direction.x;
				}

				float newZPosition = rigidbody.position.z + delta * Direction.y;
				if (newZPosition > Club.StartYPosition + MainClass.ScreenHeight*0.95f)
				{

					Direction = new Vector2(Direction.x, -Direction.y);
					Direction.Normalize();
					newZPosition = rigidbody.position.z + delta * Direction.y;					
				}
				else 
				{ 
					if (newZPosition < Club.StartYPosition - MainClass.ScreenHeight*0.2f)
						MainClass.ResetAfterDeath();
				}

				rigidbody.MovePosition(
					new Vector3 (
						newXPosition,
						rigidbody.position.y,
						newZPosition));
			}
			else if (NeedResetPosition)
			{
				rigidbody.MovePosition(
					new Vector3 (_startXPosition, _startYPosition, _startZPosition));
				NeedResetPosition = false;
			}			
		}

		public void OnCollisionEnter(Collision collision)
		{
			if (GameStarted)
			{
				Vector2 normal = 
					new Vector2(
						collision.contacts [0].normal.normalized.x, 
						collision.contacts [0].normal.normalized.z);

				if (Vector2.Angle(normal, Direction) < 90f)
				{
					float x1 = normal.x;
					float x2 = transform.position.x;
					float y1 = normal.y;
					float y2 = transform.position.z;
					if (x1 < x2 && y1 < y2)					
						normal = (Vector2.Angle(Direction, new Vector2(0f, 1f)) >= 90f) ? 
						          new Vector2(0f, 1f) : new Vector2(1f, 0f);
					else if (x1 < x2 && y1 > y2)
						normal = (Vector2.Angle(Direction, new Vector2(1f, 0f)) >= 90f) ? 
							new Vector2(1f, 0f) : new Vector2(0f, -1f);
					else if (x1 > x2 && y1 < y2)
						normal = (Vector2.Angle(Direction, new Vector2(0f, 1f)) >= 90f) ? 
						          new Vector2(0f, 1f) : new Vector2(-1f, 0f);
					else
						normal = (Vector2.Angle(Direction, new Vector2(0f, -1f)) >= 90f) ? 
							new Vector2(0f, -1f) : new Vector2(-1f, 0f);
				}

				Vector2 basis = new Vector2(-normal.y, normal.x);
				Direction = - Vector2.Dot (normal, Direction) * normal + 
					Vector2.Dot (basis, Direction) * basis;
				Direction.Normalize();
			}
		}

		public void ResetAfterDeath()
		{
			GameStarted = false;
			Direction = new Vector2 (0f, 0f);
			NeedResetPosition = true;
		}

		public void ResetAfterGameOver()
		{
			ResetAfterDeath ();
			_timer = TimeOut;
			speed = _speedBeforeStart;
		}

		public void OnSphereFaster()
		{
			speed *= 1.05f;
			_timer = TimeOut;
		}

		public void OnSphereSlower()
		{
			speed *= 0.9f;
			_timer = TimeOut;
		}
	}
}