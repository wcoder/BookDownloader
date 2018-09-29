using System;
using System.IO;
using MigraDocCore.DocumentObjectModel.MigraDoc.DocumentObjectModel.Shapes;

namespace BookConverter.Custom
{
	class CoreImageSource : ImageSource
	{
		protected override IImageSource FromBinaryImpl(string name, Func<byte[]> imageSource, int? quality = 75)
		{
			return new CoreImageSourceImpl(name, () => new MemoryStream(imageSource()), (int)quality);
		}

		protected override IImageSource FromFileImpl(string path, int? quality = 75)
		{
			return new CoreImageSourceImpl(Path.GetFileName(path),
				() => new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read), (int)quality);
		}

		protected override IImageSource FromStreamImpl(string name, Func<Stream> imageStream, int? quality = 75)
		{
			return new CoreImageSourceImpl(name, imageStream.Invoke, (int)quality);
		}
	}
}
