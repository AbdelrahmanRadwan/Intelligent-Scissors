using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntelligentScissors
{
    public struct Boundary
    {
     public  int MIN_X, MAX_X, MIN_Y, MAX_Y;
    }
    public static class floodfill
    {
        private static RGBPixel[,] selected_image;

        
        public static RGBPixel[,] fill(List<Point> selected_points, RGBPixel[,] ImageMatrix)
        {
            Boundary bondry = GET_Boundary(selected_points); // Boundary of the main selection
            selected_image = Helper.COPY_Segment(ImageMatrix,bondry); // get croped image 
            block_border(selected_points,bondry);                     // assign the selected point as blocks
            Flood_Fill(ImageOperations.GetWidth(selected_image) - 1, ImageOperations.GetHeight(selected_image) - 1);
            return selected_image;
        }
        private static void block_border(List<Point> selected_points ,Boundary bondry)
        {

            for (int i = 0; i < selected_points.Count; i++)
            {
                int Xtmp = selected_points[i].X - bondry.MIN_X;  // get the crossponding point in segment image
                int Ytmp = selected_points[i].Y - bondry.MIN_Y;
                selected_image[Ytmp, Xtmp].block = true;   
            }

        }
        private static void Flood_Fill(int Width , int Height)
        {
            ///Track DFS  on border 
            for (int i = 0; i <= Width; i++)
                if (!selected_image[0, i].block)
                    DFS(new Vector2D(i, 0));

            for (int i = 0; i <= Width; i++)
                if (!selected_image[Height, i].block)
                    DFS(new Vector2D(i, Height));


            for (int i = 0; i <= Height; i++)
                if (!selected_image[i,0].block)
                    DFS(new Vector2D(0, i));

            for (int i = 0; i <= Height; i++)
                if (!selected_image[i, Width].block)
                    DFS(new Vector2D(Width, i));
        }
        private static void DFS(Vector2D START)
        {
            Stack<Vector2D> DFS_Stack = new Stack<Vector2D>();
            DFS_Stack.Push(START);
            while (DFS_Stack.Count > 0)
            {
                Vector2D Curr = DFS_Stack.Pop();
                if(Helper.Vaild_Pixel((int)Curr.X, (int)Curr.Y, selected_image))
                {
                    if(!selected_image[(int)Curr.Y, (int)Curr.X].block)
                    {
                        selected_image[(int)Curr.Y, (int)Curr.X].block = true;
                        
                        //black or whiteen the pixel 
                        selected_image[(int)Curr.Y, (int)Curr.X].blue = 240; //bgclor
                        selected_image[(int)Curr.Y, (int)Curr.X].green = 240;
                        selected_image[(int)Curr.Y, (int)Curr.X].red = 240;


                        DFS_Stack.Push(new Vector2D(Curr.X, Curr.Y + 1));
                        DFS_Stack.Push(new Vector2D(Curr.X, Curr.Y - 1));
                        DFS_Stack.Push(new Vector2D(Curr.X + 1, Curr.Y));
                        DFS_Stack.Push(new Vector2D(Curr.X - 1, Curr.Y));
                    }
                }

            }

        }
        private static Boundary GET_Boundary(List<Point> selected_points)  // calculate  corped image Boundary
        {
            // calculate minx , maxx , miny , maxy for the main selection
            Boundary bondry;
            bondry.MAX_X = bondry.MAX_Y = -1000000000; 
            bondry.MIN_X = bondry.MIN_Y =  1000000000;

            for (int i = 0; i < selected_points.Count;i++)
            {
                int tmpx = selected_points[i].X;
                int tmpy = selected_points[i].Y;
               
                if(tmpx > bondry.MAX_X) bondry.MAX_X = tmpx;
                if (tmpx < bondry.MIN_X) bondry.MIN_X = tmpx;
                if (tmpy > bondry.MAX_Y) bondry.MAX_Y = tmpy;
                if (tmpy < bondry.MIN_Y) bondry.MIN_Y = tmpy;

            }
            return bondry;
        
        }
    }
}
