
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDK3.Image;
using VRC.SDKBase;
using VRC.Udon;

public class FormApplicationDeliver : UdonSharpBehaviour
{
    public UdonBehaviour formSelectionManagerBehaviour;
    public FormManager formManager;
    public TextureInfo textureInfo;
    private TMP_InputField[] TextAnswers;
    private VRCUrlInputField[] PicAnswers;
    private Texture2D[] photoTextures = new Texture2D[8];
    //Pool for sync and other usage
    [UdonSynced] public string[] textAnswersData = new string[32];
    [UdonSynced] public VRCUrl[] picAnswersData = new VRCUrl[8];
    void Start()
    {
        InitAnswersTMP();
    }
    public void ResetEntry(){
        foreach(TMP_InputField textAnswer in TextAnswers){
            textAnswer.text = "";
        }
        foreach(VRCUrlInputField picAnswer in PicAnswers){
            picAnswer.SetUrl(VRCUrl.Empty);
        }
    }
    private void InitAnswersTMP(){
        GameObject[] TextQAs = (GameObject[])formSelectionManagerBehaviour.GetProgramVariable("TextQAs");
        GameObject[] PicQAs = (GameObject[])formSelectionManagerBehaviour.GetProgramVariable("PicQAs");
        //initialize Questions
        TextAnswers = new TMP_InputField[TextQAs.Length];
        for (int i=0;i<TextQAs.Length;i++){
            TextAnswers[i] = (TMP_InputField)TextQAs[i].transform.GetComponentInChildren(typeof(TMP_InputField));
        }
        PicAnswers = new VRCUrlInputField[PicQAs.Length];
        for (int i=0;i<PicQAs.Length;i++){
            PicAnswers[i] = (VRCUrlInputField)PicQAs[i].transform.GetComponentInChildren(typeof(VRCUrlInputField));
        }
    }
    // fill the datapool
    private void SetAnswerData(){
        for(int i = 0; i<TextAnswers.Length;i++){
            textAnswersData[i] = TextAnswers[i].text;
        }
        for(int i = 0; i<PicAnswers.Length;i++){
            picAnswersData[i] = PicAnswers[i].GetUrl();
        }
    }
    //set the inputs (Visual)
    private void SetFromAnswerData(){
        for(int i = 0; i<TextAnswers.Length;i++){
        TextAnswers[i].text=textAnswersData[i];
        }
        for(int i = 0; i<PicAnswers.Length;i++){
            PicAnswers[i].SetUrl(picAnswersData[i]);
        }
    }

    //set target form (Visual)
    private void ApplyToForm(){
        //Get target form
        GameObject targetForm =formManager.forms[(int)formSelectionManagerBehaviour.GetProgramVariable("selectedFormIndex")];
        UdonBehaviour targetFormBehaviour = (UdonBehaviour)targetForm.GetComponent(typeof(UdonBehaviour));
        //Get target texts and pics form target form
        TMP_Text[] targetTexts = (TMP_Text[])targetFormBehaviour.GetProgramVariable("FormTexts");
        GameObject[] targetPics = (GameObject[])targetFormBehaviour.GetProgramVariable("FormPics");
        //Get changeTogether objects from target form
        ChangeTogetherWith[] changeTogetherWiths = (ChangeTogetherWith[])targetFormBehaviour.GetProgramVariable("FormChangeTogetherObjets");

        //Apply the text and pics
        //texts
        for (int i = 0; i< targetTexts.Length;i++){
            targetTexts[i].text = textAnswersData[i];
        }
        
        //pics
        IVRCImageDownload[] task = new IVRCImageDownload[targetPics.Length];
        VRCImageDownloader[] picDownloaders = new VRCImageDownloader[targetPics.Length];
        UdonBehaviour targetUdon = this.GetComponent<UdonBehaviour>();
        // Texture2D[] downloadedPics = new Texture2D[targetPics.Length];
        for (int i = 0; i< targetPics.Length;i++){
            //clear cache
            // picDownloaders[i].Dispose();
            //download pics
            if(VRCUrl.IsNullOrEmpty(picAnswersData[i])){
                Debug.Log("Empty Image");
            }else{
                picDownloaders[i]=new VRCImageDownloader();
                Debug.Log(task[i]);
                Debug.Log(picDownloaders[i]);
                Debug.Log(picAnswersData[i]);
                Debug.Log(targetPics[i]);
                Debug.Log("success");
                task[i] = picDownloaders[i].DownloadImage(picAnswersData[i],targetPics[i].GetComponent<Renderer>().material,targetUdon,textureInfo);
            }
            // targetPics[i].GetComponent<Renderer>().material.SetTexture();
        }
        for (int i = 0; i<changeTogetherWiths.Length;i++){
            changeTogetherWiths[i].SendCustomEvent("SyncContent");
        }
    }

    // The Button Trigger is here
    // Final Application will be done by referencing AnswersData instead of referencing Inputfields
    public void ApplyLicense(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(GrabAndApply));
    }
    //Main Layer
    public void GrabAndApply(){
        SetAnswerData();//Calculate Layer
        RequestSerialization();//serialize
        SendCustomEvent(nameof(VisuallyApply));//Visual Layer
    }
    //Sync Visual layer
    public override void OnDeserialization(){
        SendCustomEvent(nameof(VisuallyApply));
    }
    //Visual layer
    public void VisuallyApply(){
        //grab the data from pool
        SetFromAnswerData(); //set the inputs
        ApplyToForm(); // set the form
    }

}
