
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ChangeTogetherWith : UdonSharpBehaviour
{   
    public GameObject referenceObject;
    public bool isPhoto;
    private TMP_Text refTextObj;
    private Renderer refPhotoObj;
    //temp var
    private TMP_Text tempTextObj;
    void Start()
    {
        if(isPhoto==false){
            refTextObj = (TMP_Text)referenceObject.GetComponent(typeof(TMP_Text));
        }else{
            refPhotoObj = (Renderer)referenceObject.GetComponent(typeof(Renderer));
        }
        SyncContent();
    }
    public void NetSyncContent(){
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,nameof(SyncContent));
    }
    public void SyncContent()
    {
        //if my object is text.
        if(isPhoto==false){
            TMP_Text thisText = (TMP_Text)this.gameObject.GetComponent(typeof(TMP_Text));
            thisText.text = refTextObj.text;
        }else{
            Renderer thisRenderer = (Renderer)this.gameObject.GetComponent(typeof(Renderer));
            thisRenderer.material = refPhotoObj.material;
        }
    }
}
