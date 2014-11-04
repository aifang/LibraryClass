using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;

namespace myDLL
{
    /// <summary>
    /// IPoint,IPolyline,IPolygon之间相互转换，以及自身的创建
    /// </summary>
    public class CoordinateToGeometries
    {
        private static object _missing = Type.Missing;

        /// <summary>
        /// 创建Polygon
        /// </summary>
        /// <returns></returns>
        public static IPolygon GetPolygonGeometry()
        {
            IPoint[] centerPointArray =new IPoint[4];
            centerPointArray[0] = ConstructPoint2D(41435879.027947, 4460509.6717146039);
            centerPointArray[1] = ConstructPoint2D(41428047.5540349, 4453286.46762095);
            centerPointArray[2] = ConstructPoint2D(41428731.8575806, 4465071.6953527);
            centerPointArray[3] = ConstructPoint2D(41435879.027947, 4460509.6717146039);

            IGeometryCollection pGeometryColl = new PolygonClass();
            IPointCollection outerPointCollection = new RingClass();
            for (int i = 0; i < centerPointArray.Length; i++)
            {
                IPoint centerPoint = centerPointArray[i];
                outerPointCollection.AddPoint(centerPoint, ref _missing, ref _missing);
                
                //IPointCollection innerPointCollection = new RingClass();
            }
            pGeometryColl.AddGeometry(outerPointCollection as IGeometry, ref _missing, ref _missing);
            return pGeometryColl as IPolygon;
        }

        private static IPoint ConstructPoint2D(double x, double y)
        {
            IPoint point = new PointClass();
            point.PutCoords(x, y);
            return point;
        }
    }
}
