using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
	MyAds _adManager;

	public static bool tutorialDone = false;

	private void Start()
	{
		_adManager = FindObjectOfType<MyAds>();
	}

	public void StartGame()
	{
		if (!tutorialDone)
			SceneManager.LoadScene("Tutorial");
		else
			SceneManager.LoadScene("Lvl");
	}

	public void Menu()
	{
		SceneManager.LoadScene("Menu");
	}

	public void Surrender()
	{
		SceneManager.LoadScene("Lose");
	}

	public void Win()
	{
		if(CurrentScene() == SceneManager.GetSceneByName("Tutorial"))
		{
			_adManager.currentAdType = AdsType.video;
			_adManager.ShowAds();
			tutorialDone = true;
			StartGame();
		}
		else
			SceneManager.LoadScene("Win");
	}

	Scene CurrentScene()
	{
		return SceneManager.GetActiveScene();
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Restart()
	{
		SceneManager.LoadScene(CurrentScene().name);
	}
}
