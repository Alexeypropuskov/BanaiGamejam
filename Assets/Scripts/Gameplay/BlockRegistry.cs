using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	public class BlockRegistry : MonoBehaviour
	{
		[HideInInspector]
		public GameInstaller Installer;
		
		public List<Block> AllBlocks = new(1024);
		public List<Block> FallBlocks = new(1024);

		[HideInInspector]
		public int Falls;
		[Range(0.5f, 10f)]
		public float DelayForClearVelocity = 1f;

		public event Action<Block> OnBlockFalled;
		
		private IEnumerator Start()
		{
			Installer = FindObjectOfType<GameInstaller>();
			var blocks = FindObjectsOfType<Block>();
			foreach (var block in blocks)
				AllBlocks.Add(block);

			yield return new WaitForSeconds(DelayForClearVelocity);
			foreach (var block in AllBlocks)
			{
				block.Body.velocity = Vector3.zero;
				block.Body.angularVelocity = Vector3.zero;
			}
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
				OnBlockFalled?.Invoke(body);
			}

			StartCoroutine(HideDelay(body.gameObject));
		}

		private IEnumerator HideDelay(GameObject obj)
		{
			yield return new WaitForSeconds(10f);
			obj.SetActive(false);
		}
	}