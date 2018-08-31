using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Скрипт для сцены с главным меню (компонент Main Camera)
public class MainMenu : MonoBehaviour {

    public Button buttonPlay, buttonInfo, buttonExit;

	void Start ()
    {
        Button play = buttonPlay.GetComponent<Button>();
        Button info = buttonInfo.GetComponent<Button>();
        Button exit = buttonExit.GetComponent<Button>();

        play.onClick.AddListener(Play);
        exit.onClick.AddListener(Exit);
	}

    void Play()
    {
        SceneManager.LoadScene ("SimpleScene", LoadSceneMode.Single);
    }

    void Exit()
    {
        Application.Quit();
    }
}
