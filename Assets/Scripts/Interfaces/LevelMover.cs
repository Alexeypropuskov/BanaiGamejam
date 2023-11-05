using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
	public class LevelMover : MonoBehaviour
	{
		private Coroutine _effectCoroutine;
		private Coroutine _moveCoroutine;
		private float _time;
		private Vector3 _position;
		
		[HideInInspector]
		public Vector3[] Foundations;
		[HideInInspector]
		public int Num;
		
		[HideInInspector]
		public Transform Camera;
		[HideInInspector]
		public Transform NextTowerTransform;
		public Button NextTowerButton;

		[Space]
		public float TimeToMove = 5f;

		[Header("---Animation Parameters---")]
		public float DelayPrefAnimation = 10f;
		public Vector2 Scale;
		public Gradient Color;
		public Vector2 AnimationTime;
		public Image Border;

		private void Awake()
		{
			return;
			var found = FindObjectsOfType<Foundation>();
			Foundations = new Vector3[found.Length];
			for (int i = 0; i < found.Length; i++)
				Foundations[i] = found[i].transform.position;

			Array.Sort(Foundations, Comparison);
			
			Camera = FindObjectOfType<Camera>().transform;

			NextTowerTransform = NextTowerButton.transform;
			NextTowerButton.onClick.AddListener(StartMove);

			//_moveCoroutine = StartCoroutine(MoveTo());
			_effectCoroutine = StartCoroutine(Animation());
		}

		public void StartMove()
		{
			AudioManager.PlayEventClick();
			Num++;
			if (Num + 1 >= Foundations.Length)
			{
				NextTowerButton.gameObject.SetActive(false);
			}

			if (_moveCoroutine != null)
				StopCoroutine(_moveCoroutine);
			if (_effectCoroutine != null)
			{
				StopCoroutine(_effectCoroutine);
				NextTowerTransform.localScale = Vector3.one * Scale.x;
				Border.color = Color.Evaluate(0f);
			}
			
			_moveCoroutine = StartCoroutine(MoveTo());
			_effectCoroutine = StartCoroutine(Animation());
		}

		private int Comparison(Vector3 x, Vector3 y)
		{
			if(x.x < y.x) return -1;
			return x.x > y.x ? 1 : 0;
		}
		
		private IEnumerator MoveTo()
		{
			var target = Camera.position;
			target.x = Foundations[Num].x;
			var time = 0f;
			while (TimeToMove > time)
			{
				Camera.position = Vector3.Lerp(Camera.position, target, 1f - (TimeToMove - time) / TimeToMove);
				time += TimeManager.DeltaTime;
				yield return null;
			}
			
			_moveCoroutine = null;
		}

		private IEnumerator Animation()
		{
			yield return new WaitForSeconds(DelayPrefAnimation);

			var minScale = Vector3.one * Scale.x;
			var maxScale = Vector3.one * Scale.y;
			var timeS = 5f;
			while (true)
			{
				var time = 0f;
				var timer = Mathf.Lerp(AnimationTime.x, AnimationTime.y, 5f - timeS);
				timeS *= 0.9f;
				while (timer > time)
				{
					var delta = 1f - (timer - time) / timer;
					NextTowerTransform.localScale = Vector3.Lerp(minScale, maxScale, delta);
					Border.color = Color.Evaluate(delta);
					time += TimeManager.DeltaTime;
					yield return null;
				}

				(minScale, maxScale) = (maxScale, minScale);
			}
		}
	}
}