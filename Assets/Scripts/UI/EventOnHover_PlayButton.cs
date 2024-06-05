
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventOnHover_PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //light intensity
    [SerializeField] float minLight = 0.27f;
    [SerializeField] float maxLight = 1.27f;
    [SerializeField] Light tankLight;
    [SerializeField] float ligthChangeSpeed = 1;

    private bool isOn = false;
    private bool weClicked = false;
    private float lightIntensity = 0;




    private void Awake() {
        tankLight.intensity = minLight;
    }
    private void Update() {

        if((isOn || weClicked) && lightIntensity + Time.deltaTime*ligthChangeSpeed <= maxLight){
            //head towards max lighting
            lightIntensity += Time.deltaTime *ligthChangeSpeed;

            tankLight.intensity = lightIntensity;
        }
        else if(!isOn && lightIntensity - Time.deltaTime*ligthChangeSpeed >= minLight){
            //head towards minimum lighitng
            lightIntensity -= Time.deltaTime *ligthChangeSpeed;

            tankLight.intensity = lightIntensity;
        }
        //else nothing
    }


    public void OnPointerEnter(PointerEventData eventData){
        
        isOn = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        isOn = false;
    }

    public void OnPointerClick(PointerEventData eventData){

        //we clicked to next screen so keep the lights on
        weClicked = true;

    }

    public void OnPointerReturnToTitleScreen(){
        weClicked = false;
    }
}
