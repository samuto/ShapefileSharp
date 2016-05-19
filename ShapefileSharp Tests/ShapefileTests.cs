﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShapefileSharp.Tests.Shapefiles;
using System.Linq;

namespace ShapefileSharp.Tests
{
    [TestClass]
    public class ShapefileTests
    {
        Shapefile CitiesActual = new Shapefile(CitiesMainFile.FilePath);
        Shapefile MultiPointActual = new Shapefile(MultiPointShpFile.FilePath);
        Shapefile PolyLineActual = new Shapefile(PolyLineShpFile.FilePath);

        [TestMethod]
        public void Cities_FirstRecord_Equals()
        {
            var actual = CitiesActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.Point, actual.Shape.ShapeType);
            Assert.IsInstanceOfType(actual.Shape, typeof(IPointShape));

            var pointShape = actual.Shape as IPointShape;
            Assert.AreEqual(-57.840002473401341, pointShape.Point.X);
            Assert.AreEqual(-34.47999900541754, pointShape.Point.Y);
        }

        [TestMethod]
        public void MultiPoint_FirstRecord_Equals()
        {
            var actual = MultiPointActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.MultiPoint, actual.Shape.ShapeType);

            Assert.IsInstanceOfType(actual.Shape, typeof(IMultiPointShape<IPoint>));   
            var multiPointShape = actual.Shape as IMultiPointShape<IPoint>;

            Assert.AreEqual(1, multiPointShape.Points.Count); //TODO: Maybe test a MultiPoint record with multiple points? haha

            var firstPoint = new Point()
            {
                X = 458860,
                Y = 132410
            };

            Assert.AreEqual(firstPoint, multiPointShape.Points.First());

            var box = new BoundingBox<IPoint>()
            {
                Min = new Point()
                {
                    X = 458860,
                    Y = 132410
                },
                Max = new Point()
                {
                    X = 458860,
                    Y = 132410
                }
            };

            Assert.AreEqual(box, multiPointShape.Box);
        }

        [TestMethod]
        public void Polyline_FirstRecord_Equals()
        {
            var actual = PolyLineActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.PolyLine, actual.Shape.ShapeType);
            Assert.IsInstanceOfType(actual.Shape, typeof(IPolyLineShape));

            //TODO: Check the box...

            var polyLineShape = actual.Shape as IPolyLineShape;
            Assert.AreEqual(-74.95269, polyLineShape.Parts.First().First().X);
            Assert.AreEqual(40.04527, polyLineShape.Parts.First().First().Y);
        }
    }
}
