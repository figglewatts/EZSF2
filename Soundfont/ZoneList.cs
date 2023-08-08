using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public class ZoneList<T> where T : BaseZone
    {
        public T GlobalZone;
        public List<T> Zones;
    
        public ZoneList(List<T> zones)
        {
            if (zones.Count > 1 && zones[0].IsGlobal)
            {
                GlobalZone = zones[0];
                Zones = zones.Skip(1).ToList(); // skip the global zone
            }
            else
            {
                Zones = zones;
            }
        }

        public NoteData GetNote(int note, int velocity)
        {
            foreach (var zone in Zones)
            {
                if (zone.Range.NoteInRange(note, velocity))
                {
                    var noteResult = zone.GetNote(note, velocity);
                
                    // apply the settings from the global zone, if it existed
                    if (GlobalZone != null) noteResult.Settings.Apply(GlobalZone.Generators.ProcessGenerators());
                
                    return noteResult;
                }
            }
            throw new InvalidOperationException("ZoneList must have at least 1 zone");
        }
    }
}
