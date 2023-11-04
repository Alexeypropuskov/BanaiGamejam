using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
	public class LevelMover : MonoBehaviour
	{
		private Coroutine _coroutine;
		
		public Transform[] Foundations;
		public int Num;
		
		public Camera Camera;
		public Button NextTowerButton;

		[Space]
		public float SpeedMove;

		private void Awake()
		{
			var found = FindObjectsOfType<Foundation>();
			Foundations = new Transform[found.Length];
			for (int i = 0; i < found.Length; i++)
				Foundations[i] = found[i].transform;

			Camera = FindObjectOfType<Camera>();
			
			//NextTowerButton.onClick.AddListener(StartMove());
		}

		public void StartMove()
		{
			Num++;
			if (Num >= Foundations.Length)
			{
				
				return;
			}
		}
		
		public IEnumerator MoveTo()
		{
			yield break;
		}
	}
}