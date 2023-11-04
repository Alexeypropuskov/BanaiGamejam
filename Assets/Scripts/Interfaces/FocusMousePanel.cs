using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FocusMousePanel : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler
	{
		private Camera _camera;
		
		public RectTransform Cursor;
		[HideInInspector]
		public Image ImageCursor;

		public void OnPointerMove(PointerEventData eventData)
		{
			var pos = new Vector3(eventData.position.x, eventData.position.y, 0f);
			pos.x -= Screen.currentResolution.width / 2f;
			pos.y -= Screen.currentResolution.height / 2f;
			Cursor.localPosition = pos;
			
			//todo поворачивать курсор
		}

		private void Awake()
		{
			_camera = FindObjectOfType<Camera>();
			ImageCursor = Cursor.GetComponent<Image>();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Cursor.gameObject.SetActive(false);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Cursor.gameObject.SetActive(true);
		}
	}
