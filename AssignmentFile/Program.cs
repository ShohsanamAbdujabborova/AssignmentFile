using System;
//using.System.diagnostics bu diagnostika va monitoring funksiyalarini o'z ichiga oladi va 
using System.Diagnostics;//qancha vaqt resurs sarflanganlikni o'lchash kutubxonasi
using System.IO;
public static class Compression
{
    public static void Compress(string inputFilePath, string outputFilePath)
    {
        byte[] inputData = File.ReadAllBytes(inputFilePath);
        byte[] compressedData = RunLengthEncode(inputData);

        File.WriteAllBytes(outputFilePath, compressedData);
    }
    public static void Decompress(string inputFilePath, string outputFilePath)
    {
        byte[] compressedData = File.ReadAllBytes(inputFilePath);
        byte[] decompressedData = RunLengthDecode(compressedData);

        File.WriteAllBytes(outputFilePath, decompressedData);
    }

    private static byte[] RunLengthEncode(byte[] data)
    {
        using (MemoryStream compressedStream = new MemoryStream())
        {
            int count = 1;

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] == data[i - 1])
                {
                    count++;
                }
                else
                {
                    compressedStream.WriteByte(data[i - 1]);
                    compressedStream.WriteByte((byte)count);
                    count = 1;
                }
            }

            compressedStream.WriteByte(data[data.Length - 1]);
            compressedStream.WriteByte((byte)count);

            return compressedStream.ToArray();
        }
    }

    private static byte[] RunLengthDecode(byte[] compressedData)
    {
        using (MemoryStream decompressedStream = new MemoryStream())
        {
            for (int i = 0; i < compressedData.Length; i += 2)
            {
                byte value = compressedData[i];
                int count = compressedData[i + 1];

                for (int j = 0; j < count; j++)
                {
                    decompressedStream.WriteByte(value);
                }
            }

            return decompressedStream.ToArray();
        }
    }

    public static void MeasureAndOptimizePerformance()
    {
        string inputFilePath = "D:\\projects\\AssignmentFile\\AssignmentFile\\large_file.bin";
        string compressedFilePath = "D:\\projects\\AssignmentFile\\AssignmentFile\\compressed_file.bin";
        string decompressedFilePath = "D:\\projects\\AssignmentFile\\AssignmentFile\\decompressed_file.bin";
        GenerateLargeBinaryFile(inputFilePath, 1000000);

        //using System.Diagnostics; bu kutubxonadaa Stopwatch orqali
        //kompressiya va dekompilyatsiya jarayonlarining during time ni o'lchayapmiz

        Stopwatch compressionStopwatch = Stopwatch.StartNew();
        Compress(inputFilePath, compressedFilePath);
        compressionStopwatch.Stop();
        Console.WriteLine($"Compression Time: {compressionStopwatch.ElapsedMilliseconds} ms");

        System.Diagnostics.Stopwatch decompressionStopwatch = Stopwatch.StartNew();
        Decompress(compressedFilePath, decompressedFilePath);
        decompressionStopwatch.Stop();

        Console.WriteLine($"Decompression Time: {decompressionStopwatch.ElapsedMilliseconds} ms");
        double compressionRatio = CalculateCompressionRatio(inputFilePath, compressedFilePath);
        Console.WriteLine($"Compression Ratio: {compressionRatio:P2}");
    }

    private static double CalculateCompressionRatio(string originalFilePath, string compressedFilePath)
    {
        long originalFileSize = new FileInfo(originalFilePath).Length;
        long compressedFileSize = new FileInfo(compressedFilePath).Length;

        return (double)compressedFileSize / originalFileSize;
    }

    private static void GenerateLargeBinaryFile(string filePath, int sizeInBytes)
    {
        Random random = new Random();
        byte[] data = new byte[sizeInBytes];
        random.NextBytes(data);
        File.WriteAllBytes(filePath, data);
    }

    public static void Main()
    {
        MeasureAndOptimizePerformance();
    }
}

