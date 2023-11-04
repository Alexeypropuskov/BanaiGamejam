using System;
using UnityEngine;


	public class Block : MonoBehaviour
	{
		private Vector3 _velocity;
		private Vector3 _angularVelocity;
		
		[HideInInspector]
		public Rigidbody Body;

		public bool Calc = true;
		
		

		public void SetFroze(bool value)
		{
			if (value)
			{
				_velocity = Body.velocity;
				_angularVelocity = Body.angularVelocity;
				Body.isKinematic = true;
			}
			else
			{
				Body.isKinematic = false;
				Body.velocity = _velocity;
				Body.angularVelocity = _angularVelocity;
			}
		}

		private void Awake()
		{
			Body = GetComponent<Rigidbody>();
		}
	}