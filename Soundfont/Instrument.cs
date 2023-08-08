using System.Collections.Generic;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class Instrument
    {
        public InstrumentHeader Header { get; set; }
        public ZoneList<InstrumentZone> Zones { get; set; }

        public Instrument(InstrumentHeader header, List<InstrumentZone> zones)
        {
            Header = header;
            Zones = new ZoneList<InstrumentZone>(zones);
        }

        public NoteData GetNote(int note, int velocity)
        {
            return Zones.GetNote(note, velocity);
        }
    }
}
