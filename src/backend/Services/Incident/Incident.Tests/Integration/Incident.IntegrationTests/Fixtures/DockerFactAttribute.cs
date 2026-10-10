using System.Diagnostics;

namespace Incident.IntegrationTests.Fixtures;


public static class DockerCheck
{
    public static readonly Lazy<bool> IsAvailable = new(() =>
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "docker",
                Arguments = "info --format '{{.ServerVersion}}'",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            });

            if (process is null) return false;
            if (!process.WaitForExit(15_000)) { process.Kill(entireProcessTree: true); return false; }
            return process.ExitCode == 0;
        }
        catch
        {
            return false;
        }
    });
}

public sealed class DockerFactAttribute : FactAttribute
{
    public DockerFactAttribute()
    {
        if (!DockerCheck.IsAvailable.Value)
            Skip = "Docker no está disponible: este test necesita un daemon de Docker para levantar MongoDB (Testcontainers).";
    }
}
