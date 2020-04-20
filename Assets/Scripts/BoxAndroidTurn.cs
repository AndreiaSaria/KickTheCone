
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoxAndroidTurn : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 direction;

    public Text m_Text;
    string message;

    void Update()
    {
        //Update the Text on the screen depending on current TouchPhase, and the current direction vector
        m_Text.text = "TimeDeltatime " + Time.deltaTime + "FPS " + (1/Time.smoothDeltaTime);
        if (Input.GetButton("Jump"))
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up , ForceMode.Impulse);
        }
        else if (Input.GetButton("Slide"))
        {
            GetComponent<Rigidbody>().AddForce(-Vector3.up , ForceMode.Impulse);
        }
        // Track a single touch as a direction control.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    startPos = touch.position;
                    //message = "Begun ";
                    
                    break;

                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - startPos;
                    //transform.Rotate(direction, direction.magnitude/10);
                    //message = "Moving ";

                    break;

                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    //message = "Ending ";
                    if(direction.y > 1)
                    {
                        //GetComponent<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
                        transform.position = Vector3.up * 2;
                    }
                    else if(direction.y < 1)
                    {
                        GetComponent<Rigidbody>().AddForce(-Vector3.up, ForceMode.Impulse);
                    }
                   
                    break;
            }
        }
    }
}
