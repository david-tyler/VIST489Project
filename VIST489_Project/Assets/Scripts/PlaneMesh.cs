using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlaneMesh : MonoBehaviour
{

    public GameObject cornerPrefab; // Prefab for the visual representation of corners
    public List<Vector3> cornerPositions = new List<Vector3>(); // Stores the positions of the corners
    public Camera mainCamera; // Reference to the main camera
    public Material material;

    void Start()
    {
        //mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // On left mouse click
        {
            PlaceCorner();
        }
    }

    void PlaceCorner()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

        // Place a visual marker at the world position
        GameObject corner = Instantiate(cornerPrefab, worldPosition, Quaternion.identity);
        corner.name = "Corner " + (cornerPositions.Count + 1);

        // Add the position to the list
        cornerPositions.Add(worldPosition);

        // If we have 4 corners, generate the plane
        if (cornerPositions.Count == 4)
        {
            CreatePlane();
            cornerPositions.Clear(); // Reset for future planes
        }
    }

    void CreatePlane()
    {
        // Create a new GameObject to hold the plane mesh
        GameObject plane = new GameObject("DynamicPlane");
        MeshFilter meshFilter = plane.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = plane.AddComponent<MeshRenderer>();

        // Set up the plane mesh
        Mesh mesh = new Mesh();
       

        // Define vertices based on the 4 corners
        Vector3[] vertices = new Vector3[4];
        vertices[0] = cornerPositions[0]; // Bottom-left
        vertices[1] = cornerPositions[1]; // Bottom-right
        vertices[2] = cornerPositions[3]; // Top-left
        vertices[3] = cornerPositions[2]; // Top-right

        // Define triangles (two triangles to form a quad)
        int[] triangles = new int[]
        {
            2, 1, 0, // First triangle
            2, 3, 1  // Second triangle
        };

        // Assign the vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate normals for proper lighting
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshFilter
        meshFilter.mesh = mesh;

        // Optionally, assign a basic material to the plane
        meshRenderer.material = material;// new Material(Shader.Find("Standard"));
    }
}
