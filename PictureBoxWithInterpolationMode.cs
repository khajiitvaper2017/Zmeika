using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Zmeika
{
    public class PictureBoxWithInterpolationMode : PictureBox
    {
        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            base.OnPaint(paintEventArgs);
        }
    }
}