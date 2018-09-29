using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using static MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes.ImageSource;

namespace BookConverter.Custom
{
	class CoreImageSourceImpl : IImageSource
	{
		readonly int _quality;
		readonly Func<Stream> _getImageStream;

		public int Width { get; }

		public int Height { get; }

		public string Name { get; }

		public CoreImageSourceImpl(string name, Func<Stream> getImageStream, int quality)
		{
			Name = name;

			_getImageStream = getImageStream;
			_quality = quality;

			using (var imageStream = getImageStream.Invoke())
			using (var image = DecodeStream(imageStream))
			{
				Width = image.Width;
				Height = image.Height;
			}
		}

		public void SaveAsJpeg(MemoryStream ms)
		{
			using (var imageStream = _getImageStream())
			using (var image = DecodeStream(imageStream))
			{
				image.SaveAsJpeg(ms, new JpegEncoder
				{
					Quality = _quality,
				});
			}
		}

		Image<Rgba32> DecodeStream(Stream ras) => Image.Load(ras);
	}
}
