# UV Transfer Tool – Detailed User Guide


## 1. Introduction

The **UV Transfer Tool** is a Unity Editor extension that copies UV coordinates from one mesh to another _provided both meshes share identical vertex counts_.  
It is especially useful when you create multiple LODs or mesh variants that should reuse a common texture layout.

The tool lives entirely in the editor and does **not** run in player builds.

---

## 2. Opening the Window

`Tools ▸ UV Transfer Tool`

![Menu screenshot](UVTransferTool.png)

> If the menu item is missing make sure `UVTransferTool.cs` resides in a folder named **Editor** so Unity treats it as an editor‑only script.

---

## 3. Window Overview

| UI Element        | Description                                                                                                             |
|-------------------|-------------------------------------------------------------------------------------------------------------------------|
| **Source**        | GameObject that owns the mesh _with_ the UVs you want to copy. Can be a **MeshFilter** or **SkinnedMeshRenderer**.      |
| **Target**        | GameObject that owns the mesh that should _receive_ the UVs.                                                            |
| **UV Channel**    | Integer (0‑3). Channel 0 is the primary texture UV, 1 is usually used for lightmaps, 2‑3 are free for custom data.      |
| **UV Preview**    | Toggle preview rendering on/off. Previews are rendered at 512 × 512 resolution in real time.                            |
| **Transfer UVs**  | Executes the copy process. Opens a Save‑File dialogue for the resulting mesh asset.                                     |

Below the controls two preview areas visualize the source and target UV layouts when the **UV Preview** toggle is enabled.

---

## 4. Step‑by‑Step Instructions

1. **Select Source Mesh** – drag & drop a GameObject that has a MeshFilter or SkinnedMeshRenderer component into the **Source** slot.
2. **Select Target Mesh** – do the same for the **Target** slot.
3. **Pick UV Channel** – enter 0‑3 to choose which channel to copy. The same index on the target mesh will be overwritten.
4. **(Optional) Inspect Previews** – enable **UV Preview** to double‑check the layouts.
5. **Transfer UVs** – click the button. You are asked where to save the new mesh asset (default name `<TargetMesh>_FixedUV.asset`).
6. **Done** – the tool assigns the new mesh to the target object, leaving the original mesh unmodified.

---

## 5. How the Process Works Internally

1. Both source and target meshes are duplicated in memory so the originals stay untouched.
2. Vertex counts are compared – the transfer is aborted if they differ.
3. The specified UV array (`uv`, `uv2`, `uv3`, or `uv4`) from the **source** copy is assigned to the **target** copy.
4. The user chooses an asset path; the target copy is saved as a new `.asset` file.
5. The new mesh is assigned to the target renderer.

---

## 6. UV Preview Explained

* White lines – edges of UV triangles
* Red dots       – vertex positions (drawn slightly larger for visibility)
* Grey grid      – background 0‑1 UV space

> The preview is _approximate_. Highly dense meshes may show aliasing due to the fixed 512 px resolution.

---


## 7. Troubleshooting

| Symptom                                                                                       | Likely Cause & Fix                                                                                      |
|------------------------------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------------|
| **Error:** "Meshes must have the same vertex count"                                            | Source and target do not share topology. They must originate from the same modelling base.              |
| **Warning:** "Source mesh doesn't have UV channel X"                                           | The chosen channel on the source mesh is empty. Pick another channel or create UVs in your DCC app.     |
| The **Transfer UVs** button is disabled                                                        | One or both object fields are empty. Assign valid GameObjects.                                          |
| UV previews appear blank                                                                       | The selected channel contains no UVs; disable preview or switch channels.                              |
| Menu entry missing                                                                             | Script not inside an `Editor` folder. Move `UVTransferTool.cs` there.                                   |

---

## 8. Known Limitations

* Requires **identical vertex count** – does not attempt remapping or matching by position.
* Transfers **one** UV channel at a time; repeated runs are needed for multiple channels.
* Preview resolution is fixed; complex UVs may appear cramped.
* Editor‑only – cannot be executed at runtime.

---

## 9. FAQ

**Q:** _Can I transfer UVs between meshes with different vertex counts if they look the same?_  
**A:** No. The tool purely copies array data; mismatching indices would corrupt the layout.

**Q:** _Does it support ProBuilder meshes?_  
**A:** Any mesh that ends up as a `Mesh` asset with read‑write enabled works, including ProBuilder‑generated meshes.

**Q:** _Will materials be updated automatically?_  
**A:** Materials remain untouched – only the mesh asset is swapped.
