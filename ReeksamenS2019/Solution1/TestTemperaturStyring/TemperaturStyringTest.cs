using Temperaturstyring.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;

namespace TestTemperaturStyring;

public class TemperaturStryingTest
{
    private TemperatureControl uut;
    private ILogFile logFile;
    private IHeatRelay heatRelay;
    private IThermometer thermometer;
    
    [SetUp]
    public void Setup()
    {
        logFile = Substitute.For<ILogFile>();
        heatRelay = Substitute.For<IHeatRelay>();
        thermometer = Substitute.For<IThermometer>();
        uut = new TemperatureControl(heatRelay, logFile, thermometer);
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-100)]
    public void IsHeatOn_StateOn_ReturnsTrue(int temp)
    {
        //arrange
        uut.TemperatureChanged(5); //Turns off heat
        
        //act
        uut.TemperatureChanged(temp);  // This should switch on the heat
        
        //assert
        Assert.IsTrue(uut.IsHeatOn());
    }

    [TestCase(3)]
    [TestCase(10)]
    [TestCase(100)]
    public void IsHeatOn_StateOff_ReturnsFalse(int temp)
    {
        //arrange
        uut.TemperatureChanged(-5); //Turns on heat
        
        //act
        uut.TemperatureChanged(temp);
        
        //assert
        Assert.IsFalse(uut.IsHeatOn());
    }

    [TestCase(-1)]
    [TestCase(-2)]
    [TestCase(-100)]
    public void TemperatureChanged_TempBelowZero_HeatRelayOn(int temp)
    {
        //arrange
        uut.TemperatureChanged(5);
        
        //act
        uut.TemperatureChanged(temp);
        
        //assert
        heatRelay.Received(1).TurnOn(); //heatrelay should be turned on once
    }
    
    
    [TestCase(3)]
    [TestCase(10)]
    [TestCase(100)]
    public void TemperatureChanged_TempAboveTwo_HeatRelayOff(int temp)
    {
        //arrange
        uut.TemperatureChanged(-5);
        
        //act
        uut.TemperatureChanged(temp);
        
        //assert
        heatRelay.Received(1).TurnOff(); //heatrelay should be turned on once
    }
    
    //Tests for logs
    
}