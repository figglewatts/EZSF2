using System.IO;

namespace EZSF2.Soundfont.Chunk
{
    public interface IChunkElement
    {
        IChunkElement FromBinaryReader(BinaryReader br);
    }
}
