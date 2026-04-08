using AutoMapper;
using FluentAssertions;
using Marco.AspNetCore.Cqs.Domain.Models;
using Marco.AspNetCore.Cqs.WebApi;
using Marco.AspNetCore.Cqs.WebApi.Models.v1;
using System;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.WebApi;

public class WebApiAutoMapperProfileTests
{
    [Fact]
    public void Profile_DeveSerValido()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<WebApiAutoMapperProfile>());

        // Act
        Action act = () => config.AssertConfigurationIsValid();

        // Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Profile_DeveMapearPessoaFisicaParaDto()
    {
        // Arrange
        var config = new MapperConfiguration(cfg => cfg.AddProfile<WebApiAutoMapperProfile>());
        var mapper = config.CreateMapper();
        var source = new PessoaFisica
        {
            Cpf = "111.444.777-35",
            Nome = "Maria",
            DataNascimento = new DateTime(1990, 1, 1)
        };

        // Act
        var result = mapper.Map<PessoaFisicaGetResult>(source);

        // Assert
        result.Cpf.Should().Be(source.Cpf);
        result.Nome.Should().Be(source.Nome);
        result.DataNascimento.Should().Be(source.DataNascimento);
    }
}
