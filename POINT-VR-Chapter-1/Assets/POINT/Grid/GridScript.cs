using UnityEngine;
/// <summary>
/// Script that replaces a MeshFilter mesh with that of a grid built to certain specifications. This grid can then deform in response to the movement of assigned rigidbodies.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class GridScript : MonoBehaviour
{
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    Mesh deformingMesh;
    readonly float thickness = 0.02f;
    readonly int size_z = 8;
    readonly int size_x = 8;
    readonly int size_y = 5; //Produces a 7 x 7 x 4 grid
    readonly int divisions = 5;
    private void FixedUpdate()
    {
        Vector3[] displaced = new Vector3[size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y + 4 * divisions * size_z * size_x * (size_y - 1)]; 
        Vector3[] massPositions = new Vector3[rigidbodiesToDeformAround.Length];
        float[] masses = new float[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < masses.Length; j++) //Prefetches the mass positions and values into one cache block ahead of time
        {
            massPositions[j] = rigidbodiesToDeformAround[j].transform.position;
            masses[j] = rigidbodiesToDeformAround[j].mass;
        }
        for (int i = 0; i < size_z * size_y * size_x * 8; i+=8)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            float totalDistance = 0f;
            for (int j = 0; j < masses.Length; j++)
            {
                Vector3 direction = IndexToPos(i) - massPositions[j];
                float doubleMass = 2 * masses[j];
                float distance = 1f;
                if (doubleMass * doubleMass < direction.sqrMagnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - doubleMass / direction.magnitude));
                }
                totalDisplacement += distance * direction; //Displacement from each mass is calculated independently, but combined by vector addition
                totalDistance += distance;
            }
            Vector3 d = IndexToPos(i) - totalDisplacement; //Store the final displacement calculation for this vertex
            if (totalDistance > 1) // Normalizes the final displacement
            {
                d = IndexToPos(i) - totalDisplacement / totalDistance; //Store the final displacement calculation for this vertex
            }
            displaced[i] = d;
            displaced[i + 1] = d + new Vector3(0f, 0f, thickness);
            displaced[i + 2] = d + new Vector3(thickness, 0f, 0f);
            displaced[i + 3] = d + new Vector3(thickness, 0f, thickness);
            displaced[i + 4] = d + new Vector3(0f, thickness, 0f);
            displaced[i + 5] = d + new Vector3(0f, thickness, thickness);
            displaced[i + 6] = d + new Vector3(thickness, thickness, 0f);
            displaced[i + 7] = d + new Vector3(thickness, thickness, thickness);
        }
        for (int i = size_z * size_y * size_x * 8; i < size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y; i += 4)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            float totalDistance = 0f;
            for (int j = 0; j < masses.Length; j++)
            {
                Vector3 direction = IndexToPos(i) - massPositions[j];
                float doubleMass = 2 * masses[j];
                float distance = 1f;
                if (doubleMass * doubleMass < direction.sqrMagnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - doubleMass / direction.magnitude));
                }
                totalDisplacement += distance * direction; //Displacement from each mass is calculated independently, but combined by vector addition
                totalDistance += distance;
            }
            Vector3 d = IndexToPos(i) - totalDisplacement; //Store the final displacement calculation for this vertex
            if (totalDistance > 1) // Normalizes the final displacement
            {
                d = IndexToPos(i) - totalDisplacement / totalDistance; //Store the final displacement calculation for this vertex
            }
            displaced[i] = d;
            displaced[i + 1] = d + new Vector3(thickness, 0f, 0f);
            displaced[i + 2] = d + new Vector3(0f, thickness, 0f);
            displaced[i + 3] = d + new Vector3(thickness, thickness, 0f);
        }
        for (int i = size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y; i < size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y; i += 4)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            float totalDistance = 0f;
            for (int j = 0; j < masses.Length; j++)
            {
                Vector3 direction = IndexToPos(i) - massPositions[j];
                float doubleMass = 2 * masses[j];
                float distance = 1f;
                if (doubleMass * doubleMass < direction.sqrMagnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - doubleMass / direction.magnitude));
                }
                totalDisplacement += distance * direction; //Displacement from each mass is calculated independently, but combined by vector addition
                totalDistance += distance;
            }
            Vector3 d = IndexToPos(i) - totalDisplacement; //Store the final displacement calculation for this vertex
            if (totalDistance > 1) // Normalizes the final displacement
            {
                d = IndexToPos(i) - totalDisplacement / totalDistance; //Store the final displacement calculation for this vertex
            }
            displaced[i] = d;
            displaced[i + 1] = d + new Vector3(0f, 0f, thickness);
            displaced[i + 2] = d + new Vector3(0f, thickness, 0f);
            displaced[i + 3] = d + new Vector3(0f, thickness, thickness);
        }
        for (int i = size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y; i < size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y + 4 * divisions * size_z * size_x * (size_y - 1); i += 4)
        {
            Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
            float totalDistance = 0f;
            for (int j = 0; j < masses.Length; j++)
            {
                Vector3 direction = IndexToPos(i) - massPositions[j];
                float doubleMass = 2 * masses[j];
                float distance = 1f;
                if (doubleMass * doubleMass < direction.sqrMagnitude) //Displacement would not yield a complex number: deform at damped power
                {
                    distance = (1f - Mathf.Sqrt(1f - doubleMass / direction.magnitude));
                }
                totalDisplacement += distance * direction; //Displacement from each mass is calculated independently, but combined by vector addition
                totalDistance += distance;
            }
            Vector3 d = IndexToPos(i) - totalDisplacement; //Store the final displacement calculation for this vertex
            if (totalDistance > 1) // Normalizes the final displacement
            {
                d = IndexToPos(i) - totalDisplacement / totalDistance; //Store the final displacement calculation for this vertex
            }
            displaced[i] = d;
            displaced[i + 1] = d + new Vector3(0f, 0f, thickness);
            displaced[i + 2] = d + new Vector3(thickness, 0f, 0f);
            displaced[i + 3] = d + new Vector3(thickness, 0f, thickness);
        }
        deformingMesh.vertices = displaced; //This is where the grid actually applies all of the calculations
        deformingMesh.RecalculateNormals();
    }
    void Start()
    {
           deformingMesh = GetComponent<MeshFilter>().mesh;
           deformingMesh.Clear(false);
           Vector3[] displaced = new Vector3[
               size_z * size_y * size_x * 8 //2560 with current settings
               + 4 * divisions * (size_z - 1) * size_x * size_y //2560 + 3360 = 5920
               + 4 * divisions * size_z * (size_x - 1) * size_y //5920 + 3360 = 9280
               + 4 * divisions * size_z * size_x * (size_y - 1)]; //9280 + 3072 = 12352
           for (int i = 0; i < displaced.Length; i++)
           {
               Vector3 v = IndexToPos(i);
              displaced[i] = v;
             /* Visual Aid for debugging vertices
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube); 
            cube.transform.position = v;
            cube.transform.localScale = new Vector3(thickness, thickness, thickness);
            cube.name = "Cube " + i;
            MeshRenderer r = cube.GetComponent<MeshRenderer>();
            r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            r.receiveShadows = false;
            /*/ // End of test code
        } 
            deformingMesh.vertices = displaced;
        /*    deformingMesh.triangles = new int[] { 
                1, 2561, 2560,
                1, 3, 2561,
                1, 2560, 2562,
                5, 1, 2562,
                5, 2562, 2563,
                7, 5, 2563,
                3, 2563, 2561,
                3, 7, 2563
            }; */
        deformingMesh.triangles = GenerateTriangles();
    }
    Vector3 IndexToPos(int i) //Returns the position of vertex i in the "original" grid
    {
        int numY = 4 * divisions * size_z * size_x * (size_y - 1);
        int numX = 4 * divisions * size_z * (size_x - 1) * size_y;
        int numZ = 4 * divisions * (size_z - 1) * size_x * size_y;
        int numJunction = size_z * size_y * size_x * 8;
        float s = thickness / 2;
        if (i < numJunction) //Vertex i is in a junction
        {
            int junction = i / 8; //8 Vertices in a junction
            int y = junction / (size_x * size_z);
            int xz = junction - y * (size_x * size_z); //Position on the xz-plane
            int x = xz / size_z;
            int z = xz - x * size_z;
            switch (i & 7) //Axis priority: Z (forward), X (right), Y (up)
            {
                case 0: return new Vector3(x - s, y - s, z - s); 
                case 1: return new Vector3(x - s, y - s, z + s);
                case 2: return new Vector3(x + s, y - s, z - s);
                case 3: return new Vector3(x + s, y - s, z + s);
                case 4: return new Vector3(x - s, y + s, z - s);
                case 5: return new Vector3(x - s, y + s, z + s);
                case 6: return new Vector3(x + s, y + s, z - s);
                case 7: return new Vector3(x + s, y + s, z + s);
            }
        }
        else if (i < numJunction + numZ)
        { //Vertex i is in a z-variant direction
            int j = i - numJunction; //Index not accounting for junctions
            int xy = j / (4 * divisions * (size_z - 1)); //Position on the xy-plane
            int y = xy / size_x;
            int x = xy - y * size_x;
            int k = j - xy * (4 * divisions * (size_z - 1)); //Position within the z-bar
            int g = k / (4 * divisions); //Which "group" the vertex is in
            int h = k - g * (4 * divisions); //Position within the "group"
            float z = g + ((h / 4) + 1) / (divisions + 1f);
            switch (j & 3)
            {
                case 0: return new Vector3(x - s, y - s, z);
                case 1: return new Vector3(x + s, y - s, z);
                case 2: return new Vector3(x - s, y + s, z);
                case 3: return new Vector3(x + s, y + s, z);
            }
        }
        else if (i < numJunction + numZ + numX)
        { //Vertex i is in an x-variant direction
            int j = (i - numJunction) - numZ; //Index not accounting for junctions or direction Z
            int yz = j / (4 * divisions * (size_x - 1)); //Position on the yz-plane
            int y = yz / size_z;
            int z = yz - y * size_z;
            int k = j - yz * (4 * divisions * (size_x - 1)); //Position within the x-bar
            int g = k / (4 * divisions); //Which "group" the vertex is in
            int h = k - g * (4 * divisions); //Position within the "group"
            float x = g + ((h / 4) + 1) / (divisions + 1f);
            switch (j & 3)
            {
                case 0: return new Vector3(x, y - s, z - s);
                case 1: return new Vector3(x, y - s, z + s);
                case 2: return new Vector3(x, y + s, z - s);
                case 3: return new Vector3(x, y + s, z + s);
            }
        }
        else if (i < numJunction + numZ + numX + numY)
        { //Vertex i is in an x-variant direction
            int j = ((i - numJunction) - numZ) - numX; //Index not accounting for junctions or the other directions
            int xy = j / (4 * divisions * (size_y - 1)); //Position on the xy-plane
            int x = xy / size_z;
            int z = xy - x * size_z;
            int k = j - xy * (4 * divisions * (size_y - 1)); //Position within the y-bar
            int g = k / (4 * divisions); //Which "group" the vertex is in
            int h = k - g * (4 * divisions); //Position within the "group"
            float y = g + ((h / 4) + 1) / (divisions + 1f);
            switch (j & 3)
            {
                case 0: return new Vector3(x - s, y, z - s);
                case 1: return new Vector3(x - s, y, z + s);
                case 2: return new Vector3(x + s, y, z - s);
                case 3: return new Vector3(x + s, y, z + s);
            }
        }
        return new Vector3(-1, -1, -1); //Index is out of bounds
    }
    private int[] GenerateTriangles()
    {
        int[] tris = new int[
            3 * 8 * (divisions + 1) * (
              (size_z - 1) * size_y * size_x
            + (size_x - 1) * size_y * size_z
            + (size_y - 1) * size_x * size_z)]; //26112 triangles with current settings, becomes 3 * 26112 ints
        int tri = 0;
        for (int i = 0; i < size_x * size_y; i++) //Iterate over each z-bar
        {
            int first = i * 8 * size_z; //The base junction
            int branch = size_z * size_y * size_x * 8 + i * (4 * divisions * (size_z - 1)); //The base branching vertex
            for (int j = 0; j < size_z - 1; j++) //Iterate over each "box"
            {
                for (int k = 0; k < divisions + 1; k++) //Iterate over each segment
                {
                    //Debug.Log(branch + " " + i + " " + j + " " + k + " " + first);
                    int _1 = branch;
                    int _3 = branch + 1;
                    int _5 = branch + 2;
                    int _7 = branch + 3;
                    int _2560 = branch + 4;
                    int _2561 = branch + 5;
                    int _2562 = branch + 6;
                    int _2563 = branch + 7;
                    if (k == 0)
                    {
                        _1 = first + 1;
                        _3 = first + 3;
                        _5 = first + 5;
                        _7 = first + 7;
                        _2560 = branch;
                        _2561 = branch + 1;
                        _2562 = branch + 2;
                        _2563 = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        _2560 = first + 8;
                        _2561 = first + 10;
                        _2562 = first + 12;
                        _2563 = first + 14;
                    }
                    tris[tri++] = _1; tris[tri++] = _2561; tris[tri++] = _2560;
                    tris[tri++] = _1; tris[tri++] = _3; tris[tri++] = _2561;
                    tris[tri++] = _1; tris[tri++] = _2560; tris[tri++] = _2562;
                    tris[tri++] = _5; tris[tri++] = _1; tris[tri++] = _2562;
                    tris[tri++] = _5; tris[tri++] = _2562; tris[tri++] = _2563;
                    tris[tri++] = _7; tris[tri++] = _5; tris[tri++] = _2563;
                    tris[tri++] = _3; tris[tri++] = _2563; tris[tri++] = _2561;
                    tris[tri++] = _3; tris[tri++] = _7; tris[tri++] = _2563;
                    branch += 4;
                }
                first += 8;
            }
        }
        for (int i = 0; i < size_z * size_y; i++) //Iterate over each x-bar
        {
            int first = 8 * (i % size_z) + 8 * size_z * size_x * (i / size_z); //The base junction
            int branch = size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + i * (4 * divisions * (size_x - 1)); //The base branching vertex
            for (int j = 0; j < size_x - 1; j++) //Iterate over each "box"
            {
                for (int k = 0; k < divisions + 1; k++) //Iterate over each segment
                {
                    //Debug.Log(branch + " " + i + " " + j + " " + k + " " + first);
                    int _1 = branch;
                    int _3 = branch + 1;
                    int _5 = branch + 2;
                    int _7 = branch + 3;
                    int _2560 = branch + 4;
                    int _2561 = branch + 5;
                    int _2562 = branch + 6;
                    int _2563 = branch + 7;
                    if (k == 0)
                    {
                        _1 = first + 2;
                        _3 = first + 3;
                        _5 = first + 6;
                        _7 = first + 7;
                        _2560 = branch;
                        _2561 = branch + 1;
                        _2562 = branch + 2;
                        _2563 = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        _2560 = first + 8 * size_z;
                        _2561 = first + 1 + 8 * size_z;
                        _2562 = first + 4 + 8 * size_z;
                        _2563 = first + 5 + 8 * size_z;
                    }
                    tris[tri++] = _1; tris[tri++] = _2560; tris[tri++] = _2561;
                    tris[tri++] = _1; tris[tri++] = _2561; tris[tri++] = _3;
                    tris[tri++] = _1; tris[tri++] = _2562; tris[tri++] = _2560;
                    tris[tri++] = _5; tris[tri++] = _2562; tris[tri++] = _1;
                    tris[tri++] = _5; tris[tri++] = _2563; tris[tri++] = _2562;
                    tris[tri++] = _7; tris[tri++] = _2563; tris[tri++] = _5;
                    tris[tri++] = _3; tris[tri++] = _2561; tris[tri++] = _2563;
                    tris[tri++] = _3; tris[tri++] = _2563; tris[tri++] = _7;
                    branch += 4;
                }
                first += 8 * size_z;
            }
        }
        for (int i = 0; i < size_z * size_x; i++) //Iterate over each y-bar
        {
            int first = 8 * (i % size_z) + 8 * size_z * (i / size_z); //The base junction
            int branch = size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y + i * (4 * divisions * (size_y - 1)); //The base branching vertex
            for (int j = 0; j < size_y - 1; j++) //Iterate over each "box"
            {
                for (int k = 0; k < divisions + 1; k++) //Iterate over each segment
                {
                    //Debug.Log(branch + " " + i + " " + j + " " + k + " " + first);
                    int _1 = branch;
                    int _3 = branch + 1;
                    int _5 = branch + 2;
                    int _7 = branch + 3;
                    int _2560 = branch + 4;
                    int _2561 = branch + 5;
                    int _2562 = branch + 6;
                    int _2563 = branch + 7;
                    if (k == 0)
                    {
                        _1 = first + 4;
                        _3 = first + 5;
                        _5 = first + 6;
                        _7 = first + 7;
                        _2560 = branch;
                        _2561 = branch + 1;
                        _2562 = branch + 2;
                        _2563 = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        _2560 = first + 8 * size_z * size_x;
                        _2561 = first + 1 + 8 * size_z * size_x;
                        _2562 = first + 2 + 8 * size_z * size_x;
                        _2563 = first + 3 + 8 * size_z * size_x;
                    }
                    tris[tri++] = _1; tris[tri++] = _2561; tris[tri++] = _2560;
                    tris[tri++] = _1; tris[tri++] = _3; tris[tri++] = _2561;
                    tris[tri++] = _1; tris[tri++] = _2560; tris[tri++] = _2562;
                    tris[tri++] = _5; tris[tri++] = _1; tris[tri++] = _2562;
                    tris[tri++] = _5; tris[tri++] = _2562; tris[tri++] = _2563;
                    tris[tri++] = _7; tris[tri++] = _5; tris[tri++] = _2563;
                    tris[tri++] = _3; tris[tri++] = _2563; tris[tri++] = _2561;
                    tris[tri++] = _3; tris[tri++] = _7; tris[tri++] = _2563;
                    branch += 4;
                }
                first += 8 * size_z * size_x;
            }
        }
        return tris;
    }
}
