# UV Transfer Tool - Complete Guide

## Overview

The UV Transfer Tool is designed to solve one of the most common challenges in 3D content creation: transferring and manipulating UV coordinates between different meshes. Whether you're working with character models, environments, or props, this tool streamlines your texture mapping workflow.

This guide will walk you through all features of the UV Transfer Tool and provide tips for getting the best results.

## Table of Contents

- [Installation](#installation)
- [Basic Concepts](#basic-concepts)
- [The UV Transfer Window](#the-uv-transfer-window)
- [Transfer Methods](#transfer-methods)
- [Advanced Options](#advanced-options)
- [Batch Processing](#batch-processing)
- [Scripting API](#scripting-api)
- [Troubleshooting](#troubleshooting)
- [Tips and Best Practices](#tips-and-best-practices)

## Installation

1. Open your Unity project
2. Go to Window > Package Manager
3. Click the "+" button and select "Add package from disk..."
4. Navigate to the UVTransferTool folder and select the `package.json` file
5. Click "Add"

After installation, you can access the tool via Window > UV Tools > UV Transfer Tool.

## Basic Concepts

Before diving into the tool, it's important to understand some basic concepts:

- **Source Mesh**: The mesh with the UVs you want to transfer from
- **Target Mesh**: The mesh that will receive the new UVs
- **UV Channel**: Unity supports multiple UV sets per mesh (usually channel 0 is the main one)
- **Mapping Method**: The algorithm used to determine how UVs are transferred
- **UV Islands**: Connected groups of UV coordinates that represent distinct parts of the mesh

## The UV Transfer Window

![UV Transfer Window](UVTransferTool-Window.png)

The main window consists of several sections:

1. **Mesh Selection**
   - Source Mesh: Assign the mesh with the UVs you want to copy
   - Target Mesh: Assign the mesh that will receive the UVs

2. **UV Settings**
   - Source UV Channel: Select which UV channel to copy from (0-7)
   - Target UV Channel: Select which UV channel to write to (0-7)
   - Generate New UVs: Toggle to create a completely new UV set instead of copying

3. **Transfer Settings**
   - Mapping Method: Choose how UVs are transferred (see next section)
   - Sampling Density: Controls the precision of the transfer
   - Smoothing: Apply smoothing to the result

4. **Preview Options**
   - Show Preview: Toggle real-time preview
   - Preview Channel: Which channel to preview
   - Opacity: Control the preview overlay opacity

5. **Actions**
   - Transfer UVs: Execute the UV transfer
   - Revert: Restore original UVs
   - Save Mesh: Save the result as a new mesh asset

## Transfer Methods

The tool offers multiple mapping methods, each suited for different scenarios:

### Nearest Point

Transfers UVs based on the closest point on the source mesh. This is the fastest method and works well when:
- Meshes have similar shapes
- Meshes are aligned in the same space
- Vertex counts are similar

```
Settings to adjust:
- Max Distance: Maximum distance to search for corresponding points
- Use Spatial Hash: Accelerate search using spatial hashing (recommended for large meshes)
```

### Topology-Based

Uses topological information to transfer UVs. Best when:
- Meshes have identical topology but different shapes
- Preserving UV seams and islands is critical

```
Settings to adjust:
- Topology Threshold: How strictly to follow topology
- Edge Flow Preservation: Maintain edge flow direction
```

### Projection-Based

Projects UVs from source to target using ray casting. Ideal when:
- Meshes have very different topologies
- Working with high-to-low poly transfers
- Creating UVs for displacement maps

```
Settings to adjust:
- Projection Direction: Custom or automatic direction
- Ray Distance: Maximum distance for ray projection
- Normal Bias: Offset along normals to prevent self-intersection
```

### Parametric Mapping

Creates a parametric mapping between meshes. Works best for:
- Organic shapes with different topologies
- Character meshes at different poses
- Objects with continuous surfaces

```
Settings to adjust:
- Feature Points: Number of feature points to use
- Relaxation Iterations: Higher values give smoother results
```

## Advanced Options

### UV Island Preservation

The tool can automatically detect and preserve UV islands:

1. Enable "Preserve UV Islands" in the Advanced Options
2. Adjust the "Island Detection Threshold" to control how islands are identified
3. Use "Island Visualization" to see the detected islands before transfer

### Seam Handling

Control how UV seams are managed during transfer:

1. Enable "Detect Seams" to identify UV seams in the source mesh
2. Choose a seam handling method:
   - Preserve: Maintain source seams
   - Optimize: Create new optimized seams
   - Ignore: Disregard seams (may cause stretching)

### Custom Weight Maps

For precise control over the transfer process:

1. Enable "Use Weight Map"
2. Assign a grayscale texture as the weight map
3. Areas with higher weight (whiter) will follow the source UVs more closely

## Batch Processing

To process multiple meshes at once:

1. Click "Batch Processing" in the main window
2. Add target meshes to the list
3. Configure batch settings (same as single transfer)
4. Click "Process Batch" to apply settings to all meshes

## Scripting API

The tool can be accessed through code for pipeline integration:

```csharp
using Inimart.UVTransferTool;

// Basic transfer
UVTransfer.TransferUVs(sourceMesh, targetMesh, UVTransferMethod.NearestPoint, 0);

// Advanced transfer with options
UVTransferOptions options = new UVTransferOptions
{
    SourceUVChannel = 0,
    TargetUVChannel = 0,
    MappingMethod = UVTransferMethod.Projection,
    SamplingDensity = 0.8f,
    ApplySmoothing = true,
    SmoothingIterations = 2,
    PreserveSeams = true
};

UVTransfer.TransferUVs(sourceMesh, targetMesh, options);

// Get preview data without applying
UVPreviewData previewData = UVTransfer.GeneratePreview(sourceMesh, targetMesh, options);
```

## Troubleshooting

### Common Issues

1. **Distorted UVs**
   - Try a different mapping method
   - Increase the sampling density
   - Check that meshes are properly aligned
   - Ensure normals are properly oriented

2. **Missing UV Areas**
   - Check for disconnected vertices
   - Increase the search/ray distance
   - Verify the mesh has proper topology

3. **Poor Performance**
   - Use the "Approximate" option for large meshes
   - Reduce the sampling density
   - Enable "Use Spatial Hash" option

### Error Messages

- **"Source mesh not readable"**: Set the mesh to be readable in import settings
- **"Invalid UV channel"**: Unity supports channels 0-7, check your selection
- **"Mesh has no UVs"**: Ensure the source mesh has UVs in the selected channel

## Tips and Best Practices

1. **Preparing Your Meshes**
   - Ensure meshes have clean topology before transfer
   - For best results, align source and target in a similar pose/position
   - Use meshes with comparable vertex density for direct transfers

2. **Choosing the Right Method**
   - Nearest Point: For similar meshes or quick tests
   - Topology-Based: For preserving exact UV layout
   - Projection: For different topology meshes
   - Parametric: For organic or character meshes

3. **Optimizing Results**
   - Start with default settings and refine as needed
   - Use the preview to check results before applying
   - For character models, transfer UVs in a T-pose or reference pose
   - Save intermediate results when working with complex transfers

4. **Pipeline Integration**
   - Use the scripting API for batch processing in larger projects
   - Create presets for commonly used transfer settings
   - Consider using version control for mesh assets before major UV changes 