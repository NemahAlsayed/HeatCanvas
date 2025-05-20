using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

class Program
{
    private static int nx = 20, ny = 20; // Matrix size (modifiable)
    private static double[,] temperatureMatrix = new double[20, 20]; // ✅ Default initialization
    private static double deltaX = 1.0; // Default Δx

    static void Main()
    {
        StartWebServer();  // ✅ Start server without premature matrix calculation
    }

    static void CalculateMatrix()
    {
        // ✅ Ensure proper initialization before calculations
        temperatureMatrix = new double[nx, ny];  

        double k = 200;  // Thermal conductivity
        double h_top = 40, h_bottom = 20; // Convection coefficients
        double T_inf_top = 25, T_inf_bottom = 100; // Ambient temperatures
        double q_left = 50, q_right = 25; // Heat sources

        for (int i = 0; i < nx; i++)
        {
            for (int j = 0; j < ny; j++)
            {
                if (j == 0) // Bottom convective boundary
                    temperatureMatrix[i, j] = (2 * k * deltaX + h_bottom * T_inf_bottom) / (h_bottom + 2 * k / deltaX);
                else if (j == ny - 1) // Top convective boundary
                    temperatureMatrix[i, j] = (2 * k * deltaX + h_top * T_inf_top) / (h_top + 2 * k / deltaX);
                else if (i >= nx / 4 && i <= 3 * nx / 4 && j >= ny / 4 && j <= 3 * ny / 4) // Insulated region
                    temperatureMatrix[i, j] = 50;
                else if (i == 0) // Left heat source
                    temperatureMatrix[i, j] = (q_left + temperatureMatrix[i + 1, j] + temperatureMatrix[i, j + 1] + temperatureMatrix[i, j - 1]) / 4;
                else if (i == nx - 1) // Right heat source
                    temperatureMatrix[i, j] = (q_right + temperatureMatrix[i - 1, j] + temperatureMatrix[i, j + 1] + temperatureMatrix[i, j - 1]) / 4;
                else // Interior points
                    temperatureMatrix[i, j] = (temperatureMatrix[i + 1, j] + temperatureMatrix[i - 1, j] + temperatureMatrix[i, j + 1] + temperatureMatrix[i, j - 1]) / 4;
            }
        }
    }

    static void StartWebServer()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // ✅ Manually specify a port to avoid conflicts
        app.Urls.Add("http://0.0.0.0:8080");

        app.MapGet("/", async (HttpContext context) =>
        {
            var sb = new StringBuilder();
            sb.Append("<!DOCTYPE html><html><head><title>Heat Transfer Matrix</title></head><body>");
            sb.Append("<style>table {border-collapse: collapse; text-align: center;} td {padding: 5px; border: 1px solid black;}</style>");
            sb.Append("<h2>Heat Transfer Simulation</h2>");

            // ✅ Single input form for matrix size and Δx
            sb.Append("<form method='get' action='/calculate'>");
            sb.Append("<label>Matrix Size (nx, ny): </label>");
            sb.Append("<input type='number' name='nx' value='" + nx + "' min='5' max='50' />");
            sb.Append("<input type='number' name='ny' value='" + ny + "' min='5' max='50' /><br>");
            sb.Append("<label>Δx: </label>");
            sb.Append("<input type='number' step='0.01' name='dx' value='" + deltaX + "' /><br>");
            sb.Append("<button type='submit'>Calculate</button>");
            sb.Append("</form>");

            sb.Append("<table border='1'>");
            for (int j = 0; j < ny; j++)
            {
                sb.Append("<tr>");
                for (int i = 0; i < nx; i++)
                    sb.Append($"<td>{Math.Round(temperatureMatrix[i, j], 2)}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table></body></html>");

            context.Response.ContentType = "text/html";
            await context.Response.WriteAsync(sb.ToString(), Encoding.UTF8);
        });

        // ✅ Fix null issue with `/calculate`
        app.MapGet("/calculate", async (HttpContext context) =>
        {
            try
            {
                if (context.Request.Query.ContainsKey("nx") && context.Request.Query.ContainsKey("ny") && context.Request.Query.ContainsKey("dx"))
                {
                    int.TryParse(context.Request.Query["nx"], out nx);
                    int.TryParse(context.Request.Query["ny"], out ny);
                    double.TryParse(context.Request.Query["dx"], out deltaX);

                    temperatureMatrix = new double[nx, ny]; // ✅ Ensure matrix initialization before use
                    CalculateMatrix();
                }

                context.Response.Redirect("/");
            }
            catch (Exception ex)
            {
                await context.Response.WriteAsync($"Error: {ex.Message}");
            }
        });

        app.Run();
    }
}
