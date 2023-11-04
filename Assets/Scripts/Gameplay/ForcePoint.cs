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

		public Image FillPower;
		public TextMeshProUGUI FillValueText;

		[Space(20f)]
		public float Distance = 10f;
		public int MinForce = 1;
		public int MaxForce = 11;
		[HideInInspector]
		public int Force = 5;
		[HideInInspector]
		public bool IsOn;
		[HideInInspector]
		public bool CanWork;

		public float PowerLimit = 500f;
		public float PowerCostPerUnitInSec = 5f;
		[HideInInspector]
		public float CurrentPower;

		[Space]
		public float MultDeltaScroll = 100f;
		
		[Space]
		public float MoveSpeed = 3f;
		
		private void FixedUpdate()
		{
			if (!TimeManager.IsGame) return;
			
			Movement();
			
			CanWork = Physics.Raycast(transform.position, Vector3.right, out var hit, Distance);
			if (!CanWork) return;
			
			if (!IsOn) return;
			if (CurrentPower <= 0f)
			{
				FindObjectOfType<GameInstaller>().GameOver(this);
				return;
			}
			PowerUse(hit);
		}

		private void Movement()
		{
			var move = Controls.Mouse.Movement.ReadValue<Vector2>();
			move *= MoveSpeed * Time.fixedDeltaTime;
			transform.position += new Vector3(move.x, move.y);
		}

		private void PowerUse(RaycastHit hit)
		{
			CurrentPower -= Force * PowerCostPerUnitInSec * TimeManager.FixedDeltaTime;
			FillPower.fillAmount = CurrentPower / PowerLimit;
			FillValueText.text = CurrentPower.ToString();
			//todo учитывать дистанцию? как игромеханически это будет?
			hit.rigidbody.AddForceAtPosition(Vector3.right * Force, hit.point, ForceMode.Force);
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

			CurrentPower = PowerLimit;
			FillPower.fillAmount = 1f;
			FillValueText.text = $"{PowerLimit} / {PowerLimit}";
		}

		private void DeltaPowerOnperformed(InputAction.CallbackContext obj)
			=> PowerSlider.value += obj.ReadValue<Vector2>().y / MultDeltaScroll;

		public void OnPowerChanged(float value)
		{
			Force = Mathf.Clamp(Mathf.RoundToInt(value), MinForce, MaxForce);
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