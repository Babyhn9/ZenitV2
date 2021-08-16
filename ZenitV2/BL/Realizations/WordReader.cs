using Microsoft.Office.Interop.Word;
using ZenitV2.BL.Interfaces;
using ZenitV2.Data;
using ZenitV2.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace ZenitV2.BL.Realizations
{
    [Buissnes]
    public class WordReader : IWordReader
    {
        private class SearchSetting
        {
            public int TablePosition { get; set; }
            public int RowPossition { get; set; }
            public int ColumnPossition { get; set; }
            public string ColumnName { get; set; }
        }

        private List<string> _readingColumns = new List<string>();
        private string _pathToFile;
        private ICalculationBL _calculationBl;
        private Tables _wordTables;
        public WordReader(ICalculationBL calculationBL)
        {
            _calculationBl = calculationBL;
        }

        public void AddColumnToRead(string name)
        {
            if (!_readingColumns.Contains(name))
                _readingColumns.Add(name);
        }

        public bool BindFile(string path)
        {
            _pathToFile = path;
            _wordTables = Session.Current.CreateWordApplication().Documents.Open(_pathToFile).Tables;

            return true;
        }

        public void UnbindFile()
        {
            _pathToFile = "";
            _wordTables = null;
            _readingColumns.Clear();
            Session.Current.DestroyWordApplication();
        }

        public bool ContainsColumn(string name) => _readingColumns.Contains(name);

        public void InitForReadPZ()
        {
            _readingColumns.Clear();
            _readingColumns.Add("Comment");
            _readingColumns.Add("X (m)");
            _readingColumns.Add("Y (m)");
            _readingColumns.Add("Z (m)");
        }

        public void InitForReadSK42()
        {
            _readingColumns.Clear();
            _readingColumns.Add("Comment");
            _readingColumns.Add("Northing(m)");
            _readingColumns.Add("Easting(m)");
            _readingColumns.Add("Height (m)");
        }

        public List<List<WordInputData>> MapFile()
        {
            var result = new List<WordInputData>();
            var values = ReadFile();
            return values;
        }

        public void RemoveColumn(string name)
        {
            _readingColumns.Remove(name);
        }

        private List<List<WordInputData>> ReadFile()
        {

            List<SearchSetting> GenerateSearchSettings(List<Table> tables)
            {

                int rowPosition = 0;
                int columnPosition = 0;
                int tablePosition = 0;
                var findedSearchValues = new List<SearchSetting>();


                if (tables.Count > 0)
                {
                    //поиск индексов столбцов для чтения
                    foreach (Table table in tables)
                    {
                        tablePosition++;


                        foreach (Row row in table.Rows)
                        {
                            rowPosition++;

                            foreach (Cell cell in row.Cells)
                            {
                                if (findedSearchValues.Count == _readingColumns.Count)
                                    break;
                                
                                columnPosition++;
                                
                                if (_readingColumns.Contains(cell.Range.Text.Replace("\r\a", "")))
                                    findedSearchValues.Add(new SearchSetting
                                    {
                                        ColumnPossition = columnPosition,
                                        RowPossition = rowPosition + 1,
                                        ColumnName = cell.Range.Text.Replace("\r\a", ""),
                                        TablePosition = tablePosition
                                    });

                            }
                            columnPosition = 0;
                        }

                        rowPosition = 0;
                        columnPosition = 0;
                    }
                }

                return findedSearchValues;
            }
            List<List<WordInputData>> GetValues(List<Table> tables, List<SearchSetting> searchSettings)
            {
                var result = new List<List<WordInputData>>();
                for (var tableN = 0; tableN < tables.Count; tableN++)
                {
                    var tableValues = new List<WordInputData>();
                    var table = tables[tableN];
                    var currentSettings = searchSettings.Take(4 * (tableN + 1)).ToArray();

                    for (int row = currentSettings[0].RowPossition; row <= table.Rows.Count; row++)
                    {
                        var t1 = table.Cell(row, currentSettings[1].ColumnPossition).Range.Text.Replace("\r\a", "").Replace(".", ",");
                        var t2 = table.Cell(row, currentSettings[2].ColumnPossition).Range.Text.Replace("\r\a", "").Replace(".", ",");
                        var t3 = table.Cell(row, currentSettings[3].ColumnPossition).Range.Text.Replace("\r\a", "").Replace(".", ",");

                        var data = new WordInputData
                        {
                            Name = table.Cell(row, currentSettings[0].ColumnPossition).Range.Text.Replace("\r\a", ""),
                            North = double.Parse(t1),
                            East = double.Parse(t2),
                            Height = double.Parse(t3),
                        };

                        tableValues.Add(data);
                    }
                    result.Add(tableValues);
                }
                return result;
            }

            var tables = _wordTables;
            var allowedTables = new List<Table>();

            foreach (Table table in tables)
            {
                if (table.Cell(0, 1).Range.Text.ToLower().Contains("subnet"))
                    allowedTables.Add(table);
            }

            if (_readingColumns.Count == 0)
            {
                if (IsSK42(allowedTables.First()))
                    InitForReadSK42();
                if (IsPZ90(allowedTables.First()))
                    InitForReadPZ();
            }

            var searchSettings = GenerateSearchSettings(allowedTables);
            var result = GetValues(allowedTables, searchSettings);

            return Calculate(result);

        }


        private List<List<WordInputData>> Calculate(List<List<WordInputData>> values)
        {
            var result = new List<List<WordInputData>>();
            var wordInfo = values;
            var countOfEntries = wordInfo.Count();
            var calculatedMediumValues = new List<WordInputData>();

            for (int i = 0; i < wordInfo.First().Count(); i++)
            {
                var newMediumValues = new WordInputData();

                for (int pos = 0; pos < countOfEntries; pos++)
                {
                    var currentInfo = wordInfo[pos][i];
                    newMediumValues.Name = currentInfo.Name.Replace("СГС", "ОГП");
                    newMediumValues.North += currentInfo.North;
                    newMediumValues.East += currentInfo.East;
                    newMediumValues.Height += currentInfo.Height;
                }
                newMediumValues.North = _calculationBl.GauseRound(newMediumValues.North / countOfEntries, 3);
                newMediumValues.East = _calculationBl.GauseRound(newMediumValues.East / countOfEntries, 3);
                newMediumValues.Height = _calculationBl.GauseRound(newMediumValues.Height / countOfEntries, 3);
                calculatedMediumValues.Add(newMediumValues);
            }

            foreach (var line in wordInfo)
            {
                foreach (var item in line)
                {
                    item.East = _calculationBl.GauseRound(item.East, 3);
                    item.North = _calculationBl.GauseRound(item.North, 3);
                    item.Height = _calculationBl.GauseRound(item.Height, 3);
                }
            }


            for (int column = 0; column < wordInfo.First().Count; column++)
            {
                var list = new List<WordInputData>();

                for (int row = 0; row < countOfEntries; row++)
                {
                    list.Add(wordInfo[row][column]);
                }
                list.Add(calculatedMediumValues[column]);
                result.Add(list);
            }
            return result;
        }


        #region MyRegion
        /*
          for (int tableN = 1; tableN < document.Tables.Count; tableN++)
                    {
                        var table = document.Tables[tableN];
                        result.Add(new List<WordInputData>());

                        for (int row = 4; row <= table.Rows.Count; row++)
                        {

                            var test = "some any sting".Replace(" ", "");

                            var name = table.Cell(row, 3).Range.Text.Replace("\r\a", "");
                            var north = table.Cell(row, 4).Range.Text.Replace("\r\a", "").Replace(".", ",");
                            var east = table.Cell(row, 5).Range.Text.Replace("\r\a", "").Replace(".", ",");
                            var height = table.Cell(row, 6).Range.Text.Replace("\r\a", "").Replace(".", ",");
                            result[tableN - 1].Add(new WordInputData
                            {
                                Name = name,
                                North = double.Parse(north),
                                East = double.Parse(east),
                                Height = double.Parse(height)
                            });
                        }
                    }*/
        #endregion

        #region strange things

        /*
         * не удалять строчку range = test..., без неё, почему-то неправильно берутся значения из таблицы
         */
        private bool IsSK42(Table table)
        {
            var test = table.Rows[1].Cells;
            var range = test[1].Range;
            var text = table.Cell(0, 1).Range.Text;
            return text.Contains("SK1942");
        }
        private bool IsPZ90(Table table)
        {
            var test = table.Rows[1].Cells;
            var range = test[1].Range;
            var text = table.Cell(0, 1).Range.Text;
            return text.Contains("PZ90.11");
        }

        public IWordReader.FileType GetFileType()
        {
            if (IsPZ90(_wordTables[1]))
                return IWordReader.FileType.PZ90;

            if (IsSK42(_wordTables[1]))
                return IWordReader.FileType.SK42;

            return IWordReader.FileType.Unbind;
        }

        #endregion



    }

}

