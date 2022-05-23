using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.Data;
using ReactiveUI;

namespace RGR.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private SQLiteConnection sql_con;
        private DataSet tables;

        public DataSet Tables
        {
            get => tables;
            private set => this.RaiseAndSetIfChanged(ref tables, value);
        }
        public MainWindowViewModel()
        {
            sql_con = new SQLiteConnection("Data Source=DataBase.db;Mode=ReadWrite");
            sql_con.Open();
            SQLiteCommand command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type='table' ORDER BY 1",sql_con);
            DataTable tablesNames = new DataTable(); 
            tablesNames.Load(command.ExecuteReader());
            tables = new DataSet();
            foreach(DataRow row in tablesNames.Rows)
            {
                string name = row.ItemArray[0].ToString();
                if (name == "sqlite_sequence") continue;
                SQLiteCommand sqlTab = new SQLiteCommand("SELECT * FROM "+name, sql_con);
                DataTable table = new DataTable();
                table.Load(sqlTab.ExecuteReader());
                tables.Tables.Add(table);
                DataRow t = table.Rows[0];
            }
        }
        
        public void OnClick()
        {
            
            foreach (DataTable table in tables.Tables) {
                
                SQLiteDataAdapter adapter = new SQLiteDataAdapter("select * from "+table.TableName, sql_con);
                adapter.UpdateCommand = new SQLiteCommandBuilder(adapter).GetUpdateCommand();
                adapter.Update(tables, table.TableName);
             }
        }
        ~MainWindowViewModel()
        {
            sql_con.Close();
        }
    }
}
