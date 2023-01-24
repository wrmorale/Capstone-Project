using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISceneChanger : MonoBehaviour
{
    public void LoadScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    public void EndGame(){
        Application.Quit();
        Debug.Log("Game over man, game over");
    }
}
