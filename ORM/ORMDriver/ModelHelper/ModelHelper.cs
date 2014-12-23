using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

using Frank.Common.DAL;
using SOAFramework.ORM.Mapping;
using SOAFramework.ORM.Common;
using SOAFramework.Library;

namespace SOAFramework.ORM.ORMDriver
{
    public partial class TableModelHelper<T> where T : new()
    {
        private DataTable _dt_Condition = null;//保存查询条件
        private DataTable _dt_ClassDef = null;//保存类结构
        private TableMapping _cad_TableDef = null;

        public TableModelHelper()
        {
            _dt_ClassDef = Reflection.GetClassDef<T>(new T());
            _dt_Condition = InitConditionTable();
        }

        public TableModelHelper(DataTable ClassDef, TableMapping TableDef)
        {
            _dt_Condition = InitConditionTable();
            _dt_ClassDef = ClassDef;
            _cad_TableDef = TableDef;
        }
        /// <summary>
        /// 初始化查询条件datatable，保存查询条件
        /// </summary>
        /// <returns></returns>
        protected static DataTable InitConditionTable()
        {
            DataTable dtCondition = new DataTable();
            dtCondition.Columns.Add("Condition", typeof(string));
            dtCondition.Columns.Add("Value", typeof(string));
            dtCondition.Columns.Add("Parameters", typeof(object));
            return dtCondition;
        }

        /// <summary>
        /// 产生解析后的SQL
        /// </summary>
        /// <param name="DBEx">数据库前缀</param>
        /// <param name="SelectType">查询类型:普通的查询所有列，总数，某个字段汇总，某个字段最大值，某个字段最小值</param>
        /// <param name="ColumnName">字段名</param>
        /// <returns></returns>
        protected string GetSQL(string DBEx, string SelectType, string ColumnName, out Parameter[] Parameters)
        {
            List<Parameter> lstParameters = new List<Parameter>();
            StringBuilder sbSQL = new StringBuilder();
            DataRow[] drGroupBy = _dt_Condition.Copy().Select("Condition='groupby'");
            sbSQL.Append(" SELECT ");
            //判断查询的类型，然后决定查询哪些字段
            switch (SelectType)
            {
                case "getcount"://总数
                    sbSQL.Append(" COUNT(*) ");
                    break;
                case "getsum"://汇总
                    sbSQL.Append(" SUM([" + ColumnName + "]) ");
                    break;
                case "getmax"://最大值
                    sbSQL.Append(" MAX([" + ColumnName + "]) ");
                    break;
                case "getmin"://最小值
                    sbSQL.Append(" MIN([" + ColumnName + "]) ");
                    break;
                default://获得model
                    #region TOP
                    DataRow[] drTop = _dt_Condition.Copy().Select("Condition='top'");
                    if (drTop != null && drTop.Length > 0)
                    {
                        sbSQL.Append(" TOP " + drTop[0]["Value"].ToString() + " ");
                    }
                    #endregion
                    #region Group By -- 查询group by的字段
                    if (drGroupBy != null && drGroupBy.Length > 0)
                    {
                        for (int i = 0; i < drGroupBy.Length; i++)
                        {
                            if (drGroupBy[i]["Value"].ToString().Split(new char[] { ',' }).Length > 1)
                            {
                                if (i == 0)
                                {
                                    sbSQL.Append(drGroupBy[i]["Value"].ToString());
                                }
                                else
                                {
                                    sbSQL.Append("," + drGroupBy[i]["Value"].ToString());
                                }
                            }
                            else
                            {
                                if (i == 0)
                                {
                                    sbSQL.Append("[" + drGroupBy[i]["Value"].ToString() + "]");
                                }
                                else
                                {
                                    sbSQL.Append(",[" + drGroupBy[i]["Value"].ToString() + "]");
                                }
                            }
                        }
                    }
                    #endregion
                    #region 非group by
                    else
                    {
                        #region Distinct
                        DataRow[] drDistinct = _dt_Condition.Copy().Select("Condition='distinct'");
                        if (drDistinct != null && drDistinct.Length > 0)
                        {
                            sbSQL.Append(" DISTINCT ");
                            for (int i = 0; i < drDistinct.Length; i++)
                            {
                                if (i == 0)
                                {
                                    sbSQL.Append("[" + drDistinct[i]["Value"].ToString() + "]");
                                }
                                else
                                {
                                    sbSQL.Append(",[" + drDistinct[i]["Value"].ToString() + "]");
                                }
                            }
                        }
                        #endregion
                        #region 没有任何字段查询限制条件,查询所有字段
                        else
                        {
                            for (int i = 0; i < _dt_ClassDef.Columns.Count; i++)
                            {
                                if (i == 0)
                                {
                                    sbSQL.Append("[" + _dt_ClassDef.Columns[i].ColumnName + "]");
                                }
                                else
                                {
                                    sbSQL.Append(",[" + _dt_ClassDef.Columns[i].ColumnName + "]");
                                }
                            }
                        }
                        #endregion
                    }
                #endregion
                break;
            }
            sbSQL.Append(" FROM ");
            sbSQL.Append(_cad_TableDef.GetFullTableName(DBEx));
            #region Where
            DataRow[] drWhere = _dt_Condition.Copy().Select("Condition='where'");
            if (drWhere != null && drWhere.Length > 0)
            {
                sbSQL.Append(" WHERE ");
                for (int i = 0; i < drWhere.Length; i++)
                {
                    Parameter[] objParameters = drWhere[i]["Parameters"] as Parameter[];
                    foreach (Parameter Parameter in objParameters)
                    {
                        lstParameters.Add(Parameter);
                    }
                    if (i == 0)
                    {
                        sbSQL.Append(" (" + drWhere[i]["Value"].ToString() + ") ");
                    }
                    else
                    {
                        sbSQL.Append(" OR (" + drWhere[i]["Value"].ToString() + ") ");
                    }
                }
            }
            #endregion
            #region Group By -- 分组条件
            if (drGroupBy != null && drGroupBy.Length > 0)
            {
                sbSQL.Append(" GROUP BY ");
                for (int i = 0; i < drGroupBy.Length; i++)
                {
                    if (drGroupBy[i]["Value"].ToString().Split(new char[] { ',' }).Length > 1)
                    {
                        if (i == 0)
                        {
                            sbSQL.Append(drGroupBy[i]["Value"].ToString());
                        }
                        else
                        {
                            sbSQL.Append("," + drGroupBy[i]["Value"].ToString());
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            sbSQL.Append("[" + drGroupBy[i]["Value"].ToString() + "]");
                        }
                        else
                        {
                            sbSQL.Append(",[" + drGroupBy[i]["Value"].ToString() + "]");
                        }
                    }
                }
            }
            #region Having -- 分组后条件
            DataRow[] drHaving = _dt_Condition.Copy().Select("Condition='having'");
            if (drHaving != null && drHaving.Length > 0)
            {
                sbSQL.Append(" HAVING ");
                for (int i = 0; i < drHaving.Length; i++)
                {
                    Parameter[] objParameters = drHaving[i]["Parameters"] as Parameter[];
                    foreach (Parameter Parameter in objParameters)
                    {
                        lstParameters.Add(Parameter);
                    }
                    if (i == 0)
                    {
                        sbSQL.Append(" (" + drHaving[i]["Value"].ToString() + ") ");
                    }
                    else
                    {
                        sbSQL.Append(" OR (" + drHaving[i]["Value"].ToString() + ") ");
                    }
                }
            }
            #endregion 
            #endregion
            #region Order By
            DataRow[] drOrderBy = _dt_Condition.Copy().Select("Condition='orderby'");
            if (drOrderBy != null && drOrderBy.Length > 0)
            {
                sbSQL.Append(" ORDER BY ");
                for (int i = 0; i < drOrderBy.Length; i++)
                {
                    if (i == 0)
                    {
                        sbSQL.Append(drOrderBy[i]["Value"].ToString());
                    }
                    else
                    {
                        sbSQL.Append("," + drOrderBy[i]["Value"].ToString());
                    }
                }
            }
            #endregion
            Parameters = lstParameters.ToArray();
            return sbSQL.ToString();
        }

        internal static List<T> AssignData(DataTable dtModels)
        {
            DataTable dtClassDef = Reflection.GetClassDef(new T());
            List<T> lstReturn = null;
            if (dtModels != null && dtModels.Rows.Count > 0)
            {
                lstReturn = new List<T>();
                foreach (DataRow row in dtModels.Rows)
                {
                    T objTemp = new T();
                    row.CopyToObject(objTemp);
                    lstReturn.Add(objTemp);
                }
            }
            return lstReturn;
        }
    }
}
