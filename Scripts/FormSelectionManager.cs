
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FormSelectionManager : UdonSharpBehaviour
{
    public FormManager formManager;
    public GameObject[] TextQAs;
    public GameObject[] PicQAs;

    private GameObject[] forms; 
    private UdonBehaviour[] formsBehaviour;
    private TMP_Text[] TextQuestions;
    private TMP_Text[] PicQuestions;
    
    [UdonSynced] public int selectedFormIndex;
    void Start()
    {
        InitQuestionsTMP();
        InitForms();
        SetQAs(); // grab the syncing info
        // ChangePreview();
    }
    private void InitQuestionsTMP(){
        //initialize Questions
        TextQuestions = new TMP_Text[TextQAs.Length];
        for (int i=0;i<TextQAs.Length;i++){
            TextQuestions[i] = (TMP_Text)TextQAs[i].transform.GetComponentInChildren(typeof(TMP_Text));
        }
        PicQuestions = new TMP_Text[PicQAs.Length];
        for (int i=0;i<PicQAs.Length;i++){
            PicQuestions[i] = (TMP_Text)PicQAs[i].transform.GetComponentInChildren(typeof(TMP_Text));
        }
    }
    
    private void InitForms(){
        //get forms
        forms = formManager.forms;
        //initialize formsBehaviour
        formsBehaviour = new UdonBehaviour[forms.Length];
        for(int i = 0; i< forms.Length;i++){
            formsBehaviour[i] = (UdonBehaviour)forms[i].GetComponent(typeof(UdonBehaviour));
        }
    }

    public void GetFormIndex(){
        TMP_Dropdown targetDropdown =(TMP_Dropdown)formManager.GetProgramVariable("formsDropdown");
        selectedFormIndex = targetDropdown.value;
    }

    //visualize
    private void ResetQAs(){
        //disable all question and answers
        foreach(GameObject textQA in TextQAs){
            textQA.SetActive(false);
        }
        foreach(GameObject picQA in PicQAs){
            picQA.SetActive(false);
        }
    }
    
    //visualize
    private void SetQAs(){
        //get Text and Pic questions from specific form
        string[] textQuestionsInForm = (string[])formsBehaviour[selectedFormIndex].GetProgramVariable("TextQuestion");
        string[] picQuestionsInForm =  (string[])formsBehaviour[selectedFormIndex].GetProgramVariable("PicQuestion");
        for(int i=0;i<textQuestionsInForm.Length;i++){
            //turn on the QA
            TextQAs[i].SetActive(true);
            //set the Question according to the form info
            TextQuestions[i].text = textQuestionsInForm[i];
        }
        for(int i=0;i<picQuestionsInForm.Length;i++){
            //turn on the QA
            PicQAs[i].SetActive(true);
            //set the Question according to the form info
            PicQuestions[i].text = picQuestionsInForm[i];
        }
    }


    //Tigger here
    public void OnButtonClicked(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(SelectForm));
    }
    //Main Layer
    public void SelectForm(){
        GetFormIndex();//calculate
        RequestSerialization(); //serialize
        SendCustomEvent(nameof(applyFormSeletion));//visualize
    }
    //Sync Visual layer
    public override void OnDeserialization(){
        SendCustomEvent(nameof(applyFormSeletion));
    }
    //Visual Layer
    public void applyFormSeletion(){
        ResetQAs();
        SetQAs();
    }

}
