using System.IO;
using PrimeWeb.Tests;

string file = "indinfdump.bin";

Console.WriteLine("data parsing tests!");
Console.WriteLine($"Reading source file: {file}");

var data = File.ReadAllBytes(file);

var parser = new AppParser(data);

var result = parser.SplitHeader(data);

Console.WriteLine("header: -------");
Tools.PrintPacket(result.hpapp);

Console.WriteLine("contents: --------");
Tools.PrintPacket(result.files);

Console.ReadKey();
