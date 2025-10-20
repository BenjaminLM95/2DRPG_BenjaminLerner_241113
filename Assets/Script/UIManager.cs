using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject gameplayUI; 


    public void GamePlayEnable() 
    {
        DisableUI();
        gameplayUI.SetActive(true); 
    }

    public void MainMenuEnable() 
    {
        DisableUI();
        mainMenuUI.gameObject.SetActive(true);
    }

    public void DisableUI() 
    {
        mainMenuUI.SetActive(false);
        gameplayUI.SetActive(false);
    }

}
