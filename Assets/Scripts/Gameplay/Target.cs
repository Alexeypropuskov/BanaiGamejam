using System;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
	private float _x = float.MinValue;
	private bool _needMove;
	private TargetPoint[] _targets;

	[Header("---Visual---")]
	public SpriteRenderer Image;
	public float StartDelay = 3f;
	public float MoveSpeed = 3f;
	
	private IEnumerator Start()
	{
		_targets = FindObjectsOfType<TargetPoint>();
		Array.Sort(_targets, Comparison);

		while (StartDelay > 0f)
		{
			StartDelay -= TimeManager.DeltaTime;
			yield return null;
		}

		NeedMove(_targets[0].transform.position.x);
	}

	private int Comparison(TargetPoint x, TargetPoint y)
	{
		var a = x.transform.position.x;
		var b = y.transform.position.y;
		if(a < b) return 1;
		return a > b ? -1 : 0;
	}
	
	private void Update()
	{
		if (!TimeManager.IsGame) return;
		if (!_needMove) return;

		var transform = this.transform;
		var position = transform.position;
		position += Vector3.right * (MoveSpeed * TimeManager.DeltaTime);
		transform.position = position;

		if (_x <= position.x)
		{
			_needMove = false;
			Image.flipX = !Image.flipX;
		}
	}

	public int CountNeedToFall()
	{
		var count = 0;
		foreach (var target in _targets)
		{
			count += target.Limit;
		}

		return count;
	}
	
	public void NeedMove(float x)
	{
		_x = Mathf.Max(x, _x);
		if (Mathf.Approximately(_x, _targets[^1].transform.position.x))
		{
			_needMove = false;
			FindObjectOfType<GameInstaller>().Win();
			return;
		}

		if (_needMove) return;

		_needMove = true;
		Image.flipX = !Image.flipX;
	}
}