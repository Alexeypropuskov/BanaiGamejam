using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FocusMousePanel : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler, IPointerEnterHandler
	{
		private Camera _camera;
		private RectTransform _canvas;
		
		public RectTransform Cursor;
		[HideInInspector]
		public Image ImageCursor;

		public bool Enabled;
		
		public void OnPointerMove(PointerEventData eventData)
		{
			RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, eventData.position,
				null, out var pos);

			Cursor.anchoredPosition = pos;
			
			//todo поворачивать курсор
		}

		private void Awake()
		{
			_camera = FindObjectOfType<Camera>();
			ImageCursor = Cursor.GetComponent<Image>();
			_canvas = GetComponentInParent<Canvas>().transform as RectTransform;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			Cursor.gameObject.SetActive(false);
			Enabled = false;
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			Cursor.gameObject.SetActive(true);
			Enabled = true;
		}
	}
