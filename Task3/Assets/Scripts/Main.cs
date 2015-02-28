using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AssemblyCSharp 
{
	public class Main : MonoBehaviour 
	{
		public static SphereController Sphere;
		public ClubController Club;
		public float ScreenWidth;
		public float ScreenHeight;
		public GameObject LifePanel;
		public Text Score;

		private FieldManager _fieldManager;
		private bool _gameStarted;
		private bool _gameOver;
		private bool _death;
		private const float TimeOut = 60f;
		private float _timer = TimeOut;
		private int _lifeCount = 3;


		public void Start () 
		{
			SphereInit ();
			_fieldManager = new FieldManager (this, 10, 5, -23.5f, -26.5f, 3.5f, 1.34f);
		}
		
		public void Update () 
		{
			if (_death)
			{
				StartCoroutine(Death());
				_death = false;
			}
			if (_gameOver)
			{
				StartCoroutine(_fieldManager.Reset());
				_gameOver = false;
			}
			if (!_gameStarted) 
			{
				if (!Sphere.NeedResetPosition && Input.GetMouseButtonDown(0))
				{
					_gameStarted = true;
					Sphere.Direction = new Vector2(UnityEngine.Random.Range(-1f, 1f),1f);
					Sphere.Direction.Normalize();
					Club.GameStarted = true;
					Sphere.GameStarted = true;
				}
			}
			else
			{
				_timer -= Time.deltaTime;
				if (_timer < 0f)
				{
					_timer = TimeOut;
					StartCoroutine(_fieldManager.UpdateField());
				}
			}
		}

		public static void SphereInit()
		{
			Sphere = GameObject.Find("Sphere").GetComponent<SphereController>();
		}

		public void ResetAfterDeath()
		{
			switch (_lifeCount)
			{
			case 3 : 
				LifePanel.transform.Find ("Life 1").gameObject.SetActive(false);
				break;
			case 2 :
				LifePanel.transform.Find ("Life 2").gameObject.SetActive(false);
				break;
			case 1 :
				LifePanel.transform.Find ("Life 3").gameObject.SetActive(false);
				break;
			default:
				break;				
			}
			_lifeCount--;
			_death = true;
			Club.ResetAfterDeath ();
			Sphere.ResetAfterDeath ();
			_gameStarted = false;
			if (_lifeCount == 0) ResetAfterGameOver ();
		}

		public void ResetAfterGameOver()
		{		
			_gameOver = true;
			_timer = TimeOut;
			_lifeCount = 3;
			LifePanel.transform.Find ("Life 1").gameObject.SetActive (true);
			LifePanel.transform.Find ("Life 2").gameObject.SetActive (true);
			LifePanel.transform.Find ("Life 3").gameObject.SetActive (true);

			Club.ResetAfterGameOver ();
			Sphere.ResetAfterGameOver ();
			_gameStarted = false;
		}

		public void OnScore()
		{
			int count = (int)System.Convert.ToDecimal (Score.text);
			count ++;
			Score.text = count.ToString ();
		}

		private IEnumerator Death()
		{
			switch (_lifeCount)
			{
			case 2 :
			{
				GameObject one = LifePanel.transform.Find ("Life 2").gameObject;
				GameObject two = LifePanel.transform.Find ("Life 3").gameObject;
				for (int i = 0; i < 6; i++)
				{
					one.SetActive (!one.activeSelf);
					two.SetActive (!two.activeSelf);
					yield return new WaitForSeconds(0.5f);
				}
				break;
			}
			case 1 :
			{					
				GameObject one = LifePanel.transform.Find("Life 3").gameObject;
				for (int i = 0; i < 6; i++)
				{
					one.SetActive(!one.activeSelf);
					yield return new WaitForSeconds(0.5f);
				}
				break;
			}
			default :
				break;
			}
		}
	}
}
