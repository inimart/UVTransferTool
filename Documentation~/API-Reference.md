# UV Transfer Tool API Reference

This document provides a detailed reference for the scripting API of the UV Transfer Tool package.

## Namespace

```csharp
using Inimart.UVTransferTool;
```

## Core Classes

### UVTransfer

The main static class for performing UV transfers.

#### Methods

##### TransferUVs

```csharp
public static bool TransferUVs(Mesh sourceMesh, Mesh targetMesh, UVTransferMethod method, int sourceUVChannel = 0, int targetUVChannel = 0)
```

Transfers UVs from source mesh to target mesh using the specified method.

**Parameters:**
- `sourceMesh`: The Mesh containing the UVs to transfer.
- `targetMesh`: The Mesh that will receive the UVs.
- `method`: The mapping method to use for transfer.
- `sourceUVChannel`: The UV channel to read from (0-7).
- `targetUVChannel`: The UV channel to write to (0-7).

**Returns:**
- `bool`: True if the transfer was successful, false otherwise.

```csharp
public static bool TransferUVs(Mesh sourceMesh, Mesh targetMesh, UVTransferOptions options)
```

Advanced version with full options control.

**Parameters:**
- `sourceMesh`: The Mesh containing the UVs to transfer.
- `targetMesh`: The Mesh that will receive the UVs.
- `options`: A UVTransferOptions object with detailed settings.

**Returns:**
- `bool`: True if the transfer was successful, false otherwise.

##### GeneratePreview

```csharp
public static UVPreviewData GeneratePreview(Mesh sourceMesh, Mesh targetMesh, UVTransferOptions options)
```

Generates preview data without modifying the target mesh.

**Parameters:**
- `sourceMesh`: The Mesh containing the UVs to transfer.
- `targetMesh`: The Mesh to preview the UVs on.
- `options`: A UVTransferOptions object with detailed settings.

**Returns:**
- `UVPreviewData`: Contains the preview information including mapped UVs.

##### SaveMesh

```csharp
public static bool SaveMesh(Mesh mesh, string path, bool makeUniquePath = true)
```

Saves a mesh as an asset.

**Parameters:**
- `mesh`: The Mesh to save.
- `path`: The asset path (relative to Assets folder).
- `makeUniquePath`: If true, ensures the path doesn't overwrite existing assets.

**Returns:**
- `bool`: True if save was successful.

### UVTransferMethod

Enum defining the available UV transfer methods.

```csharp
public enum UVTransferMethod
{
    NearestPoint,      // Maps UVs based on closest points
    Topology,          // Maps based on topological similarity
    Projection,        // Projects UVs using raycasting
    Parametric,        // Uses parametric mapping
    Custom             // User-defined mapping
}
```

### UVTransferOptions

Class containing all settings for UV transfer operations.

```csharp
public class UVTransferOptions
{
    // Basic settings
    public UVTransferMethod MappingMethod { get; set; } = UVTransferMethod.NearestPoint;
    public int SourceUVChannel { get; set; } = 0;
    public int TargetUVChannel { get; set; } = 0;
    public float SamplingDensity { get; set; } = 1.0f;
    
    // Advanced settings
    public bool PreserveSeams { get; set; } = true;
    public bool PreserveUVIslands { get; set; } = true;
    public bool ApplySmoothing { get; set; } = false;
    public int SmoothingIterations { get; set; } = 1;
    
    // Method-specific settings
    public float MaxSearchDistance { get; set; } = 1.0f;
    public bool UseSpatialHash { get; set; } = true;
    public Vector3 ProjectionDirection { get; set; } = Vector3.forward;
    public bool UseCustomProjectionDirection { get; set; } = false;
    public float NormalBias { get; set; } = 0.01f;
    
    // Weight maps
    public Texture2D WeightMap { get; set; } = null;
    public bool UseWeightMap { get; set; } = false;
}
```

### UVPreviewData

Data structure returned by preview generation.

```csharp
public class UVPreviewData
{
    public Vector2[] MappedUVs { get; private set; }
    public int TargetUVChannel { get; private set; }
    public Color[] IslandColors { get; private set; }
    public Dictionary<int, int> VertexMapping { get; private set; }
    
    // Methods
    public void ApplyToMesh(Mesh targetMesh) { /* ... */ }
    public Texture2D GeneratePreviewTexture(int width, int height) { /* ... */ }
}
```

## Editor Classes

### UVTransferEditorWindow

The main editor window class (not directly accessible through scripting).

### UVTransferEditor

Static class with additional editor utilities.

```csharp
public static class UVTransferEditor
{
    // Visualizing UVs in editor
    public static void VisualizeUVs(Mesh mesh, int uvChannel = 0);
    public static void ClearUVVisualization();
    
    // Creating mesh assets
    public static string CreateMeshAsset(Mesh mesh, string suggestedPath);
    
    // Batching operations
    public static void BatchProcess(List<Mesh> targetMeshes, Mesh sourceMesh, UVTransferOptions options);
}
```

## Usage Examples

### Basic Transfer

```csharp
using UnityEngine;
using Inimart.UVTransferTool;

public class UVTransferExample : MonoBehaviour
{
    public GameObject sourceObject;
    public GameObject targetObject;
    
    public void TransferUVs()
    {
        // Get meshes
        Mesh sourceMesh = sourceObject.GetComponent<MeshFilter>().sharedMesh;
        Mesh targetMesh = targetObject.GetComponent<MeshFilter>().sharedMesh;
        
        // Perform a basic transfer from UV channel 0 to 0 using nearest point method
        bool success = UVTransfer.TransferUVs(
            sourceMesh, 
            targetMesh, 
            UVTransferMethod.NearestPoint,
            0, // Source UV channel
            0  // Target UV channel
        );
        
        // Update the target object with the modified mesh
        if (success)
        {
            Debug.Log("UV transfer completed successfully");
            targetObject.GetComponent<MeshFilter>().sharedMesh = targetMesh;
        }
    }
}
```

### Advanced Transfer with Options

```csharp
using UnityEngine;
using Inimart.UVTransferTool;

public class AdvancedUVTransferExample : MonoBehaviour
{
    public GameObject sourceObject;
    public GameObject targetObject;
    public Texture2D weightMap;
    
    public void PerformAdvancedTransfer()
    {
        // Get meshes
        Mesh sourceMesh = sourceObject.GetComponent<MeshFilter>().sharedMesh;
        Mesh targetMesh = targetObject.GetComponent<MeshFilter>().sharedMesh;
        
        // Create options with detailed settings
        UVTransferOptions options = new UVTransferOptions
        {
            MappingMethod = UVTransferMethod.Projection,
            SourceUVChannel = 0,
            TargetUVChannel = 1, // Transfer to second UV channel
            SamplingDensity = 0.9f,
            
            // Advanced settings
            PreserveSeams = true,
            PreserveUVIslands = true,
            ApplySmoothing = true,
            SmoothingIterations = 2,
            
            // Projection-specific settings
            ProjectionDirection = Vector3.up, // Project along up axis
            UseCustomProjectionDirection = true,
            NormalBias = 0.02f,
            
            // Use a weight map
            UseWeightMap = weightMap != null,
            WeightMap = weightMap
        };
        
        // Perform the transfer with our options
        bool success = UVTransfer.TransferUVs(sourceMesh, targetMesh, options);
        
        if (success)
        {
            // Save the result as a new mesh asset
            string path = UVTransfer.SaveMesh(targetMesh, "Assets/Meshes/TransferredMesh.asset");
            Debug.Log($"Mesh saved to {path}");
            
            // Update the target object
            targetObject.GetComponent<MeshFilter>().sharedMesh = targetMesh;
        }
    }
}
```

### Batch Processing

```csharp
using System.Collections.Generic;
using UnityEngine;
using Inimart.UVTransferTool;

public class BatchUVTransferExample : MonoBehaviour
{
    public GameObject sourceObject;
    public List<GameObject> targetObjects = new List<GameObject>();
    
    public void BatchTransferUVs()
    {
        Mesh sourceMesh = sourceObject.GetComponent<MeshFilter>().sharedMesh;
        List<Mesh> targetMeshes = new List<Mesh>();
        
        // Collect all target meshes
        foreach (var targetObj in targetObjects)
        {
            if (targetObj.GetComponent<MeshFilter>() != null)
            {
                targetMeshes.Add(targetObj.GetComponent<MeshFilter>().sharedMesh);
            }
        }
        
        // Configure options
        UVTransferOptions options = new UVTransferOptions
        {
            MappingMethod = UVTransferMethod.NearestPoint,
            MaxSearchDistance = 0.5f,
            SamplingDensity = 0.8f
        };
        
        // Batch process all meshes
        int index = 0;
        foreach (Mesh targetMesh in targetMeshes)
        {
            bool success = UVTransfer.TransferUVs(sourceMesh, targetMesh, options);
            if (success)
            {
                targetObjects[index].GetComponent<MeshFilter>().sharedMesh = targetMesh;
            }
            index++;
        }
    }
} 