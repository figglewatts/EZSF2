using System;
using System.Collections.Generic;
using System.IO;
using EZSF2.Soundfont.Chunk;
using EZSF2.Soundfont.Riff;

namespace EZSF2.Soundfont
{
    public class SF2
    {
        protected static readonly Dictionary<Type, uint> _elementSizes = new Dictionary<Type, uint>
        {
            { typeof(PresetHeader), PresetHeader.Size },
            { typeof(InstrumentHeader), InstrumentHeader.Size },
            { typeof(SampleHeader), SampleHeader.Size },
            { typeof(Bag), Bag.Size },
            { typeof(Modulator), Modulator.Size },
            { typeof(Generator), Generator.Size }
        };
        protected Hydra _hydra;

        protected RiffFile _underlyingRiff;
        protected byte[] _waveTable;

        public SF2(BinaryReader br)
        {
            // parse the underlying RIFF structure
            _underlyingRiff = new RiffFile().Parse(br);

            // parse low-level SF2 structures out of the loaded RIFF file
            Info = loadInfoChunk();
            _waveTable = loadSampleDataChunk();
            _hydra = loadHydraChunk();
        
            // load high-level primitives from the SF2 structures
            Samples = loadSamples(_hydra);
            Instruments = loadInstruments(_hydra, Samples);
            Presets = loadPresets(_hydra, Instruments);

            // TODO: exceptions for invalid soundfont/riff files
            // - KeyNotFoundException for missing chunks
            // - EndOfStreamException for bad data
        }

        public SoundfontInfo Info { get; set; }
        public List<Preset> Presets { get; set; }
        public List<Instrument> Instruments { get; set; }
        public List<Sample> Samples { get; set; }

        public NoteData GetNote(int preset, int note, int velocity)
        {
            var notePreset = Presets[preset];
            var noteData = notePreset.GetNote(note, velocity);
            noteData.Settings = noteData.Settings.SetNullsToDefaults(); // ensure we have defaults set
            return noteData;
        }

        protected List<Sample> loadSamples(Hydra hydra)
        {
            var sampleList = new List<Sample>();
            foreach (SampleHeader sampleHeader in hydra.SampleHeaders)
            {
                if (sampleHeader.IsZero()) break; // handle terminal record
                var sampleData = new Memory<byte>(_waveTable, (int)sampleHeader.Start * 2,
                    (int)(sampleHeader.End - sampleHeader.Start) * 2);
                sampleList.Add(new Sample(sampleHeader, sampleData));
            }
            return sampleList;
        }

        protected List<Instrument> loadInstruments(Hydra hydra, List<Sample> samples)
        {
            var instrumentList = new List<Instrument>();
            for (int i = 0; i < hydra.InstrumentHeaders.Length; i++)
            {
                var instrumentHeader = _hydra.InstrumentHeaders[i];
                if (instrumentHeader.IsZero()) break; // handle terminal record

                var startBagIdx = instrumentHeader.BagIndex;
            
                // end bag index is one before the next instrument's bag index, or the last bag if we're on the last instrument
                bool isLastInstrument = i == hydra.InstrumentHeaders.Length - 1;
                var endBagIdx = isLastInstrument ? hydra.InstrumentBags.Length : hydra.InstrumentHeaders[i + 1].BagIndex;
            
                // load zones
                var zoneList = loadInstrumentZones(startBagIdx, endBagIdx, hydra, samples);
                instrumentList.Add(new Instrument(instrumentHeader, zoneList));
            }
            return instrumentList;
        }

        protected List<Preset> loadPresets(Hydra hydra, List<Instrument> instruments)
        {
            var presetList = new List<Preset>();
            for (int i = 0; i < hydra.PresetHeaders.Length; i++)
            {
                var presetHeader = hydra.PresetHeaders[i];
                if (presetHeader.IsZero()) break; // handle terminal record
            
                var startBagIdx = presetHeader.BagIndex;
            
                // end bag index is one before the next preset's bag index, or the last bag if we're on the last preset
                bool isLastPreset = i == hydra.PresetHeaders.Length - 1;
                var endBagIdx = isLastPreset ? hydra.PresetBags.Length : hydra.PresetHeaders[i + 1].BagIndex;

                // load zones
                var zoneList = loadPresetZones(startBagIdx, endBagIdx, hydra, instruments);
                presetList.Add(new Preset(presetHeader, zoneList));
            }
            return presetList;
        }

        protected List<InstrumentZone> loadInstrumentZones(int startBagIdx, int endBagIdx, Hydra hydra, List<Sample> samples)
        {
            var zoneList = new List<InstrumentZone>();
            for (int i = startBagIdx; i < endBagIdx; i++)
            {
                var bag = hydra.InstrumentBags[i];
                bool isLastBag = i == hydra.InstrumentBags.Length - 1;
            
                var startGeneratorIdx = bag.GeneratorIndex;
                var endGeneratorIdx = isLastBag ? hydra.InstrumentGenerators.Length : hydra.InstrumentBags[i + 1].GeneratorIndex;
            
                var startModulatorIdx = bag.ModulatorIndex;
                var endModulatorIdx = isLastBag ? hydra.InstrumentModulators.Length : hydra.InstrumentBags[i + 1].ModulatorIndex;

                GeneratorList generatorList = new GeneratorList();
                for (int j = startGeneratorIdx; j < endGeneratorIdx; j++)
                {
                    var generator = hydra.InstrumentGenerators[j];
                    generatorList.List.Add(generator);
                }

                List<Modulator> modulatorList = new List<Modulator>();
                for (int j = startModulatorIdx; j < endModulatorIdx; j++)
                {
                    var modulator = hydra.InstrumentModulators[j];
                    if (modulator.IsZero()) break; // handle terminal record
                    modulatorList.Add(modulator);
                }
            
                zoneList.Add(new InstrumentZone(bag, generatorList, modulatorList, samples));
            }
            return zoneList;
        }

        protected List<PresetZone> loadPresetZones(int startBagIdx, int endBagIdx, Hydra hydra, List<Instrument> instruments)
        {
            var zoneList = new List<PresetZone>();
            for (int i = startBagIdx; i < endBagIdx; i++)
            {
                var bag = hydra.PresetBags[i];
                bool isLastBag = i == hydra.PresetBags.Length - 1;

                var startGeneratorIdx = bag.GeneratorIndex;
                var endGeneratorIdx = isLastBag ? hydra.PresetGenerators.Length : hydra.PresetBags[i + 1].GeneratorIndex;
            
                var startModulatorIdx = bag.ModulatorIndex;
                var endModulatorIdx = isLastBag ? hydra.PresetModulators.Length : hydra.PresetBags[i + 1].ModulatorIndex;

                GeneratorList generatorList = new GeneratorList();
                for (int j = startGeneratorIdx; j < endGeneratorIdx; j++)
                {
                    var generator = hydra.PresetGenerators[j];
                    generatorList.List.Add(generator);
                }

                List<Modulator> modulatorList = new List<Modulator>();
                for (int j = startModulatorIdx; j < endModulatorIdx; j++)
                {
                    var modulator = hydra.PresetModulators[j];
                    if (modulator.IsZero()) break; // handle terminal record
                    modulatorList.Add(modulator);
                }
            
                zoneList.Add(new PresetZone(bag, generatorList, modulatorList, instruments));
            }
            return zoneList;
        }

        protected SoundfontInfo loadInfoChunk()
        {
            return SoundfontInfo.CreateFromListChunk(_underlyingRiff.Chunks[ChunkID.Info]);
        }

        protected byte[] loadSampleDataChunk()
        {
            return _underlyingRiff.Chunks[ChunkID.SampleData].SubChunks[ChunkID.Sample].Data;
        }

        protected Hydra loadHydraChunk()
        {
            ListChunk pdta = _underlyingRiff.Chunks[ChunkID.Hydra];
            return new Hydra
            {
                PresetHeaders = readChunkElements<PresetHeader>(pdta.SubChunks[ChunkID.PresetHeader]),
                PresetBags = readChunkElements<Bag>(pdta.SubChunks[ChunkID.PresetBag]),
                PresetModulators = readChunkElements<Modulator>(pdta.SubChunks[ChunkID.PresetModulator]),
                PresetGenerators = readChunkElements<Generator>(pdta.SubChunks[ChunkID.PresetGenerator]),
                InstrumentHeaders = readChunkElements<InstrumentHeader>(pdta.SubChunks[ChunkID.InstrumentHeader]),
                InstrumentBags = readChunkElements<Bag>(pdta.SubChunks[ChunkID.InstrumentBag]),
                InstrumentModulators = readChunkElements<Modulator>(pdta.SubChunks[ChunkID.InstrumentModulator]),
                InstrumentGenerators = readChunkElements<Generator>(pdta.SubChunks[ChunkID.InstrumentGenerator]),
                SampleHeaders = readChunkElements<SampleHeader>(pdta.SubChunks[ChunkID.SampleHeader])
            };
        }

        protected T[] readChunkElements<T>(DataChunk chunk) where T : IChunkElement, new()
        {
            if (!_elementSizes.ContainsKey(typeof(T)))
                throw new InvalidOperationException($"unknown chunk element type '{typeof(T)}'");

            using (MemoryStream chunkDataStream = new MemoryStream(chunk.Data))
            {
                using (BinaryReader chunkDataReader = new BinaryReader(chunkDataStream))
                {
                    uint elementCount = chunk.Size / _elementSizes[typeof(T)];
                    var elements = new T[elementCount];
                    for (int i = 0; i < elementCount; i++)
                    {
                        elements[i] = (T)new T().FromBinaryReader(chunkDataReader);
                    }
                    return elements;
                }

            }
        }
    }
}
