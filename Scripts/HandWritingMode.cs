
using BestHTTP.SecureProtocol.Org.BouncyCastle.Asn1.Ocsp;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HandWritingMode : UdonSharpBehaviour
{
    public Camera targetCamera;
    public string buttonOnText = "On";
    public string buttonOffText = "Off";
    public TMP_Text handWritingButtonLabel;

    [UdonSynced]
    bool isEnabled;

    private void Start()
    {
        isEnabled = false;
        handWritingButtonLabel.text = buttonOffText;
    }

    public override void OnDeserialization()
    {
        if (!Networking.IsOwner(gameObject)){
                //set cam and text
            if(isEnabled==false){
                //turn off handwriting mode
                isEnabled = false;
                handWritingButtonLabel.text = buttonOffText;
                //change target camera culling
                targetCamera.cullingMask = 4615991;
            }else{
                //turn on handwriting mode
                isEnabled = true;
                handWritingButtonLabel.text = buttonOnText;
                //change target camera culling to PickUp Only
                targetCamera.cullingMask = 24576;
            }
        }
    }

    public void ButtonTrigger()
    {
        if (!Networking.IsOwner(gameObject)){
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
        isEnabled = !isEnabled;
        //set cam and text
        if(isEnabled==false){
            //turn off handwriting mode
            isEnabled = false;
            handWritingButtonLabel.text = buttonOffText;
            //change target camera culling
            targetCamera.cullingMask = 4615991;
        }else{
            //turn on handwriting mode
            isEnabled = true;
            handWritingButtonLabel.text = buttonOnText;
            //change target camera culling to PickUp Only
            targetCamera.cullingMask = 24576;
        }
        RequestSerialization();
        
    }
}
