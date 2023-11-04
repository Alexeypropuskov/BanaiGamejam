using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Gameplay
{
	public class ForcePoint : MonoBehaviour
	{
		[Inject]
		public Controls Controls;
		
		public float Distance = 10f;
		public float MinForce = 0f;
		public float MaxForce = 10f;
		public float Force = 5f;
		public bool IsOn;
		public bool CanWork;

		public float MoveSpeed = 3f;
		
		private void FixedUpdate()
		{
			Movement();
			
			CanWork = Physics.Raycast(transform.position, Vector3.right, out var hit, Distance);
			if (!CanWork) return;
			
			if (!IsOn) return;

			hit.rigidbody.AddForceAtPosition(Vector3.right * Force, hit.point, ForceMode.Force);
		}

		private void Movement()
		{
			var move = Controls.Mouse.Movement.ReadValue<Vector2>();
			move *= Time.fixedDeltaTime;
			transform.position += new Vector3(move.x, move.y);
		}

		private void Awake()
		{
			Controls.Mouse.Hold.performed += HoldOnperformed;
			Controls.Mouse.Hold.canceled += HoldOncanceled;
		}

		private void HoldOncanceled(InputAction.CallbackContext obj)
		{
			//Debug.Log("Input: Cancel click");
			IsOn = false;
		}

		private void HoldOnperformed(InputAction.CallbackContext obj)
		{
			//Debug.Log("Input: Hold click");
			if (CanWork) IsOn = true;
		}

		private void OnDrawGizmos()
		{
			var canColor = CanWork ? Color.green : Color.red;
			var onColor = IsOn ? Color.green : Color.red;

			var pos = transform.position;
			var dir = Vector3.right;
			DebugExtension.DrawArrow(pos, dir, onColor);
			Gizmos.color = canColor;
			Gizmos.DrawLine(pos, pos + Distance * dir);
		}
	}
}