using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace QrCodeGenerator.Controllers
{
	public class HomeController : ApiController
	{
		public string ConteudoQrCode { get; private set; }
		public int Largura { get; private set; }
		public int Altura { get; private set; }

		[HttpGet]
		public HttpResponseMessage Get(string text = "www.google.com", int largura = 150, int altura = 150)
		{
			var qrCode = GerarQRCode(largura, altura, text);
			HttpResponseMessage response = CriarRespostaHttpImagem(qrCode, ImageFormat.Png);
			return response;
		}

		private HttpResponseMessage CriarRespostaHttpImagem(Bitmap qrCode, ImageFormat formato)
		{
			HttpResponseMessage response = Request.CreateResponse();

			var ms = new MemoryStream();
			qrCode.Save(ms, formato);
			ms.Position = 0;
			response.Content = new StreamContent(ms);
			response.Content.Headers.ContentLength = ms.Length;
			response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");

			return response;
		}

		private Bitmap GerarQRCode(int width, int height, string text)
		{
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
				bw.Format = ZXing.BarcodeFormat.QR_CODE;
				var resultado = new Bitmap(bw.Write(text));
				return resultado;
			}
			catch(Exception ex)
			{
				throw new Exception("Erro na geração do QrCode - " + ex.Message);
			}
		}

	}
}
