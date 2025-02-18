# CLIRayMarchingEngine

CLIRayMarchingEngine is a command-line ray marching engine that renders scenes using ray marching techniques. It supports various geometric primitives, smooth blending, and lighting effects.

## Features

- Ray marching for rendering scenes
- Support for various geometric primitives (Box, Capsule, Octahedron, Orb, Plane)
- Smooth blending between objects
- Blinn-Phong lighting model
- 3 predefined scenes with different objects
- User controls for extremely basic scene navigation


2. Use the following controls to interact with the scenes:
    - `W`: Move forward
    - `A`: Move left
    - `S`: Move backward
    - `D`: Move right
    - `Q`: Stop rotating the light
    - `E`: Start rotating the light
    - `Spacebar`: Switch to the next scene
    - `Escape`: Exit the application

## Scenes

The engine comes with three predefined scenes:

1. **Scene One**: A basic scene with various geometric primitives.
2. **Scene Two**: A scene with smooth blending between objects.
3. **Scene Three**: An endless repeater scene with infinite primitives in every direction.

## Project Structure

- `CLIRayMarchingEngine/Controls/`: Contains user control handling classes.
- `CLIRayMarchingEngine/Model/`: Contains model classes for geometric primitives and scenes.
- `CLIRayMarchingEngine/View/`: Contains view classes for rendering the scenes.
- `CLIRayMarchingEngine/SceneSetups.cs`: Contains methods for setting up predefined scenes.
- `CLIRayMarchingEngine/HelpMaths.cs`: Contains helper math functions for smooth blending and lighting calculations.
- `CLIRayMarchingEngine/Program.cs`: The main entry point of the application.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request with your changes.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [Maths](https://iquilezles.org/articles/distfunctions/) taken from **Inigo Quilez**, obviously.
- [Blinn-Phong reflection model](https://en.wikipedia.org/wiki/Blinn%E2%80%93Phong_reflection_model) for the lighting model.