﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace myDLL
{
    class Temp
    {
        #region"Add Shapefile Using OpenFileDialog"


        ///<summary>Add a shapefile to the ActiveView using the Windows.Forms.OpenFileDialog control.</summary>
        ///
        ///<param name="activeView">An IActiveView interface</param>
        /// 
        ///<remarks></remarks>
        public void AddShapefileUsingOpenFileDialog(ESRI.ArcGIS.Carto.IActiveView activeView)
        {
            //parameter check
            if (activeView == null)
            {
                return;
            }

            // Use the OpenFileDialog Class to choose which shapefile to load.
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "Shapefiles (*.shp)|*.shp";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.Multiselect = false;


            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // The user chose a particular shapefile.

                // The returned string will be the full path, filename and file-extension for the chosen shapefile. Example: "C:\test\cities.shp"
                string shapefileLocation = openFileDialog.FileName;

                if (shapefileLocation != "")
                {
                    ESRI.ArcGIS.Geodatabase.IWorkspaceFactory workspaceFactory = new ESRI.ArcGIS.DataSourcesFile.ShapefileWorkspaceFactoryClass();

                    // System.IO.Path.GetDirectoryName(shapefileLocation) returns the directory part of the string. Example: "C:\test\"
                    ESRI.ArcGIS.Geodatabase.IFeatureWorkspace featureWorkspace = (ESRI.ArcGIS.Geodatabase.IFeatureWorkspace)workspaceFactory.OpenFromFile(System.IO.Path.GetDirectoryName(shapefileLocation), 0); // Explicit Cast

                    // System.IO.Path.GetFileNameWithoutExtension(shapefileLocation) returns the base filename (without extension). Example: "cities"
                    ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = featureWorkspace.OpenFeatureClass(System.IO.Path.GetFileNameWithoutExtension(shapefileLocation));

                    ESRI.ArcGIS.Carto.IFeatureLayer featureLayer = new ESRI.ArcGIS.Carto.FeatureLayerClass();
                    featureLayer.FeatureClass = featureClass;
                    featureLayer.Name = featureClass.AliasName;
                    featureLayer.Visible = true;
                    activeView.FocusMap.AddLayer(featureLayer);

                    // Zoom the display to the full extent of all layers in the map
                    activeView.Extent = activeView.FullExtent;
                    activeView.PartialRefresh(ESRI.ArcGIS.Carto.esriViewDrawPhase.esriViewGeography, null, null);
                }
                else
                {
                    // The user did not choose a shapefile.
                    // Do whatever remedial actions as necessary
                    // System.Windows.Forms.MessageBox.Show("No shapefile chosen", "No Choice #1",
                    //                                     System.Windows.Forms.MessageBoxButtons.OK,
                    //                                     System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                // The user did not choose a shapefile. They clicked Cancel or closed the dialog by the "X" button.
                // Do whatever remedial actions as necessary.
                // System.Windows.Forms.MessageBox.Show("No shapefile chosen", "No Choice #2",
                //                                      System.Windows.Forms.MessageBoxButtons.OK,
                //                                      System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }
        #endregion

        #region   点选功能
        public ESRI.ArcGIS.Geodatabase.IFeatureCursor GetAllFeaturesFromPointSearchInGeoFeatureLayer(System.Double searchTolerance, ESRI.ArcGIS.Geometry.IPoint point, ESRI.ArcGIS.Carto.IGeoFeatureLayer geoFeatureLayer, ESRI.ArcGIS.Carto.IActiveView activeView)
        {

            if (searchTolerance < 0 || point == null || geoFeatureLayer == null || activeView == null)
            {
                return null;
            }
            ESRI.ArcGIS.Carto.IMap map = activeView.FocusMap;

            // Expand the points envelope to give better search results    
            ESRI.ArcGIS.Geometry.IEnvelope envelope = point.Envelope;
            envelope.Expand(searchTolerance, searchTolerance, false);   //dx,dy

            ESRI.ArcGIS.Geodatabase.IFeatureClass featureClass = geoFeatureLayer.FeatureClass;
            System.String shapeFieldName = featureClass.ShapeFieldName;

            // Create a new spatial filter and use the new envelope as the geometry    
            ESRI.ArcGIS.Geodatabase.ISpatialFilter spatialFilter = new ESRI.ArcGIS.Geodatabase.SpatialFilterClass();
            spatialFilter.Geometry = envelope;
            spatialFilter.SpatialRel = ESRI.ArcGIS.Geodatabase.esriSpatialRelEnum.esriSpatialRelEnvelopeIntersects;
            spatialFilter.set_OutputSpatialReference(shapeFieldName, map.SpatialReference);
            spatialFilter.GeometryField = shapeFieldName;

            // Do the search
            ESRI.ArcGIS.Geodatabase.IFeatureCursor featureCursor = featureClass.Search(spatialFilter, false);

            return featureCursor;
        }
        #endregion

    }
}
