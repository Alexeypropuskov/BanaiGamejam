using System.Collections.Generic;
using UnityEngine;


	public class BlockRegistry : MonoBehaviour
	{
		public List<Block> AllBlocks = new(1024);
		public List<Block> FallBlocks = new(1024);

		private void Awake()
		{
			var blocks = FindObjectsOfType<Block>();
			foreach (var block in blocks)
				AllBlocks.Add(block);
		}

		private void OnTriggerEnter(Collider other)
		{
			var body = other.GetComponent<Block>();
			if (body == null) return;
			if (FallBlocks.Contains(body) || !AllBlocks.Contains(body)) return;
			
			FallBlocks.Add(body);
		}
	}