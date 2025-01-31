using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;


public class FormManager : UdonSharpBehaviour
{
    public GameObject[] forms; 
    public TMP_Dropdown formsDropdown;

    void Start()
    {
        string[] formsNames = new string[forms.Length];
        //get form names
        for (int i = 0; i < forms.Length; i++)
        {
            UdonBehaviour formInfo = (UdonBehaviour)forms[i].GetComponent(typeof (UdonBehaviour));
            formsNames[i] = (string)formInfo.GetProgramVariable("FormName");
        }
        //add form names to dropdown
        formsDropdown.AddOptions(formsNames);
    }
    void Update(){

    }
}
