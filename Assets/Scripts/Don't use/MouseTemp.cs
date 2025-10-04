using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseTemp : MonoBehaviour
{

    //[SerializeField] GraphicRaycaster m_Raycaster;
    public int cursorSpeed_curr = 2500;

    //private int width;
    //private int height;


    // Start is called before the first frame update
    void Start()
    {
        //m_Raycaster = GetComponent<GraphicRaycaster>();


        //Cursor.visible = false; // Hide the system cursor
        Cursor.lockState = CursorLockMode.Confined; // Confine cursor to game window

        //width = (int)transform.parent.GetComponent<RectTransform>().rect.width;
        //height = (int)transform.parent.GetComponent<RectTransform>().rect.height;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 moveDelta = new Vector3(mouseX, mouseY, 0) * cursorSpeed_curr * Time.deltaTime;
        transform.position += moveDelta;

        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        transform.position = pos;

        if (Input.GetKeyDown(KeyCode.Space)) { FireRay(); }

    }

    /*
        private void Click()
        {
            Debug.Log("Click:");
            Camera mainCamera = Camera.main;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(mainCamera, transform.position);

            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            PointerEventData pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = transform.position;

            List<RaycastResult> results = new List<RaycastResult>();
            m_Raycaster.Raycast(pointerEventData, results);
            foreach (RaycastResult result in results)
            {
                Debug.Log("In Loop:" + result.gameObject.name);
                Button hitButton = result.gameObject.GetComponent<Button>();
                if (hitButton != null)
                {
                    Debug.Log("Hit " + result.gameObject.name);
                    hitButton.OnPointerClick(pointerEventData);
                }
            }


        }
    */

    public void FireRay()
    {
        Debug.Log("pressed");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("something was hit");
            Debug.Log(hit.transform.name);

            if (hit.collider != null)
            {
                hit.collider.enabled = false;
            }
        }
    }



}
