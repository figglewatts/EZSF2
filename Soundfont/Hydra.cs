using EZSF2.Soundfont.Chunk;

namespace EZSF2.Soundfont
{
    public struct Hydra
    {
        public PresetHeader[] PresetHeaders;
        public Bag[] PresetBags;
        public Modulator[] PresetModulators;
        public Generator[] PresetGenerators;
        public InstrumentHeader[] InstrumentHeaders;
        public Bag[] InstrumentBags;
        public Modulator[] InstrumentModulators;
        public Generator[] InstrumentGenerators;
        public SampleHeader[] SampleHeaders;
    }
}
