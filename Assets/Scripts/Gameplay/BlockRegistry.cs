using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class BlockRegistry : MonoBehaviour
	{
		public GameInstaller Installer;
		
		public List<Block> AllBlocks = new(1024);
		public List<Block> FallBlocks = new(1024);

		[HideInInspector]
		public int Falls;
		[Range(0.5f, 10f)]
		public float DelayForClearVelocity = 1f;
		
		private IEnumerator Start()
		{
			Installer = FindObjectOfType<GameInstaller>();
			var blocks = FindObjectsOfType<Block>();
			foreach (var block in blocks)
				AllBlocks.Add(block);

			//AllBlocks.Sort(Comparison);
			
			yield return new WaitForSeconds(DelayForClearVelocity);
			foreach (var block in AllBlocks)
			{
				block.Body.velocity = Vector3.zero;
				block.Body.angularVelocity = Vector3.zero;
			}
		}

		/*private int Comparison(Block a, Block b)
		{
			var x = a.transform.position;
			var y = b.transform.position;
			if(x.y < y.y) return -1;
			return x.y > y.y ? 1 : 0;
		}

		private void MoveToDownBlocks()
		{
			Physics.ClosestPoint()
			var hits = new RaycastHit[]
			foreach (var block in AllBlocks)
			{
				block.
			}
		}*/
		
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