using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class WinPresenter : MonoBehaviour
{
    private void Awake() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        foreach (GameObject gameManager in GameObject.FindGameObjectsWithTag("GameManager"))
            Destroy(gameManager);
        root.Q<Button>("BackButton").clicked += () => SceneManager.LoadScene("Title_Scene");
        // root.Q<Button>("Start").clicked += () => Debug.Log("test");
    }
}
