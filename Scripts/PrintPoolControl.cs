
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class PrintPoolControl : UdonSharpBehaviour
{
    public FormManager formManager;
    public TMP_Dropdown targetDropdown;
    public VRCObjectPool[] formPools;
    public TMP_Text selectionLog;
    public string selectionLogText = "Selected Form Index: ";
    [UdonSynced] public int selectedFormIndex;
    void Start()
    {
        //initialize dropdown
        string[] formNames = new string[formManager.forms.Length];
        for(int i =0 ;i<formNames.Length;i++){
            formNames[i] = formManager.forms[i].name;
        }
        targetDropdown.AddOptions(formNames);
        //get selectedFormIndex to sync selection
        targetDropdown.value = selectedFormIndex;
    }

    void Update(){
        selectionLog.text = selectionLogText+selectedFormIndex.ToString();
    }

    public void OnChangeDropdownValue(){
            Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
            selectedFormIndex = targetDropdown.value;
            RequestSerialization();
        }
    public override void OnDeserialization()
    {
        targetDropdown.value = selectedFormIndex;
    }

    public void ResetTargetPool(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        Networking.SetOwner(Networking.LocalPlayer,formPools[selectedFormIndex].gameObject);

        GameObject[] poolObjects = formPools[selectedFormIndex].Pool;
        foreach(GameObject poolObject in poolObjects){
            formPools[selectedFormIndex].Return(poolObject);
        }
    }
    public void PrintFromTargetForm(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        Networking.SetOwner(Networking.LocalPlayer,formPools[selectedFormIndex].gameObject);
        
        GameObject poolObject = formPools[selectedFormIndex].TryToSpawn();
        if(poolObject!=null){            
            poolObject.SetActive(true);
        }
    }

}
