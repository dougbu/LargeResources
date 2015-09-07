using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LargeResources
{
    public class Program
    {
        private static readonly Assembly _assembly = typeof(Program).GetTypeInfo().Assembly;
        private static readonly Dictionary<string, string> _pathToResource = new Dictionary<string, string>
        {
            { "resources/a.txt", "LargeResources.resources.a.txt" },
            { "resources/b.txt", "LargeResources.resources.b.txt" },
            { "resources/c.txt", "LargeResources.resources.c.txt" },
            { "resources/d.txt", "LargeResources.resources.d.txt" },
        };

        public void Main(string[] args)
        {
            foreach (var kvp in _pathToResource)
            {
                var success = true;
                var fileContent = new StreamReader(new FileStream(kvp.Key, FileMode.Open)).ReadToEnd();
                var resourceContent = new StreamReader(_assembly.GetManifestResourceStream(kvp.Value)).ReadToEnd();
                if (fileContent.Length > resourceContent.Length)
                {
                    success = false;
                    Console.WriteLine($"{ kvp.Key } is { fileContent.Length - resourceContent.Length } chars longer than { kvp.Value }.");
                    Console.WriteLine($"\tFile ends with     '...{ fileContent.Substring(fileContent.Length - 20) }'.");
                    Console.WriteLine($"\tResource ends with '...{ resourceContent.Substring(resourceContent.Length - 20) }'.");
                }
                else if (fileContent.Length < resourceContent.Length)
                {
                    success = false;
                    Console.WriteLine($"{ kvp.Key } is { resourceContent.Length - fileContent.Length } chars shorter than { kvp.Value }.");
                    Console.WriteLine($"\tFile ends with     '...{ fileContent.Substring(fileContent.Length - 20) }'.");
                    Console.WriteLine($"\tResource ends with '...{ resourceContent.Substring(resourceContent.Length - 20) }'.");
                }
                else
                {
                    for (var i = 0; i < fileContent.Length; i++)
                    {
                        if (fileContent[i] != resourceContent[i])
                        {
                            success = false;

                            var length = (i + 20 > fileContent.Length) ? fileContent.Length - i : 20;
                            Console.WriteLine($"Mismatch at index '{ i }' in { kvp.Key }.");
                            Console.WriteLine($"\tFile contains     '...{ fileContent.Substring(i, length) }'.");
                            Console.WriteLine($"\tResource contains '...{ resourceContent.Substring(i, length) }'.");
                            break;
                        }
                    }
                }

                if (success)
                {
                    Console.WriteLine($"Success for { kvp.Key }.");
                }
            }
        }
    }
}
