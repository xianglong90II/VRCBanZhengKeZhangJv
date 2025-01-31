
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SyncInput : UdonSharpBehaviour
{
    private TMP_InputField targetInputField;
    [UdonSynced,SerializeField]private string _syncingText = "";
    void Start(){
        targetInputField = (TMP_InputField)this.gameObject.GetComponent(typeof(TMP_InputField));
    }
    public void OnTriggered(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        //set localtext to syncing text
        _syncingText = targetInputField.text;
        RequestSerialization();
    }
    public override void OnDeserialization(){
        targetInputField.text = _syncingText;
    }
}
