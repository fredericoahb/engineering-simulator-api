using Xunit;
using EngineeringSimulator.Domain.Calculations;
using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Tests;

public class RayleighCalculatorTests
{
    [Fact]
    public void Calculate_NominalAirConvection_ReturnsCorrectRa()
    {
        double g = 9.81, beta = 3.41e-3, deltaT = 20, l = 0.1, nu = 1.56e-5, alpha = 2.21e-5;
        var expected = (g * beta * deltaT * Math.Pow(l, 3)) / (nu * alpha);
        var result = RayleighCalculator.Calculate(g, beta, deltaT, l, nu, alpha);
        Assert.Equal(expected, result, precision: 2);
    }

    [Fact]
    public void Calculate_SmallRa_ClassifiesAsConductionDominant()
    {
        var result = RayleighCalculator.Calculate(9.81, 3.41e-3, 1.0, 0.001, 1.56e-5, 2.21e-5);
        Assert.True(result < RayleighCalculator.ConvectionOnsetThreshold);
        Assert.Contains("conduction", RayleighCalculator.Classify(result).ToLower());
    }

    [Fact]
    public void Calculate_LargeRa_ClassifiesAsTurbulent()
    {
        var result = RayleighCalculator.Calculate(9.81, 3.41e-3, 50, 1.0, 1.56e-5, 2.21e-5);
        Assert.True(result > RayleighCalculator.TurbulentConvectionThreshold);
        Assert.Contains("Turbulent", RayleighCalculator.Classify(result));
    }

    [Theory]
    [InlineData(0, 1, 1, 1, 1, 1)]
    [InlineData(1, 0, 1, 1, 1, 1)]
    [InlineData(1, 1, 0, 1, 1, 1)]
    [InlineData(1, 1, 1, 0, 1, 1)]
    [InlineData(1, 1, 1, 1, 0, 1)]
    [InlineData(1, 1, 1, 1, 1, 0)]
    public void Calculate_AnyZeroInput_ThrowsDomainValidation(
        double g, double beta, double deltaT, double l, double nu, double alpha)
    {
        Assert.Throws<DomainValidationException>(
            () => RayleighCalculator.Calculate(g, beta, deltaT, l, nu, alpha));
    }

    [Fact]
    public void Calculate_CubicScaling_VerifyL3Dependence()
    {
        double g = 9.81, beta = 1e-3, deltaT = 10, nu = 1e-5, alpha = 1e-5;
        var ra1 = RayleighCalculator.Calculate(g, beta, deltaT, 1.0, nu, alpha);
        var ra2 = RayleighCalculator.Calculate(g, beta, deltaT, 2.0, nu, alpha);
        Assert.Equal(ra1 * 8.0, ra2, precision: 2);
    }
}
