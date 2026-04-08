using FluentAssertions;
using Marco.AspNetCore.Cqs.Infra.Data.Dapper;
using Marco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.QueryHandlers;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.Infra;

public class ConsultarPessoaFisicaPorCpfQueryHandlerTests
{
    [Fact]
    public void Ctor_DeveLancarExcecao_QuandoContextForNulo()
    {
        // Arrange
        ContextReadOnly? context = null;

        // Act
        Action act = () => new ConsultarPessoaFisicaPorCpfQueryHandler(context!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("contextReadOnly");
    }

    [Fact]
    public async Task Handle_DeveRetornarNull_QuandoCpfForNulo()
    {
        // Arrange
        var settings = new SqlServerReadOnlySettings { DefaultConnection = "Server=.;Database=Fake;User Id=sa;Password=123;" };
        var context = new ContextReadOnly(settings);
        var sut = new ConsultarPessoaFisicaPorCpfQueryHandler(context);
        var query = new arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery("11144477735");

        var cpfProperty = query.GetType().GetProperty("Cpf", BindingFlags.Instance | BindingFlags.Public);
        cpfProperty!.SetValue(query, null);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_DeveRetornarPessoaFake_QuandoCpfForValido()
    {
        // Arrange
        var settings = new SqlServerReadOnlySettings { DefaultConnection = "Server=.;Database=Fake;User Id=sa;Password=123;" };
        var context = new ContextReadOnly(settings);
        var query = new arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery("11144477735");
        var sut = new ConsultarPessoaFisicaPorCpfQueryHandler(context);

        // Act
        var result = await sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Cpf.Should().Be("111.444.777-35");
        result.Nome.Should().Be("Maria da Silva");
        result.DataNascimento.Should().BeCloseTo(DateTime.Now.AddYears(-36), precision: TimeSpan.FromSeconds(2));
    }
}
