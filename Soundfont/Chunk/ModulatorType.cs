﻿namespace EZSF2.Soundfont.Chunk
{
    public enum ModulatorType : ushort
    {
        NoController = 0,
        NoteOnVelocity = 2,
        NoteOnKeyNumber = 3,
        PolyPressure = 10,
        ChannelPressure = 13,
        PitchWheel = 14,
        PitchWheelSensitivity = 16,
        Link = 127
    }
}
