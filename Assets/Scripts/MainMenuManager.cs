using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string _level1;
    [SerializeField] private string _level2;
    [SerializeField] private GameObject _levelSelector;
    public void PlayGame()
    {
        SceneManager.LoadScene(_level1);
    }

    public void OpenLevelSelector()
    {
        _levelSelector.SetActive(true);
    }
    public void CloseLevelSelector()
    {
        _levelSelector.SetActive(false);
    }

    public void LaunchLevel1()
    {
        SceneManager.LoadScene(_level1);

    }
    public void LaunchLevel2()
    {
        SceneManager.LoadScene(_level2);

    }

    public void Quit()
    {
        Application.Quit();
    }
}
