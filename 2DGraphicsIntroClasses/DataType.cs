using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum DataType : byte
{
    NoImageData = 0, // no image data is present
    UncompressedColorMappedImage = 1,
    UncompressedTrueColorImage = 2,
    UncompressedBlackAndWhiteImage = 3,
    RleColorMappedImage = 9, // run-length encoded color-mapped image
    RleTrueColorImage = 10, // run-length encoded true-color image
    RleBlackAndWhiteImage = 11 // run-length encoded black-and-white (grayscale) image
}
