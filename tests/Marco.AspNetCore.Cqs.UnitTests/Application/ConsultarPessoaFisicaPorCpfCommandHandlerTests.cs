using FluentAssertions;
using Marco.AspNetCore.Cqs.Application.CommandHandlers;
using Marco.AspNetCore.Cqs.Application.Commands;
using Marco.AspNetCore.Cqs.Domain.Models;
using MediatR;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.Application;

public class ConsultarPessoaFisicaPorCpfCommandHandlerTests
{
    [Fact]
    public void Ctor_DeveLancarExcecao_QuandoMediatorForNulo()
    {
        // Arrange
        IMediator? mediator = null;

        // Act
        Action act = () => new ConsultarPessoaFisicaPorCpfCommandHandler(mediator!);

        // Assert
        act.Should().Throw<ArgumentNullException>().WithParameterName("mediator");
    }

    [Fact]
    public async Task Handle_DeveEncaminharQueryParaMediatorERetornarPessoaFisica()
    {
        // Arrange
        const string cpf = "11144477735";
        var command = new ConsultarPessoaFisicaPorCpfCommand(cpf);
        var cancellationToken = new CancellationTokenSource().Token;
        var expected = new PessoaFisica { Cpf = "111.444.777-35", Nome = "Maria" };

        var mediatorMock = new Mock<IMediator>();
        mediatorMock
            .Setup(x => x.Send(It.IsAny<arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery>(), cancellationToken))
            .ReturnsAsync(expected);

        var sut = new ConsultarPessoaFisicaPorCpfCommandHandler(mediatorMock.Object);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSameAs(expected);
        mediatorMock.Verify(x => x.Send(
            It.Is<arco.AspNetCore.Cqs.Infra.Data.Dapper.CQS.Queries.ConsultarPessoaFisicaPorCpfQuery>(q => q.Cpf.Numero == cpf),
            cancellationToken), Times.Once);
    }
}
