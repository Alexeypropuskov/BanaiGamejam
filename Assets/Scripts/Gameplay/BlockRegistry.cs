using System.Collections.Generic;
using UnityEngine;


	public class BlockRegistry : MonoBehaviour
	{
		public GameInstaller Installer;
		
		public List<Block> AllBlocks = new(1024);
		public List<Block> FallBlocks = new(1024);

		public int Falls;
		
		private void Awake()
		{
			Installer = FindObjectOfType<GameInstaller>();
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
			if (body.Calc)
			{
				Installer.UpdateScore();
				Falls++;
			}
			
			
		}
	}