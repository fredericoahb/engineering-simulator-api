using Xunit;
using EngineeringSimulator.Domain.Calculations;
using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Tests;

public class WobbeCalculatorTests
{
    [Fact]
    public void Calculate_NominalValues_ReturnsCorrectWobbeIndex()
    {
        var result = WobbeCalculator.Calculate(39.0, 0.60);
        var expected = 39.0 / Math.Sqrt(0.60);
        Assert.Equal(expected, result, precision: 6);
    }

    [Fact]
    public void Calculate_UnitDensity_ReturnsPcsUnchanged()
    {
        var result = WobbeCalculator.Calculate(50.0, 1.0);
        Assert.Equal(50.0, result, precision: 10);
    }

    [Fact]
    public void Calculate_SmallDensity_ReturnsLargeWobbe()
    {
        var result = WobbeCalculator.Calculate(40.0, 0.01);
        Assert.True(result > 300.0);
    }

    [Theory]
    [InlineData(0, 0.6)]
    [InlineData(-5, 0.6)]
    public void Calculate_InvalidPcs_ThrowsDomainValidation(double pcs, double density)
    {
        Assert.Throws<DomainValidationException>(() => WobbeCalculator.Calculate(pcs, density));
    }

    [Theory]
    [InlineData(39.0, 0)]
    [InlineData(39.0, -1)]
    public void Calculate_InvalidDensity_ThrowsDomainValidation(double pcs, double density)
    {
        Assert.Throws<DomainValidationException>(() => WobbeCalculator.Calculate(pcs, density));
    }

    [Fact]
    public void Interpret_TypicalNaturalGas_ReturnsGroupH()
    {
        var note = WobbeCalculator.Interpret(50.0);
        Assert.Contains("Group H", note);
    }
}
