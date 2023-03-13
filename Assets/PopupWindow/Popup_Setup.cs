
using UnityEngine.UIElements;
using UnityEngine;

namespace PopupTest
{
    public class Popup_Setup : MonoBehaviour
    {
        private void OnEnable() {
            UIDocument ui = GetComponent<UIDocument>();
            VisualElement root = ui.rootVisualElement;

            PopupWindow popup = new PopupWindow();
            root.Add(popup);

            popup.confirmed += () => Debug.Log("Confirmed");

            popup.cancelled += () => Debug.Log("Cancelled");
            popup.cancelled += () => root.Remove(popup);


        }
    }
}

