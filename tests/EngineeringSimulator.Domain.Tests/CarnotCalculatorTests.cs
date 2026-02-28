using Xunit;
using EngineeringSimulator.Domain.Calculations;
using EngineeringSimulator.Domain.Exceptions;

namespace EngineeringSimulator.Domain.Tests;

public class CarnotCalculatorTests
{
    [Fact]
    public void Calculate_NominalSteamEngine_ReturnsCorrectEfficiency()
    {
        var result = CarnotCalculator.Calculate(500, 300);
        Assert.Equal(0.4, result, precision: 10);
    }

    [Fact]
    public void Calculate_LargeTemperatureRatio_ReturnsHighEfficiency()
    {
        var result = CarnotCalculator.Calculate(1000, 100);
        Assert.Equal(0.9, result, precision: 10);
    }

    [Fact]
    public void Calculate_SmallTemperatureDifference_ReturnsLowEfficiency()
    {
        var result = CarnotCalculator.Calculate(310, 300);
        Assert.True(result > 0 && result < 0.05);
    }

    [Fact]
    public void Calculate_ResultAlwaysLessThanOne()
    {
        var result = CarnotCalculator.Calculate(10000, 1);
        Assert.True(result < 1.0);
    }

    [Fact]
    public void ToPercentage_ConvertsCorrectly()
    {
        Assert.Equal(40.0, CarnotCalculator.ToPercentage(0.4), precision: 10);
    }

    [Fact]
    public void Calculate_ThEqualsZero_Throws()
    {
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(0, 300));
    }

    [Fact]
    public void Calculate_TcEqualsZero_Throws()
    {
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(500, 0));
    }

    [Fact]
    public void Calculate_ThEqualsTc_Throws()
    {
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(300, 300));
    }

    [Fact]
    public void Calculate_ThLessThanTc_Throws()
    {
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(200, 300));
    }

    [Fact]
    public void Calculate_NegativeTemperatures_Throws()
    {
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(-100, 300));
        Assert.Throws<DomainValidationException>(() => CarnotCalculator.Calculate(500, -100));
    }
}
