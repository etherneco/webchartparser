using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebChartParse.Models;

namespace WebChartParse.Controllers
{
    public class ChartController : Controller
    {
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }
        // GET: Chart
        public ActionResult Draw(string id)
        {

            if(id != null)
            if (!id.Equals(""))
            {
               byte[] data = Convert.FromBase64String(id);
               id = ASCIIEncoding.ASCII.GetString(data);
            }

            int width = 1920;
            int height = 1080;

            Image image = new Bitmap(width, height);

            int xMax = 10;
            int yMax = 10;
            int xMin = -10;
            int yMin = -10;
            int Xcounter = 1;
            int Ycounter = 1;


            int rangeChartX = xMax - xMin;
            int centerX = rangeChartX / 2;
            float scaleX = width / rangeChartX;

            int rangeChartY = yMax - yMin;
            int centerY = rangeChartY / 2;
            float scaleY = height / rangeChartY;


            double xStep = 0.1;

            Pen pen;

            using (Graphics g = Graphics.FromImage(image))
            {
                for (int i = xMin; i <= xMax+Xcounter; i += Xcounter)
                {
                    if (i == 0)
                        pen = new Pen(Color.Black, 5);
                    else
                        pen = new Pen(Color.Gray, 1);
                    float xRes = (i + centerX) * scaleX;
                    g.DrawLine(pen, xRes, 0, xRes, height);

                }

                for (int i = yMin; i <= yMax+Ycounter; i += Ycounter)
                {
                    if (i == 0)
                        pen = new Pen(Color.Black, 5);
                    else
                        pen = new Pen(Color.Gray, 1);
                    float yRes = (i + centerY) * scaleY;
                    g.DrawLine(pen, 0, yRes, width, yRes);

                }

                if (id != null)
                {
                    g.DrawString(id, new Font(FontFamily.GenericSansSerif, 16, FontStyle.Bold), new SolidBrush(Color.Green), 5, 5);


                    id = id.ToLower();
          
                    double xStart = xMin;
                    double yStart = new Parser().parse(id.Replace("x", "("+xStart+")"));

                    float moveLineX = (float)((xStart + centerX) * scaleX);
                    float moveLineY = (float)((centerY - yStart) * scaleY);

                    pen = new Pen(Color.Red, 3);

                    double exactly = 1 / xStep;
                    for (double i = xMin*exactly; i <= (xMax + Xcounter)*exactly; i += 1)
                    {
                        double x = i / exactly;
                        double y = new Parser().parse(id.Replace("x", "(" + x + ")"));
                        float lineToX = (float)((x + centerX) * scaleX);
                        float lineToY = (float)((centerY - y) * scaleY);
                        g.DrawLine(pen, moveLineX, moveLineY, lineToX, lineToY);
                        moveLineX = lineToX;
                        moveLineY = lineToY;


                    }

                }


            }

            System.IO.MemoryStream ms = new MemoryStream();

            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return File(ms.ToArray(), "image/png");


        }
    }
}