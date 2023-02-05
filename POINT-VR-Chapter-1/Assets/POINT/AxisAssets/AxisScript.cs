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
    private GameObject XTip;
    private GameObject YTip;
    private GameObject ZTip;


    private Vector3 XBodyScale;
    private Vector3 YBodyScale;
    private Vector3 ZBodyScale;




    public Vector3 scale = new Vector3(1,1,1);








    // Start is called before the first frame update
    void Start()
    {
        //XAxis = GameObject.Find("XAxis").GetComponent<Transform>();
        //YAxis = GameObject.Find("YAxis").GetComponent<Transform>();
       //ZAxis = GameObject.Find("ZAxis").GetComponent<Transform>();
        XBody = GameObject.Find("XBody");
        YBody = GameObject.Find("YBody");
        ZBody = GameObject.Find("ZBody");
        XTip = GameObject.Find("XTip");
        YTip = GameObject.Find("YTip");
        ZTip = GameObject.Find("ZTip");

        XBodyScale = XBody.transform.localScale;
        YBodyScale = YBody.transform.localScale;
        ZBodyScale = ZBody.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {


        XBody.transform.localScale = new Vector3(1,scale.x,1);
        YBody.transform.localScale = new Vector3(1, scale.y, 1);
        ZBody.transform.localScale = new Vector3(1, scale.z, 1);
        XAxis.transform.localPosition = new Vector3(scale.x/4 - (1 / (float)5.5), ((float)0.146)+0, 0);
        YAxis.transform.localPosition = new Vector3(0, ((float)0.159)+scale.y/4-(1/ (float)5.5), 0);
        ZAxis.transform.localPosition = new Vector3(0,((float)0.146) + 0, scale.z/4 - (1 / (float)5.5));
        //XBody.transform.localScale = scale;
        //YBody.transform.localScale = scale;
        //ZBody.transform.localScale = scale;
        XTip.transform.localPosition = new Vector3(0, 0, scale.x / 20 -((float).05));
        YTip.transform.localPosition = new Vector3( 0, scale.y / 20 - ((float).05), 0);
        ZTip.transform.localPosition = new Vector3( scale.z / 20 - ((float).05), 0,0);


    }
}
