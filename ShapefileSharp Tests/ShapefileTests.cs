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
        Shapefile PolygonActual = new Shapefile(PolygonShpFile.FilePath);
        Shapefile PointMActual = new Shapefile(PointMShpFile.FilePath);
        Shapefile MultiPointMActual = new Shapefile(MultiPointMShpFile.FilePath);
        Shapefile PointZActual = new Shapefile(PointZShpFile.FilePath);

        [TestMethod]
        public void Cities_FirstRecord_Equals()
        {
            var actual = CitiesActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.Point, actual.Shape.ShapeType);
            Assert.IsInstanceOfType(actual.Shape, typeof(IPointShape<IPoint>));

            var pointShape = actual.Shape as IPointShape<IPoint>;
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

            Assert.IsInstanceOfType(actual.Shape, typeof(IPolyLineShape<IPoint>));
            var polyLineShape = actual.Shape as IPolyLineShape<IPoint>;

            Assert.AreEqual(1, polyLineShape.Lines.Count);
            Assert.AreEqual(12, polyLineShape.Lines.First().Points.Count);

            var firstPoint = new Point()
            {
                X = -74.95269,
                Y = 40.04527
            };
            Assert.AreEqual(firstPoint, polyLineShape.Lines.First().Points.First());


            var box = new BoundingBox<IPoint>()
            {
                Min = new Point()
                {
                    X = -74.95269,
                    Y = 40.04527
                },
                Max = new Point()
                {
                    X = -74.94871,
                    Y = 40.04969
                }
            };
            Assert.AreEqual(box, polyLineShape.Box);
        }

        [TestMethod]
        public void Polygon_FirstRecord_Equals()
        {
            var actual = PolygonActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.Polygon, actual.Shape.ShapeType);

            Assert.IsInstanceOfType(actual.Shape, typeof(IPolygonShape<IPoint>));
            var polygonShape = actual.Shape as IPolygonShape<IPoint>;

            Assert.AreEqual(1, polygonShape.Rings.Count);
            Assert.AreEqual(26, polygonShape.Rings.First().Points.Count);

            var firstPoint = new Point()
            {
                X = -69.996937628999916,
                Y = 12.577582098000036
            };
            Assert.AreEqual(firstPoint, polygonShape.Rings.First().Points.First());


            var box = new BoundingBox<IPoint>()
            {
                Min = new Point()
                {
                    X = -70.062408006999874,
                    Y = 12.417669989000046
                },
                Max = new Point()
                {
                    X = -69.876820441999939,
                    Y = 12.632147528000104
                }
            };
            Assert.AreEqual(box, polygonShape.Box);
        }

        [TestMethod]
        public void PointM_FirstRecord_Equals()
        {
            var actual = PointMActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.PointM, actual.Shape.ShapeType);

            Assert.IsInstanceOfType(actual.Shape, typeof(IPointShape<IPointM>));
            var pointShape = actual.Shape as IPointShape<IPointM>;

            var expectedPoint = new Point()
            {
                X = 10,
                Y = 10,
                M = 100
            };

            Assert.AreEqual(expectedPoint, pointShape.Point);
        }

        [TestMethod]
        public void MultiPointM_FirstRecord_Equals()
        {
            var actual = MultiPointMActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.MultiPointM, actual.Shape.ShapeType);

            Assert.IsInstanceOfType(actual.Shape, typeof(IMultiPointShape<IPointM>));
            var multiPointShape = actual.Shape as IMultiPointShape<IPointM>;

            Assert.AreEqual(3, multiPointShape.Points.Count);

            var firstPoint = new Point()
            {
                X = 10,
                Y = 10,
                M = 100
            };
            Assert.AreEqual(firstPoint, multiPointShape.Points.First());

            var box = new BoundingBox<IPointM>()
            {
                Min = new Point()
                {
                    X = 0,
                    Y = 5,
                    M = 50
                },
                Max = new Point()
                {
                    X = 10,
                    Y = 10,
                    M = 75
                }
            };
            Assert.AreEqual(box, multiPointShape.Box);
        }

        [TestMethod]
        public void PointZ_FirstRecord_Equals()
        {
            var actual = PointZActual.First();

            //TODO: There must be a better way to store these expected values...
            Assert.AreEqual(1, actual.Header.RecordNumber);
            Assert.AreEqual(ShapeType.PointZ, actual.Shape.ShapeType);

            Assert.IsInstanceOfType(actual.Shape, typeof(IPointShape<IPointZ>));
            var pointShape = actual.Shape as IPointShape<IPointZ>;

            var expectedPoint = new Point()
            {
                X = 10,
                Y = 10,
                Z= 100,
                M = -1.7976931348623157E+308
            };

            Assert.AreEqual(expectedPoint, pointShape.Point);
        }
    }
}
