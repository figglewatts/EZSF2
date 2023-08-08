using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace EZSF2.Soundfont.Riff
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RiffFile
    {
        public string ID { get; set; }
        public uint Size { get; set; }
        public string Format { get; set; }
        public Dictionary<string, ListChunk> Chunks { get; set; }

        public RiffFile Parse(BinaryReader br)
        {
            ID = Encoding.ASCII.GetString(br.ReadBytes(count: 4));
            Size = br.ReadUInt32();
            Format = Encoding.ASCII.GetString(br.ReadBytes(count: 4));
            readSubChunks(br);
            return this;
        }

        private void readSubChunks(BinaryReader br)
        {
            Chunks = new Dictionary<string, ListChunk>();
            uint curByte = 0;
            while (curByte < Size)
            {
                ListChunk chunk = new ListChunk().Parse(br);
                curByte += chunk.Size + ListChunk.HeaderLength;
                Chunks[chunk.Format] = chunk;
            }
        }
    }
}
