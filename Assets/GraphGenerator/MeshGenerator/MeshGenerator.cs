using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] triangles;
}

public class MeshGenerator
{
    private Vector2 stepSize;

    public MeshGenerator(Vector2 stepSize)
    {
        this.stepSize = stepSize;
    }

    public MeshData GenerateMeshData(Func<float, float, float> surface, Vector2 lowerBounds, Vector2 upperBounds)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        Vector2Int size = new Vector2Int(
            Mathf.FloorToInt((upperBounds.x - lowerBounds.x) / stepSize.x),
            Mathf.FloorToInt((upperBounds.y - lowerBounds.y) / stepSize.y)
        );

        Vector3[][] surfaceMap = new Vector3[size.x][];
        for (int x = 0; x < size.x; x++)
        {
            surfaceMap[x] = new Vector3[size.y];
            for (int z = 0; z < size.y; z++)
            {
                surfaceMap[x][z] = new Vector3(
                    lowerBounds.x + (x * stepSize.x),
                    surface(lowerBounds.x + (x * stepSize.x), lowerBounds.y + (z * stepSize.y)),
                    lowerBounds.y + (z * stepSize.y)
                );
            }
        }

        for (int x = 0; x < (size.x - 1); x++)
        {
            for (int z = 0; z < (size.y - 1); z++)
            {
                // Triangle 1
                vertices.Add(surfaceMap[x][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 0));

                vertices.Add(surfaceMap[x][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 1));

                vertices.Add(surfaceMap[x + 1][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 1));

                // Triangle 2
                vertices.Add(surfaceMap[x][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 0));

                vertices.Add(surfaceMap[x + 1][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 1));

                vertices.Add(surfaceMap[x + 1][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 0));

                // Triangle 1 - back
                vertices.Add(surfaceMap[x][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 0));

                vertices.Add(surfaceMap[x + 1][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 1));

                vertices.Add(surfaceMap[x][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 1));

                // Triangle 2 - back
                vertices.Add(surfaceMap[x][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(0, 0));

                vertices.Add(surfaceMap[x + 1][z]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 0));

                vertices.Add(surfaceMap[x + 1][z + 1]);
                triangles.Add(vertices.Count - 1);
                uvs.Add(new Vector2(1, 1));
            }
        }

        return new MeshData() {
            vertices = vertices.ToArray(),
            uvs = uvs.ToArray(),
            triangles = triangles.ToArray()
        };
    }
}
