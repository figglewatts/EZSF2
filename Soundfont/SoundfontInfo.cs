using System;
using System.Text;
using EZSF2.Soundfont.Riff;

namespace EZSF2.Soundfont
{
    public struct SoundfontInfo
    {
        public int? Version;         // ifil
        public string SoundEngine;  // isng
        public string Name;         // INAM
        public string RomName;      // irom
        public int? RomVersion;      // iver
        public string CreationDate; // ICRD
        public string Authors;      // IENG
        public string Product;      // IPRD
        public string Copyright;    // ICOP
        public string Comments;     // ICMT
        public string Tools;        // ISFT

        public static SoundfontInfo CreateFromListChunk(ListChunk infoList)
        {
            SoundfontInfo info = new SoundfontInfo();

            // ifil chunk
            if (infoList.SubChunks.TryGetValue("ifil", out DataChunk ifilChunk))
            {
                info.Version = BitConverter.ToInt32(ifilChunk.Data, 0);
            }

            // isng chunk
            if (infoList.SubChunks.TryGetValue("isng", out DataChunk isngChunk))
            {
                info.RomName = Encoding.ASCII.GetString(isngChunk.Data);
            }

            // INAM chunk
            if (infoList.SubChunks.TryGetValue("INAM", out DataChunk inamChunk))
            {
                info.RomName = Encoding.ASCII.GetString(inamChunk.Data);
            }

            // irom chunk
            if (infoList.SubChunks.TryGetValue("irom", out DataChunk iromChunk))
            {
                info.RomName = Encoding.ASCII.GetString(iromChunk.Data);
            }

            // iver chunk
            if (infoList.SubChunks.TryGetValue("iver", out DataChunk iverChunk))
            {
                info.RomVersion = iverChunk.Data[0] << (16 + iverChunk.Data[1]);
            }

            // ICRD chunk
            if (infoList.SubChunks.TryGetValue("ICRD", out DataChunk icrdChunk))
            {
                info.CreationDate = Encoding.ASCII.GetString(icrdChunk.Data);
            }

            // IENG chunk
            if (infoList.SubChunks.TryGetValue("IENG", out DataChunk iengChunk))
            {
                info.Authors = Encoding.ASCII.GetString(iengChunk.Data);
            }

            // IPRD chunk
            if (infoList.SubChunks.TryGetValue("IPRD", out DataChunk iprdChunk))
            {
                info.Product = Encoding.ASCII.GetString(iprdChunk.Data);
            }

            // ICOP chunk
            if (infoList.SubChunks.TryGetValue("ICOP", out DataChunk icopChunk))
            {
                info.Copyright = Encoding.ASCII.GetString(icopChunk.Data);
            }

            // ICMT chunk
            if (infoList.SubChunks.TryGetValue("ICMT", out DataChunk icmtChunk))
            {
                info.Comments = Encoding.ASCII.GetString(icmtChunk.Data);
            }

            // ISFT chunk
            if (infoList.SubChunks.TryGetValue("ISFT", out DataChunk isftChunk))
            {
                info.Tools = Encoding.ASCII.GetString(isftChunk.Data);
            }

            return info;
        }
    }
}
