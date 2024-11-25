using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Compatibility;
using System;
using System.Collections.Generic;
using CommunityToolkit.Maui;
using Grid = Microsoft.Maui.Controls.Grid;
using Microsoft.Maui.Storage;
using CommunityToolkit.Maui.Storage;
using System.Text;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;

namespace Lab1MAUI
{
    public partial class MainPage : ContentPage
    {
        int countColumn = 4; 
        int countRow = 4; 
        List<List<InputStructure>> cells = new List<List<InputStructure>>();

        public MainPage()
        {
            InitializeComponent();
            CreateGrid();

            for (int i = 0; i < countColumn; i++)
            {
                cells.Add(new List<InputStructure>());
            }
        }

        public void CreateGrid()
        {
            AddColumnsAndColumnLabels();
            AddRowsAndCellEntries();
        }

        public void AddColumnsAndColumnLabels()
        {
            grid.RowDefinitions.Add(new RowDefinition());
            for (int col = 0; col < countColumn + 1; col++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                if (col > 0)
                {
                    var label = new Label
                    {
                        Text = GetColumnName(col),
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Grid.SetRow(label, 0);
                    Grid.SetColumn(label, col);
                    grid.Children.Add(label);
                }
            }
        }

        public void AddRowsAndCellEntries()
        {
            for (int i = 0; i < countColumn; i++)
            {
                cells.Add(new List<InputStructure>());
            }

            for (int row = 0; row < countRow; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                var label = new Label
                {
                    Text = (row + 1).ToString(),
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                Grid.SetRow(label, row + 1);
                Grid.SetColumn(label, 0);
                grid.Children.Add(label);
                for (int col = 0; col < countColumn; col++)
                {
                    var entry = new Entry
                    {
                        Text = "",
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center
                    };
                    entry.Unfocused += Entry_Unfocused; 
                    entry.Focused += Entry_Focused;
                    Grid.SetRow(entry, row + 1);
                    Grid.SetColumn(entry, col + 1);
                    grid.Children.Add(entry);

                    cells[col].Add(new InputStructure() { Formula = String.Empty, Entry = entry });
                }
            }
        }

        public string GetColumnName(int colIndex)
        {
            int dividend = colIndex;
            string columnName = string.Empty;
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }
            return columnName;
        }

        public void Entry_Unfocused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            int row = Grid.GetRow(entry) - 1;
            int col = Grid.GetColumn(entry) - 1;
            string content = entry.Text;

            cells[col][row].Formula = content;
        }

        public void Entry_Focused(object sender, FocusEventArgs e)
        {
            var entry = (Entry)sender;
            var row = Grid.GetRow(entry) - 1;
            var col = Grid.GetColumn(entry) - 1;
            var content = entry.Text;
            if (cells[col][row].Formula.Length > 0)
            {
                cells[col][row].Entry.Text = cells[col][row].Formula;
            } else
            {
                cells[col][row].Entry.Text = content;
            }
            
        }

        public void onEvaluateButton(object sender, EventArgs e)
        {
            for (int i = 0; i < countColumn; i++)
            {
                for (int j = 0; j < countRow; j++)
                {
                    CalculateCell(i, j);
                }
            }
        }

        public void CalculateCell(int i, int j)
        {
            try
            {
                if (cells[i][j].Formula == String.Empty)
                    return;

                if (float.TryParse(cells[i][j].Formula, out float val))
                {
                    cells[i][j].Entry.Text = val.ToString();
                    return;
                }


                string result = Evaluator.Evaluate(CalcLink(cells[i][j].Formula)).ToString();

                if (result == "∞")
                    cells[i][j].Entry.Text = "ERROR";
                else
                    cells[i][j].Entry.Text = result;
            }
            catch
            {
                cells[i][j].Entry.Text = "ERROR";
            }
        }
        public void setCell(int i, int j, string value)
        {
            cells[i][j].Formula = value;
        }

        public string getCell(int i, int j)
        {
            return cells[i][j].Entry.Text;
        }
        public string CalcLink(string input)
        {
            if (string.IsNullOrEmpty(input))
                return null;

            StringBuilder output = new StringBuilder();
            Dictionary<string, string> cellValues = new Dictionary<string, string>();

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]) && i + 1 < input.Length && char.IsDigit(input[i + 1]))
                {
                    int col = char.ToUpper(input[i]) - 'A';
                    int j = i + 1;
                    while (j < input.Length && char.IsDigit(input[j]))
                    {
                        j++;
                    }
                    int row = int.Parse(input.Substring(i + 1, j - i - 1)) - 1;
                    string cellAddress = $"{char.ToUpper(input[i])}{row + 1}";

                    if (!cellValues.ContainsKey(cellAddress))
                    {
                        string cellValue;
                        if (col < cells.Count && row < cells[col].Count && cells[col][row].Formula != string.Empty)
                        {
    
                            cellValue = CalcLink(cells[col][row].Formula);
                            cellValues[cellAddress] = cellValue;
                        }
                        else if (col < cells.Count && row < cells[col].Count)
                        {

                            cellValue = cells[col][row].Entry.Text;
                            cellValues[cellAddress] = cellValue;
                        }
                        else
                        {

                            return "ERROR";
                        }
                    }

                    output.Append(cellValues[cellAddress]);
                    i = j - 1; 
                }
                else
                {
                    output.Append(input[i]);
                }
            }

            return output.ToString().Replace(" ", "");
        }

        public int NextNumber(string input, int index)
        {
            bool isNum = int.TryParse(input.Substring(index), out int num);

            if (isNum)

                return 10 * num + NextNumber(input, ++index);
            else
                return 0;
        }

        public static int CountDigits(int number)
        {
            if (number == 0)
            {
                return 1;
            }

            int count = 0;
            while (number != 0)
            {
                number /= 10;
                count++;
            }
            return count;
        }

        public async void onSaveButton(object sender, EventArgs e)
        {
            int currentCountRow = grid.RowDefinitions.Count;
            int currentCountColumn = grid.ColumnDefinitions.Count;

            string[][] cellValues = new string[currentCountRow][];

            for (int i = 0; i < currentCountRow; i++)
            {
                cellValues[i] = new string[currentCountColumn];
            }

            List<string> results = new List<string>();

            foreach (var child in grid.Children.OfType<Entry>())
            {
                int row = grid.GetRow(child);
                int col = grid.GetColumn(child);

                if (row == 0 || col == 0)
                    continue;

                cellValues[row - 1][col - 1] = ((Entry)child).Text;
            }

            bool saveResult = await FileOperation.Save(cellValues);
            string strSaveResult = saveResult ? "Файл успішно збережено." : "Файл не збережено";
            await DisplayAlert("Результат збереження", strSaveResult, "Ок");

        }

        public async void onReadButton(object sender, EventArgs e)
        {
            string[,] results = await FileOperation.Open();

            if (results == null)
                return;

            foreach (var child in grid.Children.OfType<Entry>())
            {
                int row = grid.GetRow(child);
                int col = grid.GetColumn(child);

                if (row == 0 || col == 0)
                    continue;
                try
                {
                    ((Entry)child).Text = results[row - 1, col - 1];
                    
                }
                catch { }
            }
        }

        public async void onExitButton(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Підтвердження", "Ви дійсно хочете вийти?",
           "Так", "Ні");
                if (answer)
                {
                    System.Environment.Exit(0);
                }
        }

        public async void onInfoButton(object sender, EventArgs e)
        {
            await DisplayAlert("Довідка", "Лабораторна робота Номер 1, Група К-23 ТОКАРЕВ IЛЛЯ\n Варіант 1: бінарні операції, mod div, унарні операції, піднесення в степінь",
           "OK");
        }

        public async void onDeleteRowButton(object sender, EventArgs e)
        {
            if (grid.RowDefinitions.Count > 2) // Check if there are more than 1 data row
            {
                int columnValue = grid.ColumnDefinitions.Count;
                int rowValue = grid.RowDefinitions.Count;

                for (int i = 0; i < columnValue; i++)
                {
                    var childToRemove = grid.Children.FirstOrDefault(child => grid.GetRow(child) == rowValue - 1 && grid.GetColumn(child) == i);
                    if (childToRemove != null)
                    {
                        grid.Children.Remove(childToRemove);
                    }
                }
                grid.RowDefinitions.RemoveAt(grid.RowDefinitions.Count - 1);
                countRow--;

                for (int i = 0; i < countColumn; i++)
                {
                    cells[i].RemoveAt(countRow);
                }
            }
            else
            {
                await DisplayAlert("Error", "Ви не можете видалити рядок, коли залишився лише один.", "OK");
            }
        }



        public async void onDeleteColumnButton(object sender, EventArgs e)
        {
            if (grid.ColumnDefinitions.Count > 2) 
            {
                int rowValue = grid.RowDefinitions.Count;
                int columnValue = grid.ColumnDefinitions.Count;

                for (int i = 0; i < rowValue; i++)
                {
                    var childToRemove = grid.Children.FirstOrDefault(child => grid.GetRow(child) == i && grid.GetColumn(child) == columnValue - 1);
                    if (childToRemove != null)
                    {
                        grid.Children.Remove(childToRemove);
                    }
                }
                grid.ColumnDefinitions.RemoveAt(grid.ColumnDefinitions.Count - 1);
                countColumn--;
                cells.RemoveAt(countColumn);
            }
            else
            {
                await DisplayAlert("Error", "Ви не можете видалити стовпчик, коли залишився лише один.", "OK");
            }
        }

        public void onAddRowButton(object sender, EventArgs e)
        {
            int newRow = grid.RowDefinitions.Count;
            int countColumn = grid.ColumnDefinitions.Count;

            countRow++;
            grid.RowDefinitions.Add(new RowDefinition());
            var label = new Label
            {
                Text = newRow.ToString(),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, newRow);
            Grid.SetColumn(label, 0);
            grid.Children.Add(label);
            for (int col = 0; col < countColumn - 1; col++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                entry.Unfocused += Entry_Unfocused;
                entry.Focused += Entry_Focused;
                Grid.SetRow(entry, newRow);
                Grid.SetColumn(entry, col + 1);
                grid.Children.Add(entry);

                cells[col].Add(new InputStructure() { Formula = String.Empty, Entry = entry });
            }
        }

        public void onAddColumnButton(object sender, EventArgs e)
        {
            int newColumn = grid.ColumnDefinitions.Count;
            int countRow = grid.RowDefinitions.Count;

            countColumn++;
            cells.Add(new List<InputStructure>());
            grid.ColumnDefinitions.Add(new ColumnDefinition());
          
            var label = new Label
            {
                Text = GetColumnName(newColumn),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            Grid.SetRow(label, 0);
            Grid.SetColumn(label, newColumn);
            grid.Children.Add(label);
            
            for (int row = 0; row < countRow - 1; row++)
            {
                var entry = new Entry
                {
                    Text = "",
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };
                entry.Unfocused += Entry_Unfocused;
                entry.Focused += Entry_Focused;
                Grid.SetRow(entry, row + 1);
                Grid.SetColumn(entry, newColumn);
                grid.Children.Add(entry);

                cells[countColumn - 1].Add(new InputStructure() { Formula = String.Empty, Entry = entry });
            }
        }
    }
}