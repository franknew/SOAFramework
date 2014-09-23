using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Model
{
    public interface IViewInterface<T> where T : new()
    {
        int GetMax(string ColumnName, string DBEx);

        int GetMin(string ColumnName, string DBEx);

        int GetCount(string DBEx);

        IViewInterface<T> Distinct(string ColumnName);

        T[] GetModels(int PageIndex, int PageSize, string IDColumnName, string DBEx);

        T[] GetModels(int PageIndex, int PageSize, string IDColumnName);

        T[] GetModels();

        T[] GetModels(string DBEx);
        
        DataTable ToDataTable();

        IViewInterface<T> Where(string Where, object[] Parameters, string ParameterType);

        IViewInterface<T> Where(string Where, SqlParameter[] Parameters);

        IViewInterface<T> Where(string Where, OleDbParameter[] Parameters);

        IViewInterface<T> Where(string Where);

        IViewInterface<T> Top(int TopN);

        IViewInterface<T> OrderBy(string ColumnName, string Order);

        IViewInterface<T> OrderBy(string ColumnName, Enum_Order Order);

        IViewInterface<T> Having(string Where, object[] Parameters, string ParameterType);

        IViewInterface<T> Having(string Where, SqlParameter[] Parameters);

        IViewInterface<T> Having(string Where, OleDbParameter[] Parameters);

        IViewInterface<T> Having(string Where);

        IViewInterface<T> GroupBy(string ColumnName);
    }
}
