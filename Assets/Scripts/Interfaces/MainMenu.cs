using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public const string c_progressKey = "Progress";
	private const int c_shift = 1;//0 - MainMenu

	public Button ResetProgressButton;
	public Button ContinueButton;
	public Button ExitButton;
	public Button TutorialButton;
	public Slider VolumeSlider;
	
	[Header("---Sound---")]
	public AudioClip MainMenuSound;
	
	private void Awake()
	{
		ResetProgressButton.onClick.AddListener(ResetProgress);
		ContinueButton.onClick.AddListener(ContinueGame);
		ExitButton.onClick.AddListener(Exit);
		//TutorialButton.onClick.AddListener(Tutorial);
		VolumeSlider.onValueChanged.AddListener(OnVolumeChanged);
	}

	private void Start()
	{
		AudioManager.SetSoundtrack(MainMenuSound);
		VolumeSlider.value = PlayerPrefs.GetFloat(AudioManager.c_volumePresetKey, .3f);
	}

	public void ResetProgress()
	{
		AudioManager.PlayEventClick();
		PlayerPrefs.SetInt(c_progressKey, 0);
	}

	public void ContinueGame()
	{
		AudioManager.PlayEventClick();
		SceneManager.LoadScene(PlayerPrefs.GetInt(c_progressKey) + c_shift);
	}

	public void Exit()
	{
		AudioManager.PlayEventClick();
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void Tutorial()
	{
		AudioManager.PlayEventClick();
		/*TutorialMain.SetActive(true);
		_tutIndex = 0;
		_tutorialPanels[_tutIndex].SetActive(true);*/
	}

	public void OnVolumeChanged(float value)
	{
		AudioManager.Volume = value;
	}
}