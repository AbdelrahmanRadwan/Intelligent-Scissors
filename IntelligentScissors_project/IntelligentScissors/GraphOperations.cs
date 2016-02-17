using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace IntelligentScissors
{

    public static class Helper
    {



        public static double Distance(int N1, int N2, int Width)
        {
            Vector2D P1 = Helper.Unflatten(N1, Width);
            Vector2D P2 = Helper.Unflatten(N2, Width);
            return Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2));
        }
        public static bool Vaild_Pixel(int X, int Y, RGBPixel[,] ImageMatrix)
        {
            bool Vaild_X = (X >= 0 && X < ImageOperations.GetWidth(ImageMatrix));
            bool Vaild_Y = (Y >= 0 && Y < ImageOperations.GetHeight(ImageMatrix));
            return Vaild_X && Vaild_Y;
        }
        public static RGBPixel[,] COPY(RGBPixel[,] ImageMatrix)
        {
            int Width = ImageOperations.GetWidth(ImageMatrix);
            int Height = ImageOperations.GetHeight(ImageMatrix);


            RGBPixel[,] selected_image = new RGBPixel[Height, Width];
            for (int r = 0; r < Height; r++)
                for (int c = 0; c < Width; c++)
                    selected_image[r, c] = ImageMatrix[r, c];

            return selected_image;
        }
        public static RGBPixel[,] COPY_Segment(RGBPixel[,] ImageMatrix, Boundary bondry)
        {
            // copy a segment  from image matrix into anew one
            int Width = bondry.MAX_X - bondry.MIN_X; // new segment widtrh 
            int Height = bondry.MAX_Y - bondry.MIN_Y; // new segment height


            RGBPixel[,] selected_image = new RGBPixel[Height + 1, Width + 1];
            for (int r = 0; r <= Height; r++)
                for (int c = 0; c <= Width; c++)
                    selected_image[r, c] = ImageMatrix[bondry.MIN_Y + r, bondry.MIN_X + c];

            return selected_image;
        }
        public static bool IN_Boundary(int Target, Boundary bondry, int Width)
        {
            Vector2D Target2d = Helper.Unflatten(Target, Width);
            bool Vaild_X = (Target2d.X >= bondry.MIN_X && Target2d.X < bondry.MAX_X);
            bool Vaild_Y = (Target2d.Y >= bondry.MIN_Y && Target2d.Y < bondry.MAX_Y);

            return Vaild_X && Vaild_Y;
        }
        //take point in a 2d plane and retrun the crossponding in(the small segment) boundry plane 

        public static Point crosspond(Point P, Boundary bondry)
        {
            P.X = P.X + bondry.MIN_X;
            P.Y = P.Y + bondry.MIN_Y;
            return P;
        }
        public static List<Point> crosspond(List<Point> Path, Boundary bondry)
        {
            for (int i = 0; i < Path.Count; i++)
                Path[i] = Helper.crosspond(Path[i], bondry);
            return Path;
        }

        public static int crosspond(int node_number, Boundary bondry, int main_Width, int segment_Width)
        {
            Vector2D node2d = Helper.Unflatten(node_number, main_Width);
            node2d.X = node2d.X - bondry.MIN_X;
            node2d.Y = node2d.Y - bondry.MIN_Y;
            int newnode = Helper.Flatten((int)node2d.X, (int)node2d.Y, segment_Width);
            return newnode;
        }

        /// <summary>
        /// convert 2d index to 1d index  
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <returns>node number in flatten 1d array</returns>

        public static int Flatten(int X, int Y, int width)
        {
            return (X) + (Y * width);
        }

        /// <summary>
        ///convert 1d index to 2d index 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="width"></param>
        /// <returns> vector2d  (X,Y) </X></returns>
        public static Vector2D Unflatten(int Index, int width)
        {
            // y -> row ,  x -> column  
            return new Vector2D((int)Index % (int)width, (int)Index / width);
        }

        public static List<T> AppendToList<T>(List<T> dest, List<T> sourc)
        {
            if (dest == null || sourc == null)
            {

                throw new ArgumentNullException();
            }


            List<T> tmp = dest;
            for (int i = 0; i < sourc.Count; i++)
            {
                tmp.Add(sourc[i]);
            }
            return tmp;

        }
        public static List<T> AppendToList<T>(List<T> dest, T[] sourc)
        {
            if (dest == null || sourc == null)
            {
                return null;
                throw new ArgumentNullException();

            }


            List<T> tmp = dest;
            for (int i = 0; i < sourc.Length; i++)
            {
                tmp.Add(sourc[i]);
            }
            return tmp;

        }


    }

    public class Edge
    {
        public int From, To;
        public double Weight;
        public Edge(int From, int To, double Weight)
        {
            this.From = From;
            this.To = To;
            this.Weight = Weight;
        }
    }

    public static class GraphOperations
    {
        #region Graph Constraction
        public static List<Edge> Get_neighbours(int Node_Index, RGBPixel[,] ImageMatrix)
        {

            List<Edge> neighbours = new List<Edge>();


            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);

            //get x , y indices of the node
            var unflat = Helper.Unflatten(Node_Index, Width);
            int X = (int)unflat.X, Y = (int)unflat.Y;


            // calculate the gradient with right and bottom neighbour
            var Gradient = ImageOperations.CalculatePixelEnergies(X, Y, ImageMatrix);

            if (X < Width - 1) // have a right neighbour ?  
            {
                //add to neighbours list with cost 1/G
                if (Gradient.X == 0)
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X + 1, Y, Width), 10000000000000000));
                else
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X + 1, Y, Width), 1 / (Gradient.X)));

            }

            if (Y < Height - 1) // have a Bottom neighbour ?
            {
                //add to neighbours list with cost 1/G
                if (Gradient.Y == 0)
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X, Y + 1, Width), 10000000000000000));
                else
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X, Y + 1, Width), 1 / (Gradient.Y)));
            }

            if (Y > 0) // have a Top neighbour ?
            {

                // calculate the gradient with top neighbour
                Gradient = ImageOperations.CalculatePixelEnergies(X, Y - 1, ImageMatrix);

                //add to neighbours list with cost 1/G
                if (Gradient.Y == 0)
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X, Y - 1, Width), 10000000000000000));
                else
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X, Y - 1, Width), 1 / (Gradient.Y)));

            }

            if (X > 0) // have a Left neighbour ?
            {

                // calculate the gradient with left neighbour
                Gradient = ImageOperations.CalculatePixelEnergies(X - 1, Y, ImageMatrix);

                //add to neighbours list with cost 1/G 
                if (Gradient.X == 0)
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X - 1, Y, Width), 10000000000000000));
                else
                    neighbours.Add(new Edge(Node_Index, Helper.Flatten(X - 1, Y, Width), 1 / (Gradient.X)));


            }


            return neighbours; // return nei
        }

        public static List<List<Edge>> Graph_Constraction(RGBPixel[,] ImageMatrix)
        {

            int Height = ImageOperations.GetHeight(ImageMatrix);
            int Width = ImageOperations.GetWidth(ImageMatrix);

            // constract empty adjacency List 
            List<List<Edge>> adj_list = new List<List<Edge>>();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {

                    int node_index = Helper.Flatten(j, i, Width); // get flat pixel x,y to 1d number 

                    //constract neighbours list of current pixel(node_index) and add it in  the adj list
                    adj_list.Add(Get_neighbours(node_index, ImageMatrix));
                }
            }

            return adj_list; // return graph adj list
        }

        #endregion
    }



}
