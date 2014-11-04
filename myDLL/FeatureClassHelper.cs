using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace myDLL
{
    public class FeatureClassHelper
    {
        /// <summary>
        /// 获取数据FeatueClass的workspaceFactory
        /// </summary>
        /// <param name="pFeatureClass"></param>
        /// <returns></returns>
        public static string getWorkspaceFactoryName(IFeatureClass pFeatureClass)
        {
            string str=((IDataset)pFeatureClass).Workspace.WorkspaceFactory.ToString();
            return str.Substring(str.LastIndexOf('.') + 1);
        }

        public static string getWorkspaceFactoryName(IDataset pDataset)
        {
            string str = pDataset.Workspace.WorkspaceFactory.ToString();
            return str.Substring(str.LastIndexOf('.') + 1);
        }

        ///<summary>Simple helper to create a featureclass in a geodatabase.</summary>
        /// 
        ///<param name="workspace">workspace与featureDataset其中只有一个能为空</param>
        ///<param name="featureDataset">IFeatureDataset或者null,优先在featureDataset中创建FeatureClass</param>
        ///<param name="featureClassName">用于打开已有的FeatureClass或者创建新的FeatureClass</param>
        ///<param name="fields">字段集合，可以为null</param>
        ///<param name="CLSID">A UID value or null. Example "esriGeoDatabase.Feature" or Nothing</param>
        ///<param name="CLSEXT">A UID value or null.</param>
        ///<param name="strConfigKeyword">""或者数据库表名(RDBMS table string for ArcSDE)</param>
        ///<param name="createType">是否覆盖已存在的FeatureClass</param>
        ///  
        ///<returns>An IFeatureClass interface or a Nothing</returns>
        ///  
        ///<remarks>
        ///  (1) 若featureClassName已存在则根据createType的值来判断是否覆盖.
        ///  (2) featureDataset is not null时，在featureDataset创建FeatureClass，其他在workspace中创建.
        ///  (3) featureClass继承featureDataset的空间参考.
        ///  (4) 字段为null时赋予默认字段.
        ///</remarks>
        public static ESRI.ArcGIS.Geodatabase.IFeatureClass CreateFeatureClass(ESRI.ArcGIS.Geodatabase.IWorkspace2 workspace, ESRI.ArcGIS.Geodatabase.IFeatureDataset featureDataset, System.String featureClassName, ESRI.ArcGIS.Geodatabase.IFields fields, ESRI.ArcGIS.esriSystem.UID CLSID, ESRI.ArcGIS.esriSystem.UID CLSEXT, System.String strConfigKeyword, bool createType, ESRI.ArcGIS.Geometry.esriGeometryType geometryType)
        {
            if (featureClassName == "") return null; 
            if (workspace == null && featureDataset == null) return null;//检查必须项

            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass;
            ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)workspace; // Explicit Cast

            if (workspace.get_NameExists(ESRI.ArcGIS.Geodatabase.esriDatasetType.esriDTFeatureClass, featureClassName)) //feature class with that name already exists 
            {
               
                featureClass = featureWorkspace.OpenFeatureClass(featureClassName);
                if (createType) //判断是否覆盖创建
                {
                    IDataset fDataset = (IDataset)featureClass;
                    if (fDataset.CanDelete()) fDataset.Delete();//可删除则删除
                }
                else return featureClass;
            }

            // assign the class id value if not assigned
            if (CLSID == null)
            {
                CLSID = new ESRI.ArcGIS.esriSystem.UIDClass();
                CLSID.Value = "esriGeoDatabase.Feature";
            }

            ESRI.ArcGIS.Geodatabase.IObjectClassDescription objectClassDescription = new ESRI.ArcGIS.Geodatabase.FeatureClassDescriptionClass();
            IFeatureClassDescription fcDescription = new FeatureClassDescriptionClass();

            // 当字段不存在时，补充必要字段
            if (fields == null)
            {
                // 获取必须字段
                fields = objectClassDescription.RequiredFields;
                //获取ShapeField字段
                int shapeFieldIndex = fields.FindField(fcDescription.ShapeFieldName);
                IField shapeField = fields.get_Field(shapeFieldIndex);
                ESRI.ArcGIS.Geodatabase.IFieldsEdit fieldsEdit = (ESRI.ArcGIS.Geodatabase.IFieldsEdit)fields; // 显示转换 
                ESRI.ArcGIS.Geodatabase.IFieldEdit fieldEdit = (ESRI.ArcGIS.Geodatabase.IFieldEdit)shapeField; // 显示转换 
                // Modify the GeometryDef object before using the fields collection to create a // feature class.
                IGeometryDef geometryDef = shapeField.GeometryDef;
                IGeometryDefEdit geometryDefEdit = (IGeometryDefEdit)geometryDef;

                // 设置featureClass的图形类型,默认是polygon.
                if (geometryType ==ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryNull)
                    geometryDefEdit.GeometryType_2 = ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon;
                else
                    geometryDefEdit.GeometryType_2 = geometryType;
                geometryDefEdit.HasM_2 = false;
                geometryDefEdit.HasZ_2 = false;
                geometryDefEdit.GridCount_2 = 1;

                // Set the first grid size to zero and allow ArcGIS to determine a valid grid size.
                geometryDefEdit.set_GridSize(0, 0);
                geometryDefEdit.SpatialReference_2 = SpatialReferenceHelper.createSpatialReference(false,2356); //((ESRI.ArcGIS.Geodatabase.IField)(((ESRI.ArcGIS.Geodatabase.IFeatureClass)(pLayers[2].FeatureClass)).Fields.get_Field(1))).GeometryDef

                #region  编辑字段
                //ESRI.ArcGIS.Geodatabase.IFieldsEdit fieldsEdit = (ESRI.ArcGIS.Geodatabase.IFieldsEdit)fields; // 显示转换                
                
                //ESRI.ArcGIS.Geodatabase.IField field = new ESRI.ArcGIS.Geodatabase.FieldClass();

                ////增加一个用户自定义的string类型字段
                //ESRI.ArcGIS.Geodatabase.IFieldEdit fieldEdit = (ESRI.ArcGIS.Geodatabase.IFieldEdit)field; // 显示转换

                ////设置字段属性
                //fieldEdit.Name_2 = "SampleField";
                //fieldEdit.Type_2 = ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeString;
                //fieldEdit.IsNullable_2 = true;
                //fieldEdit.AliasName_2 = "Sample Field Column";
                //fieldEdit.DefaultValue_2 = "test";
                //fieldEdit.Editable_2 = true;
                //fieldEdit.Length_2 = 100;

                //// add field to field collection
                //fieldsEdit.AddField(field);
                //fields = (ESRI.ArcGIS.Geodatabase.IFields)fieldsEdit; // 显示转换
                #endregion
            }

            System.String strShapeField = "";

            // 查找Shape字段，获取字段名
            for (int j = 0; j < fields.FieldCount; j++)
            {
                if (fields.get_Field(j).Type == ESRI.ArcGIS.Geodatabase.esriFieldType.esriFieldTypeGeometry)
                {
                    strShapeField = fields.get_Field(j).Name;
                }
            }

            //使用IFieldChecker创建一个验证字段的集合
            ESRI.ArcGIS.Geodatabase.IFieldChecker fieldChecker = new ESRI.ArcGIS.Geodatabase.FieldCheckerClass();
            ESRI.ArcGIS.Geodatabase.IEnumFieldError enumFieldError = null;
            ESRI.ArcGIS.Geodatabase.IFields validatedFields = null;
            fieldChecker.ValidateWorkspace = (ESRI.ArcGIS.Geodatabase.IWorkspace)workspace;
            fieldChecker.Validate(fields, out enumFieldError, out validatedFields);

            // 可在这个位置查看字段错误位置
            // which fields were modified during validation.

            // 创建用户featureClass
            if (featureDataset == null)// 如果featureDataset不存在则建立在Workspace级别中
            {
                featureClass = featureWorkspace.CreateFeatureClass(featureClassName, validatedFields, CLSID, CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, strShapeField, strConfigKeyword);
            }
            else
            {
                featureClass = featureDataset.CreateFeatureClass(featureClassName, validatedFields, CLSID, CLSEXT, ESRI.ArcGIS.Geodatabase.esriFeatureType.esriFTSimple, strShapeField, strConfigKeyword);
            }
            return featureClass;
        }
    }
}
