using UnityEngine;
using System.Collections.Generic;
using System;

public class LabyrinthManager : MonoBehaviour
{
    [Header("Field Settings")]
    [SerializeField] private int fieldWidth = 50;
    [SerializeField] private int fieldHeight = 50;

    [Header("Sphere Arrangement")]
    [SerializeField, Range(1f, 5f)] private float sphereSpacing = 1f; // Controls spacing between spheres (higher = more space)

    [Header("Sphere Prefab")]
    [SerializeField] private GameObject spherePrefab;

    [Header("Path Settings")]
    [SerializeField] private Transform player; // Reference to player transform
    [SerializeField] private float proximityThreshold = 5f; // Distance at which spheres change from white to path colors
    [SerializeField] private Color pathColor = Color.green; // Color for spheres on the valid path
    [SerializeField] private Color nonPathColor = Color.red; // Color for nearby spheres not on the valid path
    [SerializeField] private Color defaultColor = Color.white; // Default color when player is far away

    [Header("Path Definition")]
    [Tooltip("Define path with 'O' for valid and 'X' for invalid. Each string represents one row.")]
    [SerializeField] private string[] pathStrings = new string[50]; // Default to 50 rows for 50x50 grid

    private GameObject[,] spheres;
    private Material[,] sphereMaterials;
    private bool[,] pathMap; 


    void Start()
    {
        GenerateSphereField();
        InitializePathMap();
    }

    void Update()
    {
        UpdateSphereColors();
    }

    void InitializePathMap()
    {
        pathMap = new bool[fieldWidth, fieldHeight];

        InitializeFromStrings();

        // Apply initial colors - all spheres start white
        for (int x = 0; x < fieldWidth; x++)
        {
            for (int z = 0; z < fieldHeight; z++)
            {
                if (sphereMaterials[x, z] != null)
                {
                    sphereMaterials[x, z].SetColor("_Color", defaultColor);
                }
            }
        }

        Debug.Log($"Initialized path map with dimensions: {pathMap.GetLength(0)}x{pathMap.GetLength(1)}");
    }

    void InitializeFromStrings()
    {
        for (int row = 0; row < fieldHeight; row++)
        {
            print($"Row {row}: {pathStrings[row]}"); // Debug output for each row
            string currentRow = pathStrings[row];

            for (int col = 0; col < fieldHeight; col++)
            {
                // 'O' means valid path, anything else (including 'X') is invalid
                pathMap[col, row] = (currentRow[col] == 'O');
            }
        }
    }

    void UpdateSphereColors()
    {
        Vector3 playerPos = player.position;

        for (int x = 0; x < spheres.GetLength(0); x++)
        {
            for (int z = 0; z < spheres.GetLength(1); z++)
            {
                if (spheres[x, z] != null && sphereMaterials[x, z] != null)
                {
                    // Calculate distance from player to this sphere
                    float distance = Vector3.Distance(playerPos, spheres[x, z].transform.position);

                    // Check if this position is on the valid path
                    bool isOnPath = IsOnPath(x, z);

                    // Determine color based on distance and path status
                    if (distance <= proximityThreshold)
                    {

                        // Only change color when sphere is within proximity threshold
                        if (isOnPath)
                        {
                            // Path sphere within reach - set to green
                            sphereMaterials[x, z].SetColor("_Color", pathColor);
                        }
                        else
                        {
                            // Non-path sphere within reach - set to red
                            sphereMaterials[x, z].SetColor("_Color", nonPathColor);
                        }
                    }
                    else
                    {
                        // Far away - use default color (white)
                        sphereMaterials[x, z].SetColor("_Color", defaultColor);
                    }
                }
            }
        }
    }

    bool IsOnPath(int x, int z)
    {
        // Check for array bounds and then check the pathMap
        if (x >= 0 && x < pathMap.GetLength(0) && z >= 0 && z < pathMap.GetLength(1))
        {
            return pathMap[x, z];
        }

        return false;
    }

    void GenerateSphereField()
    {
        // Initialize arrays to keep track of all spheres and their materials
        spheres = new GameObject[fieldWidth, fieldHeight];
        sphereMaterials = new Material[fieldWidth, fieldHeight];

        // Create parent object to keep hierarchy clean
        GameObject fieldParent = new GameObject("SphereField");
        fieldParent.transform.parent = transform;

        // Calculate actual spacing between spheres
        float actualSpacing = sphereSpacing;

        for (int x = 0; x < fieldWidth; x++)
        {
            for (int z = 0; z < fieldHeight; z++)
            {
                // Calculate position with the new spacing
                float posX = x * actualSpacing;
                float posZ = z * actualSpacing;

                // Center the field
                posX -= (fieldWidth - 1) * actualSpacing / 2f;
                posZ -= (fieldHeight - 1) * actualSpacing / 2f;

                Vector3 spherePosition = new Vector3(posX, 0f, posZ);

                // Create the sphere
                GameObject sphere;
                sphere = Instantiate(spherePrefab, spherePosition, Quaternion.identity, fieldParent.transform);
                sphere.name = $"Sphere_{x}_{z}";

                // Store reference to the sphere and its material
                spheres[x, z] = sphere;

                // Get and store the material that has the _Color property
                Renderer renderer = sphere.GetComponent<Renderer>();
                if (renderer != null)
                {
                    sphereMaterials[x, z] = renderer.material;
                    sphereMaterials[x, z].SetColor("_Color", defaultColor); // Set initial color
                }
            }
        }

        Debug.Log($"Created sphere grid with dimensions: {fieldWidth}x{fieldHeight}");
    }
}
