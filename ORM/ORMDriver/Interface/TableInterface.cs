using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;


namespace Model
{
    public interface ITableInterface<T> where T : new()
    {
        int GetMax(string ColumnName, string DBEx);

        int GetMin(string ColumnName, string DBEx);

        int GetCount(string DBEx);

        ITableInterface<T> Distinct(string ColumnName);

        T[] GetModels(int PageIndex, int PageSize, string IDColumnName, string DBEx);

        T[] GetModels(int PageIndex, int PageSize, string IDColumnName);

        T[] GetModels();

        T[] GetModels(string DBEx);

        DataTable ToDataTable();

        ITableInterface<T> Where(string Where, object[] Parameters, string ParameterType);

        ITableInterface<T> Where(string Where, SqlParameter[] Parameters);

        ITableInterface<T> Where(string Where, OleDbParameter[] Parameters);

        ITableInterface<T> Where(string Where);

        ITableInterface<T> Top(int TopN);

        ITableInterface<T> OrderBy(string ColumnName, string Order);

        ITableInterface<T> OrderBy(string ColumnName, Enum_Order Order);

        ITableInterface<T> Having(string Where, object[] Parameters, string ParameterType);

        ITableInterface<T> Having(string Where, SqlParameter[] Parameters);

        ITableInterface<T> Having(string Where, OleDbParameter[] Parameters);

        ITableInterface<T> Having(string Where);

        ITableInterface<T> GroupBy(string ColumnName);

        string Save();

        bool Update(string Where);

        bool Update();

        bool Delete();
    }
}
