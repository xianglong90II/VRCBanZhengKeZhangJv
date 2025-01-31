
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using System;

public class PolaroidCam : UdonSharpBehaviour
{
    private GameObject[] forms; 
    private UdonBehaviour[] formsBehaviour;

    public FormManager formManager;
    public TMP_Dropdown formPicsDropdown;

    public Material camPreviewMaterial;
    public GameObject camObject;

    private GameObject[] targetPictureObjects;

    public Camera myCamera;

    private RenderTexture[] renderTextures;

    public PolaroidCamSelectPic polaroidCamSelector;

    [UdonSynced,SerializeField] private bool _CamActive;
    void Start()
    {
        initPolaroidCam();
        myCamera.enabled=true;
        myCamera.targetTexture = renderTextures[polaroidCamSelector.selectedDropdownValue];
        SetCamPreview();
    }
    private void initPolaroidCam(){
        //clear options first
        formPicsDropdown.ClearOptions();
        //initialize forms and behaviours
        forms = formManager.forms;
        formsBehaviour = new UdonBehaviour[forms.Length];
        for(int i = 0; i< forms.Length;i++){
            formsBehaviour[i] = (UdonBehaviour)forms[i].GetComponent(typeof(UdonBehaviour));
        }
        
        //get options for dropdown
        // get pic names length
        int formPicsNamesLength = 0;
        for (int i=0; i< forms.Length;i++){
            GameObject[] tempFormPics = (GameObject[])formsBehaviour[i].GetProgramVariable("FormPics");
            for (int j=0; j<tempFormPics.Length;j++){
                formPicsNamesLength ++;
            }
        }
        //get full options and set target picture objects
        targetPictureObjects = new GameObject[formPicsNamesLength];
        string[] formPicsNames = new string[formPicsNamesLength];
        int tempFormPicsNamesLength = 0;
        for (int i=0; i< forms.Length;i++){
            GameObject[] tempFormPics = (GameObject[])formsBehaviour[i].GetProgramVariable("FormPics");
            for (int j=0; j<tempFormPics.Length;j++){
                // add to target Picture Objects
                targetPictureObjects[tempFormPicsNamesLength]=tempFormPics[j];
                Debug.Log(tempFormPics[j].name);
                // get pic names and set together
                formPicsNames[tempFormPicsNamesLength]= tempFormPics[j].name;
                tempFormPicsNamesLength++;
            }
        }
        
        formPicsDropdown.AddOptions(formPicsNames);

        //create Render Textures
        renderTextures = new RenderTexture[formPicsNames.Length];
        for(int i=0; i<renderTextures.Length; i++){
            renderTextures[i] = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
            renderTextures[i].Create();
        }
    }

    public void TakePicture(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        if(_CamActive){
            _CamActive = false;
            TakePictureLocal();
            RequestSerialization();
            // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(TakePictureLocal));
        }else{
            _CamActive = true;
            TurnOnCamera();
            RequestSerialization();
            // SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(TurnOnCamera));
        }
    }
    void Update(){
        if(_CamActive){
            camObject.SetActive(true);
        }else{
            //set camera off
            camObject.SetActive(false);
        }
    }
    public override void OnDeserialization()
    {
        if(_CamActive){
            TakePictureLocal();
        }else{
            TurnOnCamera();
        }
    }
    public void TakePictureLocal(){
        if (targetPictureObjects.Length != 0){
            SetCamPreview();
            Debug.Log("Triggered");
            int selectedDropdownValue = polaroidCamSelector.selectedDropdownValue;
            //get target renderer from target picture
            Renderer targetRenderer = (Renderer)targetPictureObjects[selectedDropdownValue].GetComponent(typeof(Renderer));
            //get target render texure
            myCamera.targetTexture = renderTextures[selectedDropdownValue];
            targetRenderer.material.SetTexture("_MainTex",myCamera.activeTexture,UnityEngine.Rendering.RenderTextureSubElement.Color);
            // _CamActive=false;
        }
    }
    public void TurnOnCamera(){
        SetCamPreview();
        myCamera.targetTexture = renderTextures[polaroidCamSelector.selectedDropdownValue];
        // _CamActive=true;
    }

    public void NetClearCamPictures(){
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,nameof(ClearCamPictures));
    }
    private void ClearCamPictures(){
        foreach (RenderTexture crt in renderTextures){
            crt.Release();
        }
    }
    public void NetSetCamPreview(){
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All,nameof(SetCamPreview));
    }
    private void SetCamPreview(){
        camPreviewMaterial.SetTexture("_MainTex",myCamera.activeTexture,UnityEngine.Rendering.RenderTextureSubElement.Color);
    }
}
