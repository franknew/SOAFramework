using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Reflection;
using SOAFramework.ORM.Mapping;

namespace SOAFramework.ORM.Common
{
    internal class Reflection
    {
        /// <summary>
        /// ����������,��һ��Ϊ�Ƿ��������ڶ���Ϊ�ֶ����ͣ�������Ϊ�ֶγ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns>��һ��Ϊ�Ƿ��������ڶ���Ϊ�ֶ����ͣ�������Ϊ�ֶγ���</returns>
        public static DataTable GetClassDef<T>(T t)
        {
            DataTable dtClassDef = new DataTable();
            if (null != t)
            {
                PropertyInfo[] piProperties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                dtClassDef.TableName = t.GetType().Name;
                if (piProperties != null && piProperties.Length > 0)
                {
                    //����������
                    for (int i = 0; i < piProperties.Length; i++)
                    {
                        if (piProperties[i].Name != null)
                        {
                            dtClassDef.Columns.Add(piProperties[i].Name, typeof(string));
                        }
                    }
                    //�������Զ�������
                    //�������
                    DataRow drPrimaryTemp = dtClassDef.NewRow();
                    //�����������
                    DataRow drTypeTemp = dtClassDef.NewRow();
                    //��ó���
                    DataRow drLenghtTemp = dtClassDef.NewRow();
                    //�Ƿ�������
                    DataRow drIsIdentiry = dtClassDef.NewRow();
                    foreach (PropertyInfo piProperty in piProperties)
                    {
                        ColumnMapping objAttribute = new ColumnMapping();
                        object[] objAtrributes = piProperty.GetCustomAttributes(typeof(ColumnMapping), false);
                        drPrimaryTemp[piProperty.Name] = "0";
                        drTypeTemp[piProperty.Name] = "";
                        drLenghtTemp[piProperty.Name] = "-1";
                        if (objAtrributes != null && objAtrributes.Length > 0)
                        {
                            objAttribute = (ColumnMapping)objAtrributes[0];
                        }

                        if (objAttribute.IsPrimaryKey)
                        {
                            drPrimaryTemp[piProperty.Name] = "1";
                        }
                        else
                        {
                            drPrimaryTemp[piProperty.Name] = "0";
                        }
                        if (objAttribute.AttrType != null)
                        {
                            drTypeTemp[piProperty.Name] = objAttribute.AttrType.Name;
                        }
                        drLenghtTemp[piProperty.Name] = objAttribute.Lenght.ToString();
                        drIsIdentiry[piProperty.Name] = objAttribute.IsAutoIncrease;
                    }
                    dtClassDef.Rows.Add(drPrimaryTemp);
                    dtClassDef.Rows.Add(drTypeTemp);
                    dtClassDef.Rows.Add(drLenghtTemp);
                    dtClassDef.Rows.Add(drIsIdentiry);
                }
            }
            return dtClassDef;
        }

        public static void GetColumnsDef<T>(Dictionary<string, List<ColumnMapping>> Dic, T t, string TableName)
        {
            if (Dic.ContainsKey(TableName))
            {
                return;
            }
            if (null == t)
            {
                return;
            }
            Type tTemp = t.GetType();
            PropertyInfo[] piProperties = tTemp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (null == piProperties || 0 == piProperties.Length)
            {
                return;
            }
            List<ColumnMapping> lstProperty = new List<ColumnMapping>();
            foreach (PropertyInfo piTemp in piProperties)
            {
                ColumnMapping padProperty = new ColumnMapping();
                object[] objAttribute = piTemp.GetCustomAttributes(typeof(ColumnMapping), true);
                if (null != objAttribute && 0 < objAttribute.Length)
                {
                    padProperty = objAttribute[0] as ColumnMapping;
                }
                lstProperty.Add(padProperty);
            }
            Dic.Add(TableName, lstProperty);
        }

        /// <summary>
        /// ��ö����ֵ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="ClassDef">������Խṹ</param>
        /// <returns></returns>
        public static DataTable GetClassValues<T>(T t, DataTable ClassDef)
        {
            DataTable dtValues = null;
            if (t != null && ClassDef != null)
            {
                PropertyInfo[] piProperties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                dtValues = ClassDef.Clone(); 
                //������ֵ
                DataRow drValueTemp = dtValues.NewRow();
                bool blHasValue = false;
                for (int i = 0; i < piProperties.Length; i++)
                {
                    object objValue = piProperties[i].GetValue(t, null);
                    if (null != objValue && null != drValueTemp[piProperties[i].Name])
                    {
                        drValueTemp[piProperties[i].Name] = objValue.ToString();
                        blHasValue = true;
                    }
                }
                if (blHasValue)
                {
                    dtValues.Rows.Add(drValueTemp);
                }
            }
            return dtValues;
        }

        public static object GetProperty<T>(T t, string Name)
        {
            object objValue = null;
            if (t != null)
            {
                PropertyInfo piProperty = t.GetType().GetProperty(Name);
                if (piProperty != null)
                {
                    objValue = piProperty.GetValue(t, null);
                }
            }
            return objValue;
        }

        public static object GetField<T>(T t, string Name)
        {
            object objValue = null;
            if (t != null)
            {
                FieldInfo fiProperty = t.GetType().GetField(Name);
                if (fiProperty != null)
                {
                    objValue = fiProperty.GetValue(t);
                }
            }
            return objValue;
        }

        public static void SetProperty<T>(T t, string Name, object Value)
        {
            if (t != null && !string.IsNullOrEmpty(Name))
            {
                PropertyInfo piProperty = t.GetType().GetProperty(Name);
                piProperty.SetValue(t, Value, null);
            }
        }

        public static void SetField<T>(T t, string Name, object Value)
        {
            if (t != null && !string.IsNullOrEmpty(Name))
            {
                FieldInfo fiProperty = t.GetType().GetField(Name);
                fiProperty.SetValue(t, Value);
            }
        }
    }
}
