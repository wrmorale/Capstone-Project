using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MenuPresenter : MonoBehaviour
{
    private void Awake() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        root.Q<Button>("Start").clicked += () => SceneManager.LoadScene("SampleScene");
        root.Q<Button>("ControlButton").clicked += () => SceneManager.LoadScene("Controls_Scene");
        root.Q<Button>("CreditButton").clicked += () => SceneManager.LoadScene("Credits_Scene");
        root.Q<Button>("QuitButton").clicked += () =>  Application.Quit();
        // root.Q<Button>("Start").clicked += () => Debug.Log("test");
    }
}
