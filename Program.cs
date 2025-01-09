using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Windows.Forms;

namespace CompanyManagementApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                Log.Information("Starting application");

                var services = ConfigureServices();
                using var scope = services.CreateScope();

                // Get DataContext and ensure the database and tables are created
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();

                try
                {
                    // Apply any pending migrations
                    context.Database.Migrate();
                    Log.Information("Database migrations applied successfully.");
                }
                catch (Exception migrationEx)
                {
                    Log.Error(migrationEx, "Error applying migrations.");
                    MessageBox.Show($"Error applying migrations: {migrationEx.Message}", "Migration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;  // Exit if migrations fail
                }

                // Seed initial data
                var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
                seeder.Seed();
                Log.Information("Database seeding completed.");

                // Start the application with the login form
                var authController = scope.ServiceProvider.GetRequiredService<AuthController>();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new LoginForm(authController, scope.ServiceProvider));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly.");
                MessageBox.Show($"Application error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Register DbContext with Scoped lifetime
                .AddDbContext<DataContext>(options => options.UseSqlite("Data Source=companyinfo.db"), ServiceLifetime.Scoped)

                // Register Services with Scoped lifetime
                .AddScoped<DataSeeder>()
                .AddScoped<AuthService>()
                .AddScoped<AuthController>()
                .AddScoped<EmployeeService>()
                .AddScoped<EmployeeController>()
                .AddScoped<ProjectService>()
                .AddScoped<ProjectController>()
                .AddScoped<ClientService>()
                .AddScoped<ClientController>()

                // Register Forms with Transient lifetime to allow multiple instances
                 .AddTransient<RegisterForm>()  // Add RegisterForm registration
                .AddTransient<LoginForm>()
                .AddTransient<DashboardForm>()
                .AddTransient<ListEmployeesForm>()
                .AddTransient<ProjectManagementForm>()  // Register Project Management Form
                .AddTransient<ProjectForm>()            // Register Project Form
                .AddTransient<ClientManagementForm>()  // Add Client Management Form
                .BuildServiceProvider();
        }
    }
}
