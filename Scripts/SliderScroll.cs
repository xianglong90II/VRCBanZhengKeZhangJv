
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SliderScroll : UdonSharpBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Slider inverseSlider;
    //slider value should be 0-1
    [SerializeField, UdonSynced] private float sliderValue;

    void Start()
    {
    }
    public void OnValueChanged(){
        sliderValue = slider.value;
        //set inverseSlider
        inverseSlider.value = sliderValue;
        if(slider.value != sliderValue){
            Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
            RequestSerialization();
        }
    }
    public override void OnDeserialization()
    {
        slider.value = sliderValue;
        //set inverseSlider
        inverseSlider.value = sliderValue;
    }
}
