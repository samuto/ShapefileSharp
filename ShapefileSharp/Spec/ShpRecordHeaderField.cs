﻿using System;
using System.IO;

namespace ShapefileSharp.Spec
{
    internal sealed class ShpRecordHeaderField : FixedField<IShpRecordHeader>
    {
        public ShpRecordHeaderField(WordCount offset) : base(offset)
        {
            RecordNumber = new IntField(offset, Endianness.Big);
            ContentLength = new WordCountField(offset + RecordNumber.Length);
        }

        public static readonly WordCount FieldLength = IntField.FieldLength + WordCountField.FieldLength;

        //TODO: Does anything actually use this?  Can this class be a Field<T>?
        public override WordCount Length
        {
            get
            {
                return FieldLength;
            }
        }

        private IntField RecordNumber { get; }
        private WordCountField ContentLength { get; }

        public override IShpRecordHeader Read(BinaryReader reader, WordCount origin)
        {
            reader.BaseStream.Position = origin.Bytes;

            return new ShpRecordHeader()
            {
                RecordNumber = RecordNumber.Read(reader, origin),
                ContentLength = ContentLength.Read(reader, origin)
            };
        }

        public override void Write(BinaryWriter writer, IShpRecordHeader value, WordCount origin)
        {
            throw new NotImplementedException();
        }
    }
}
