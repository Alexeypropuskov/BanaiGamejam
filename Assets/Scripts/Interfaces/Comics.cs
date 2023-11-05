using UnityEngine;
using UnityEngine.UI;

	public class Comics : MonoBehaviour
	{
		private int _index = 0;
		private GameObject[] _lists;
		
		[Space]
		public GameObject Root;
		public GameObject ListRoot;
		public Button NextButton;
		public Button PrevButton;
		public Button CloseButton;
		
		public void Show()
		{
			var trs = ListRoot.GetComponentsInChildren<Transform>();
			_lists = new GameObject[trs.Length - 1];
			Root.SetActive(true);
			for(var i = 1; i < trs.Length; i++)
			{
				_lists[i - 1] = trs[i].gameObject;
				_lists[i - 1].SetActive(false);
			}
		
			NextButton.onClick.AddListener(NextPage);
			PrevButton.onClick.AddListener(PrevPage);
			CloseButton.onClick.AddListener(Close);
			
			_lists[_index].SetActive(true);
		}

		public void NextPage()
		{
			AudioManager.PlayEventClick();
			_lists[_index].SetActive(false);
			_index = Mathf.Clamp(_index + 1, 0, _lists.Length - 1);
			_lists[_index].SetActive(true);
		}

		public void PrevPage()
		{
			AudioManager.PlayEventClick();
			_lists[_index].SetActive(false);
			_index = Mathf.Clamp(_index - 1, 0, _lists.Length - 1);
			_lists[_index].SetActive(true);
		}
		
		public void Close()
		{
			AudioManager.PlayEventClick();
			Root.SetActive(false);
			_lists[_index].SetActive(false);
			_index = 0;
		}
	}