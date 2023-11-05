using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


	public class ForcePoint : MonoBehaviour
	{
		private bool _isEffect;
		private Camera _camera;
		
		public Controls Controls;

		public Slider PowerSlider;
		public TextMeshProUGUI PowerValueText;
		public TextMeshProUGUI MinPowerValueText;
		public TextMeshProUGUI MaxPowerValueText;

		public Image FillPower;
		public TextMeshProUGUI FillValueText;
		public ParticleSystem Effect;

		[Space(20f)]
		public float Distance = 10f;
		public int MinForce = 1;
		public int MaxForce = 11;
		[HideInInspector]
		public int Force = 5;
		[HideInInspector]
		public bool IsOn;

		public float PowerLimit = 500f;
		public float PowerCostPerUnitInSec = 5f;
		[HideInInspector]
		public float CurrentPower;
		public FocusMousePanel Focus;

		[Space]
		public float MultDeltaScroll = 100f;
		
		private void Update()
		{
			if (CalcLogic())
			{
				if (!_isEffect)
				{
					_isEffect = true;
					Effect.Play();
					AudioManager.PlayAttack();
				}
			}
			else if (_isEffect)
			{
				_isEffect = false;
				Effect.Stop();
				AudioManager.StopAttack();
			}
		}

		private bool CalcLogic()
		{
			if (!TimeManager.IsGame) return false;
			
			if (!Focus.Enabled) return false;
			Movement();

			if (!IsOn) return false;
			if (CurrentPower <= 0f)
			{
				FindObjectOfType<GameInstaller>().GameOver(this);
				return false;
			}
			PowerUse();
			
			return true;
		}

		private void Movement()
		{
			var mouse = Mouse.current.position.value;
			var point = _camera.ScreenToWorldPoint(new Vector3(mouse.x, mouse.y, 10f));
			point.z = 0f;
			transform.position = point;
		}

		private void PowerUse()
		{
			CurrentPower -= Force * PowerCostPerUnitInSec * TimeManager.FixedDeltaTime;
			FillPower.fillAmount = CurrentPower / PowerLimit;
			FillValueText.text = Mathf.Clamp(Mathf.RoundToInt(CurrentPower), 0f, float.MaxValue).ToString();
			if (Physics.Raycast(transform.position, Vector3.right, out var hit, Distance))
				hit.rigidbody.AddForceAtPosition(Vector3.right * Force, hit.point, ForceMode.Force);
		}

		private void Start()
		{
			_camera = FindObjectOfType<Camera>();
			
			Controls = FindObjectOfType<GameInstaller>().Controls;
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
			FillValueText.text = PowerLimit.ToString();
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
			IsOn = false;
		}

		private void HoldOnperformed(InputAction.CallbackContext obj)
		{
			IsOn = true;
		}

		private void OnDrawGizmos()
		{
			var canColor = IsOn ? Color.green : Color.red;
			var onColor = IsOn ? Color.green : Color.red;

			var pos = transform.position;
			var dir = Vector3.right;
			DebugExtension.DrawArrow(pos, dir, onColor);
			Gizmos.color = canColor;
			Gizmos.DrawLine(pos, pos + Distance * dir);
		}
	}