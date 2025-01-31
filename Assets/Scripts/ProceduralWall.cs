using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ProceduralWall : MonoBehaviour
{
    public float playAreaWidth = 100f; // Width of the playable area
    public float playAreaLength = 100f; // Length of the playable area
    public float wallHeight = 5f; // Height of the wall

    void Start()
    {
        GenerateWall();
    }

    void GenerateWall()
    {
        // Create a new mesh
        Mesh mesh = new Mesh();

        // Define vertices for the wall
        Vector3[] vertices = new Vector3[16]; // Double the vertices for both sides
        vertices[0] = new Vector3(-playAreaWidth / 2, 0, -playAreaLength / 2); // Bottom-left corner (outside)
        vertices[1] = new Vector3(-playAreaWidth / 2, wallHeight, -playAreaLength / 2); // Bottom-left corner (top, outside)
        vertices[2] = new Vector3(playAreaWidth / 2, 0, -playAreaLength / 2); // Bottom-right corner (outside)
        vertices[3] = new Vector3(playAreaWidth / 2, wallHeight, -playAreaLength / 2); // Bottom-right corner (top, outside)
        vertices[4] = new Vector3(playAreaWidth / 2, 0, playAreaLength / 2); // Top-right corner (outside)
        vertices[5] = new Vector3(playAreaWidth / 2, wallHeight, playAreaLength / 2); // Top-right corner (top, outside)
        vertices[6] = new Vector3(-playAreaWidth / 2, 0, playAreaLength / 2); // Top-left corner (outside)
        vertices[7] = new Vector3(-playAreaWidth / 2, wallHeight, playAreaLength / 2); // Top-left corner (top, outside)

        // Duplicate vertices for the inside faces
        for (int i = 0; i < 8; i++)
        {
            vertices[i + 8] = vertices[i]; // Inside vertices are the same as outside vertices
        }

        // Define triangles for the wall (two triangles per side, for both outside and inside)
        int[] triangles = new int[] {
            // Outside faces
            0, 1, 2,
            2, 1, 3,
            2, 3, 4,
            4, 3, 5,
            4, 5, 6,
            6, 5, 7,
            6, 7, 0,
            0, 7, 1,

            // Inside faces (flipped normals)
            8, 10, 9,
            10, 11, 9,
            10, 12, 11,
            12, 13, 11,
            12, 14, 13,
            14, 15, 13,
            14, 8, 15,
            8, 9, 15
        };

        // Define UVs for the vertices
        Vector2[] uvs = new Vector2[16];
        float uvScale = 0.5f; // Adjust this to control the texture tiling

        // Outside UVs
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(0, uvScale);
        uvs[2] = new Vector2(playAreaWidth * uvScale, 0);
        uvs[3] = new Vector2(playAreaWidth * uvScale, uvScale);
        uvs[4] = new Vector2(playAreaWidth * uvScale, 0);
        uvs[5] = new Vector2(playAreaWidth * uvScale, uvScale);
        uvs[6] = new Vector2(0, 0);
        uvs[7] = new Vector2(0, uvScale);

        // Inside UVs (same as outside)
        for (int i = 0; i < 8; i++)
        {
            uvs[i + 8] = uvs[i];
        }

        // Assign vertices, triangles, and UVs to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        // Recalculate normals for proper lighting
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        GetComponent<MeshFilter>().mesh = mesh;

        // Add a MeshCollider for collision
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }
}
