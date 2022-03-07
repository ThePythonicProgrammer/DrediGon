using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    public string MainGame;
    public string MainMenu;
    public string SettingsMainMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(MainGame);
    }

    public void SettingsOpen()
    {
        SceneManager.LoadScene(SettingsMainMenu);
    }

    public void SettingsMainClose()
    {
            SceneManager.LoadScene(MainMenu); 
    }

    public void SettingsPauseClose()
    {
        SceneManager.LoadScene(MainGame);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenuLoad()
    {
        SceneManager.LoadScene(MainMenu);
    }



}
