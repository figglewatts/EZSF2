using System.Collections.Generic;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class Preset
    {
        public PresetHeader Header { get; set; }
        public ZoneList<PresetZone> Zones { get; set; }

        public Preset(PresetHeader header, List<PresetZone> zones)
        {
            Header = header;
            Zones = new ZoneList<PresetZone>(zones);
        }

        public NoteData GetNote(int note, int velocity)
        {
            return Zones.GetNote(note, velocity);
        }
    }
}
