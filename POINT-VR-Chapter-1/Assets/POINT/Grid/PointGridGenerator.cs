using System.Collections;
using System.Collections.Generic;
// using UnityEditor;
using UnityEngine;

public class PointGridGenerator : MonoBehaviour
{
    private Camera currentCamera = null;
    public int radius;
    public int density;
    public float size;
    public Rigidbody[] rigidbodiesToDeformAround;
    public GameObject gridPoint;

    [HideInInspector]
    public List<GameObject> objs;
    [HideInInspector]
    public bool is_show;

    IEnumerator WaitForPlayerSpawn()
    {
        yield return new WaitUntil(() => Camera.current != null);

        // Start camera initialization
        currentCamera = Camera.current;
        //player = currentCamera.transform.parent.gameObject;

        yield break;
    }

    public void CreateGrid()
    {
        // vertices = new GameObject[radius * 2 * radius * 2 * radius];
        if (is_show)
        {
            return;
        }

        if (gridPoint != null)
        {
            Debug.Log("Loaded prefab: " + gridPoint.gameObject.name);
        }

        for (int d = -radius; d < radius; d++)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    GameObject obj = Instantiate(gridPoint);
                    Debug.Log("Loaded obj: " + obj.gameObject.name);
                    
                    obj.transform.parent = transform;
                    obj.transform.localScale = obj.transform.localScale * size;
                    //obj.transform.position = currentCamera.transform.position + new Vector3(1, 0, 0) * (d + 1) * density
                    //    + new Vector3(0, 1, 0) * (i + 1) * density + new Vector3(0, 0, 1) * (j + 1) * density;

                    // First vector3 is starting position
                    obj.transform.position = new Vector3(0, 0, 0) + new Vector3(1, 0, 0) * (d + 1) * density + new Vector3(0, 1, 0) * (i + 1) * density + new Vector3(0, 0, 1) * (j + 1) * density;

                    objs.Add(obj);
                }
            }
        }

        is_show = true;
    }


    public void DeleteGrid()
    {
        if (is_show == false)
        {
            return;
        }
        for (int i = 0; i < objs.Count; i++)
        {
            DestroyImmediate(objs[i]);
        }
        objs = new List<GameObject>();
        is_show = false;
    }


    void Start()
    {
        StartCoroutine(WaitForPlayerSpawn()); // Fix camera spawning, currently currentCamera does not set
        is_show = false;
        objs = new List<GameObject>();

        CreateGrid();

    }

    private void FixedUpdate()
    {
        if (is_show == false)
        {
            return;
        }

        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        float[] masses = new float[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
            masses[j] = rigidbodiesToDeformAround[j].mass;
        }

        for (int i = 0; i < objs.Count; i++)
        {

            Vector3 org_pos;
            int dd = (int)(i / (2 * radius * 2 * radius));
            int ii = (int)((i - dd * (2 * radius * 2 * radius)) / (2 * radius));
            int jj = (int)(i - dd * (2 * radius * 2 * radius) - ii * (2 * radius));
            //org_pos = currentCamera.transform.position + new Vector3(1, 0, 0) * (dd + 1 - radius) * density
            //            + new Vector3(0, 1, 0) * (ii + 1 - radius) * density + new Vector3(0, 0, 1) * (jj + 1 - radius) * density;
            org_pos = new Vector3(0, 0, 0) + new Vector3(1, 0, 0) * (dd + 1 - radius) * density + new Vector3(0, 1, 0) * (ii + 1 - radius) * density + new Vector3(0, 0, 1) * (jj + 1 - radius) * density;

            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = org_pos - massPositions[j];
                float doubleMass = 2 * masses[j];
                float distance = 1f;
                if (doubleMass * doubleMass < direction.sqrMagnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - doubleMass / direction.magnitude));
                }
                totalDisplacement += distance * direction; //Displacement from each mass is calculated independently, but combined by vector addition
            }

            objs[i].transform.position = org_pos - totalDisplacement; //Store the final displacement calculation for this vertex
        }
    }

}
