using System;
using System.Collections;
using UnityEngine;

public class Target : MonoBehaviour
{
	private int _current = -1;
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

		NeedMove();
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

		var target = _targets[_current].transform.position;
		if (target.x <= position.x)
		{
			_needMove = false;
			Image.flipX = !Image.flipX;
		}
	}

	public void NeedMove()
	{
		_current = Mathf.Clamp(_current + 1, 0, _targets.Length - 1);
		if (_needMove) return;
		
		if (_current >= _targets.Length)
		{
			_needMove = false;
			FindObjectOfType<GameInstaller>().Win();
			return;
		}

		_needMove = true;
		Image.flipX = !Image.flipX;
	}
}