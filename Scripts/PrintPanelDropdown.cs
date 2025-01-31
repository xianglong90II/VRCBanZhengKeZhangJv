
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PrintPanelDropdown : UdonSharpBehaviour
{
    private TMP_Dropdown _targetDropdown;
    public PrintPoolControl printPoolControl;
    void Start(){
        _targetDropdown = (TMP_Dropdown)this.gameObject.GetComponent(typeof(TMP_Dropdown));
    }
    public void OnChangeDropdownValue(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        printPoolControl.selectedFormIndex = _targetDropdown.value;
        RequestSerialization();
    }
    public override void OnDeserialization()
    {
        _targetDropdown.value = printPoolControl.selectedFormIndex;
    }
}
