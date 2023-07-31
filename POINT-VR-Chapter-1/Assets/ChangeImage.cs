using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite newButtonImage;
    public Button button;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        button.image.sprite = newButtonImage;
        
    }
}
