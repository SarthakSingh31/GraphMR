using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDisplay : MonoBehaviour
{
    private MeshGenerator meshGenerator;


    // Start is called before the first frame update
    void Start()
    {
        meshGenerator = new MeshGenerator(new Vector2(0.01f, 0.01f));

        Func<float, float, float> cone = (x, z) =>
        {
            float a = 1;
            float b = 1;
            float c = 1;

            return c * Mathf.Sqrt((Mathf.Pow(x, 2) / Mathf.Pow(a, 2)) + (Mathf.Pow(z, 2) / Mathf.Pow(b, 2)));
        };

        MeshData meshData = meshGenerator.GenerateMeshData(
            cone,
            new Vector2(-1, -1),
            new Vector2(1, 1)
        );

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uvs;

        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
