using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

struct TGAHeader
{
    public byte IdLength;
    public byte ColorMapType;
    public DataType DataTypeCode;

    // field #4. Color map specification
    public short ColorMapOrigin; // index of first color map entry that is included in the file
    public short ColorMapLength; // number of entries of the color map that are included in the file
    public byte ColorMapDepth;   // number of bits per pixel

    // field #5. Image specification
    public short OriginX; // absolute coordinate of lower-left corner for displays where origin is at the lower left
    public short OriginY; // as for X-origin
    public short Width;   // width in pixels
    public short Height;  // height in pixels
    public byte BitsPerPixel;     // pixel depth
    public byte ImageDescriptor;  // bits 3-0 give the alpha channel depth, bits 5-4 give direction
}
