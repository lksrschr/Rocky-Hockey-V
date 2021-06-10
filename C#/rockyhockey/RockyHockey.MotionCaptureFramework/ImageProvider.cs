using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.Structure;
using AForge.Video.DirectShow;

namespace RockyHockey.MotionCaptureFramework
{
    public abstract class ImageProvider
    {
		public TimedImage lastCapture { get; protected set; }

        public Image<Bgr, Byte> puckdetectionPicture { get; set; }

        public bool IsReady { get; set; } = false;

        public Bitmap nextFrame { get; set; }
        public bool SliceImage { get; protected set; }
        public abstract TimedImage getTimedImage();
        public abstract void finalize();
        public abstract int getFPS();
    }

    public struct TimedImage
    {
        public Mat image;
        public long timeStamp;

        public Bitmap GetImage()
        {
            return image?.Bitmap;
        }
    }
}
