using PhotoSauce.MagicScaler;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Text;

namespace ImageAsciify
{
	public class Asciify
	{
		public static string ImageToHTML(string filePath, bool histogramEqualization, int maxLength)
		{
			char[] chars = new char[] { 'W', 'M', 'Q', 'R', 'O', 'S', 'C', 'V', '?', ')', '>', '!', ':', ',', '.' };
			StringBuilder sb = new StringBuilder();
			string html_head = "<!DOCTYPE html><html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"></head><body style = \"font-size: 2px; line-height: 50%;\"><pre>\n";
			string html_tail = "</pre></body></html>";
			sb.Append(html_head);

			var outStream = new MemoryStream();

			MagicImageProcessor.ProcessImage(filePath, outStream, new ProcessImageSettings());

			outStream.Seek(0, SeekOrigin.Begin);

			using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(outStream))
			{
				int m = Math.Max(image.Height, image.Width);
				double delta = Math.Max(m * 1.0 / maxLength, 1);
				int w = (int)(image.Width / delta);
				int h = (int)(image.Height / delta);

				if (histogramEqualization)
				{
					image.Mutate(x => x.HistogramEqualization());
				}

				image.Mutate(x => x
					 .Resize(w, h)
					 .Grayscale());

				for (int y = 0; y < h; y++)
				{
					for (int x = 0; x < w; x++)
					{
						int l = image[x, y].R;
						if (y > 0 && y < h - 1 && x > 0 && x < w - 1)
						{
							l = l * 5 - image[x, y - 1].R - image[x, y + 1].R - image[x - 1, y].R - image[x + 1, y].R;
							if (l < 0) l = 0;
							if (l > 255) l = 255;
						}
						sb.Append(chars[l * 14 / 255]);
					}
					sb.Append("\n");
				}
			}
			sb.Append(html_tail);

			outStream.Dispose();

			return sb.ToString();
		}
	}
}
