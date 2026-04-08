using AutoMapper;
using FluentAssertions;
using Marco.AspNetCore.Cqs.Application.Commands;
using Marco.AspNetCore.Cqs.Domain.Models;
using Marco.AspNetCore.Cqs.WebApi;
using Marco.AspNetCore.Cqs.WebApi.Controllers.v1;
using Marco.AspNetCore.Cqs.WebApi.Models.v1;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.WebApi;

public class PessoasFisicasControllerTests
{
    private static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<WebApiAutoMapperProfile>());
        return config.CreateMapper();
    }

    [Fact]
    public void Ctor_DeveLancarExcecao_QuandoMediatorForNulo()
    {
        // Arrange
        IMediator? mediator = null;
        var mapper = CreateMapper();

        // Act
        Action act = () => new PessoasFisicasController(mediator!, mapper);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("mediator");
    }

    [Fact]
    public void Ctor_DeveLancarExcecao_QuandoMapperForNulo()
    {
        // Arrange
        var mediator = new Mock<IMediator>().Object;
        IMapper? mapper = null;

        // Act
        Action act = () => new PessoasFisicasController(mediator, mapper!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("mapper");
    }

    [Fact]
    public async Task GetByCpfCommandAsync_DeveRetornarNotFound_QuandoPessoaNaoEncontrada()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<ConsultarPessoaFisicaPorCpfCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PessoaFisica?)null);

        var sut = new PessoasFisicasController(mediatorMock.Object, CreateMapper());

        // Act
        var result = await sut.GetByCpfCommandAsync("11144477735");

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetByCpfCommandAsync_DeveRetornarOk_QuandoPessoaEncontrada()
    {
        // Arrange
        var pessoa = new PessoaFisica
        {
            Cpf = "111.444.777-35",
            Nome = "Maria",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<ConsultarPessoaFisicaPorCpfCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pessoa);

        var sut = new PessoasFisicasController(mediatorMock.Object, CreateMapper());

        // Act
        var actionResult = await sut.GetByCpfCommandAsync("11144477735");

        // Assert
        var result = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = result.Value.Should().BeOfType<PessoaFisicaGetResult>().Subject;
        payload.Cpf.Should().Be(pessoa.Cpf);
        payload.Nome.Should().Be(pessoa.Nome);
        payload.DataNascimento.Should().Be(pessoa.DataNascimento);
    }

    [Fact]
    public async Task GetByCpfQueryAsync_DeveRetornarNotFound_QuandoPessoaNaoEncontrada()
    {
        // Arrange
        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PessoaFisica?)null);

        var sut = new PessoasFisicasController(mediatorMock.Object, CreateMapper());

        // Act
        var result = await sut.GetByCpfQueryAsync("11144477735");

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetByCpfQueryAsync_DeveRetornarOk_QuandoPessoaEncontrada()
    {
        // Arrange
        var pessoa = new PessoaFisica
        {
            Cpf = "111.444.777-35",
            Nome = "Maria",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pessoa);

        var sut = new PessoasFisicasController(mediatorMock.Object, CreateMapper());

        // Act
        var actionResult = await sut.GetByCpfQueryAsync("11144477735");

        // Assert
        var result = actionResult.Should().BeOfType<OkObjectResult>().Subject;
        var payload = result.Value.Should().BeOfType<PessoaFisicaGetResult>().Subject;
        payload.Cpf.Should().Be(pessoa.Cpf);
        payload.Nome.Should().Be(pessoa.Nome);
        payload.DataNascimento.Should().Be(pessoa.DataNascimento);
    }
}
