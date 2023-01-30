using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.CoreModule.GameObject;

public class AxisScript : MonoBehaviour
{
    public GameObject XAxis;
    public GameObject YAxis;
    public GameObject ZAxis;
    private GameObject XBody;
    private GameObject YBody;
    private GameObject ZBody;
    public Vector3 scale = new Vector3(1,1,1);
    //TODO: Fine-tune moveent of axes wrt scale
    public void scaleAxes(float x_scale, float y_scale, float z_scale)
    {
 
    }
    // Start is called before the first frame update
    void Start()
    {
        //XAxis = GameObject.Find("XAxis").GetComponent<Transform>();
        //YAxis = GameObject.Find("YAxis").GetComponent<Transform>();
       //ZAxis = GameObject.Find("ZAxis").GetComponent<Transform>();
        XBody = GameObject.Find("XBody");
        YBody = GameObject.Find("YBody");
        ZBody = GameObject.Find("ZBody");
    }

    // Update is called once per frame
    void Update()
    {
        XBody.transform.localScale = new Vector3(1,scale.x,1);
        YBody.transform.localScale = new Vector3(1, scale.y, 1);
        ZBody.transform.localScale = new Vector3(1, scale.z, 1);
        XAxis.transform.localPosition = new Vector3(scale.x/4 - (1 / 4), ((float)0.146)+0, 0);
        YAxis.transform.localPosition = new Vector3(0, ((float)0.159)+scale.y/4-(1/4), 0);
        ZAxis.transform.localPosition = new Vector3(0,((float)0.146) + 0, scale.z/4 - (1 / 4));
        //XBody.transform.localScale = scale;
        //YBody.transform.localScale = scale;
        //ZBody.transform.localScale = scale;
    }
}
