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
        ///<summary>Use the SpatialReferenceDialog to change the coordinate system or spatial reference of the map.</summary>
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
    }
}
