using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Project.Helpers
{
    public class DatatableParser
    {
        public DatatableParser()
        {
            ColumnMappings = new Dictionary<string, string>();
            ValueMappings = new Dictionary<string, Dictionary<string, string>>();
        }

        Dictionary<string, string> ColumnMappings { get; set; }
        Dictionary<string, Dictionary<string, string>> ValueMappings { get; set; }

        public void AddMapping(string SheetColumn, string TargetColumn)
        {
            ColumnMappings.Add(SheetColumn, TargetColumn);
        }

        public string AddValueMapping(string columnName, string inputValue, string targetValue)
        {
            if (ValueMappings.ContainsKey(columnName))
            {
                var targetMapping = ValueMappings[columnName];
                targetMapping.Add(inputValue, targetValue);
            }
            else
            {
                ValueMappings.Add(columnName, new Dictionary<string, string>() { { inputValue, targetValue } });
            }
            return inputValue;
        }

        public List<TKey> Parse<TKey>(DataTable table)
            where TKey : new()
        {
            List<TKey> result = new List<TKey>();
            var properties = typeof(TKey).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (table != null)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var item = new TKey();
                    var row = table.Rows[i];
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        var column = table.Columns[j];
                        var columnName = column.ColumnName;
                        if (ColumnMappings.ContainsKey(columnName))
                        {
                            var targetColumn = ColumnMappings[columnName];
                            var targetProperty = properties.Where(a => a.Name == targetColumn).FirstOrDefault();
                            var cellValue = row[column].ToString();
                            targetProperty.SetValue(item, GetValue(columnName, cellValue));
                        }
                        else
                        {
                            var targetProperty = properties.Where(a => a.Name == columnName).FirstOrDefault();
                            if (targetProperty != null)
                            {
                                var cellValue = row[column].ToString();
                                targetProperty.SetValue(item, GetValue(columnName, cellValue));
                            }
                        }
                    }
                    result.Add(item);
                }
            }
            return result;
        }

        string GetValue(string columnName, string inputValue)
        {
            if (ValueMappings.ContainsKey(columnName))
            {
                var targetMapping = ValueMappings[columnName];
                if (targetMapping.ContainsKey(inputValue))
                {
                    return targetMapping[inputValue];
                }
            }
            return inputValue;
        }
    }
}
