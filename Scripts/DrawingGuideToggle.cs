
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class DrawingGuideToggle : UdonSharpBehaviour
{
    public GameObject toggleObject;
    public string buttonOnText = "On";
    public string buttonOffText = "Off";
    public TMP_Text drawingGuideToggleButtonLabel;

    [UdonSynced]
    bool isEnabled;

    private void Start()
    {
        isEnabled = toggleObject.activeSelf;
        if(isEnabled){
                drawingGuideToggleButtonLabel.text = buttonOnText;
            }else{
                drawingGuideToggleButtonLabel.text = buttonOffText;
            }
    }

    public override void OnDeserialization()
    {
        if (!Networking.IsOwner(gameObject)){
            toggleObject.SetActive(isEnabled);
            if(isEnabled){
                drawingGuideToggleButtonLabel.text = buttonOnText;
            }else{
                drawingGuideToggleButtonLabel.text = buttonOffText;
            }
        }
    }

    public void ButtonTrigger()
    {
        if (!Networking.IsOwner(gameObject)){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        isEnabled = !isEnabled;
        Debug.Log(isEnabled);
        toggleObject.SetActive(isEnabled);
        if(isEnabled){
            drawingGuideToggleButtonLabel.text = buttonOnText;
        }else{
            drawingGuideToggleButtonLabel.text = buttonOffText;
        }
        RequestSerialization();
    }    
}
