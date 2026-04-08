using FluentAssertions;
using Marco.AspNetCore.Cqs.Infra.Data.Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Marco.AspNetCore.Cqs.UnitTests.Common;

public class SqlServerReadOnlySettingsTests
{
    [Fact]
    public void Validate_DeveFalhar_QuandoDefaultConnectionNaoForInformada()
    {
        // Arrange
        var sut = new SqlServerReadOnlySettings { DefaultConnection = null! };
        var context = new ValidationContext(sut);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(sut, context, results, validateAllProperties: true);

        // Assert
        isValid.Should().BeFalse();
        results.Should().Contain(r => r.MemberNames.Contains(nameof(SqlServerReadOnlySettings.DefaultConnection)));
    }

    [Fact]
    public void Validate_DevePassar_QuandoDefaultConnectionForInformada()
    {
        // Arrange
        var sut = new SqlServerReadOnlySettings { DefaultConnection = "Server=.;Database=Fake;" };
        var context = new ValidationContext(sut);
        var results = new List<ValidationResult>();

        // Act
        var isValid = Validator.TryValidateObject(sut, context, results, validateAllProperties: true);

        // Assert
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }
}
