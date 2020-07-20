using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace QrCodeGenerator_exe
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				var text = args[0];
				var path = args[1];

				var dir = Path.GetDirectoryName(path);
				if (!Directory.Exists(dir))
					Directory.CreateDirectory(dir);

				text = text.Replace('#', ' ');
				var qrCode = GerarQRCodeDataMatrix(text);
				ConvertTo24bpp(qrCode)
					.Save(path, ImageFormat.Bmp);
			}
			catch (Exception ex)
			{
				throw new Exception($"Erro na criação do QrCode - {ex.Message}");
			}
		}

		private static Bitmap GerarQRCodeDataMatrix(string text)
		{
			var width = 94;
			var height = 94;
			try
			{
				var bw = new ZXing.BarcodeWriter();
				var encOptions = new ZXing.Common.EncodingOptions()
				{
					Width = width,
					Height = height,
					Margin = 0
				};
				bw.Options = encOptions;
				bw.Format = ZXing.BarcodeFormat.DATA_MATRIX;

				var resultado = new Bitmap(bw.Write(text), width, height);
				return resultado;
			}
			catch (Exception ex)
			{
				throw new Exception("Erro na geração do QrCode - " + ex.Message);
			}
		}
		public static Bitmap ConvertTo24bpp(Image img)
		{
			var bmp = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			using (var gr = Graphics.FromImage(bmp))
				gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
			return bmp;
		}

	}
}
