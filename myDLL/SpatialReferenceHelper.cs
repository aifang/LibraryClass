using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.CatalogUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Framework;

namespace myDLL
{
    public class SpatialReferenceHelper
    {
        ///<summary>更改地图的空间参考</summary>
        ///
        ///<param name="hWnd">The application window handle.0</param>
        ///<param name="map">An IMap interface.</param>
        /// 
        ///<remarks></remarks>
        public static void ChangeMapSpatialReference(System.Int32 hWnd, IMap map)
        {
            if (map == null)
            {
                return;
            }

            ISpatialReferenceDialog2 spatialReferenceDialog = new SpatialReferenceDialogClass();
            ISpatialReference spatialReference = spatialReferenceDialog.DoModalCreate(true, false, false, hWnd);
            if ((!(map.SpatialReferenceLocked)))
            {
                map.SpatialReference = spatialReference;
            }
        }

        #region
        
        /// <summary>
        /// 创建空间参考
        /// </summary>
        /// <param name="coorditateType">地理坐标系为true，投影坐标系为false</param>
        /// <param name="spatialRefEnum">WKID：4326 GCS_WGS_1984为地理坐标系，21478 Beijing1954GK_18N(北京54)和2365 Xian1980_3_Degree_GK_Zone_41(西安80)</param>
        /// <returns></returns>
        public static ESRI.ArcGIS.Geometry.ISpatialReference createSpatialReference(System.Boolean coorditateType,System.Int32 spatialRefEnum)
        {
            //创建SpatialReferenceEnvironmentClass对象
            ISpatialReferenceFactory3 pSpaRefFactory = new SpatialReferenceEnvironmentClass();
            if(coorditateType)
            {
                //创建地理坐标系对象
                IGeographicCoordinateSystem pNewGeoSys = pSpaRefFactory.CreateGeographicCoordinateSystem(spatialRefEnum);
                return pNewGeoSys;
            }
            else
            {
                //创建投影坐标系
                IProjectedCoordinateSystem pNewProjsys = pSpaRefFactory.CreateProjectedCoordinateSystem(spatialRefEnum);
                return pNewProjsys;
            }
        }
        #endregion
    }
}
