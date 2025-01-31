
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PolaroidCamControl : UdonSharpBehaviour
{
    public PolaroidCam polaroidCam;
    void Start()
    {
        
    }
    public override void OnPickupUseDown(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        polaroidCam.SendCustomEvent("TakePicture");
    }
}
