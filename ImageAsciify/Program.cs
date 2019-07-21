using CommandLine;
using System;
using System.IO;

namespace ImageAsciify
{
	class Program
	{
		public class Options
		{
			[Option('g', HelpText = "直方图均衡化", Required = false, Default = false)]
			public bool HistogramEqualization { get; set; }

			[Value(0, HelpText = "图像文件路径", Required = true)]
			public string FilePath { get; set; }

			[Option('l', Default = 512, HelpText = "最大像素", Required = false)]
			public int MaxLength { get; set; }

			[Option('s', Default = 0, HelpText = "锐化卷积核（可选0~3）", Required = false)]
			public int SharpenKernel { get; set; }

			[Option('b', Default = 4, HelpText = "灰度位数（可选4/2/1）", Required = false)]
			public int BitDepth { get; set; }
		}

		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(o =>
				{
					var html = Asciify.ImageToHTML(o.FilePath, o.HistogramEqualization, o.MaxLength, o.SharpenKernel,o.BitDepth);
					string tempFile = Path.GetTempFileName() + ".html";
					File.WriteAllText(tempFile, html);
					System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
					{
						UseShellExecute = true,
						FileName = tempFile
					});
				});
		}
	}
}
