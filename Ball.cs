using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace BigBallGame
{
    class Ball
    {
        public string Type { get; set; }
        public int Radius { get; set; }
        public Point Center { get; set; }
        public int DX { get; set;}
        public int DY { get; set; }
        public Color BallColor { get; set; }

        static Random rand = new Random();

        public async void Show(Graphics grp)
        {
            grp.DrawEllipse(new Pen(BallColor,Radius*2), Center.X, Center.Y, Radius*2, Radius*2);
        }
        public Ball GetRandomBall(string type,PictureBox pictureBox)
        {
            Ball ball = new Ball();
            ball.Type = type;
            ball.Radius = rand.Next(5, 10);
            ball.Center = new Point(rand.Next(20, pictureBox.Width - 20), rand.Next(20, pictureBox.Height - 20));
            ball.DX = rand.Next(-10, 10);
            ball.DY = rand.Next(-10, 10);
            
            if (ball.Type == "regular" || ball.Type == "repelent")
                ball.BallColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
            else
            {
                ball.BallColor = Color.Black;
                ball.DX = ball.DY = 0;
            }
            return ball;
        }

        public bool Intersects(Ball other)
        {
            if (DistanceSquared(this.Center, other.Center) <= (this.Radius + other.Radius)* (this.Radius + other.Radius))
                return true;
            return false;
        }

        private double DistanceSquared(Point a,Point b)
        {
            int x = a.X - b.X;
            int y = a.Y - b.Y;
            return x*x + y*y; 
        }

        public static List<Ball> TreatIntersection(List<Ball> balls, int i, int j)
        {
            string types = balls[i].Type + " " + balls[j].Type;
            switch (types)
            {
                case "regular regular":
                    if (balls[i].Radius >= balls[j].Radius)
                    {
                        balls[i].Radius += balls[j].Radius;
                        balls.RemoveAt(j);
                    }
                    else
                    {
                        balls[j].Radius += balls[i].Radius;
                        balls.RemoveAt(i);
                    }
                    break;
                case "regular repelent": balls[j].BallColor = balls[i].BallColor; balls[i].DX *= -1; balls[j].DX *= -1; break;
                case "repelent regular": balls[i].BallColor = balls[j].BallColor; balls[j].DX *= -1; balls[i].DX *= -1; break;
                case "repelent repelent": (balls[i].BallColor, balls[j].BallColor) = (balls[j].BallColor, balls[i].BallColor); balls[i].DX *= -1; balls[j].DX *= -1; break;
                case "monster repelent": balls[j].Radius /= 2; balls[j].DX *= -1; balls[j].DY *= -1;break;
                case "repelent monster": balls[i].Radius /= 2; balls[i].DX *= -1; balls[i].DY *= -1;break;
                case "monster regular": balls[i].Radius += balls[j].Radius; balls.RemoveAt(j); break;
                case "regular monster": balls[j].Radius += balls[i].Radius; balls.RemoveAt(i); break;
            }
            return balls;
        }
    }
}
