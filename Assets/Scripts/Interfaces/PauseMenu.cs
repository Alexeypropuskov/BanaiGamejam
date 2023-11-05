using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	private BlockRegistry _registry;
	
	public Slider Volume;
	public Button RestartButton;
	public Button ResumeButton;
	public Button ExitButton;

	private void Awake()
	{
		_registry = FindObjectOfType<BlockRegistry>();
		
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
		AudioManager.PlayEventClick();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnResume()
	{
		AudioManager.PlayEventClick();
		TimeManager.IsGame = true;
		gameObject.SetActive(false);
		foreach (var block in _registry.AllBlocks)
			block.SetFroze(false);
	}

	public void OnExit()
	{
		AudioManager.PlayEventClick();
		SceneManager.LoadScene(0);
	}
}