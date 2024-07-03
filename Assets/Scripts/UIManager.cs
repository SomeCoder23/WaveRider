using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject mainMenu, gameHUD, loseMenu;

    private void Start()
    {
        DisplayMainMenu();
       // Time.timeScale = 0f;
    }

    void DisplayMainMenu()
    {
        gameHUD.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
        loseMenu.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        //Time.timeScale = 1f;
        mainMenu.gameObject.SetActive(false);
        gameHUD.gameObject.SetActive(true);
        Boat.instance.StartBoat();
        HUD_Manager.instance.StartTimer();
    }

    public void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        HUD_Manager.instance.PauseTimer(pause);
        pauseMenu.gameObject.SetActive(pause);
    }

    //event for mainMenu button in pause menu
    public void EndGame()
    {
        DisplayMainMenu();
        HUD_Manager.instance.Restart();
        Boat.instance.StartBoat(false);
        MapManager.instance.ResetMap();
        
    }


    public void DisplayLoseWindow()
    {
        loseMenu.gameObject.SetActive(true);
    }




    

    
}

