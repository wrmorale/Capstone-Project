using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour{
    
    SkinnedMeshRenderer mesh;
    Color origColor;
    float flashTime = 1f;

    void Start(){
        mesh = GetComponent<SkinnedMeshRenderer>();
        origColor = mesh.material.GetColor("_MainTex_ST");
    }


    public void FlashStart(){
        mesh.material.SetColor("_MainTex_ST", Color.red);
        Invoke("FlashStop", flashTime);
    }

    public void FlashStop(){
        mesh.material.SetColor("_MainTex_ST", origColor);
    }
}