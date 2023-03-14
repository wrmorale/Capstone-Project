using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour {

    
    [SerializeField]
    private Image healthbar;

    public void SetMaxHealth(float health){
       healthbar.fillAmount = health;
    }

    public void SetHealth(float health){
        healthbar.fillAmount = health;
    }
}