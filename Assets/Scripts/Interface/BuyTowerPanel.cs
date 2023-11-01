using System;
using UnityEngine;
using UnityEngine.UI;

namespace Netologia.TowerDefence.Interface
{
	public class BuyTowerPanel : MonoBehaviour
	{
		[SerializeField]
		private Button _physicTowerButton;
		[SerializeField]
		private Button _fireTowerButton;
		[SerializeField]
		private Button _iceTowerButton;

		public event Action<ElementalType> OnBuyTowerHandler; 

		private void Awake()
		{
			_physicTowerButton.onClick.AddListener(BuyPhysic);
			_fireTowerButton.onClick.AddListener(BuyFire);
			_iceTowerButton.onClick.AddListener(BuyIce);
		}

		private void OnDestroy()
		{
			_physicTowerButton.onClick.RemoveListener(BuyPhysic);
			_fireTowerButton.onClick.RemoveListener(BuyFire);
			_iceTowerButton.onClick.RemoveListener(BuyIce);
		}

		private void BuyPhysic()
			=> OnBuyTowerHandler?.Invoke(ElementalType.Physic);
		private void BuyFire()
			=> OnBuyTowerHandler?.Invoke(ElementalType.Fire);
		private void BuyIce()
			=> OnBuyTowerHandler?.Invoke(ElementalType.Ice);
	}
}