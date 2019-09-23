using System;
using System.IO;

/// <summary>
/// This is the class used to draw images to the screen.
/// </summary>
class Image
{
    /// <summary>
    /// The bitmap
    /// </summary>
	internal byte [] buffer;

    /// <summary>
    /// The width of the image
    /// </summary>
	public int Width { get; }

    /// <summary>
    /// The height of the image
    /// </summary>
	public int Height { get; }

    /// <summary>
    /// The colour format of the image (RGB, Greyscale or RGBA)
    /// </summary>
	public Format Format { get; }

    /// <summary>
    /// The number of bytes per row for the specified format
    /// Greyscale = 1 bytes
    /// RGB = 3 bytes
    /// RGBA = 4 bytes
    /// </summary>
	public int BytesPerRow {
		get {
			return Width * (int)Format;
		}
	}

    /// <summary>
    /// Records the size of the image in bytes
    /// </summary>
    /// <param name="width">The width of the image</param>
    /// <param name="height">The height of the image</param>
    /// <param name="format">The number of bytes per row for the specified format</param>
	public Image (int width, int height, Format format)
	{
		Width = width;
		Height = height;
		Format = format;

		buffer = new byte [height * BytesPerRow];
	}
    
    /// <summary>
    /// Vertically inverts the image to make it appear upside-down
    /// </summary>
	public void VerticalFlip ()
	{
        // The number of bytes per pixel (bpp) based on what format the image is in
		var bpp = (int)Format;
		int bytesPerLine = Width * bpp;

        // Right shifts to get rid of the lowest ordered bit
		var half = Height >> 1;
		for (int l = 0; l < half; l++) {
			var l1 = l * bytesPerLine;
			var l2 = (Height - 1 - l) * bytesPerLine;

			for (int i = 0; i < bytesPerLine; i++) {
				byte pixel = buffer [l1 + i];
				buffer [l1 + i] = buffer [l2 + i];
				buffer [l2 + i] = pixel;
			}
		}
	}

    /// <summary>
    /// Clears the screen by setting each bit in the bitmap to 0
    /// </summary>
	public void Clear ()
	{
		for (int i = 0; i < buffer.Length; i++)
			buffer [i] = 0;
	}

    /// <summary>
    /// Gets or sets the color of the pixel at the given coordinate
    /// </summary>
    /// <param name="x">X coordinate</param>
    /// <param name="y">Y coordinate</param>
    /// <returns></returns>
	public Color this [int x, int y] {
		get {
			if (x < 0 || x >= Width) throw new ArgumentException ("x");
			if (y < 0 || y >= Height) throw new ArgumentException ("y");

			var offset = GetOffset (x, y);
			var len = (int)Format;
			int value = 0;
			for (var ch = 0; ch < 4; ch++)
				value = (value << 8) | (ch < len ? buffer [offset++] : 0xFF);

			return new Color (value, Format);
		}
		set {
			if (x < 0 || x >= Width) return; //throw new ArgumentException ($"{nameof(x)}={x} {nameof(Width)}={Width}");
			if (y < 0 || y >= Height) return; // throw new ArgumentException ($"{nameof(y)}={y} {nameof(Height)}={Height}");

			var offset = GetOffset (x, y);
			var v = value.value;
			var len = (int)Format;
			for (int ch = 0; ch < len; ch++)                   // 0123
				buffer [offset++] = (byte)(v >> (3 - ch) * 8); // BGRA
		}
	}

    /// <summary>
    /// Offsets the position of the image on the canvas
    /// </summary>
    /// <param name="x">Horizontal position</param>
    /// <param name="y">Vertical Position</param>
    /// <returns></returns>
	int GetOffset (int x, int y)
	{
		return y * BytesPerRow + x * (int)Format;
	}

    /// <summary>
    /// Creates a file in the specific path for the output stream.  Writes the header to the stream and then, 
    /// if the file is not compressed, writes the buffer data to the stream. If the file is compressed, decompress it.
    /// </summary>
    /// <param name="path">The path and name of the file to create.</param>
    /// <param name="rle">True if the data is compressed, false if it is not.</param>
    /// <returns>Always returns true</returns>
	public bool WriteToFile (string path, bool rle = true)
	{
		var bpp = (int)Format;
		using (var writer = new BinaryWriter (File.Create (path))) {
			var header = new TGAHeader {
				IdLength = 0, // The IDLength set to 0 indicates that there is no image identification field in the TGA file
				ColorMapType = 0, // a value of 0 indicates that no palette is included
				BitsPerPixel = (byte)(bpp * 8),
				Width = (short)Width,
				Height = (short)Height,
				DataTypeCode = DataTypeFor (bpp, rle),
				ImageDescriptor = (byte)(0x20 | (Format == Format.BGRA ? 8 : 0)) // top-left origin
			};
			WriteTo (writer, header);
			if (!rle)
				writer.Write (buffer);
			else
				UnloadRleData (writer);
		}
		return true;
	}

    /// <summary>
    /// Loads image using the given path
    /// </summary>
    /// <param name="path">The path and name of the file to be loaded</param>
    /// <returns>The Loaded Image</returns>
	public static Image Load (string path)
	{
		using (var reader = new BinaryReader (File.OpenRead (path))) {
			var header = ReadHeader (reader);

			var height = header.Height;
			var width = header.Width;
			var bytespp = header.BitsPerPixel >> 3;
			var format = (Format)bytespp;

			if (width <= 0 || height <= 0)
				throw new InvalidProgramException ($"bad image size: width={width} height={height}");
			if (format != Format.BGR && format != Format.BGRA && format != Format.GRAYSCALE)
				throw new InvalidProgramException ($"unknown format {format}");

			var img = new Image (width, height, format);

			switch (header.DataTypeCode) {
			case DataType.UncompressedTrueColorImage:
			case DataType.UncompressedBlackAndWhiteImage:
				reader.Read (img.buffer, 0, img.buffer.Length);
				break;
			case DataType.RleTrueColorImage:
			case DataType.RleBlackAndWhiteImage:
				img.LoadRleData (reader);
				break;
			default:
				throw new InvalidProgramException ($"unsupported image format {header.DataTypeCode}");
			}

			if ((header.ImageDescriptor & 0x20) == 0)
				img.VerticalFlip ();

			return img;
		}
	}

    /// <summary>
    /// uses a BinaryWriter to write the header's unsigned bytes to the current stream and advances the stream position by one byte
    /// </summary>
    /// <param name="writer">Writes an unsigned byte to the current stream and advances the stream position by one byte.</param>
    /// <param name="header">files include headers that define the image size, number of colors, and other information needed to display the image. 
    /// http://www.fastgraph.com/help/image_file_header_formats.html</param>
	static void WriteTo (BinaryWriter writer, TGAHeader header)
	{
		writer.Write (header.IdLength);
		writer.Write (header.ColorMapType);
		writer.Write ((byte)header.DataTypeCode);
		writer.Write (header.ColorMapOrigin);
		writer.Write (header.ColorMapLength);
		writer.Write (header.ColorMapDepth);
		writer.Write (header.OriginX);
		writer.Write (header.OriginY);
		writer.Write (header.Width);
		writer.Write (header.Height);
		writer.Write (header.BitsPerPixel);
		writer.Write (header.ImageDescriptor);
	}

    /// <summary>
    /// Reads the underlying stream to create a header
    /// </summary>
    /// <param name="reader">Reads characters from the underlying stream and advances the current position of the stream in accordance 
    /// with the Encoding used and the specific character being read from the stream.</param>
    /// <returns>Returns the header that was created</returns>
	static TGAHeader ReadHeader (BinaryReader reader)
	{
		var header = new TGAHeader {
			IdLength = reader.ReadByte (),
			ColorMapType = reader.ReadByte (),
			DataTypeCode = (DataType)reader.ReadByte (),
			ColorMapOrigin = reader.ReadInt16 (),
			ColorMapLength = reader.ReadInt16 (),
			ColorMapDepth = reader.ReadByte (),
			OriginX = reader.ReadInt16 (),
			OriginY = reader.ReadInt16 (),
			Width = reader.ReadInt16 (),
			Height = reader.ReadInt16 (),
			BitsPerPixel = reader.ReadByte (),
			ImageDescriptor = reader.ReadByte ()
		};
		return header;
	}

    /// <summary>
    /// Uses a writer to decompress the data from the RLE
    /// </summary>
    /// <param name="writer">Writes an unsigned byte to the current stream and advances the stream position by one byte.</param>
    /// <returns>Returns true when it's finished decompressing</returns>
	bool UnloadRleData (BinaryWriter writer)
	{
		const int max_chunk_length = 128;
		int npixels = Width * Height;
		int curpix = 0;
		var bpp = (int)Format;

		while (curpix < npixels) {
			int chunkstart = curpix * bpp;
			int curbyte = curpix * bpp;
			int run_length = 1;
			bool literal = true;
			while (curpix + run_length < npixels && run_length < max_chunk_length && curpix + run_length < curpix + Width) {
				bool succ_eq = true;
				for (int t = 0; succ_eq && t < bpp; t++)
					succ_eq = (buffer [curbyte + t] == buffer [curbyte + t + bpp]);
				curbyte += bpp;
				if (1 == run_length)
					literal = !succ_eq;
				if (literal && succ_eq) {
					run_length--;
					break;
				}
				if (!literal && !succ_eq)
					break;
				run_length++;
			}
			curpix += run_length;

			writer.Write ((byte)(literal ? run_length - 1 : 128 + (run_length - 1)));
			writer.Write (buffer, chunkstart, literal ? run_length * bpp : bpp);
		}
		return true;
	}

    /// <summary>
    /// Uses a reader to compress the data using the Run-Length Encoder
    /// </summary>
    /// <param name="reader">Reads characters from the underlying stream and advances the current position of the stream in accordance 
    /// with the Encoding used and the specific character being read from the stream.</param>
	void LoadRleData (BinaryReader reader)
	{
		var pixelcount = Width * Height;
		var currentpixel = 0;
		var currentbyte = 0;

		var bytespp = (int)Format;
		var color = new byte [4];

		do {
			var chunkheader = reader.ReadByte ();
			if (chunkheader < 128) {
				chunkheader++;
				for (int i = 0; i < chunkheader; i++) {
					for (int t = 0; t < bytespp; t++)
						buffer [currentbyte++] = reader.ReadByte ();
					currentpixel++;
					if (currentpixel > pixelcount)
						throw new InvalidProgramException ("Too many pixels read");
				}
			} else {
				chunkheader -= 127;
				reader.Read (color, 0, bytespp);
				for (int i = 0; i < chunkheader; i++) {
					for (int t = 0; t < bytespp; t++)
						buffer [currentbyte++] = color [t];
					currentpixel++;
					if (currentpixel > pixelcount)
						throw new InvalidProgramException ("Too many pixels read");
				}
			}
		} while (currentpixel < pixelcount);
	}

    /// <summary>
    /// Takes in the Bits-Per-Pixel (bpp) as an int and converts it back into a format and
    /// takes that format and decides whether it has been compressed or not based on whether rle is true or false and returns
    /// a data type based on that
    /// </summary>
    /// <param name="bpp">Bits-Per-Pixel</param>
    /// <param name="rle">Run-length Encoder</param>
    /// <returns>returns a data type</returns>
	static DataType DataTypeFor (int bpp, bool rle)
	{
		var format = (Format)bpp;
		if (format == Format.GRAYSCALE)
			return rle ? DataType.RleBlackAndWhiteImage : DataType.UncompressedBlackAndWhiteImage;
		return rle ? DataType.RleTrueColorImage : DataType.UncompressedTrueColorImage;
	}
}


