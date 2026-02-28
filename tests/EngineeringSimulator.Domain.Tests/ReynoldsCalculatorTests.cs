using Xunit;
using EngineeringSimulator.Domain.Calculations;
using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Tests;

public class ReynoldsCalculatorTests
{
    [Fact]
    public void Calculate_NominalPipeFlow_ReturnsCorrectRe()
    {
        var result = ReynoldsCalculator.Calculate(998, 1.5, 0.05, 0.001);
        Assert.Equal(74850, result, precision: 2);
    }

    [Fact]
    public void Calculate_LaminarFlow_ClassifiesCorrectly()
    {
        var result = ReynoldsCalculator.Calculate(1.0, 0.01, 0.01, 0.001);
        Assert.Equal("Laminar", ReynoldsCalculator.Classify(result));
    }

    [Fact]
    public void Calculate_TransitionFlow_ClassifiesCorrectly()
    {
        var result = ReynoldsCalculator.Calculate(1000, 0.003, 1.0, 0.001);
        Assert.Equal(3000, result, precision: 2);
        Assert.Equal("Transition", ReynoldsCalculator.Classify(result));
    }

    [Fact]
    public void Calculate_TurbulentFlow_ClassifiesCorrectly()
    {
        var result = ReynoldsCalculator.Calculate(998, 2.0, 0.1, 0.001);
        Assert.True(result >= 4000);
        Assert.Equal("Turbulent", ReynoldsCalculator.Classify(result));
    }

    [Fact]
    public void Classify_ExactBoundary2300_ReturnsTransition()
    {
        Assert.Equal("Transition", ReynoldsCalculator.Classify(2300));
    }

    [Fact]
    public void Classify_ExactBoundary4000_ReturnsTurbulent()
    {
        Assert.Equal("Turbulent", ReynoldsCalculator.Classify(4000));
    }

    [Theory]
    [InlineData(0, 1, 1, 1)]
    [InlineData(1, 0, 1, 1)]
    [InlineData(1, 1, 0, 1)]
    [InlineData(1, 1, 1, 0)]
    [InlineData(-1, 1, 1, 1)]
    public void Calculate_InvalidInputs_ThrowsDomainValidation(
        double rho, double v, double d, double mu)
    {
        Assert.Throws<DomainValidationException>(
            () => ReynoldsCalculator.Calculate(rho, v, d, mu));
    }
}
