
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventOnHover_PlayButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //light intensity
    [SerializeField] float minLight = 0.27f;
    [SerializeField] float maxLight = 1.27f;
    [SerializeField] Light tankLight;
    [SerializeField] float ligthChangeSpeed = 1;

    private bool isHover = false;
    private float lightIntensity = 0;




    private void Awake() {
        tankLight.intensity = minLight;
    }
    private void Update() {

        if(isHover && lightIntensity + Time.deltaTime*ligthChangeSpeed <= maxLight){
            //head towards max lighting
            lightIntensity += Time.deltaTime *ligthChangeSpeed;

            tankLight.intensity = lightIntensity;
        }
        else if(!isHover && lightIntensity - Time.deltaTime*ligthChangeSpeed >= minLight){
            //head towards minimum lighitng
            lightIntensity -= Time.deltaTime *ligthChangeSpeed;

            tankLight.intensity = lightIntensity;
        }
        //else nothing
    }


    public void OnPointerEnter(PointerEventData eventData){
        
        isHover = true;
    }

    public void OnPointerExit(PointerEventData eventData){
        isHover = false;
    }
}
