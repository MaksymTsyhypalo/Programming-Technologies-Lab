using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP.InformationComputation.LayeredArchitecture.Data;
using TP.InformationComputation.LayeredArchitecture.Logic;
using TP.InformationComputation.LayeredArchitecture.Logic.AbstractLayerInterface;

namespace LayeredArchitecture.Tests;

[TestClass]
public class SimpleTest
{
    // Test 1: Verify Data Layer returns correct values
    [TestMethod]
    public void DataLayer_Returns_CorrectCategoryCounts()
    {
        // Arrange
        var dataLayer = DataLayerAbstract.CreateLinq2SQL();

        // Act & Assert
        Assert.AreEqual(10, dataLayer.GetCategory1Count(),
            "Category1 should have 10 products");
        Assert.AreEqual(5, dataLayer.GetCategory2Count(),
            "Category2 should have 5 products");
    }

    // Test 2: Verify ServiceC works correctly
    [TestMethod]
    public void ServiceC_Returns_Category1Count()
    {
        // Arrange
        var dataLayer = DataLayerAbstract.CreateLinq2SQL();
        var serviceC = new ServiceC(dataLayer);

        // Act & Assert
        Assert.AreEqual(10, serviceC.GetProductCount(),
            "ServiceC should return Category1 count");
    }

    // Test 3: Verify full service chain calculation
    [TestMethod]
    public void ServiceChain_Calculates_TotalProducts()
    {
        // Arrange
        var dataLayer = DataLayerAbstract.CreateLinq2SQL();
        var serviceChain = new ServiceA(
            new ServiceB(
                new ServiceC(dataLayer),
                dataLayer
            )
        );

        // Act & Assert
        Assert.AreEqual(15, serviceChain.GetProductCount(),
            "Service chain should sum both categories (10 + 5)");
    }
}