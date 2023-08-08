using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EZSF2.Soundfont.Riff
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DataChunk
    {
        public const int HeaderLength = 4 + 4; // length of ID + Size

        public string ID { get; set; }
        public uint Size { get; set; }

        public byte[] Data { get; set; }

        public DataChunk Parse(BinaryReader br)
        {
            ID = Encoding.ASCII.GetString(br.ReadBytes(count: 4));
            Size = br.ReadUInt32();
            Data = br.ReadBytes((int)Size);
            return this;
        }
    }
}
