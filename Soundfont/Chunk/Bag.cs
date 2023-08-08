using System.IO;

namespace EZSF2.Soundfont.Chunk
{
    public class Bag : IChunkElement
    {
        public const int Size = 2 * 2;

        public ushort GeneratorIndex { get; set; }
        public ushort ModulatorIndex { get; set; }

        public IChunkElement FromBinaryReader(BinaryReader br)
        {
            GeneratorIndex = br.ReadUInt16();
            ModulatorIndex = br.ReadUInt16();
            return this;
        }
    }
}
