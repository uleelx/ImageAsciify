using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Text;

namespace ImageAsciify
{
	public class Asciify
	{
		private static int[][] kernels = {
			new int[]
			{
				 0, -1,  0,
				-1,  5, -1,
				 0, -1,  0
			},
			new int[]
			{
				-1,  0, -1,
				 0,  5,  0,
				-1,  0, -1
			},
			new int[]
			{
				-1, -1, -1,
				-1,  9, -1,
				-1, -1, -1
			},
			new int[]
			{
				-1, -2, -1,
				-2, 13, -2,
				-1, -2, -1
			},
		};
		public static string ImageToHTML(string filePath, bool histogramEqualization, int maxLength, int sharpenKernel, int BitDepth)
		{
			char[] chars = new char[] { '@', '%', 'M', 'Q', 'R', 'O', 'S', 'Z', 'Y', ')', '>', '!', ':', ',', '.', ' ' };
			int[] kernal = kernels[sharpenKernel];
			StringBuilder sb = new StringBuilder();
			string html_head = "<!DOCTYPE html><html><head><meta http-equiv=\"content-type\" content=\"text/html; charset=UTF-8\"></head><body><pre style=\"font-size: 2px; line-height: 50%;\">\n";
			string html_tail = "</pre></body></html>";
			sb.Append(html_head);

			using (Image<Rgba32> image = (Image<Rgba32>)Image.Load(filePath))
			{
				image.Mutate(x => x.AutoOrient());

				int m = Math.Max(image.Height, image.Width);
				double delta = Math.Max(m * 1.0 / maxLength, 1);
				int w = (int)(image.Width / delta);
				int h = (int)(image.Height / delta);

				if (histogramEqualization)
				{
					image.Mutate(x => x.HistogramEqualization());
				}

				image.Mutate(x => x
					 .Resize(w, h, KnownResamplers.Bicubic)
					 .Grayscale());

				int maxCharIndex = (1 << BitDepth) - 1;

				for (int y = 0; y < h; y++)
				{
					for (int x = 0; x < w; x++)
					{
						int l = image[x, y].R;
						if (y > 0 && y < h - 1 && x > 0 && x < w - 1)
						{
							l = kernal[0] * image[x - 1, y - 1].R
							  + kernal[1] * image[x, y - 1].R
							  + kernal[2] * image[x + 1, y - 1].R
							  + kernal[3] * image[x - 1, y].R
							  + kernal[4] * image[x, y].R
							  + kernal[5] * image[x + 1, y].R
							  + kernal[6] * image[x - 1, y + 1].R
							  + kernal[7] * image[x, y + 1].R
							  + kernal[8] * image[x + 1, y + 1].R;
							if (l < 0) l = 0;
							if (l > 255) l = 255;
						}

						sb.Append(chars[(int)Math.Round(l * maxCharIndex / 255.0) * 15 / maxCharIndex]);
					}
					sb.Append("\n");
				}
			}
			sb.Append(html_tail);

			return sb.ToString();
		}
	}
}
