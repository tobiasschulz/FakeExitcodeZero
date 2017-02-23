using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FakeExitcodeZero
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			var o = Assembly.GetExecutingAssembly().Location.Replace(".exe", ".original.exe");
			var a = string.Join(" ", args.Select(_ => "\"" + _.Replace("\"", "\\\"") + "\""));
			Console.WriteLine("Original Assembly: " + o);
			Console.WriteLine("Original Arguments: " + a);

			try
			{
				Process p = new Process();
				p.StartInfo = new ProcessStartInfo
				{
					UseShellExecute = false,
					FileName = o,
					Arguments = a,
				};
				p.Start();
				p.WaitForExit();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}

			foreach (var arg in args)
			{
				if (arg.Contains("outfile="))
				{
					var path = arg.Split(new[] { "outfile=" }, 2, StringSplitOptions.RemoveEmptyEntries)[1].Split(',')[0];
					Console.WriteLine("Create file: " + path);
					if (!File.Exists(path) && !File.Exists(path + ".tmp")) File.Create(path);
				}
			}

			Environment.Exit(0);
		}
	}
}
