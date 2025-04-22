# UV Transfer Tool

A Unity editor tool for transferring and manipulating UV coordinates between meshes, making texture mapping workflows faster and easier.

## Features

- Transfer UVs from one mesh to another with similar topology
- Project UVs from a source mesh onto a target mesh using position, normal, or ray-based mapping
- Automatically detect and handle seams and overlapping areas
- Preserve UV islands when transferring between different mesh resolutions
- Support for multiple UV channels and UDIM workflows
- Batch processing for multiple meshes
- Undo/redo compatibility
- Real-time preview of UV transfer results

## Installation

### Via Unity Package Manager

1. Open your Unity project
2. Go to Window > Package Manager
3. Click the "+" button and select "Add package from disk..."
4. Navigate to the UVTransferTool folder and select the `package.json` file
5. Click "Add"

### Manual Installation

1. Copy the entire UVTransferTool folder into your Unity project's "Packages" directory
2. Restart Unity or refresh the package list

## Quick Start

1. Open the UV Transfer Tool window: 
   - Go to Window > UV Tools > UV Transfer Tool

2. Basic UV Transfer:
   - Select your target mesh (the mesh to receive new UVs)
   - Assign the source mesh (the mesh with the UVs you want to transfer)
   - Choose the mapping method
   - Click "Transfer UVs"

## Usage Examples

### Simple UV Transfer

```csharp
// You can also access the tool via scripting:
using Inimart.UVTransferTool;

// Get meshes
Mesh sourceMesh = sourceObject.GetComponent<MeshFilter>().sharedMesh;
Mesh targetMesh = targetObject.GetComponent<MeshFilter>().sharedMesh;

// Transfer UVs from channel 0
UVTransfer.TransferUVs(sourceMesh, targetMesh, UVTransferMethod.NearestPoint, 0);

// Apply the modified mesh
targetObject.GetComponent<MeshFilter>().sharedMesh = targetMesh;
```

### Advanced UV Projection

For complex meshes where direct transfer doesn't work well, try using the projection method:

1. Position your source and target meshes in the same space
2. In the UV Transfer Tool window:
   - Select "Projection" as the mapping method
   - Adjust the projection parameters
   - Enable "Show Preview" to see the results before applying
   - Click "Transfer UVs" when satisfied

## Advanced Options

- **Smoothing**: Apply smoothing to the transferred UVs to reduce distortion
- **Island Preservation**: Keep UV islands intact during transfer
- **Custom Mapping**: Define your own vertex correspondences for precise control
- **Batch Processing**: Process multiple meshes with the same settings

## Requirements

- Unity 2021.3 or newer
- Recommended: Unity with ProBuilder for editing mesh assets

## Troubleshooting

- **Distorted UVs**: Try adjusting the mapping method or increasing the sampling density
- **Missing UV Areas**: Check for disconnected vertices or non-manifold geometry in your meshes
- **Performance Issues**: For very large meshes, try using the "Approximate" mode

## Contributing

To contribute to the project, contact the authors or open an issue on GitHub.

## License

This package is distributed under the MIT License. See [LICENSE.md](LICENSE.md) for details.
