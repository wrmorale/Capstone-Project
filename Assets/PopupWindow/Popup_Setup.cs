using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using HudElements;

namespace PopupTest
{
    public class Popup_Setup : MonoBehaviour
    {
        public GameObject pauseUI;
        // UIDocument ui;
        // VisualElement root;
        // PopupWindow popup;
        void OnEnable() {
            UIDocument ui = GetComponent<UIDocument>();
            VisualElement root = ui.rootVisualElement;

            PopupWindow popup = new PopupWindow();
            root.Add(popup);

            popup.confirmed += () => Debug.Log("Confirmed");

            popup.cancelled += () => Debug.Log("Cancelled");
            popup.cancelled += () => Time.timeScale = 1;
            popup.cancelled += () => root.Remove(popup);
            popup.cancelled += () => pauseUI.SetActive(false);
        }

        // UIDocument ui = GetComponent<UIDocument>();
        // VisualElement root = ui.rootVisualElement;

        // PopupWindow popup = new PopupWindow();
        
        // void Update()
        //     {
        
        //     if (Time.timeScale == 0.001f){
        //         // Time.timeScale = 1;
        //         UIDocument ui = GetComponent<UIDocument>();
        //         VisualElement root = ui.rootVisualElement;

        //         PopupWindow popup = new PopupWindow();
        //         root.Add(popup);

        //         popup.confirmed += () => Debug.Log("Confirmed");

        //         popup.cancelled += () => Debug.Log("Cancelled");
        //         popup.cancelled += () => Time.timeScale = 1;
        //         popup.cancelled += () => root.Remove(popup);
        //     }
            // if (popup){
            //     popup.confirmed += () => Debug.Log("Confirmed");

            //     popup.cancelled += () => Debug.Log("Cancelled");
            //     popup.cancelled += () => Time.timeScale = 1;
            //     popup.cancelled += () => root.Remove(popup);
            // }
            // popup.confirmed += () => Debug.Log("Confirmed");

            // popup.cancelled += () => Debug.Log("Cancelled");
            // popup.cancelled += () => Time.timeScale = 1;
            // popup.cancelled += () => root.Remove(popup);
        // }
        // private void OnEnable() {
        //     UIDocument ui = GetComponent<UIDocument>();
        //     VisualElement root = ui.rootVisualElement;

        //     PopupWindow popup = new PopupWindow();
        //     root.Add(popup);

        //     popup.confirmed += () => Debug.Log("Confirmed");

        //     popup.cancelled += () => Debug.Log("Cancelled");
        //     popup.cancelled += () => root.Remove(popup);


        // }
    }
}

