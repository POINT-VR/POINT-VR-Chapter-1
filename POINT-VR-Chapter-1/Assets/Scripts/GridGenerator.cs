using System.Collections;
using System.Collections.Generic;
// using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class GridGenerator : MonoBehaviour
{
    public Camera mainCamera;
    public int radius;
    public int density;
    public float size;
    public List<GameObject> objs;
    private bool is_show;
    public Rigidbody[] rigidbodiesToDeformAround;

    /*
    [CustomEditor(typeof(GridGenerator))]
    private class ObjectBuilderEditor : Editor
    {

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridGenerator gridGreater = (GridGenerator)target;
            if (GUILayout.Button("Create Grid"))
            {
                gridGreater.CreateGrid();
            }
            if (GUILayout.Button("Delete Grid"))
            {
                gridGreater.DeleteGrid();
            }

        }
    }
    */

    public void CreateGrid()
    {
        // vertices = new GameObject[radius * 2 * radius * 2 * radius];
        if (is_show)
        {
            return;
        }

        GameObject prefab = (GameObject)Resources.Load("Vec2");

        for (int d = -radius; d < radius; d++)
        {
            for (int i = -radius; i < radius; i++)
            {
                for (int j = -radius; j < radius; j++)
                {
                    GameObject obj = Instantiate(prefab);
                    obj.transform.parent = transform;
                    obj.transform.localScale = obj.transform.localScale * size;
                    obj.transform.position = mainCamera.transform.position + new Vector3(1, 0, 0) * (d + 1) * density
                        + new Vector3(0, 1, 0) * (i + 1) * density + new Vector3(0, 0, 1) * (j + 1) * density;

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
        is_show = false;
        objs = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (is_show == false)
        {
            return;
        }

        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < rigidbodiesToDeformAround.Length; j++) //Puts the mass positions on the stack ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
        }

        for (int i = 0; i < objs.Count; i++)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            for (int j = 0; j < rigidbodiesToDeformAround.Length; j++)
            {
                Vector3 direction = objs[i].transform.position - massPositions[j];
                float distance = 1f;
                if (rigidbodiesToDeformAround[j].mass < direction.magnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - rigidbodiesToDeformAround[j].mass / direction.magnitude));
                }
                totalDisplacement += distance * direction / rigidbodiesToDeformAround.Length; //Displacement from each mass is calculated independently, but combined by vector addition
            }
            
            Vector3 org_pos;
            int dd = (int)(i / (2 * radius * 2 * radius));
            int ii = (int)((i - dd * (2 * radius * 2 * radius)) / (2 * radius));
            int jj = (int)(i - dd * (2 * radius * 2 * radius) - ii * (2 * radius));
            org_pos = mainCamera.transform.position + new Vector3(1, 0, 0) * (dd + 1 - radius) * density
                        + new Vector3(0, 1, 0) * (ii + 1 - radius) * density + new Vector3(0, 0, 1) * (jj + 1 - radius) * density;

            objs[i].transform.position = org_pos - totalDisplacement; //Store the final displacement calculation for this vertex
            // objs[i].transform.position = objs[i].transform.position * 10f;
        }
    }

    /* 
    public static void GetChildObj(Transform Trans, ref List<GameObject> list)
    {
        for (int i = 0; i < Trans.childCount; i++)
        {
            GameObject obj = Trans.GetChild(i).gameObject;
            list.Add(obj);
            GetChildObj(Trans.GetChild(i), ref list);
        }
    }
    */

}
