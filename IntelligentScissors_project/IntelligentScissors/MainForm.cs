using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
       // List<List<Edge>> Graph;
        List<int> parent_list;
        bool ismousedown = false;
        bool AutoAnchor_WORK = false;
        int frequancy = 57;

        List<Point> AnchorPts;

        float[] DashPattern = { (float)1, (float)0.000000000001 };

        Point AnchorSize = new Point(5, 5);

        

        List<Point> Mainselction;   // LIST OF ALL SELECTED POINT

        Point[] curr_path;
        
        int curr_source = -1, main_source = -1;

        Pen m_pen = new Pen(Brushes.Orange, 1); // MAIN SELECTION PEN
        Pen c_pen = new Pen(Brushes.Aqua, 1);  // CURRENET PATH PEN
        
        RGBPixel[,] Square_segment; // dikstra square
        Boundary SB; // dikstra square

        int Prev_mouse_pos;
        void init()
        {
            AnchorPts = new List<Point>();
            Mainselction = new List<Point>();
        }




        public MainForm()
        {
            InitializeComponent();
            init();
        }

        RGBPixel[,] ImageMatrix;


        void reset()
        {

            curr_path = null;
            AnchorPts.Clear();
            Mainselction.Clear();
            curr_source = -1;
            Prev_mouse_pos = -1;
            main_source = -1;
            ismousedown = false;
            AutoAnchor_WORK = false;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {


            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                reset();
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            //Graph = GraphOperations.Graph_Constraction(ImageMatrix);


        }


        float clr = 0.0f;
        float W8interval = .02f;
        
        public void update(MouseEventArgs e)
        {
            var g = pictureBox1.CreateGraphics();
            if (clr > W8interval * 2)
            {


                if (ImageMatrix != null)
                {
                  var mouseNode = Helper.Flatten(e.X, e.Y, ImageOperations.GetWidth(ImageMatrix));

                  if (curr_source != -1 && Prev_mouse_pos!=mouseNode)
                    {
                        Prev_mouse_pos = mouseNode;
                        if (Helper.IN_Boundary(mouseNode, SB, ImageOperations.GetWidth(ImageMatrix)))
                        {
                            int Segment_mouse = Helper.crosspond(mouseNode, SB,
                                ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(Square_segment));
                            List<Point> segmentpath = new List<Point>();
                            segmentpath = ShortestPath_Operations.Backtracking(parent_list, Segment_mouse, ImageOperations.GetWidth(Square_segment));
                            List<Point> Curpath = Helper.crosspond(segmentpath, SB);
                            curr_path =Curpath.ToArray();


                            if (ismousedown && AutoAnchor_WORK)
                            {
                                double freq = (double)frequancy / 1000;
                                Autoancor.Update(Curpath, freq);
                                List<Point> cooledpath = Autoancor.anchor_path();
                                if (cooledpath.Count > 0)
                                {
                                    Point anchor = cooledpath[cooledpath.Count - 1];
                                    AnchorPts.Add(anchor);
                                    curr_source = Helper.Flatten(anchor.X, anchor.Y, ImageOperations.GetWidth(ImageMatrix));
                                    //curr_path = cooledpath.ToArray();
                                    Helper.AppendToList<Point>(Mainselction, cooledpath);

                                    SB = new Boundary();
                                    SB = ShortestPath_Operations.Square_Boundary(curr_source,
                                        ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1);
                                    //make a square segment
                                    Square_segment = Helper.COPY_Segment(ImageMatrix, SB);
                                    // currsrc in segment
                                    int newsrc = Helper.crosspond(curr_source, SB, ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(Square_segment));
                                    parent_list = ShortestPath_Operations.Dijkstra(newsrc, Square_segment);
                                    Autoancor.reset();
                                }
                            }
                        }
                        else
                            curr_path = null;


                        #region bounse
                        
                        
                       // if (ismousedown && AutoAnchor_WORK)
                      //  {
                            /*
                            //if (curr_path.Length >= frequancy)
                            if (Helper.Distance(curr_source, mouseNode, ImageOperations.GetWidth(ImageMatrix)) >= frequancy)
                            {
                                Helper.AppendToList<Point>(Mainselction, curr_path);
                                AnchorPts.Add(e.Location);
                                curr_source = mouseNode;
                                parent_list = ShortestPath_Operations.Dijkstra(Graph, curr_source);
                            }
                           */
                        //}
                        #endregion
                        
                    }
                }

                clr = 0.0f;
            }

            if (clr > W8interval)
            {
                pictureBox1.Refresh();
                g.Dispose();
            }

            clr += .019f;
        }


        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
            //Graph = GraphOperations.Graph_Constraction(ImageMatrix);
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {  
            update(e);
            X_TXTBOX.Text = e.X.ToString(); //mouse text box 
            Y_TXTBOX.Text = e.Y.ToString();
            if(pictureBox1.Image!=null)
                NODETXTBOX.Text = Helper.Flatten(e.X, e.Y, ImageOperations.GetWidth(ImageMatrix)).ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

            if (ImageMatrix != null)
            {
                var g = e.Graphics;
                for (int i = 0; i < AnchorPts.Count; i++)
                {
                    g.FillEllipse(Brushes.Yellow, new Rectangle(
                        new Point(AnchorPts[i].X - AnchorSize.X / 2, AnchorPts[i].Y - AnchorSize.Y / 2),
                        new Size(AnchorSize)));
                }

                if (curr_path != null)
                    if (curr_path.Length > 10)
                        customDrawer.drawDottedLine(g, c_pen, curr_path, DashPattern);


                if (Mainselction != null && Mainselction.Count > 5)
                    customDrawer.drawDottedLine(e.Graphics, m_pen, Mainselction.ToArray(), DashPattern);
            }

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                var clicked_node = Helper.Flatten(e.X, e.Y, ImageOperations.GetWidth(ImageMatrix));

                if (curr_source != clicked_node)
                {
                    if (curr_source == -1) // in the first click save frist clicked anchor
                        main_source = clicked_node;
                    else
                        Helper.AppendToList<Point>(Mainselction, curr_path);

                    curr_source = clicked_node;
                    AnchorPts.Add(e.Location);

                     SB = new Boundary();
                    SB =ShortestPath_Operations.Square_Boundary(curr_source,
                        ImageOperations.GetWidth(ImageMatrix) - 1, ImageOperations.GetHeight(ImageMatrix) - 1);
                    //make a square segment
                     Square_segment = Helper.COPY_Segment(ImageMatrix, SB);
                    // currsrc in segment
                    int newsrc = Helper.crosspond(curr_source,SB,ImageOperations.GetWidth(ImageMatrix),ImageOperations.GetWidth(Square_segment));
                    parent_list = ShortestPath_Operations.Dijkstra(newsrc, Square_segment);
                    Autoancor.reset();
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            ismousedown = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            ismousedown = false;
        }
        private void AutoAnchor_Click(object sender, EventArgs e)
        {
            AutoAnchor_WORK = true;
        }
        private void frequancyNUD_ValueChanged(object sender, EventArgs e)
        {
            frequancy = (int)frequancyNUD.Value;
        }
        private void crop_Click(object sender, EventArgs e)
        {
            if (curr_source != main_source)
            {
                // check if first node in shortest path range of last node
                //if yes get it fast else try to get it by dikstra
                if (Helper.IN_Boundary(main_source, SB, ImageOperations.GetWidth(ImageMatrix)))
                {
                    int Segment_mouse = Helper.crosspond(main_source, SB,ImageOperations.GetWidth(ImageMatrix), ImageOperations.GetWidth(Square_segment));
                    List<Point> segmentpath = new List<Point>();
                    segmentpath = ShortestPath_Operations.Backtracking(parent_list, Segment_mouse, ImageOperations.GetWidth(Square_segment));
                    curr_path = Helper.crosspond(segmentpath, SB).ToArray();
                }
                else
                     curr_path = ShortestPath_Operations.GenerateShortestPath(curr_source, main_source, ImageMatrix).ToArray();
                
                Helper.AppendToList<Point>(Mainselction, curr_path);
           
                //flod fill and crop
                RGBPixel[,] selected_image = floodfill.fill(Mainselction, ImageMatrix);
                CropedImage CI = new CropedImage(selected_image);
                CI.Show();
                //rest to the defualt
                reset();
            }
        }
        private void clear_Click(object sender, EventArgs e)
        {
            reset();
        }

        
        


    }
}



public static class customDrawer
{



    /// <summary>
    /// old function to draw dashed line 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="arr"></param>
    /// <param name="interval"></param>
 /*   static void drawDottedLine(PaintEventArgs e, Pen p, PointF[] arr, int interval)
    {

        for (int i = 0; i < arr.Length - interval; i += interval * 2)
        {
            PointF[] tmpArr = new PointF[interval];
            for (int j = 0, ii = i; j < interval; j++, ii++)
                tmpArr[j] = arr[ii];

            e.Graphics.DrawCurve(p, tmpArr);

        }


    }*/
    /// <summary>
    /// new func 
    /// </summary>
    /// <param name="e"></param>
    /// <param name="arr"></param>
    /// <param name="_dash_vals"></param>
    public static void drawDottedLine(Graphics g, Pen p, Point[] arr, float[] _dash_vals)
    {
        p.DashPattern = _dash_vals;
        g.DrawCurve(p, arr);
    }


    public static void drawDottedLine(Graphics g, Pen p, Point A, Point B, float[] _dash_vals)
    {
        Point[] arr = new Point[2];
        arr[0] = A;
        arr[1] = B;

        drawDottedLine(g, p, arr, _dash_vals);

    }
    /*
    public static void drawCrossHair(Graphics g, MouseEventArgs em, Pen P)
    {
        var orgin = em.Location;
        // horizontal 
        var p1 = new Point(0, orgin.Y);
        var p3 = new Point(1000, orgin.Y);

        // vertical
        var p2 = new Point(orgin.X, 0);
        var p4 = new Point(orgin.X, 1000);


        g.DrawLine(P, p1, p3);
        g.DrawLine(P, p2, p4);

    }
*/
}
