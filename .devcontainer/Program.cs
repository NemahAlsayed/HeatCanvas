using System;
using Gtk;

class HeatTransferApp : Window
{
    private const int nx = 20, ny = 20; // Matrix size
    private double[,] temperatureMatrix = new double[nx, ny];

    public HeatTransferApp() : base("Heat Transfer Matrix")
    {
        SetDefaultSize(600, 400);
        InitMatrix();
        ShowAll();
    }

    private void InitMatrix()
    {
        for (int i = 0; i < nx; i++)
            for (int j = 0; j < ny; j++)
                temperatureMatrix[i, j] = CalculateTemperature(i, j);
    }

    private double CalculateTemperature(int i, int j)
    {
        return (i + j) % 100; // Simplified calculation
    }

    static void Main()
    {
        Application.Init();
        new HeatTransferApp();
        Application.Run();
    }
}
