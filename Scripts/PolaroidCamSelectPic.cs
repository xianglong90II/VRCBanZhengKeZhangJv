
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PolaroidCamSelectPic : UdonSharpBehaviour
{
    public TMP_Dropdown formPicsDropDown;
    public TMP_Text logText;
    public string selectionLogText = "Picture Index Is: ";
    [UdonSynced]public int selectedDropdownValue = 0;
    void Start()
    {
        logText.text = selectionLogText + selectedDropdownValue.ToString();
    }
    public void NetApplySelection(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        selectedDropdownValue = formPicsDropDown.value;
        ApplySelection();
        RequestSerialization();
        // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(ApplySelection));
    }
    public override void OnDeserialization(){
        ApplySelection();
    }
    private void ApplySelection(){
        logText.text = selectionLogText + selectedDropdownValue.ToString();
    }
}
