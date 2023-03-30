﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using PHYSICS;


namespace PHYSICS
{
    public class Box
    {
        int id;
        bool self;
        List<Vec2> xp2 = new List<Vec2>();//-------------
        float minX, maxX, minY, maxY,endurance;
        Vec2 pos;
        int width, height;
        PointF[] pts;
        Color color, c1;
        SolidBrush brush;
        Pen p;
        Pen alarm = new Pen(Color.Red, 10);
        static Random rand = new Random();
        public VPoint a, b, c, d,center;
        public VPole p1, p2, p3, p4, p5, p6;
        List<PointF> dPts;
        public bool disposed,col,destroyed=false;
        private static Image image = birds.box;
        public Box(int x, int y, int width, int height, int id,float endurance)
        {
            this.width = width;
            this.height = height;
            this.endurance= endurance;
            this.id = id;
            pos = new Vec2(x, y);

            
            a = new VPoint(x - (width / 2), y - (height / 2), rand.Next(5), rand.Next(-2, 2), id);
            b = new VPoint(x + (width / 2), y - (height / 2), id + 1);
            c = new VPoint(x + (width / 2), y + (height / 2), id + 2);
            d = new VPoint(x - (width / 2), y + (height / 2), id + 3);
            
            
            a.FromBody = true;
            b.FromBody = true;
            c.FromBody = true;
            d.FromBody = true;



            brush = new SolidBrush(Color.FromArgb(50, 65, 255));

            Init(a, b, c, d);
        }



        public void Init(VPoint a, VPoint b, VPoint c, VPoint d)
        {
            col = false;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            /*
            this.a.IsPinned=true;
            this.b.IsPinned = true;
            this.c.IsPinned = true;
            this.d.IsPinned = true;
            */
            p1 = new VPole(a, b);
            p2 = new VPole(b, c);
            p3 = new VPole(c, d);
            p4 = new VPole(d, a);
            p5 = new VPole(b, d);
            p6 = new VPole(c, a);
            pts = new PointF[4];
            ///Madera
            float stif = 0.5f;
            p1.setStif(stif);  // Lado 1
            p2.setStif(stif);  // Lado 2
            p3.setStif(stif);  // Lado 3
            p4.setStif(stif);  // Lado 4
            p5.setStif(stif);  // Diagonal 1
            p6.setStif(stif);  // Diagonal 2
            //DAMP
            float dmp = 2.5f;
            p1.setDamp(dmp);  // Lado 1
            p2.setDamp(dmp); // Lado 2
            p3.setDamp(dmp);  // Lado 3
            p4.setDamp(dmp);  // Lado 4
            p5.setDamp(dmp);  // Diagonal 1
            p6.setDamp(dmp);  // Diagonal 2



            c1 = Color.FromArgb(218, 159, 89);
            brush = new SolidBrush(c1);

            color = Color.FromArgb(Math.Max(0,c1.R - 50), Math.Max(0,c1.G - 50), Math.Max(0,c1.B - 50));

            p = new Pen(color, 14);
            p = new Pen(color, a.Radius);
            dPts = new List<PointF>();
        }

        public void SetColor(int R,int G,int B)
        {
            c1= Color.FromArgb(R, G, B);
           
        }

        public void Render(Graphics g, int width, int height)
        {
            Update(width, height);
            //g.DrawRectangle(Pens.Yellow, mX, mY, Mx - mX, My - mY);

            dPts.Clear();
            dPts.Add(new PointF(pts[0].X - a.Radius, pts[0].Y - a.Radius));
            dPts.Add(new PointF(pts[1].X + a.Radius, pts[1].Y - a.Radius));
            dPts.Add(new PointF(pts[2].X + a.Radius, pts[2].Y + a.Radius));
            dPts.Add(new PointF(pts[3].X - a.Radius, pts[3].Y + a.Radius));

            DrawBox(g);
        }

        private void Update(int width, int height)
        {

            a.Update(width, height); b.Update(width, height); c.Update(width, height); d.Update(width, height);
            
            if (!col)
            {
                a.Constraints(width, height); b.Constraints(width, height); c.Constraints(width, height); d.Constraints(width, height);
                
            }
            else
            {
                a.setGravity(1000); b.setGravity(1000); c.setGravity(1000); d.setGravity(1000);
            }



            p1.Update(); p2.Update(); p3.Update(); p4.Update(); 
            p5.Update(); p6.Update();

            pts[0] = new PointF(a.Pos.X, a.Pos.Y);
            pts[1] = new PointF(b.Pos.X, b.Pos.Y);
            pts[2] = new PointF(c.Pos.X, c.Pos.Y);
            pts[3] = new PointF(d.Pos.X, d.Pos.Y);

            BoundingBox();
        }
        //--------------------------------------------------------------------------
        public void BoundingBox()
        {
            minX = float.MaxValue;
            maxX = float.MinValue;
            minY = float.MaxValue;
            maxY = float.MinValue;

            minX = Math.Min(Math.Min(pts[0].X, pts[1].X), Math.Min(pts[2].X, pts[3].X));
            minY = Math.Min(Math.Min(pts[0].Y, pts[1].Y), Math.Min(pts[2].Y, pts[3].Y));

            maxX = Math.Max(Math.Max(pts[0].X, pts[1].X), Math.Max(pts[2].X, pts[3].X));
            maxY = Math.Max(Math.Max(pts[0].Y, pts[1].Y), Math.Max(pts[2].Y, pts[3].Y));
        }

        public void React(Graphics g, List<VPoint> pts, int width, int height)//----------------
        {
            Render(g, width, height);

            for (int p = 0; p < pts.Count; p++)
                React(g, pts[p]);//*/
        }


        private bool React(Graphics g, VPoint p)//--------------------------
        {
            if (p == null || p.FromBody)
                return false;

            //g.DrawRectangle(Pens.Blue, mX, mY, Mx - mX, My - mY);//check for collision

            EdgeCollision(g, p);

            return false;
        }

        public void EdgeCollision(Graphics g, VPoint p)//---------------------------------
        {
            int index;
            float distace, tmp;
            xp2.Clear();

            distace = float.MaxValue;
            VPole a, b, c, d;

            pos.X = pts[0].X + pts[1].X + pts[2].X + pts[3].X;
            pos.Y = pts[0].Y + pts[1].Y + pts[2].Y + pts[3].Y;

            pos.X /= 4;
            pos.Y /= 4;

            index = -1;

            a = new VPole(new VPoint((int)pts[0].X, (int)pts[0].Y), new VPoint((int)pts[1].X, (int)pts[1].Y));
            b = new VPole(new VPoint((int)pts[1].X, (int)pts[1].Y), new VPoint((int)pts[2].X, (int)pts[2].Y));
            c = new VPole(new VPoint((int)pts[2].X, (int)pts[2].Y), new VPoint((int)pts[3].X, (int)pts[3].Y));
            d = new VPole(new VPoint((int)pts[3].X, (int)pts[3].Y), new VPoint((int)pts[0].X, (int)pts[0].Y));
            
            FindIntersections(a, p.Pos);
            FindIntersections(b, p.Pos);
            FindIntersections(c, p.Pos);
            FindIntersections(d, p.Pos);

            for (int point = 0; point < xp2.Count; point++)
            {
                tmp = xp2[point].Distance(p.Pos);
                if (tmp < distace)
                {
                    distace = tmp;
                    index = point;
                }
            }

            if (distace < p.Radius + 4)
            {
                g.DrawLine(Pens.AliceBlue, xp2[index].X, xp2[index].Y, p.Pos.X, p.Pos.Y);
                g.DrawPolygon(alarm, pts);

                //si la velocidad es suficiente para romper el cubo
                if (p.vel.X> endurance || p.vel.Y/5 >endurance)
                {
                    col = true;
                    destroyed=true;
                }
                //substraer la velocidad reduciendo la dureza
                else
                {
                    endurance -= p.vel.Y/5;
                    endurance-= p.vel.X;
                }
                /*
                this.a.IsPinned = false;
                this.b.IsPinned = false;
                
                this.c.IsPinned = false;
                this.d.IsPinned = false;
                */

                if (!p.IsPinned)//----------------FALTA CREAR LA REACCIÓN DE LA CAJA MOVIENDO LOS DOS PUNTOS DE MASA CORRESPONDIENTES AL RESORTE
                {
                    
                    Vec2 temp = p.Pos;
                    p.Pos = p.Old + 0.01f;
                    p.Old = temp - 30f;

                    this.a.setGravity(2);
                    this.b.setGravity(2);
                    this.c.setGravity(2);
                    this.d.setGravity(2);

                    

                    
                    
                    float minusTemp = 0.05f;
                    this.a.Pos = this.a.Old + .01f;
                    this.a.Old = temp - minusTemp;

                   
                    this.b.Pos = this.b.Old + .01f;
                    this.b.Old = temp - minusTemp;

                    /*
                    this.c.Pos = this.b.Old + .01f;
                    this.c.Old = temp - minusTemp;

                    /*
                    this.d.Pos = this.d.Old + .01f;
                    this.d.Old = temp - minusTemp;
                    */
                    p.setGravity(8);
                    p.collides= true;
                }
                
            }
        }

        private void FindIntersections(VPole pole, Vec2 p)//--------------------------
        {
            if (Util.HasIn(pole.P1, pole.P2, p))
            {
                xp2.Add(Util.GetPointLineIntersection(pole.P1, pole.P2, p));
            }
        }

        private void DrawBox(Graphics g)
        {
            int centerX = ((int)pts[0].X + (int)pts[1].X + (int)pts[2].X + (int)pts[3].X) / 4;
            int centerY = ((int)pts[0].Y + (int)pts[1].Y + (int)pts[2].Y + (int)pts[3].Y) / 4;

            int d1 = (int)Math.Sqrt(Math.Pow(pts[0].X - centerX, 2) + Math.Pow(pts[0].Y - centerY, 2));
            int d2 = (int)Math.Sqrt(Math.Pow(pts[1].X - centerX, 2) + Math.Pow(pts[1].Y - centerY, 2));
            int d3 = (int)Math.Sqrt(Math.Pow(pts[2].X - centerX, 2) + Math.Pow(pts[2].Y - centerY, 2));
            int d4 = (int)Math.Sqrt(Math.Pow(pts[3].X - centerX, 2) + Math.Pow(pts[3].Y - centerY, 2));

            Point[] ptsNew = new Point[pts.Length];

            ptsNew[0].X = centerX + (int)(((pts[0].X - centerX) / (double)d1) * (d1 + 28));
            ptsNew[0].Y = centerY + (int)(((pts[0].Y - centerY) / (double)d1) * (d1 + 28));

            ptsNew[1].X = centerX + (int)(((pts[1].X - centerX) / (double)d2) * (d2 + 28));
            ptsNew[1].Y = centerY + (int)(((pts[1].Y - centerY) / (double)d2) * (d2 + 28));

            ptsNew[2].X = centerX + (int)(((pts[2].X - centerX) / (double)d3) * (d3 + 28));
            ptsNew[2].Y = centerY + (int)(((pts[2].Y - centerY) / (double)d3) * (d3 + 28));

            ptsNew[3].X = centerX + (int)(((pts[3].X - centerX) / (double)d4) * (d4 + 28));
            ptsNew[3].Y = centerY + (int)(((pts[3].Y - centerY) / (double)d4) * (d4 + 28));

            g.DrawImage(image, new Rectangle((int)ptsNew[0].X, (int)ptsNew[0].Y, (int)(ptsNew[2].X - ptsNew[0].X), (int)(ptsNew[2].Y - ptsNew[0].Y)));
        }
    }
}
