
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PoolFormObject : UdonSharpBehaviour
{
    public TMP_Text[] copyTextsFrom;
    public Renderer[] copyPicsFrom;
    public TMP_Text[] targetTexts;
    public Renderer[] targetPics;

    void OnEnable(){
        LocalApplyToPoolObject();
    }

    private void LocalApplyToPoolObject(){
        Debug.Log("Triggered");
        for(int i=0; i< copyTextsFrom.Length; i++){
            targetTexts[i].text = copyTextsFrom[i].text;
        }
        for(int i=0; i< copyPicsFrom.Length; i++){
            Texture cpTex = copyPicsFrom[i].material.mainTexture;
            if(cpTex!=null){
                RenderTexture renderTexture = new RenderTexture(cpTex.width, cpTex.height, 32);
                VRCGraphics.Blit(cpTex, renderTexture);
                targetPics[i].material.mainTexture=renderTexture;
            }
        }
    }
}
