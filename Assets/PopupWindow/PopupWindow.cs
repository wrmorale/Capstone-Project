using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace PopupTest
{
    public class PopupWindow : VisualElement
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<PopupWindow> { }

        private const string styleResource = "PopupWindowStyleSheet";
        private const string ussPopup = "popup_window";
        private const string ussPopupContainer = "popup_container";
        private const string ussHorContainer = "horizontal_container";
        private const string ussPopupMsg = "popup_msg";
        private const string ussPopupButton = "popup_button";
        private const string ussCancel = "button_cancel";
        private const string ussConfirm = "button_confirm";

        public PopupWindow()
        {
            styleSheets.Add(Resources.Load<StyleSheet>(styleResource));
            AddToClassList(ussPopupContainer);

            VisualElement window = new VisualElement();
            window.AddToClassList(ussPopup);
            hierarchy.Add(window);

            //Text section
            VisualElement horizontalContainerText = new VisualElement();
            horizontalContainerText.AddToClassList(ussHorContainer);
            window.Add(horizontalContainerText);

            Label msgLabel = new Label();
            msgLabel.text = "PAUSED";
            msgLabel.AddToClassList(ussPopupMsg);
            horizontalContainerText.Add(msgLabel);

            // Button Section
            VisualElement horizontalContainerButton = new VisualElement();
            horizontalContainerButton.AddToClassList(ussHorContainer);
            window.Add(horizontalContainerButton);

            Button confirmButton = new Button() { text = "CONFIRM"};
            confirmButton.AddToClassList(ussPopupButton);
            confirmButton.AddToClassList(ussConfirm);
            horizontalContainerButton.Add(confirmButton);
            
            Button cancelButton = new Button() { text = "CANCEL"};
            cancelButton.AddToClassList(ussPopupButton);
            cancelButton.AddToClassList(ussCancel);
            horizontalContainerButton.Add(cancelButton);
            
            confirmButton.clicked += OnConfirm;
            cancelButton.clicked += OnCancel;
        }

        public event Action confirmed;
        public event Action cancelled;

        private void OnConfirm() {
            confirmed?.Invoke();
        }

        private void OnCancel() {
            cancelled?.Invoke();
        }
    }
}

