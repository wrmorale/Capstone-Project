using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HudElements
{
    public class PhoneBar : VisualElement
    {
        public int width{get; set;}
        public int height{get; set;}

        private VisualElement pbParent;
        private VisualElement pbBackground;
        private VisualElement pbForeground;

        public new class UxmlFactory: UxmlFactory<PhoneBar, UxmlTraits>{}

        public new class UxmlTraits : VisualElement.UxmlTraits{

            UxmlIntAttributeDescription m_width = new UxmlIntAttributeDescription(){name = "width", defaultValue = 300};
            UxmlIntAttributeDescription m_height = new UxmlIntAttributeDescription(){name = "height", defaultValue = 50};

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc){
                base.Init(ve, bag, cc);
                var ate = ve as PhoneBar;
                ate.width =  m_width.GetValueFromBag(bag, cc);
                ate.height = m_height.GetValueFromBag(bag, cc);

                ate.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UI Documents/Phone");
                VisualElement phonebar = vt.Instantiate();
                ate.pbParent = phonebar.Q<VisualElement>("Phone");
                ate.pbBackground = phonebar.Q<VisualElement>("Background");
                ate.pbForeground = phonebar.Q<VisualElement>("Foreground");
                ate.Add(phonebar);

                ate.pbParent.style.width = ate.width;
                ate.pbParent.style.height = ate.height;
                ate.style.width = ate.width;
                ate.style.height = ate.height;


            }

        }

    }
}

