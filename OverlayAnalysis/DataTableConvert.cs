using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace OverlayAnalysis
{
    class DataTableConvert
    {
       
        public static DataTable CreateDataTable(ITable ArcTable)
        {
            DataTable pDataTable = new DataTable(ArcTable.ToString()); //创建一个dataTable表
            IField pField = null;
            DataColumn pDataColumn;

            for (int i = 0; i < ArcTable.Fields.FieldCount; i++)  //根据每个字段的属性建立DataColumn对象
            {
                pField = ArcTable.Fields.get_Field(i);
                //新建一个DataColumn并设置其属性
                pDataColumn = new DataColumn(pField.AliasName);
                if (pField.Name == ArcTable.OIDFieldName)
                {
                    pDataColumn.Unique = true;           //字段值是否唯一
                }
                pDataColumn.AllowDBNull = pField.IsNullable;
                pDataColumn.Caption = pField.AliasName;
                pDataColumn.DataType = System.Type.GetType(ParseFieldType(pField.Type));
                pDataColumn.DefaultValue = pField.DefaultValue;
                if (pField.VarType == 8)
                {
                    pDataColumn.MaxLength = pField.Length;
                }
                if (pDataTable.Columns.Contains(pDataColumn.ColumnName))
                {
                    pDataColumn.ColumnName = pDataColumn.ColumnName + "1";
                }
                pDataTable.Columns.Add(pDataColumn);
                pField = null;
                pDataColumn = null;
            }
            //填充DataTable数据
            DataRow pDataRow = null;
            ICursor pCursor = ArcTable.Search(null, false);
            IRow pRow = pCursor.NextRow();
            string shapeType = getShapeType(ArcTable);
            int n = 0;
            while (pRow != null)
            {
                pDataRow = pDataTable.NewRow();
                for (int i = 0; i < pRow.Fields.FieldCount; i++)
                {
                    if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeGeometry)
                    {
                        pDataRow[i] = shapeType;
                    }
                    else if (pRow.Fields.get_Field(i).Type == esriFieldType.esriFieldTypeBlob)//标签
                    {
                        pDataRow[i] = "Element";
                    }
                    else pDataRow[i] = pRow.get_Value(i);
                }
                pDataTable.Rows.Add(pDataRow);
                pDataRow = null;
                n++;
                //if (n == 500)  ////为保证效率，一次只装载最多500条记录
                //{
                //    pRow = null;
                //}
                //else
                //{
                pRow = pCursor.NextRow();
                //}
            }
            return pDataTable;
        }

        //数据类型转换
        public static string ParseFieldType(esriFieldType fieldType)
        {
            switch (fieldType)
            {
                case esriFieldType.esriFieldTypeBlob:
                    return "System.String";
                case esriFieldType.esriFieldTypeDate:
                    return "System.DateTime";
                case esriFieldType.esriFieldTypeDouble:
                    return "System.Double";
                case esriFieldType.esriFieldTypeGeometry:
                    return "System.String";
                case esriFieldType.esriFieldTypeGlobalID:
                    return "System.String";
                case esriFieldType.esriFieldTypeGUID:
                    return "System.String";
                case esriFieldType.esriFieldTypeInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeOID:
                    return "System.String";
                case esriFieldType.esriFieldTypeRaster:
                    return "System.String";
                case esriFieldType.esriFieldTypeSingle:
                    return "System.Single";
                case esriFieldType.esriFieldTypeSmallInteger:
                    return "System.Int32";
                case esriFieldType.esriFieldTypeString:
                    return "System.String";
                default:
                    return "System.String";
            }
        }
        //获取表类型
        public static string getShapeType(ITable table)
        {
            IFeatureClass featureClass = (IFeatureClass)table;
            switch (featureClass.ShapeType)
            {
                case esriGeometryType.esriGeometryPoint:
                    return "点";
                case esriGeometryType.esriGeometryPolyline:
                    return "线";
                case esriGeometryType.esriGeometryPolygon:
                    return "面";
                default:
                    return "";
            }
        }
    }
    
}
