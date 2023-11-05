using System;
using UnityEngine;

public class Target : MonoBehaviour
{
	private bool _needRun = true;

	public float DistanceToTower = 5f;

	[Header("---Visual---")]
	public SpriteRenderer Image;
	public SphereCollider Collider;

	private void Update()
	{
		if (_needRun)
		{
			
		}
	}
}