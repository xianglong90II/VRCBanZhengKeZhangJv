
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ChangeBGColor : UdonSharpBehaviour
{
    private Color[] ColorList = new Color[5];
    private Material currentMat;
    [SerializeField,UdonSynced] int _ColorIndex = 0;
    void Start()
    {
        Renderer currentRenderer = (Renderer)this.GetComponent(typeof(Renderer));
        currentMat = currentRenderer.material;
        ColorList[0] = Color.red;
        ColorList[1] = Color.green;
        ColorList[2] = Color.blue;
        ColorList[3] = Color.white;
        ColorList[4] = Color.black;
    }
    public void NetChangeColor(){
        Networking.SetOwner(Networking.LocalPlayer,this.gameObject);
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner,nameof(ChangeColor));
    }
    public void ChangeColor(){
        //calculate
        if (_ColorIndex<4){
            _ColorIndex++;
        }else{
            _ColorIndex = 0;
        }
        //serialize
        RequestSerialization();
        //visuallize
        currentMat.color = ColorList[_ColorIndex];
    }

    public override void OnDeserialization(){
        currentMat.color = ColorList[_ColorIndex];
    }

}
