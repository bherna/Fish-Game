using UnityEngine;

public class DropSpin : MonoBehaviour
{
    enum SpinType {Spin, Tilt, AllSpin};
    [SerializeField] SpinType spinType;
    [SerializeField] int spinSpeed;
    [SerializeField] GameObject dropSprite;


    //need to keep a reference of the x rotation, just incase its not 0
    private int xBase; //should be a whole number angle

    //spin angle (y)
    private float spinAngle = 0;


    //tilt type spin currentl angle
    private float tiltAngle = 0; //0 == straight up
    //what are currentl angle we are heading towards is - or + tilt
    [SerializeField] float tiltMax = 45; 
    private bool isPositive = true;
    [SerializeField] int tiltSpeed;
    private float timer; //out of a 100 


    //
    private void Start() {
        //save the whole number part, who cares if its 1 degree off
        xBase = (int)dropSprite.transform.rotation.eulerAngles.x;

    }

    // Update is called once per frame
    void Update()
    {
        switch(spinType){

            //mostly used by coins
            case SpinType.Spin:
                //simple rotate on y axis animation
                dropSprite.transform.Rotate(0, Time.deltaTime*spinSpeed,  0, Space.Self);
                break;

            //mostly used my diamond/ gems
            case SpinType.Tilt:
                //this one is a bit more complicated, now we have a loop
                //we tilt our obj on the x axis either - or +, given a max tilt
                //if we reach max, we start heading to the other side
                //while doing this, we also do a continous y spin rotation, like simple spin
                GetTilt();
                GetSpin();
                dropSprite.transform.rotation = Quaternion.Euler(xBase + tiltAngle, spinAngle, 0);
                break;

            //used for the foods
            case SpinType.AllSpin:
                var xyz = Time.deltaTime * spinSpeed;
                dropSprite.transform.Rotate(xyz, xyz, xyz, Space.Self);
                break;

        }
    }


    //part of the tilt logic
    //just for cleaner code, we move the x tilt down here
    private void GetTilt(){


        //first get our new tilt angle
        timer += Time.deltaTime * tiltSpeed / 100; //positve
        
        //if we are heading towards the positive x tilt
        if(isPositive){
            tiltAngle = Mathf.Lerp(-tiltMax, tiltMax, timer);
        }
        //else negative x tilt
        else{
            tiltAngle = Mathf.Lerp(tiltMax, -tiltMax, timer);
        }

        //
        //do we switch directions?
        if(timer >= 1){
            Debug.Log("switch");
            isPositive ^= true; //invert bool// switch direction
            timer = 0;          //reset timer
        }
    }

    //get the new current y spin angle
    private void GetSpin(){
        spinAngle += Time.deltaTime*spinSpeed;
    }
}
