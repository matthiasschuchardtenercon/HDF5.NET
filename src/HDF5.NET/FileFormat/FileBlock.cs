﻿using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HDF5.NET
{
    public abstract class FileBlock
    {
        #region Constructors

        public FileBlock(BinaryReader reader)
        {
            this.Reader = reader;
        }

        #endregion

        #region Properties

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        internal BinaryReader Reader { get; }

        #endregion

        #region Methods

        public virtual void Validate()
        {
            //
        }

        public virtual void Print(ILogger logger)
        {
            logger.LogWarning($"Printing of file block type '{this.GetType()}' is not implemented.");
        }

        private protected void ValidateSignature(byte[] actual, byte[] expected)
        {
            var actualString = Encoding.ASCII.GetString(actual);
            var expectedString = Encoding.ASCII.GetString(expected);

            if (actualString != expectedString)
                throw new Exception($"The actual signature '{actualString}' does not match the expected signature '{expectedString}'.");
        }

        private protected ulong ReadUlong(byte byteCount)
        {
            return byteCount switch
            {
                1 => this.Reader.ReadByte(),
                2 => this.Reader.ReadUInt16(),
                4 => this.Reader.ReadUInt32(),
                8 => this.Reader.ReadUInt64(),
                _ => throw new NotSupportedException("The byte count is invalid.")
            };
        }

        #endregion
    }
}