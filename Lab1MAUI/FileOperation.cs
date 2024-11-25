using CommunityToolkit.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1MAUI
{
    internal static class FileOperation
    {
        public static async Task<bool> Save(string[][] cellValues)
        {
            var formattedData = new StringBuilder();
            foreach (var row in cellValues)
            {
                formattedData.AppendLine(string.Join("\t", row));
            }

            var dataStream = new MemoryStream(Encoding.Default.GetBytes(formattedData.ToString()));

            var saveOutcome = await FileSaver.Default.SaveAsync("test.txt", dataStream);

            return saveOutcome.IsSuccessful;
        }

        public static async Task<string[,]> Open()
        {
            var selectedFile = await FilePicker.PickAsync();

            if (selectedFile == null) return null;

            try
            {
                using var fileStream = await selectedFile.OpenReadAsync();
                using var reader = new StreamReader(fileStream);

                var dataLines = new List<string[]>();
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    dataLines.Add(line.Split('\t'));
                }

                if (dataLines.Count == 0) return null;

                var dataArray = new string[dataLines.Count, dataLines[0].Length];

                for (int i = 0; i < dataLines.Count; i++)
                {
                    for (int j = 0; j < dataLines[i].Length; j++)
                    {
                        dataArray[i, j] = dataLines[i][j];
                    }
                }

                return dataArray;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while reading file: {ex.Message}");
                return null;
            }
        }
    }
}
