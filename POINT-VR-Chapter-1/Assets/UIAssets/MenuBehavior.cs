using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MenuBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var left = UnityEngine.InputSystem.XR.XRController.leftHand;
        if (left == null)
        {
            return;
        }
        /*       var board = Keyboard.current;
               if (board == null)
               {
                   return;
               }
               if (board.spaceKey.isPressed)
       */
        if (left.IsPressed())
        {
            if (transform.position.y < 50) {
                transform.Translate(Vector3.up * 150); 
            }
        }
        else
        {
            if (transform.position.y > -300)
            {
                transform.Translate(Vector3.up * -150);
            }
        }
    }
}
