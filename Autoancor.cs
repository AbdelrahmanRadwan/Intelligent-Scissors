using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace IntelligentScissors
{
    public static class Autoancor
    {
        public static List<int> redrawn;
        public static List<double> cool_Time;
        public static List<Point> Live_Wire;

        public static void reset()
        {
            Live_Wire = new List<Point>();
            cool_Time = new List<double>();
            redrawn = new List<int>(); 
        }
        public static void Update(List<Point> Path , double Ctime)
        { 
             int pathsize = Path.Count; int I = 0; 
             int Live_wiresize = Live_Wire.Count; int J = 0;

            while (I < pathsize &&  J < Live_wiresize)
            {
                if (Path[I] == Live_Wire[J])
                {
                    cool_Time[J] += Ctime;
                    redrawn[J] += 1;
                }
                else
                {
                    Live_Wire[J] = Path[I];
                    cool_Time[J] = 0;
                    redrawn[J]   = 0;
                }
                I++; J++;
            }

            while (I < pathsize)
            {
                Live_Wire.Add(Path[I]);
                cool_Time.Add(0);
                redrawn.Add(0);
                I++;
            }
            while (J < Live_wiresize)
            {
               
                Live_Wire[J] = new Point(-1, -1);
                cool_Time[J] = 0;
                redrawn[J] = 0;
                J++;
            }
        }
        
        public static List<Point> anchor_path()
        {
            List<Point> cooledpath = new List<Point>();
            int frezed = 0;
            for (int i = 0; i < Live_Wire.Count; i++)
            {
                if (redrawn[i] >= 8 && cool_Time[i] > 1) // freez point condition
                {
                    frezed = i;
                }
            }
            for (int i = 0; i <frezed; i++)
            {
                cooledpath.Add(Live_Wire[i]);
            }
            return cooledpath;
        }

    }




    #region oldclass
    //   public static class Autoancor
//    {
//  /*     
//       public static List<Point> best_anchor(List<Point> Path, int frequancy, RGBPixel[,] ImageMatrix)
//        {
//            List<Point> bestpath = new List<Point>();
//            bestpath.Add(Path[0]);
//            Point bestanchor = Path[0];
//            double maxW = -100000000000;
           
//            //double  prevW = 0;

//           for (int i = 0; i < Path.Count-1; i++)
//            {
//                Point cur = Path[i]; //curr point in path 
//                Point nxt = Path[i+1]; // next point
//                int curindx = Helper.Flatten(cur.X, cur.Y,ImageOperations.GetWidth(ImageMatrix));
//                int nxtindx = Helper.Flatten(nxt.X, nxt.Y, ImageOperations.GetWidth(ImageMatrix));

//                List<Edge> neibours = GraphOperations.Get_neighbours(curindx, ImageMatrix);
//               // prevW = NxtW;

//                double NxtW = Get_Weight(nxtindx, neibours);
                
//               if (NxtW > maxW)
//                {
//                    bestanchor = nxt;
//                    maxW = NxtW;
//                }
               
//                //int Bestneibour = Best_neibour(neibours);
//                //if (Bestneibour != nxtindx)  break;
               
//               // bestpath.Add(nxt);
//               // if (bestpath.Count == frequancy) break;
//            }
//           int K = 1;
//           while (Path[K] != bestanchor )//&& bestpath.Count <= frequancy)
//           {
//               bestpath.Add(Path[K]);
//               K++;
//           }
//            return bestpath;
//        }
       
//*/

//       public static List<Point> best_anchor9(List<Point> Path, int frequancy, RGBPixel[,] ImageMatrix)
//        {
//           /*
//            List<List<Edge>> N = new List<List<Edge>>();

//            for (int i = 0; i < Path.Count - 1; i++)
//            {
//                Point curr = Path[i]; //curr point in path 
//                int curindx1 = Helper.Flatten(curr.X, curr.Y, ImageOperations.GetWidth(ImageMatrix));
//                List<Edge> neibours1 = GraphOperations.Get_neighbours(curindx1, ImageMatrix);
//                N.Add(neibours1);
//            }
//           */
//           List<Point> bestpath = new List<Point>();
//            bestpath.Add(Path[0]);
//           if(Path.Count == 1)
//               return bestpath;

//           Point cur = Path[0]; //curr point in path 
//           Point nxt = Path[1]; // next point
//           int curindx = Helper.Flatten(cur.X, cur.Y, ImageOperations.GetWidth(ImageMatrix));
//           int nxtindx = Helper.Flatten(nxt.X, nxt.Y, ImageOperations.GetWidth(ImageMatrix));
//           List<Edge> neibours = GraphOperations.Get_neighbours(curindx, ImageMatrix);
//           Edge Nxt_neibour = Get_Neibour(nxtindx, neibours);
           
//           Edge prev_neibour;
//           Point bestanchor = Path[1]; 
           
//           for (int i = 1; i < Path.Count; i++)
//            {
//                 cur = bestanchor; //curr point in path 
//                 bestpath.Add(bestanchor);
//                 prev_neibour = Nxt_neibour;

              
//                 curindx = Helper.Flatten(cur.X, cur.Y,ImageOperations.GetWidth(ImageMatrix));

//                 neibours = GraphOperations.Get_neighbours(curindx, ImageMatrix);
//                 int Bestneibour = Best_neibour(prev_neibour.From, neibours);
//                 Nxt_neibour = Get_Neibour(Bestneibour, neibours);
          
//                 Vector2D tmp = Helper.Unflatten(Bestneibour,ImageOperations.GetWidth(ImageMatrix));
//                 bestanchor = new Point((int)tmp.X, (int)tmp.Y);
//            }
          
//            return bestpath;
//        }



       
//       public static List<Point> enhance(List<Point> Path,  RGBPixel[,] ImageMatrix)
//        {
//           List<Point> bestpath = new List<Point>();
//            bestpath.Add(Path[0]);
//           if(Path.Count == 1)
//               return bestpath;

//           Point bestanchor = Path[0]; 
//           Point cur = Path[0]; //curr point in path 
//           Point nxt = Path[1]; // next point
//           int curindx = Helper.Flatten(cur.X, cur.Y, ImageOperations.GetWidth(ImageMatrix));
//           int nxtindx = Helper.Flatten(nxt.X, nxt.Y, ImageOperations.GetWidth(ImageMatrix));
//           List<Edge> neibours = GraphOperations.Get_neighbours(curindx, ImageMatrix);
           
//           Edge prev_neibour;               
//           Edge Nxt_neibour = Get_Neibour(nxtindx, neibours);
           
//           for (int i = 0; i < Path.Count - 1; i++)
//           {
//               cur = Path[i]; //curr point in path 
//               nxt = Path[i + 1]; // next point
//               curindx = Helper.Flatten(cur.X, cur.Y, ImageOperations.GetWidth(ImageMatrix));
//               nxtindx = Helper.Flatten(nxt.X, nxt.Y, ImageOperations.GetWidth(ImageMatrix));

//               neibours = GraphOperations.Get_neighbours(curindx, ImageMatrix);

//               prev_neibour = Nxt_neibour;

//               int Bestneibour = Best_neibour(prev_neibour.From, neibours);
//               Vector2D tmp = Helper.Unflatten(Bestneibour, ImageOperations.GetWidth(ImageMatrix));
//               bestanchor = new Point((int)tmp.X, (int)tmp.Y);
//               bestpath.Add(bestanchor);
             

//           }
//           return bestpath;

//           }
//       private static Edge Get_Neibour(int TO, List<Edge> neibours)
//       {
//           for (int i = 0; i < neibours.Count; i++)
//           {
//               if (neibours[i].To == TO)
//                return  neibours[i]; 
//           }
//           return new Edge(0,0,0);
//       }
//       private static double Get_Weight(int TO, List<Edge> neibours)
//       {
//           for (int i = 0; i < neibours.Count; i++)
//           {
//               if (neibours[i].To == TO)
//                   return neibours[i].Weight;
//           }
//           return 0;
//       }



//       private static int Best_neibour(int prev ,List<Edge> neibours)
//       {
//           int BN = -10000; //Best neibour (minmal)
//           double minW = 10000000000000000000;
//           for (int i = 0; i < neibours.Count; i++)
//           {
//               if (neibours[i].Weight < minW && neibours[i].To !=prev )
//               {
//                   minW = neibours[i].Weight;
//                   BN = neibours[i].To;
//               }
//           }   
//           return BN;
//       }

//    }
 #endregion
}
