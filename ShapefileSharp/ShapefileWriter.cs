﻿using ShapefileSharp.Spec;
using System;
using System.IO;

namespace ShapefileSharp
{
    public sealed class ShapefileWriter<T> : IDisposable where T:IShape<IPoint>
    {
        public ShapefileWriter(string shpFilePath) : base()
        {
            ShpStream = new FileStream(shpFilePath, FileMode.Create);
            ShxStream = new FileStream(Path.ChangeExtension(shpFilePath, ".shx"), FileMode.Create);

            ShpWriter = new BinaryWriter(ShpStream);
            ShxWriter = new BinaryWriter(ShxStream);

            ShpHeader = new ShapefileHeader();
            ShxHeader = new ShapefileHeader();

            WriteHeaders();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    WriteHeaders();

                    ShpWriter.Dispose();
                    ShxWriter.Dispose();

                    ShpStream.Dispose();
                    ShxStream.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        private readonly FileStream ShpStream;
        private readonly FileStream ShxStream;

        private readonly BinaryWriter ShpWriter;
        private readonly BinaryWriter ShxWriter;

        private readonly ShapefileHeader ShpHeader;
        private readonly ShapefileHeader ShxHeader;

        private BoundingBox<PointZ> BoundingBox;

        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// The path the the .shp file that is being written.
        /// </summary>
        public string ShpFilePath
        {
            get
            {
                return ShpStream.Name;
            }
        }

        /// <summary>
        /// The path the the .shx file that is being written.
        /// </summary>
        public string ShxFilePath
        {
            get
            {
                return ShxStream.Name;
            }
        }

        private void WriteHeaders()
        {
            ShpHeader.FileLength = WordCount.FromBytes(ShpWriter.BaseStream.Length);
            ShxHeader.FileLength = WordCount.FromBytes(ShxWriter.BaseStream.Length);

            if (BoundingBox != null) {
                BoundingBox<PointZ> box = new BoundingBox<PointZ>()
                {
                    Min = new PointZ(BoundingBox.Min),
                    Max = new PointZ(BoundingBox.Max)
                };

                if (box.Min.IsNoDataM())
                {
                    box.Min.M = 0;
                }

                if (box.Max.IsNoDataM())
                {
                    box.Max.M = 0;
                }

                ShpHeader.BoundingBox = box;
                ShxHeader.BoundingBox = box;
            }

            ShpHeader.ShapeType = ShapeTypeMap.GetShapeType<T>();

            var shpHeaderField = new ShapefileHeaderField(WordCount.Zero);
            var shxHeaderField = new ShapefileHeaderField(WordCount.Zero);

            shpHeaderField.Write(ShpWriter, ShpHeader);
            shxHeaderField.Write(ShxWriter, ShxHeader);
        }

        /// <summary>
        /// The RecordNumber for the next shape.
        /// </summary>
        private int RecordNumber = 1;

        public IShapefileRecord<T> Write(IShape shape)
        {
            return Write((T)shape);
        }

        public IShapefileRecord<T> Write(T shape)
        {
            //TODO: Write BoundingBox merge method(s)...
            dynamic shapeBox = shape.Box;

            if (BoundingBox == null)
            {
                BoundingBox = new BoundingBox<PointZ>()
                {
                    Min = new PointZ(shapeBox.Min),
                    Max = new PointZ(shapeBox.Max)
                };
            } else {
                BoundingBox.Min.Minimize(shapeBox.Min);
                BoundingBox.Max.Maximize(shapeBox.Max);
            }

            var shpRecordOffset = WordCount.FromBytes(ShpWriter.BaseStream.Position);
            var shpContentOffset = shpRecordOffset + ShpRecordHeaderField.FieldLength;

            var record = new ShapefileRecord<T>()
            {
                RecordNumber = RecordNumber,
                Shape = shape
            };

            //TODO: Write the SHP header first...
            //      But we have to write the shape, to figure out its length, to add the length to the header...
            var shapeField = new ShapeField(shpContentOffset);
            shapeField.Write(ShpWriter, shape);

            var shpStreamLengthAfterRecordWrite = ShpWriter.BaseStream.Length;
                
            var shpHeader = new ShpRecordHeader()
            {
                //TODO: The shapeField should just have a Length... vs assuming the writer will be at the correct length...
                ContentLength = WordCount.FromBytes(shpStreamLengthAfterRecordWrite) - shpContentOffset,
                RecordNumber = RecordNumber
            };

            var shpHeaderField = new ShpRecordHeaderField(shpRecordOffset);
            shpHeaderField.Write(ShpWriter, shpHeader);

            //Reset so we are at the correct position for the next shape...
            ShpWriter.BaseStream.Position = shpStreamLengthAfterRecordWrite;



            var shxRecord = new ShxRecord()
            {
                Offset = shpRecordOffset,
                ContentLength = WordCount.FromBytes((int)ShpWriter.BaseStream.Position) - shpRecordOffset
            };

            var shxRecordField = new ShxRecordField(WordCount.FromBytes(ShxWriter.BaseStream.Position));
            shxRecordField.Write(ShxWriter, shxRecord);


            RecordNumber++;

            return record;
        }
    }
}
