using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public const string c_progressKey = "Progress";
	private const int c_shift = 1;//0 - MainMenu
	private int _tutIndex = 0;
	private GameObject[] _tutorialPanels;

	public Button ResetProgressButton;
	public Button ContinueButton;
	public Button ExitButton;
	public Button TutorialButton;
	public Slider VolumeSlider;

	[Space]
	public GameObject TutorialMain;
	public GameObject RootTutorials;
	public Button NextTutorialButton;
	public Button PreviousTutorialButton;
	public Button CloseTutorialButton;
	
	private void Awake()
	{
		ResetProgressButton.onClick.AddListener(ResetProgress);
		ContinueButton.onClick.AddListener(ContinueGame);
		ExitButton.onClick.AddListener(Exit);
		TutorialButton.onClick.AddListener(Tutorial);
		VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);

		var trs = RootTutorials.GetComponentsInChildren<Transform>();
		_tutorialPanels = new GameObject[trs.Length - 1];
		TutorialMain.SetActive(false);
		for(var i = 1; i < trs.Length; i++)
		{
			_tutorialPanels[i - 1] = trs[i].gameObject;
			_tutorialPanels[i - 1].SetActive(false);
		}
		if (_tutorialPanels.Length == 0)
			TutorialButton.gameObject.SetActive(false);
		
		NextTutorialButton.onClick.AddListener(NextPage);
		PreviousTutorialButton.onClick.AddListener(PrevPage);
		CloseTutorialButton.onClick.AddListener(CloseTutorial);
	}

	public void ResetProgress()
	{
		PlayerPrefs.SetInt(c_progressKey, 0);
	}

	public void ContinueGame()
	{
		SceneManager.LoadScene(PlayerPrefs.GetInt(c_progressKey) + c_shift);
	}

	public void Exit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void Tutorial()
	{
		TutorialMain.SetActive(true);
		_tutIndex = 0;
		_tutorialPanels[_tutIndex].SetActive(true);
	}

	public void OnVolumeChanged(float value)
	{
		AudioManager.Volume = value;
	}

	public void NextPage()
	{
		_tutorialPanels[_tutIndex].SetActive(false);
		_tutIndex = Mathf.Clamp(_tutIndex + 1, 0, _tutorialPanels.Length - 1);
		_tutorialPanels[_tutIndex].SetActive(true);
	}

	public void PrevPage()
	{
		_tutorialPanels[_tutIndex].SetActive(false);
		_tutIndex = Mathf.Clamp(_tutIndex - 1, 0, _tutorialPanels.Length - 1);
		_tutorialPanels[_tutIndex].SetActive(true);
	}

	public void CloseTutorial()
	{
		TutorialMain.SetActive(false);
		_tutorialPanels[_tutIndex].SetActive(false);
		_tutIndex = 0;
	}
}