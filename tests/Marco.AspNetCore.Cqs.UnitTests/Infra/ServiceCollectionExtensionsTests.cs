using FluentAssertions;
using Marco.AspNetCore.Cqs.Infra.Data.Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.Infra;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDapper_DeveLancarExcecao_QuandoServicesForNulo()
    {
        // Arrange
        IServiceCollection? services = null;
        var settings = new SqlServerReadOnlySettings { DefaultConnection = "fake" };

        // Act
        Action act = () => services!.AddDapper(settings);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("services");
    }

    [Fact]
    public void AddDapper_DeveLancarExcecao_QuandoSettingsForNulo()
    {
        // Arrange
        var services = new ServiceCollection();
        SqlServerReadOnlySettings? settings = null;

        // Act
        Action act = () => services.AddDapper(settings!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("sqlServerReadOnlySettings");
    }

    [Fact]
    public void AddDapper_DeveRegistrarDependencias()
    {
        // Arrange
        var services = new ServiceCollection();
        var settings = new SqlServerReadOnlySettings { DefaultConnection = "fake" };

        // Act
        services.AddDapper(settings);

        // Assert
        services.Should().Contain(s => s.ServiceType == typeof(SqlServerReadOnlySettings));
        services.Should().Contain(s => s.ServiceType == typeof(ContextReadOnly));
    }

    [Fact]
    public void AddCustomApplicationServices_DeveExecutarSemExcecao()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        Action act = () => services.AddCustomApplicationServices();

        // Assert
        act.Should().NotThrow();
    }
}
