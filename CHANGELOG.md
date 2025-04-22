# Changelog

## [1.0.0] – 2024‑05‑13

### Added

- **Initial public release** of the **UV Transfer Tool**.
- `UVTransferTool` EditorWindow under **Tools ▸ UV Transfer Tool**.
- Support for transferring UV channels **0–3** between meshes with identical vertex counts.
- Live 512×512 UV previews for both source and target meshes.
- Automatic validation of vertex counts and informative console messages.
- Works with meshes referenced by **MeshFilter** _or_ **SkinnedMeshRenderer**.
- Creates a new mesh asset (`<OriginalName>_FixedUV.asset`) to preserve the original mesh.
- Documentation: README and detailed user guide (see `Documentation~/UVTransferTool-Guide.md`).
