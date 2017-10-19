using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace SOAFramework.ORM.Mapping
{
    public class Condition
    {
        #region attribute
        private List<string> mLst_Where = new System.Collections.Generic.List<string>();

        private List<string> mLst_GroupBy = new System.Collections.Generic.List<string>();

        private List<string> mLst_OrderBy = new System.Collections.Generic.List<string>();

        private List<string> mLst_Max = new System.Collections.Generic.List<string>();

        private List<string> mLst_Min = new System.Collections.Generic.List<string>();

        private List<string> mLst_Distinct = new System.Collections.Generic.List<string>();

        private List<string> mLst_Count = new System.Collections.Generic.List<string>();
        #endregion

        #region property
        public List<string> Where
        {
            get { return mLst_Where; }
        }

        public List<string> GroupBy
        {
            get { return mLst_GroupBy; }
        }

        public List<string> OrderBy
        {
            get { return mLst_OrderBy; }
        }

        public List<string> Max
        {
            get { return mLst_Max; }
        }

        public List<string> Min
        {
            get { return mLst_Min; }
        }

        public List<string> Distinct
        {
            get { return mLst_Distinct; }
        }

        public List<string> Count
        {
            get { return mLst_Count; }
        }
        #endregion

        #region method
        public void AddWhere(string Where)
        {
            mLst_Where.Add(Where);
        }

        public void AddGroupBy(string Column)
        {
            AddToList(mLst_GroupBy, Column);
        }

        public void AddOrderBy(string Column)
        {
            AddToList(mLst_OrderBy, Column);
        }

        public void AddMax(string Column)
        {
            AddToList(mLst_Max, Column);
        }

        public void AddMin(string Column)
        {
            AddToList(mLst_Min, Column);
        }

        public void AddDistinct(string Column)
        {
            AddToList(mLst_Distinct, Column);
        }

        public void AddCount(string Column)
        {
            if (string.IsNullOrEmpty(Column))
            {
                Column = "*";
            }
            AddToList(mLst_Count, Column);
        }

        private void AddToList(List<string> StringList, string Value)
        {
            if (StringList.Exists(t => t == Value))
            {
                StringList.Add(Value);
            }
        }
        #endregion
    }
}
