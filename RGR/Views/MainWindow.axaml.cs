using Avalonia.Controls;
using Avalonia.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace RGR.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dGrid.CellEditEnding += OnCellEditEnd;
        }

        private void OnCellEditEnd(object sender, DataGridCellEditEndingEventArgs args)
        {
            if (args.EditAction != DataGridEditAction.Commit) return;
            DataGrid dataGrid = sender as DataGrid;
            string index = args.Column.Header as string;
            string a = (args.EditingElement as TextBox).Text;

            DataRowCollection rows = dataGrid.Items as DataRowCollection;

            rows[dataGrid.SelectedIndex].BeginEdit();
            rows[dataGrid.SelectedIndex][index] = a;
            rows[dataGrid.SelectedIndex].EndEdit();
        }
        public void OnSelect(object sender, SelectionChangedEventArgs args)
        {
            if (args.AddedItems.Count == 0) return;
            DataTable table = args.AddedItems[0] as DataTable;
            TabControl tab = sender as TabControl;
            if (table != null)
            {
                int i = 0;
                dGrid.Columns.Clear();
                dGrid.Items = table.Rows;
                foreach(DataColumn col in table.Columns)
                {
                    dGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = col.ColumnName,
                        Binding = new Binding($"ItemArray[{i}]"),
                        IsReadOnly = false
                    });
                    i++;
                }
            }
        }
    }
}
