using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct Color
{
    // the value stored as little endian:
    // ARGB -> BGRA
    // xRGB -> BGRx
    public readonly int value;
    public readonly Format format;

    public byte this[int offset]
    {
        get
        {
            if (offset > 3) // int has only 4 bytes (0, 1, 2, 3)
                throw new ArgumentOutOfRangeException();
            return (byte)(value >> 8 * (3 - offset));
        }
    }

    public static Color Red = new Color(red: 255, green: 0, blue: 0, alpha: 255);
    public static Color Green = new Color(red: 0, green: 255, blue: 0, alpha: 255);
    public static Color Blue = new Color(red: 0, green: 0, blue: 255, alpha: 255);
    public static Color Black = new Color(red: 0, green: 0, blue: 0, alpha: 255);
    public static Color White = new Color(red: 255, green: 255, blue: 255, alpha: 255);
    public static Color Yellow = new Color(red: 225, green: 225, blue: 0, alpha: 255);

    public Color(byte red, byte green, byte blue, byte alpha)
        : this((blue << 24) | (green << 16) | (red << 8) | alpha, Format.BGRA)
    {
    }

    public Color(byte red, byte green, byte blue)
        : this((blue << 24) | (green << 16) | (red << 8) | 0xFF, Format.BGR)
    {
    }

    public Color(byte value)
        : this(value, Format.GRAYSCALE)
    {
    }

    public Color(int value, Format format)
    {
        this.value = value;
        this.format = format;
    }

    public static Color operator *(Color color, float intensivity)
    {
        intensivity = Math.Max(0f, Math.Min(1f, intensivity));
        var ch0 = (byte)(color[0] * intensivity);
        var ch1 = (byte)(color[1] * intensivity);
        var ch2 = (byte)(color[2] * intensivity);
        var ch3 = color[3];
        return new Color(ch0 << 24 | ch1 << 16 | ch2 << 8 | ch3, color.format);
    }
}