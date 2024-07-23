using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public static bool isSinglePlayer = false;
    public void SinglePlayer()
    {
        isSinglePlayer = true;
        SceneManager.LoadScene("GamePlay");
    }
    public void LocalMultiplayer()
    {
        isSinglePlayer = false;
        SceneManager.LoadScene("GamePlay");
    }

}
