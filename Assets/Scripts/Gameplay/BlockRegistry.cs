using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class BlockRegistry : MonoBehaviour
	{
		public List<Rigidbody> AllBlocks = new(1024);
		public List<Rigidbody> FallBlocks = new(1024);

		private void Awake()
		{
			var blocks = FindObjectsOfType<Block>();
			foreach (var block in blocks)
			{
				var body = block.GetComponent<Rigidbody>();
				if(body != null)
					AllBlocks.Add(body);
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			var body = other.attachedRigidbody;
			if (body == null) return;
			if (FallBlocks.Contains(body) || !AllBlocks.Contains(body)) return;
			
			FallBlocks.Add(body);
		}
	}
}