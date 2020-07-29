using System.Drawing;
using System.IO;

namespace Breaker.Helpers.Extensions
{
    public static class HelperExtensions
    {
        //Convert byte[] array to Image:
        public static Image ToImage(this byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
