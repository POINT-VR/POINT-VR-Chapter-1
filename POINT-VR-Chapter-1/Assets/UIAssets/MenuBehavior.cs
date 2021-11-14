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
       /* 
                var board = Keyboard.current;
               if (board == null)
               {
                   return;
               }
               if (board.spaceKey.isPressed)
     */
        var left = UnityEngine.InputSystem.XR.XRController.leftHand;
        if (left == null)
        {
            return;
        }
        if (left.IsPressed())
        {
            if (transform.position.y < 0) {
                transform.Translate(Vector3.up * 150); 
            }
        }
        else
        {
            if (transform.position.y > -100)
            {
                transform.Translate(Vector3.up * -150);
            }
        }
    }
}
