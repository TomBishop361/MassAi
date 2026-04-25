# MassAi

A small game project demonstrating the use of **flow-fields** for intelligent agent pathfinding and movement. Flow-fields update and change dynamically when buildings are placed on the grid, creating an adaptive navigation system for AI-controlled entities.

![MassAi Demo](https://github.com/user-attachments/assets/ebc06500-5a36-48ca-82ea-3309066ebc30)

## Overview

MassAi showcases an efficient approach to multi-agent pathfinding using flow-field algorithms. Instead of computing individual paths for each agent, the system creates a unified flow-field that guides all agents toward their destinations while dynamically adapting to environmental changes such as building placement.

### Key Features

-  **Flow-Field Pathfinding**: Efficient grid-based navigation system for multiple agents
-  **Dynamic Obstacles**: Buildings can be placed on the grid with real-time flow-field recalculation
-  **Multi-Agent Coordination**: Smooth movement of multiple AI agents using shared flow-fields
-  **Performance Optimized**: Designed for efficient pathfinding of large numbers of agents

## Technology Stack

| Language | Percentage | Purpose |
|----------|-----------|---------|
| **ShaderLab** | 52.8% | GPU-accelerated rendering and visual effects |
| **C#** | 27.9% | Core game logic and AI systems |
| **HLSL** | 10.9% | Advanced shader programming for graphics |
| **Mathematica** | 8.4% | Mathematical computations and algorithm analysis |

## Project Structure

This project is built primarily for **Unity** with a focus on:

- **Game Logic**: C# for pathfinding algorithms, agent behavior, and game mechanics
- **Analysis**: Mathematica for mathematical modeling and algorithm validation

## How It Works

### Flow-Fields Algorithm

1. **Grid-Based Navigation**: The world is divided into a grid of cells
2. **Field Calculation**: A flow-field is computed from the destination, storing movement vectors for each cell
3. **Agent Movement**: Agents follow the flow-field vectors, moving toward the goal
4. **Dynamic Updates**: When buildings are placed, the flow-field is recalculated to avoid obstacles

### Building Placement

When a building is placed on the grid:
- Affected cells in the flow-field are marked as obstacles
- The flow-field is recalculated for pathways around the new structure
- Agents automatically adapt their movement to the updated field

## Performance Considerations

- Flow-fields are more efficient than traditional pathfinding for large numbers of agents
- Suitable for games requiring hundreds of agents with reactive pathfinding

---

For more information, questions, or suggestions, please open an issue on the repository.
# MassAi

A small game project demonstrating the use of **flow-fields** for intelligent agent pathfinding and movement. Flow-fields update and change dynamically when buildings are placed on the grid, creating an adaptive navigation system for AI-controlled entities.

![MassAi Demo](https://github.com/user-attachments/assets/ebc06500-5a36-48ca-82ea-3309066ebc30)

## Overview

MassAi showcases an efficient approach to multi-agent pathfinding using flow-field algorithms. Instead of computing individual paths for each agent, the system creates a unified flow-field that guides all agents toward their destinations while dynamically adapting to environmental changes such as building placement.

### Key Features

-  **Flow-Field Pathfinding**: Efficient grid-based navigation system for multiple agents
-  **Dynamic Obstacles**: Buildings can be placed on the grid with real-time flow-field recalculation
-  **Multi-Agent Coordination**: Smooth movement of multiple AI agents using shared flow-fields
-  **Performance Optimized**: Designed for efficient pathfinding of large numbers of agents

## Technology Stack

| Language | Percentage | Purpose |
|----------|-----------|---------|
| **ShaderLab** | 52.8% | GPU-accelerated rendering and visual effects |
| **C#** | 27.9% | Core game logic and AI systems |
| **HLSL** | 10.9% | Advanced shader programming for graphics |
| **Mathematica** | 8.4% | Mathematical computations and algorithm analysis |

## Project Structure

This project is built primarily for **Unity** with a focus on:

- **Game Logic**: C# for pathfinding algorithms, agent behavior, and game mechanics
- **Analysis**: Mathematica for mathematical modeling and algorithm validation

## How It Works

### Flow-Fields Algorithm

1. **Grid-Based Navigation**: The world is divided into a grid of cells
2. **Field Calculation**: A flow-field is computed from the destination, storing movement vectors for each cell
3. **Agent Movement**: Agents follow the flow-field vectors, moving toward the goal
4. **Dynamic Updates**: When buildings are placed, the flow-field is recalculated to avoid obstacles

### Building Placement

When a building is placed on the grid:
- Affected cells in the flow-field are marked as obstacles
- The flow-field is recalculated for pathways around the new structure
- Agents automatically adapt their movement to the updated field

## Performance Considerations

- Flow-fields are more efficient than traditional pathfinding for large numbers of agents
- Suitable for games requiring hundreds of agents with reactive pathfinding

---

For more information, questions, or suggestions, please open an issue on the repository.

https://github.com/user-attachments/assets/ebc06500-5a36-48ca-82ea-3309066ebc30

