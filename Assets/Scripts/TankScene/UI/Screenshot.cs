
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    private void Update(){
        
        if(Input.GetKeyUp(KeyCode.P)){ // capture screen shot on 'P'

            string folderPath = System.IO.Directory.GetCurrentDirectory()+ "/" + "Assets/Screenshots/"; // the path of your project folder

            if (!System.IO.Directory.Exists(folderPath)) // if this path does not exist yet
                System.IO.Directory.CreateDirectory(folderPath);  // it will get created
            
            var screenshotName =
                                    "Screenshot_" +
                                    System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + // puts the current time right into the screenshot name
                                    ".png"; // put youre favorite data format here
            ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName),2); // takes the sceenshot, the "2" is for the scaled resolution, you can put this to 600 but it will take really long to scale the image up
            Debug.Log(folderPath + screenshotName); // You get instant feedback in the console
        }
    }
}
