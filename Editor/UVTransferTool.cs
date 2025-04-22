using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UVTransferTool : EditorWindow
{
    private GameObject sourceObject;
    private GameObject targetObject;
    private int uvChannel = 0;
    private Vector2 scrollPosition;
    private Texture2D sourcePreview;
    private Texture2D targetPreview;
    private bool showPreview = true;

    [MenuItem("Tools/UV Transfer Tool")]
    static void Init()
    {
        UVTransferTool window = (UVTransferTool)EditorWindow.GetWindow(typeof(UVTransferTool));
        window.minSize = new Vector2(350, 450);
        window.titleContent = new GUIContent("UV Transfer");
        window.Show();
    }

    void OnGUI()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("UV Transfer Tool", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
        
        // Create a horizontal layout for source object
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Source:", GUILayout.Width(60));
        sourceObject = (GameObject)EditorGUILayout.ObjectField(sourceObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        
        // Create a horizontal layout for target object
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Target:", GUILayout.Width(60));
        targetObject = (GameObject)EditorGUILayout.ObjectField(targetObject, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();
        
        // UV Channel
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UV Channel:", GUILayout.Width(80));
        uvChannel = EditorGUILayout.IntField(uvChannel);
        EditorGUILayout.EndHorizontal();
        
        // Preview options
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("UV Preview:", GUILayout.Width(80));
        showPreview = EditorGUILayout.Toggle(showPreview);
        EditorGUILayout.EndHorizontal();
        
        if (showPreview)
        {
            UpdatePreviews();
            
            float availableWidth = EditorGUIUtility.currentViewWidth - 40;
            float previewSize = availableWidth;
            
            // Drawing previews
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Source UV Preview:", EditorStyles.boldLabel);
            if (sourcePreview != null)
            {
                Rect sourceRect = GUILayoutUtility.GetRect(previewSize, previewSize);
                EditorGUI.DrawPreviewTexture(sourceRect, sourcePreview);
            }
            else
            {
                EditorGUILayout.LabelField("No preview available.");
            }
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Target UV Preview:", EditorStyles.boldLabel);
            if (targetPreview != null)
            {
                Rect targetRect = GUILayoutUtility.GetRect(previewSize, previewSize);
                EditorGUI.DrawPreviewTexture(targetRect, targetPreview);
            }
            else
            {
                EditorGUILayout.LabelField("No preview available.");
            }
        }
        
        EditorGUILayout.Space(20);
        EditorGUI.BeginDisabledGroup(sourceObject == null || targetObject == null);
        if (GUILayout.Button("Transfer UVs"))
        {
            TransferUVs();
        }
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.EndScrollView();
    }

    private void UpdatePreviews()
    {
        if (!showPreview)
            return;

        // Generate Source Preview
        if (sourceObject != null)
        {
            SkinnedMeshRenderer sourceSMR = sourceObject.GetComponentInChildren<SkinnedMeshRenderer>();
            MeshFilter sourceMF = sourceObject.GetComponentInChildren<MeshFilter>();
            
            Mesh sourceMesh = null;
            if (sourceSMR != null)
                sourceMesh = sourceSMR.sharedMesh;
            else if (sourceMF != null)
                sourceMesh = sourceMF.sharedMesh;
                
            if (sourceMesh != null)
                sourcePreview = GenerateUVPreview(sourceMesh, uvChannel);
        }
        
        // Generate Target Preview
        if (targetObject != null)
        {
            SkinnedMeshRenderer targetSMR = targetObject.GetComponentInChildren<SkinnedMeshRenderer>();
            MeshFilter targetMF = targetObject.GetComponentInChildren<MeshFilter>();
            
            Mesh targetMesh = null;
            if (targetSMR != null)
                targetMesh = targetSMR.sharedMesh;
            else if (targetMF != null)
                targetMesh = targetMF.sharedMesh;
                
            if (targetMesh != null)
                targetPreview = GenerateUVPreview(targetMesh, uvChannel);
        }
    }

    private Texture2D GenerateUVPreview(Mesh mesh, int channel)
    {
        // Create texture
        int resolution = 512;
        Texture2D texture = new Texture2D(resolution, resolution);
        Color[] colors = new Color[resolution * resolution];
        for (int i = 0; i < colors.Length; i++)
            colors[i] = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        texture.SetPixels(colors);
        
        // Get UV coordinates based on selected channel
        Vector2[] uvs = null;
        switch (channel)
        {
            case 0: uvs = mesh.uv; break;
            case 1: uvs = mesh.uv2; break;
            case 2: uvs = mesh.uv3; break;
            case 3: uvs = mesh.uv4; break;
            default: uvs = mesh.uv; break;
        }
        
        if (uvs == null || uvs.Length == 0)
        {
            texture.Apply();
            return texture;
        }
        
        // Draw UV lines
        int[] triangles = mesh.triangles;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            if (triangles[i] >= uvs.Length || triangles[i + 1] >= uvs.Length || triangles[i + 2] >= uvs.Length)
                continue;
                
            Vector2 uv1 = uvs[triangles[i]];
            Vector2 uv2 = uvs[triangles[i + 1]];
            Vector2 uv3 = uvs[triangles[i + 2]];
            
            DrawLine(texture, uv1, uv2, resolution);
            DrawLine(texture, uv2, uv3, resolution);
            DrawLine(texture, uv3, uv1, resolution);
        }
        
        // Draw UV points
        for (int i = 0; i < uvs.Length; i++)
        {
            Vector2 uv = uvs[i];
            int x = Mathf.RoundToInt(uv.x * resolution);
            int y = Mathf.RoundToInt(uv.y * resolution);
            
            if (x >= 0 && x < resolution && y >= 0 && y < resolution)
            {
                texture.SetPixel(x, y, Color.red);
                
                // Make the point slightly larger
                for (int ox = -1; ox <= 1; ox++)
                {
                    for (int oy = -1; oy <= 1; oy++)
                    {
                        int nx = x + ox;
                        int ny = y + oy;
                        if (nx >= 0 && nx < resolution && ny >= 0 && ny < resolution)
                        {
                            texture.SetPixel(nx, ny, Color.red);
                        }
                    }
                }
            }
        }
        
        texture.Apply();
        return texture;
    }

    private void DrawLine(Texture2D texture, Vector2 from, Vector2 to, int resolution)
    {
        // Bresenham's line algorithm
        int x0 = Mathf.RoundToInt(from.x * resolution);
        int y0 = Mathf.RoundToInt(from.y * resolution);
        int x1 = Mathf.RoundToInt(to.x * resolution);
        int y1 = Mathf.RoundToInt(to.y * resolution);
        
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        
        while (true)
        {
            if (x0 >= 0 && x0 < resolution && y0 >= 0 && y0 < resolution)
                texture.SetPixel(x0, y0, Color.white);
                
            if (x0 == x1 && y0 == y1)
                break;
                
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    void TransferUVs()
    {
        if (sourceObject == null || targetObject == null)
        {
            Debug.LogError("Please select both objects!");
            return;
        }

        // Try to find mesh renderers in both objects
        SkinnedMeshRenderer sourceSMR = sourceObject.GetComponentInChildren<SkinnedMeshRenderer>();
        MeshFilter sourceMF = sourceObject.GetComponentInChildren<MeshFilter>();
        
        SkinnedMeshRenderer targetSMR = targetObject.GetComponentInChildren<SkinnedMeshRenderer>();
        MeshFilter targetMF = targetObject.GetComponentInChildren<MeshFilter>();
        
        Mesh sourceMesh = null;
        Mesh targetMesh = null;
        
        // Get source mesh
        if (sourceSMR != null)
            sourceMesh = sourceSMR.sharedMesh;
        else if (sourceMF != null)
            sourceMesh = sourceMF.sharedMesh;
        else
        {
            Debug.LogError("Source object doesn't have a mesh!");
            return;
        }
        
        // Get target mesh
        if (targetSMR != null)
            targetMesh = targetSMR.sharedMesh;
        else if (targetMF != null)
            targetMesh = targetMF.sharedMesh;
        else
        {
            Debug.LogError("Target object doesn't have a mesh!");
            return;
        }
        
        // Create copies of the meshes
        Mesh sourceMeshCopy = Instantiate(sourceMesh);
        Mesh targetMeshCopy = Instantiate(targetMesh);

        if (sourceMeshCopy.vertexCount != targetMeshCopy.vertexCount)
        {
            Debug.LogError("Meshes must have the same vertex count! Source: " + 
                           sourceMeshCopy.vertexCount + " vs Target: " + 
                           targetMeshCopy.vertexCount);
            return;
        }

        // Copy UVs from source to target based on selected channel
        switch (uvChannel)
        {
            case 0:
                if (sourceMeshCopy.uv.Length > 0)
                    targetMeshCopy.uv = sourceMeshCopy.uv;
                else
                    Debug.LogWarning("Source mesh doesn't have UV channel 0.");
                break;
            case 1:
                if (sourceMeshCopy.uv2.Length > 0)
                    targetMeshCopy.uv2 = sourceMeshCopy.uv2;
                else
                    Debug.LogWarning("Source mesh doesn't have UV channel 1.");
                break;
            case 2:
                if (sourceMeshCopy.uv3.Length > 0)
                    targetMeshCopy.uv3 = sourceMeshCopy.uv3;
                else
                    Debug.LogWarning("Source mesh doesn't have UV channel 2.");
                break;
            case 3:
                if (sourceMeshCopy.uv4.Length > 0)
                    targetMeshCopy.uv4 = sourceMeshCopy.uv4;
                else
                    Debug.LogWarning("Source mesh doesn't have UV channel 3.");
                break;
            default:
                Debug.LogWarning("Unsupported UV channel: " + uvChannel);
                break;
        }

        // Create a new mesh asset
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Mesh", 
            targetMesh.name + "_FixedUV", 
            "asset", 
            "Save mesh with transferred UVs"
        );
        
        if (path.Length > 0)
        {
            AssetDatabase.CreateAsset(targetMeshCopy, path);
            AssetDatabase.SaveAssets();
            
            // Assign the new mesh to the target
            if (targetSMR != null)
                targetSMR.sharedMesh = targetMeshCopy;
            else if (targetMF != null)
                targetMF.sharedMesh = targetMeshCopy;
            
            Debug.Log("UVs successfully transferred and new mesh created at: " + path);
            
            // Refresh the preview
            UpdatePreviews();
        }
    }
}