﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapefileSharp.Tests.Shapefiles;

namespace ShapefileSharp.Tests
{
    [TestClass]
    public class IShapeMainFileTests
    {
        private readonly IShapeMainFile CitiesExpected = new CitiesMainFile();
        private readonly IShapeMainFile CitiesActual = ShapeMainFileFactory.Create(CitiesMainFile.FilePath);

        private readonly IShapeMainFile CountriesExpected = new CountriesMainFile();
        private readonly IShapeMainFile CountriesActual = ShapeMainFileFactory.Create(CountriesMainFile.FilePath);

        [TestMethod]
        public void ShapeType_Equals()
        {
            Assert.AreEqual(CitiesExpected.Header.ShapeType, CitiesActual.Header.ShapeType);
            Assert.AreEqual(CountriesExpected.Header.ShapeType, CountriesActual.Header.ShapeType);
        }

        [TestMethod]
        public void BoundingBox_Equals()
        {
            Assert.AreEqual(CitiesExpected.Header.BoundingBox, CitiesActual.Header.BoundingBox);
            Assert.AreEqual(CountriesExpected.Header.BoundingBox, CountriesActual.Header.BoundingBox);
        }
    }
}