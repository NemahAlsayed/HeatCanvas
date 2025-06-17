#HeatCanvas

**HeatCanvas** is a dynamic C# simulation tool that calculates and visualizes steady-state heat distribution across a 2D surface using finite difference methods. Built with c# it allows users to specify custom grid dimensions and spatial resolution.

## 🔥 Features
- Customizable grid size (`nx`, `ny`) and spacing (`Δx`)
- Implements convective boundary conditions and fixed temperature zones
- Interactive UI for entering parameters and viewing results
- Displays results in a scrollable, responsive DataGridView

## 🧠 How It Works
The app solves the 2D heat equation numerically by applying boundary conditions:
- **Top and Bottom Edges:** Convective conditions  
- **Center Area:** Insulated zone  
- **Left and Right Edges:** Heat flux imposed  

Temperature values are then rendered in a tabular grid representing the domain.

## 🚀 Getting Started
1. Clone this repo:  
   ```bash
   git clone https://github.com/NemahAlsayed/HeatCanvas.git
