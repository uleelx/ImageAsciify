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
		}

		static void Main(string[] args)
		{
			Parser.Default.ParseArguments<Options>(args)
				.WithParsed(o =>
				{
					var html = Asciify.ImageToHTML(o.FilePath, o.HistogramEqualization, o.MaxLength);
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
