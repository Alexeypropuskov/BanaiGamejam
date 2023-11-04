using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;


	public class ForcePoint : MonoBehaviour
	{
		[Inject]
		public Controls Controls;

		public Slider PowerSlider;
		public TextMeshProUGUI PowerValueText;
		public TextMeshProUGUI MinPowerValueText;
		public TextMeshProUGUI MaxPowerValueText;

		[Space(20f)]
		public float Distance = 10f;
		public float MinForce = 0f;
		public float MaxForce = 10f;
		public float Force = 5f;
		public bool IsOn;
		public bool CanWork;

		public float MoveSpeed = 3f;
		
		private void FixedUpdate()
		{
			if (!TimeManager.IsGame) return;
			
			Movement();
			
			CanWork = Physics.Raycast(transform.position, Vector3.right, out var hit, Distance);
			if (!CanWork) return;
			
			if (!IsOn) return;

			hit.rigidbody.AddForceAtPosition(Vector3.right * Force, hit.point, ForceMode.Force);
		}

		private void Movement()
		{
			var move = Controls.Mouse.Movement.ReadValue<Vector2>();
			move *= MoveSpeed * Time.fixedDeltaTime;
			transform.position += new Vector3(move.x, move.y);
		}

		private void Awake()
		{
			Controls.Mouse.Hold.performed += HoldOnperformed;
			Controls.Mouse.Hold.canceled += HoldOncanceled;
			Controls.Mouse.DeltaPower.performed += DeltaPowerOnperformed;

			PowerSlider.minValue = MinForce;
			MinPowerValueText.text = MinForce.ToString();
			PowerSlider.maxValue = MaxForce;
			MaxPowerValueText.text = MaxForce.ToString();
			PowerSlider.onValueChanged.AddListener(OnPowerChanged);
			PowerSlider.value = (MaxForce + MinForce) / 2f;
		}

		private void DeltaPowerOnperformed(InputAction.CallbackContext obj)
			=> OnPowerChanged(obj.ReadValue<float>());

		public void OnPowerChanged(float value)
		{
			Force = Mathf.Clamp(value, MinForce, MaxForce);
			PowerValueText.text = Force.ToString();
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