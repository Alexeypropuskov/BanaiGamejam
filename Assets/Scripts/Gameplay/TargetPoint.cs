using System;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
	private BlockRegistry _reg;
	private Target _target;
	
	public Block[] Blocks;

	public int Limit = 3;
	
	private void Awake()
	{
		_target = FindObjectOfType<Target>();
		_reg = FindObjectOfType<BlockRegistry>();
		_reg.OnBlockFalled += CheckBlock;
	}

	private void CheckBlock(Block obj)
	{
		var index = Array.FindIndex(Blocks, t => t == obj);
		if (index == -1) return;

		Limit--;

		if (Limit <= 0)
		{
			_reg.OnBlockFalled -= CheckBlock;
			_target.NeedMove(transform.position.x);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
}