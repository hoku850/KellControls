using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellControls.KellTable.Models;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.Odbc;
using System.Collections;

namespace KellControls.KellTable
{
    public static class Utility
    {
        /// <summary>
        /// 载入数据表到表格控件中
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dt"></param>
        public static void LoadDataTable(Table table, DataTable dt)
        {
            if (table.ColumnModel == null)
            {
                table.ColumnModel = new ColumnModel();
                ColumnModel columnModel = table.ColumnModel;
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.DataType.Name.ToLower())
                    {
                        case "string":
                            columnModel.Columns.Add(new TextColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "boolean":
                            columnModel.Columns.Add(new CheckBoxColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "datetime":
                            columnModel.Columns.Add(new DateTimeColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "image":
                            columnModel.Columns.Add(new ImageColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "int32":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "uint32":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "int16":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "uint16":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "byte":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "sbyte":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "int64":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "uint64":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "single":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "double":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "decimal":
                            columnModel.Columns.Add(new NumberColumn(dc.ColumnName, dc.DataType));
                            break;
                        case "color":
                            columnModel.Columns.Add(new ColorColumn(dc.ColumnName, dc.DataType));
                            break;
                        default:
                            //columnModel.Columns.Add(new ProgressBarColumn(dc.ColumnName, dc.DataType));
                            //columnModel.Columns.Add(new ButtonColumn(dc.ColumnName, dc.DataType));
                            //columnModel.Columns.Add(new ComboBoxColumn(dc.ColumnName, dc.DataType));
                            break;
                    }
                }
            }

            if (dt.Columns.Count != table.ColumnModel.Columns.Count)
                throw new Exception("DataTable的列数跟KellTable的列数不一致！");

            if (table.TableModel == null)
            {
                table.TableModel = new TableModel();
                TableModel tableModel = table.TableModel;
                foreach (DataRow dr in dt.Rows)
                {
                    List<Cell> cells = new List<Cell>();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        switch (dc.DataType.Name.ToLower())
                        {
                            case "string":
                                cells.Add(new Cell(dr[dc].ToString()));
                                break;
                            case "boolean":
                                cells.Add(new Cell(dr[dc].ToString(), Convert.ToBoolean(dr[dc])));
                                break;
                            case "datetime":
                                cells.Add(new Cell(dr[dc]));
                                break;
                            case "image":
                                cells.Add(new Cell("", (Image)dr[dc]));
                                break;
                            case "int32":
                            case "uint32":
                            case "int16":
                            case "uint16":
                            case "byte":
                            case "sbyte":
                            case "int64":
                            case "uint64":
                            case "single":
                            case "double":
                            case "decimal":
                                cells.Add(new Cell(dr[dc]));
                                break;
                            case "color":
                                cells.Add(new Cell("", (Color)dr[dc]));
                                break;
                        }
                    }
                    Row row = new Row(cells.ToArray());
                    tableModel.Rows.Add(row);
                }
            }
            else
            {
                TableModel tableModel = table.TableModel;
                if (dt.Rows.Count < tableModel.Rows.Count)
                {
                    for (int i = dt.Rows.Count; i < tableModel.Rows.Count; i++)
                        tableModel.Rows.RemoveAt(i);
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    if (j > tableModel.Rows.Count - 1)
                    {
                        List<Cell> cells = new List<Cell>();
                        foreach (DataColumn dc in dt.Columns)
                        {
                            switch (dc.DataType.Name.ToLower())
                            {
                                case "string":
                                    cells.Add(new Cell(dr[dc].ToString()));
                                    break;
                                case "boolean":
                                    cells.Add(new Cell(dr[dc].ToString(), Convert.ToBoolean(dr[dc])));
                                    break;
                                case "datetime":
                                    cells.Add(new Cell(dr[dc]));
                                    break;
                                case "image":
                                    cells.Add(new Cell("", (Image)dr[dc]));
                                    break;
                                case "int32":
                                case "uint32":
                                case "int16":
                                case "uint16":
                                case "byte":
                                case "sbyte":
                                case "int64":
                                case "uint64":
                                case "single":
                                case "double":
                                case "decimal":
                                    cells.Add(new Cell(dr[dc]));
                                    break;
                                case "color":
                                    cells.Add(new Cell("", (Color)dr[dc]));
                                    break;
                            }
                            Row row = new Row(cells.ToArray());
                            tableModel.Rows.Add(row);
                        }
                    }
                    else
                    {
                        Row row = tableModel.Rows[j];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            DataColumn dc = dt.Columns[i];
                            switch (dc.DataType.Name.ToLower())
                            {
                                case "string":
                                    row.Cells[i].Text = dr[dc].ToString();
                                    break;
                                case "boolean":
                                    row.Cells[i].Text = dr[dc].ToString();
                                    row.Cells[i].Checked = Convert.ToBoolean(dr[dc]);
                                    break;
                                case "datetime":
                                    row.Cells[i].Data = dr[dc];
                                    break;
                                case "image":
                                    row.Cells[i].Image = (Image)dr[dc];
                                    break;
                                case "int32":
                                case "uint32":
                                case "int16":
                                case "uint16":
                                case "byte":
                                case "sbyte":
                                case "int64":
                                case "uint64":
                                case "single":
                                case "double":
                                case "decimal":
                                    row.Cells[i].Data = dr[dc];
                                    break;
                                case "color":
                                    row.Cells[i].Data = dr[dc];
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 将表格控件转化为数据表
        /// </summary>
        /// <param name="table"></param>
        public static DataTable ConvertToDataTable(Table table)
        {
            if (table.ColumnModel == null)
                return new DataTable();

            DataTable dt = new DataTable();
            foreach (Column col in table.ColumnModel.Columns)
            {
                dt.Columns.Add(col.Text, col.DataType);
            }

            if (table.TableModel == null)
                return dt;

            foreach (Row row in table.TableModel.Rows)
            {
                DataRow dr = dt.NewRow();
                ArrayList array = new ArrayList();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    Cell c = row.Cells[i];
                    Column col = table.ColumnModel.Columns[i];
                    switch (col.DataType.Name.ToLower())
                    {
                        case "string":
                            array.Add(c.Text);
                            break;
                        case "boolean":
                            array.Add(c.Checked);
                            break;
                        case "datetime":
                            array.Add(c.Data);
                            break;
                        case "image":
                            array.Add(c.Image);
                            break;
                        case "int32":
                        case "uint32":
                        case "int16":
                        case "uint16":
                        case "byte":
                        case "sbyte":
                        case "int64":
                        case "uint64":
                        case "single":
                        case "double":
                        case "decimal":
                            array.Add(c.Data);
                            break;
                        case "color":
                            array.Add(c.Data);
                            break;
                    }
                }
                dr.ItemArray = array.ToArray();
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
