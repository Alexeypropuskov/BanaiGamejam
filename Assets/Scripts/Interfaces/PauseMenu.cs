using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	public Slider Volume;
	public Button RestartButton;
	public Button ResumeButton;
	public Button ExitButton;

	private void Awake()
	{
		Volume.onValueChanged.AddListener(OnVolumeChanged);
		RestartButton.onClick.AddListener(OnRestart);
		ResumeButton.onClick.AddListener(OnResume);
		ExitButton.onClick.AddListener(OnExit);
	}

	public void OnVolumeChanged(float value)
	{
		AudioManager.Volume = value;
	}

	public void OnRestart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnResume()
	{
		TimeManager.IsGame = true;
		gameObject.SetActive(false);
	}

	public void OnExit()
	{
		SceneManager.LoadScene(0);
	}
}