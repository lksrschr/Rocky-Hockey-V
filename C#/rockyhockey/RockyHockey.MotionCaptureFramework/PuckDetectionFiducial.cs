using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Text;
using Emgu.CV.Util;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Aruco;

namespace RockyHockey.MotionCaptureFramework
{
    public class PuckDetectionFiducial
    {
        ImageProvider imageProvider;


        public PuckDetectionFiducial(ImageProvider imageProvider)
        {
            this.imageProvider = imageProvider;
        }
        public System.Drawing.Point GetPuckPosition()
        {
            Image<Bgr, byte> emguImage = imageProvider.puckdetectionPicture; //transform bitmap to byte image because emgu cv needs a byte image
            System.Drawing.Point returnPoint = new System.Drawing.Point();


            Dictionary.PredefinedDictionaryName name = new Dictionary.PredefinedDictionaryName(); //initialize predefinde Dictionary
            Dictionary Dict = new Dictionary(name);
            VectorOfVectorOfPointF Corners = new VectorOfVectorOfPointF();
            VectorOfInt Ids = new VectorOfInt();
            DetectorParameters Parameters = DetectorParameters.GetDefault();

            VectorOfVectorOfPointF Rejected = new VectorOfVectorOfPointF();
            ArucoInvoke.DetectMarkers(emguImage, Dict, Corners, Ids, Parameters, Rejected);

            MCvScalar test = new MCvScalar(255, 0, 0);
            ArucoInvoke.DrawDetectedMarkers(emguImage, Corners, Ids, test); //draws deteced Aruco Marker on the image


            PointF[][] cornersOfDetectedMarker = Corners.ToArrayOfArray();

            
            if (cornersOfDetectedMarker != null)
            {
                returnPoint.X = (int)((cornersOfDetectedMarker[0][0].X + cornersOfDetectedMarker[0][2].X) / 2);
                returnPoint.Y = (int)((cornersOfDetectedMarker[0][0].Y + cornersOfDetectedMarker[0][2].Y) / 2);
                return returnPoint;
            }
            else return new System.Drawing.Point(0,0);
        }
    }

   
}
