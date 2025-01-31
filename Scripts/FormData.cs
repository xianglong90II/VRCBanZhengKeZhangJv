
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FormData : UdonSharpBehaviour
{
    public string FormName;
    // public Texture2D PreviewImage;
    public string[] TextQuestion;
    public string[] PicQuestion;
    public GameObject FormObject;
    public TMP_Text[] FormTexts;
    public GameObject[] FormPics;
    public ChangeTogetherWith[] FormChangeTogetherObjets;

    void Start()
    {
        
    }
}
