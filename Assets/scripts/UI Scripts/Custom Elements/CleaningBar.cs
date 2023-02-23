using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace HudElements
{
    public class CleaningBar : VisualElement, INotifyValueChanged<float>
    {
        public int width{ get; set;}

        public int height{ get; set;}

        public void SetValueWithoutNotify(float newValue){
            m_value = newValue;
        }

        private float m_value;

        public float value{
            
            get{
                m_value = Mathf.Clamp(m_value, 0, 1);
                return m_value;
            }

            set{
                if(EqualityComparer<float>.Default.Equals(m_value, value)){
                    return;
                }
                if(this.panel != null){
                    using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this.m_value, value)){
                        pooled.target = (IEventHandler) this;
                        this.SetValueWithoutNotify(value);
                        this.SendEvent((EventBase)pooled);
                    }
                }else{
                    SetValueWithoutNotify(value);
                }
            }

        }

        public enum FillType{
            Horizontal, 
            Vertical
        }

        public FillType fillType;

        private VisualElement cbParent;
        private VisualElement cbBackground;
        private VisualElement cbForeground;   

        public new class UxmlFactory: UxmlFactory<CleaningBar, UxmlTraits>{}

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription m_width = new UxmlIntAttributeDescription(){name = "width", defaultValue = 300};
            UxmlIntAttributeDescription m_height = new UxmlIntAttributeDescription(){name = "height", defaultValue = 50};
            UxmlFloatAttributeDescription m_value = new UxmlFloatAttributeDescription(){name = "value", defaultValue = 1};
            UxmlEnumAttributeDescription<CleaningBar.FillType> m_fillType = new UxmlEnumAttributeDescription<FillType>(){name = "fill-type", defaultValue = 0};

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate    = ve as CleaningBar;
                ate.width  = m_width.GetValueFromBag(bag, cc);
                ate.height = m_height.GetValueFromBag(bag, cc);
                ate.value = m_value.GetValueFromBag(bag, cc);
                ate.fillType = m_fillType.GetValueFromBag(bag, cc);

                ate.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UI Documents/CleaningBar");
                VisualElement cleaningbar = vt.Instantiate();
                ate.cbParent     = cleaningbar.Q<VisualElement>("CleaningBar");
                ate.cbBackground = cleaningbar.Q<VisualElement>("Background");
                ate.cbForeground = cleaningbar.Q<VisualElement>("Foreground");
                ate.Add(cleaningbar);
                
                ate.cbParent.style.width = ate.width;
                ate.cbParent.style.height = ate.height;
                ate.style.width  = ate.width;
                ate.style.height = ate.height;
                ate.FillCleaning();

            }
        }
        private void FillCleaning(){

            if(fillType == FillType.Horizontal){
                    cbForeground.style.scale = new Scale(new Vector3(value, 1, 0));
            }
            else{
                    cbForeground.style.scale = new Scale(new Vector3(1, value, 0));
            }
        }

    }
}


