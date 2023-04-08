using UnityEngine;
using System.Threading;
/// <summary>
/// Script that replaces a MeshFilter mesh with that of a the ZYX-ordered junction-based grid.
/// This grid can then deform in response to the movement of assigned rigidbodies.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class GridScript : MonoBehaviour
{
    /// <summary>
    /// We will get the masses from the Rigidbody components
    /// </summary>
    public Rigidbody[] rigidbodiesToDeformAround;
    Mesh deformingMesh;
    /// <summary>
    /// The thickness of the grid's "bars". 
    /// </summary>
    readonly float thickness = 0.02f;
    readonly int size_z = 8;
    readonly int size_x = 8;
    readonly int size_y = 5; //Produces a 7 x 7 x 4 grid
    /// <summary>
    /// The number of subjunctions on each "bar".
    /// </summary>
    readonly int divisions = 5;
    /// <summary>
    /// This struct is used to simplify the mass-position calculation and make memory use more compact
    /// </summary>
    struct Mass
    {
        public Vector3 position;
        public float mass;
    }
    /// <summary>
    /// Calculates the displacement each FixedUpdate.
    /// </summary>
    private void FixedUpdate()
    {
        Vector3[] displaced = new Vector3[size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y + 4 * divisions * size_z * size_x * (size_y - 1)]; 
        Mass[] masses = new Mass[rigidbodiesToDeformAround.Length];
        for (int j = 0; j < masses.Length; j++) //Prefetches the mass positions and values into one cache block ahead of time and organizes them into structs
        {
            masses[j] = new Mass
            {
                mass = rigidbodiesToDeformAround[j].mass,
                position = rigidbodiesToDeformAround[j].position
            };
        }
        int midpoint = ((displaced.Length / 2 - 1) | 7) + 1; //Increments of 8 only
        Thread t = new Thread(() => ThreadRoutine(displaced, masses, midpoint, displaced.Length));
        t.Start();
        ThreadRoutine(displaced, masses, 0, midpoint);
        t.Join();
        deformingMesh.vertices = displaced; //This is where the grid actually applies all of the calculations
        deformingMesh.RecalculateNormals();
    }
    /// <summary>
    /// This parallelizable function writes into the displacement vector for each vertex in response to the masses.
    /// Ensure the bounds for each thread do not overlap.
    /// </summary>
    /// <param name="displaced">The list of vertices to write to</param>
    /// <param name="masses">The list of masses to deform in response to</param>
    /// <param name="inclusive">The lower bound of vertex indices</param>
    /// <param name="exclusive">The upper bound of vertex indices</param>
    private void ThreadRoutine(Vector3[] displaced, Mass[] masses, int inclusive, int exclusive)
    {
        int i = inclusive;
        int upper = Mathf.Min(size_z * size_y * size_x * 8, exclusive);
        for (; i < upper; i += 8) //8-Junction case
        {
            Vector3 disp = GetDisplacementForVertex(masses, i);
            float xNew = disp.x + thickness;
            float yNew = disp.y + thickness;
            float zNew = disp.z + thickness;
            displaced[i] = disp;
            displaced[i + 1] = new Vector3(disp.x, disp.y, zNew);
            displaced[i + 2] = new Vector3(xNew, disp.y, disp.z);
            displaced[i + 3] = new Vector3(xNew, disp.y, zNew);
            displaced[i + 4] = new Vector3(disp.x, yNew, disp.z);
            displaced[i + 5] = new Vector3(disp.x, yNew, zNew);
            displaced[i + 6] = new Vector3(xNew, yNew, disp.z);
            displaced[i + 7] = new Vector3(xNew, yNew, zNew);
        }
        upper = Mathf.Min(size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y, exclusive);
        for (; i < upper; i += 4) //Z-Variant 4-subjunction case
        {
            Vector3 disp = GetDisplacementForVertex(masses, i);
            float xNew = disp.x + thickness;
            float yNew = disp.y + thickness;
            displaced[i] = disp;
            displaced[i + 1] = new Vector3(xNew, disp.y, disp.z);
            displaced[i + 2] = new Vector3(disp.x, yNew, disp.z);
            displaced[i + 3] = new Vector3(xNew, yNew, disp.z);
        }
        upper = Mathf.Min(size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y, exclusive);
        for (; i < upper; i += 4) //X-Variant 4-subjunction case
        {
            Vector3 disp = GetDisplacementForVertex(masses, i);
            float yNew = disp.y + thickness;
            float zNew = disp.z + thickness;
            displaced[i] = disp;
            displaced[i + 1] = new Vector3(disp.x, disp.y, zNew);
            displaced[i + 2] = new Vector3(disp.x, yNew, disp.z);
            displaced[i + 3] = new Vector3(disp.x, yNew, zNew);
        }
        upper = Mathf.Min(size_z * size_y * size_x * 8 + 4 * divisions * (size_z - 1) * size_x * size_y + 4 * divisions * size_z * (size_x - 1) * size_y + 4 * divisions * size_z * size_x * (size_y - 1), exclusive);
        for (; i < upper; i += 4) //Y-Variant 4-subjunction case
        {
            Vector3 disp = GetDisplacementForVertex(masses, i);
            float xNew = disp.x + thickness;
            float zNew = disp.z + thickness;
            displaced[i] = disp;
            displaced[i + 1] = new Vector3(disp.x, disp.y, zNew);
            displaced[i + 2] = new Vector3(xNew, disp.y, disp.z);
            displaced[i + 3] = new Vector3(xNew, disp.y, zNew);
        }
    }
    /// <summary>
    /// Calculates the new position of the vertex at index i after it deforms due to masses.
    /// </summary>
    /// <param name="masses"> The list of structs detailing each mass and its position </param>
    /// <param name="i"> The index of the specified vertex </param>
    /// <returns> The deformed position of the vertex </returns>
    private Vector3 GetDisplacementForVertex(Mass[] masses, int i)
    {
        Vector3 totalDisplacement = new Vector3(0f, 0f, 0f);
        float totalDistance = 0f;
        for (int j = 0; j < masses.Length; j++)
        {
            Vector3 direction = IndexToPos(i) - masses[j].position;
            float doubleMass = 2 * masses[j].mass;
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
        return d;
    }
    /// <summary>
    /// This replaces the attached mesh with the grid.
    /// </summary>
    void Start()
    {
        deformingMesh = GetComponent<MeshFilter>().mesh;
        deformingMesh.Clear(false);
        Vector3[] displaced = new Vector3[
            size_z * size_y * size_x * 8 //2560 with default settings
            + 4 * divisions * (size_z - 1) * size_x * size_y //2560 + 3360 = 5920
            + 4 * divisions * size_z * (size_x - 1) * size_y //5920 + 3360 = 9280
            + 4 * divisions * size_z * size_x * (size_y - 1)]; //9280 + 3072 = 12352
        for (int i = 0; i < displaced.Length; i++) {
            displaced[i] = IndexToPos(i);
        }
        deformingMesh.vertices = displaced;
        deformingMesh.triangles = GenerateTriangles();
    }   
    /// <summary>
    /// Gets the position of the index i in the ZXY enumerated grid point scheme.
    /// 
    /// The first size_z * size_y * size_x * 8 grid points are in junctions.
    /// The next 4 * divisions * (size_z - 1) * size_x * size_y grid points are the subjunctions in the z-variant direction.
    /// The next 4 * divisions * size_z * (size_x - 1) * size_y grid points are the subjunctions in the x-variant direction.
    /// The last 4 * divisions * size_z * size_x * (size_y - 1) grid points are the subjunctions in the y-variant direction.
    /// 
    /// Within each junction or subjunction, the ZYX ordering is maintained so vertex coordinates are in the same relative order regardless of index.
    /// 
    /// </summary>
    /// <param name="i"> The vertex index </param>
    /// <returns> The position of vertex i in world space </returns>
    Vector3 IndexToPos(int i) 
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
            + (size_y - 1) * size_x * size_z)]; //26112 triangles with default settings, becomes 3 * 26112 ints
        int tri = 0;
        for (int i = 0; i < size_x * size_y; i++) //Iterate over each z-bar
        {
            int first = i * 8 * size_z; //The base junction
            int branch = size_z * size_y * size_x * 8 + i * (4 * divisions * (size_z - 1)); //The base branching vertex
            for (int j = 0; j < size_z - 1; j++) //Iterate over each "box"
            {
                for (int k = 0; k < divisions + 1; k++) //Iterate over each segment
                {
                    int z_minor_x_minor_y_minor_vertex = branch;
                    int z_major_x_minor_y_minor_vertex = branch + 1;
                    int z_minor_x_major_y_minor_vertex = branch + 2;
                    int z_major_x_major_y_minor_vertex = branch + 3;
                    int z_minor_x_minor_y_major_vertex = branch + 4;
                    int z_major_x_minor_y_major_vertex = branch + 5;
                    int z_minor_x_major_y_major_vertex = branch + 6;
                    int z_major_x_major_y_major_vertex = branch + 7;
                    if (k == 0)
                    {
                        z_minor_x_minor_y_minor_vertex = first + 1;
                        z_major_x_minor_y_minor_vertex = first + 3;
                        z_minor_x_major_y_minor_vertex = first + 5;
                        z_major_x_major_y_minor_vertex = first + 7;
                        z_minor_x_minor_y_major_vertex = branch;
                        z_major_x_minor_y_major_vertex = branch + 1;
                        z_minor_x_major_y_major_vertex = branch + 2;
                        z_major_x_major_y_major_vertex = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        z_minor_x_minor_y_major_vertex = first + 8;
                        z_major_x_minor_y_major_vertex = first + 10;
                        z_minor_x_major_y_major_vertex = first + 12;
                        z_major_x_major_y_major_vertex = first + 14;
                    }
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex; //3 Assignments per row correspond to one triangle
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex;
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex; tris[tri++] = z_minor_x_major_y_major_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_major_y_minor_vertex; tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex; tris[tri++] = z_major_x_minor_y_major_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
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
                    int z_minor_x_minor_y_minor_vertex = branch;
                    int z_major_x_minor_y_minor_vertex = branch + 1;
                    int z_minor_x_major_y_minor_vertex = branch + 2;
                    int z_major_x_major_y_minor_vertex = branch + 3;
                    int z_minor_x_minor_y_major_vertex = branch + 4;
                    int z_major_x_minor_y_major_vertex = branch + 5;
                    int z_minor_x_major_y_major_vertex = branch + 6;
                    int z_major_x_major_y_major_vertex = branch + 7;
                    if (k == 0)
                    {
                        z_minor_x_minor_y_minor_vertex = first + 2;
                        z_major_x_minor_y_minor_vertex = first + 3;
                        z_minor_x_major_y_minor_vertex = first + 6;
                        z_major_x_major_y_minor_vertex = first + 7;
                        z_minor_x_minor_y_major_vertex = branch;
                        z_major_x_minor_y_major_vertex = branch + 1;
                        z_minor_x_major_y_major_vertex = branch + 2;
                        z_major_x_major_y_major_vertex = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        z_minor_x_minor_y_major_vertex = first + 8 * size_z;
                        z_major_x_minor_y_major_vertex = first + 1 + 8 * size_z;
                        z_minor_x_major_y_major_vertex = first + 4 + 8 * size_z;
                        z_major_x_major_y_major_vertex = first + 5 + 8 * size_z;
                    }
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex; tris[tri++] = z_major_x_minor_y_major_vertex; //3 Assignments per row correspond to one triangle
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex; tris[tri++] = z_major_x_minor_y_minor_vertex;
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex; tris[tri++] = z_minor_x_minor_y_minor_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex; tris[tri++] = z_minor_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex; tris[tri++] = z_minor_x_major_y_minor_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex; tris[tri++] = z_major_x_major_y_minor_vertex;
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
                    int z_minor_x_minor_y_minor_vertex = branch;
                    int z_major_x_minor_y_minor_vertex = branch + 1;
                    int z_minor_x_major_y_minor_vertex = branch + 2;
                    int z_major_x_major_y_minor_vertex = branch + 3;
                    int z_minor_x_minor_y_major_vertex = branch + 4;
                    int z_major_x_minor_y_major_vertex = branch + 5;
                    int z_minor_x_major_y_major_vertex = branch + 6;
                    int z_major_x_major_y_major_vertex = branch + 7;
                    if (k == 0)
                    {
                        z_minor_x_minor_y_minor_vertex = first + 4;
                        z_major_x_minor_y_minor_vertex = first + 5;
                        z_minor_x_major_y_minor_vertex = first + 6;
                        z_major_x_major_y_minor_vertex = first + 7;
                        z_minor_x_minor_y_major_vertex = branch;
                        z_major_x_minor_y_major_vertex = branch + 1;
                        z_minor_x_major_y_major_vertex = branch + 2;
                        z_major_x_major_y_major_vertex = branch + 3;
                        branch -= 4;
                    }
                    if (k == divisions)
                    {
                        z_minor_x_minor_y_major_vertex = first + 8 * size_z * size_x;
                        z_major_x_minor_y_major_vertex = first + 1 + 8 * size_z * size_x;
                        z_minor_x_major_y_major_vertex = first + 2 + 8 * size_z * size_x;
                        z_major_x_major_y_major_vertex = first + 3 + 8 * size_z * size_x;
                    }
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex; //3 Assignments per row correspond to one triangle
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_minor_y_major_vertex;
                    tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_minor_y_major_vertex; tris[tri++] = z_minor_x_major_y_major_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_minor_x_minor_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex;
                    tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_minor_x_major_y_major_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_major_y_minor_vertex; tris[tri++] = z_minor_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex; tris[tri++] = z_major_x_minor_y_major_vertex;
                    tris[tri++] = z_major_x_minor_y_minor_vertex; tris[tri++] = z_major_x_major_y_minor_vertex; tris[tri++] = z_major_x_major_y_major_vertex;
                    branch += 4;
                }
                first += 8 * size_z * size_x;
            }
        }
        return tris;
    }
}
