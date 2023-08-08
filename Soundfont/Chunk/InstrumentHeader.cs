using System;
using System.IO;
using System.Text;

namespace EZSF2.Soundfont.Chunk
{
    public class InstrumentHeader : IChunkElement
    {
        public const int Size = 20 + 2;

        public string Name { get; set; }
        public ushort BagIndex { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            Name = Encoding.ASCII.GetString(br.ReadBytes(count: 20)).Trim('\0').Trim();
            BagIndex = br.ReadUInt16();
            return this;
        }

        public bool IsZero() => Name.Equals("EOI", StringComparison.InvariantCulture);
    }
}
