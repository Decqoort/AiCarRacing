using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private MapDisplay mapDisplay;
    [SerializeField] private Slider sliderLaps;
    [SerializeField] private Slider sliderBots;
    public void PlayGame()
    {
        Map map = mapDisplay.GetCurrentMap();
        //Debug.Log(map.sceneToLoad.name);
        GameManager.counLaps = (int)sliderLaps.value;
        GameManager.countBots = (int)sliderBots.value;
        SceneManager.LoadScene(map.mapIndex + 1);
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #elif UNITY_WEBGL
            Application.ExternalEval("window.close();");
    #elif UNITY_STANDALONE
            Application.Quit();
    #else
            Application.Quit();
    #endif
    }
}
