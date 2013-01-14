using System;
using System.IO;
using System.Reflection;
using CommandLine;
using woanware;

namespace xor
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                AssemblyName assemblyName = assembly.GetName();

                Console.WriteLine(Environment.NewLine + "xor v" + assemblyName.Version.ToString(3) + Environment.NewLine);

                Options options = new Options();
                if (CommandLineParser.Default.ParseArguments(args, options) == false)
                {
                    return;
                }

                string fileName = System.IO.Path.GetFileName(options.Input);
                string outputPath = System.IO.Path.GetPathRoot(options.Input);

                byte[] tempKey = Text.HexStringToBytes(options.Key);
                int key = BitConverter.ToInt32(tempKey, 0);

                File.Delete(System.IO.Path.Combine(outputPath, fileName + ".xor"));

                switch (options.Mode)
                {
                    case "1":
                        PerformByteXor(options.Input, System.IO.Path.Combine(outputPath, fileName + ".xor"), key, 1);
                        break;
                    case "2":
                        PerformByteXor(options.Input, System.IO.Path.Combine(outputPath, fileName + ".xor"), key, 2);
                        break;
                    case "4":
                        PerformByteXor(options.Input, System.IO.Path.Combine(outputPath, fileName + ".xor"), key, 4);
                        break;
                    case "r":
                        PerformRollingXor(options.Input, System.IO.Path.Combine(outputPath, fileName + ".xor"), tempKey);
                        break;
                    default:
                        return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="key"></param>
        /// <param name="size"></param>
        private static void PerformByteXor(string inputFile, 
                                           string outputFile, 
                                           int key, 
                                           int size)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open, FileAccess.Read)))
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                byte[] buffer = new byte[size];
                int count;
                while ((count = binaryReader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    int temp = BitConverter.ToInt32(buffer, 0);
                    int xored = temp ^ key;
                    binaryWriter.Write(xored);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputFile"></param>
        /// <param name="outputFile"></param>
        /// <param name="key"></param>
        /// <param name="size"></param>
        private static void PerformRollingXor(string inputFile,
                                              string outputFile,
                                              byte[] key)
        {
            using (BinaryReader binaryReader = new BinaryReader(File.Open(inputFile, FileMode.Open, FileAccess.Read)))
            using (BinaryWriter binaryWriter = new BinaryWriter(File.Open(outputFile, FileMode.Create)))
            {
                byte[] buffer = new byte[key.Length];
                int count;
                while ((count = binaryReader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    for (int keyIndex = 0; keyIndex < key.Length; keyIndex++)
                    {
                        byte xored = (byte)(buffer[keyIndex] ^ key[keyIndex]);
                        binaryWriter.Write(xored);
                    }
                }
            }
        }
    }
}
