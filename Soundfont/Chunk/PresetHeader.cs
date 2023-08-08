using System;
using System.IO;
using System.Text;

namespace EZSF2.Soundfont.Chunk
{
    public class PresetHeader : IChunkElement
    {
        public const int Size = 20 + 2 * 3 + 4 * 3;

        public string Name { get; set; }
        public ushort Preset { get; set; }
        public ushort Bank { get; set; }
        public ushort BagIndex { get; set; }
        public uint Library { get; set; }
        public uint Genre { get; set; }
        public uint Morphology { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            Name = Encoding.ASCII.GetString(br.ReadBytes(count: 20)).Trim('\0').Trim();
            Preset = br.ReadUInt16();
            Bank = br.ReadUInt16();
            BagIndex = br.ReadUInt16();
            Library = br.ReadUInt32();
            Genre = br.ReadUInt32();
            Morphology = br.ReadUInt32();
            return this;
        }

        public bool IsZero() => Name.Equals("EOP", StringComparison.InvariantCulture) && Genre == 0 && Library == 0 &&
                                Morphology == 0 && Preset == 0;
    }
}
