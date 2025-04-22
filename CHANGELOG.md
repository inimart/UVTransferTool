# Changelog

## [1.0.0] - 2024-09-15

### Added
- Initial release of UV Transfer Tool
- Core UV transfer functionality with multiple mapping methods:
  - Nearest point
  - Position-based
  - Normal-based
  - Ray projection
- Custom editor window with transfer settings and preview
- Support for transferring multiple UV channels
- Batch processing capability for multiple meshes
- Integration with Unity's Undo/Redo system
- Comprehensive mesh validation and error reporting
- Real-time preview of UV mapping results

### Known Issues
- Performance may degrade with extremely high-poly meshes (>100k vertices)
- UDIM support is preliminary and may require additional refinement in some cases
- Requires mesh objects to be readable in Unity

## [0.9.0] - 2024-09-05

### Added
- Beta release for internal testing
- Core mapping algorithms implemented
- Basic UI for the editor window

### Fixed
- Fixed UV island preservation in complex meshes
- Addressed seam handling in normal-based projection

## [0.8.0] - 2024-09-01

### Added
- Alpha release for limited user testing
- Initial implementation of UV transfer algorithms
- Prototype editor interface

### Known Issues
- Limited error handling
- No support for mesh validation
