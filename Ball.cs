using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace BigBallGame
{
    class Ball
    {
        private string Type { get; set; }
        private int Radius { get; set; }
        private Point Center { get; set; }
        private int DX { get; set;}
        private int DY { get; set; }
        private Color BallColor { get; set; }

        private static Random rand = new Random();

        public async void Show(Graphics grp)
        {
            grp.FillEllipse(new SolidBrush(BallColor), Center.X - Radius, Center.Y - Radius, Radius*2, Radius*2);
        }
        private Ball GetRandomBall(string type, PictureBox pictureBox)
        {
            Ball newBall = new Ball()
            {
                Type = type,
                Radius = rand.Next(10, 20),
                Center = new Point(rand.Next(20, pictureBox.Width - 20), rand.Next(20, pictureBox.Height - 20)),
                DX = rand.Next(-10, 10),
                DY = rand.Next(-10, 10),
                BallColor = Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255)),
            };
            
            if (newBall.Type == "monster")
            {
                newBall.BallColor = Color.Black;
                newBall.DX = newBall.DY = 0;
            }

            return newBall;
        }

        public bool Intersects(Ball other)
        {
            if (Distance(this.Center, other.Center) <= (this.Radius + other.Radius))
                return true;
            return false;
        }

        private double Distance(Point a,Point b)
        {
            int x = a.X - b.X;
            int y = a.Y - b.Y;
            return Math.Sqrt(x*x + y*y); 
        }

        private static List<Ball> TreatIntersectionBetweenBalls(List<Ball> balls, int i, int j)
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
                case "repelent repelent": (balls[i].BallColor, balls[j].BallColor) = (balls[j].BallColor, balls[i].BallColor); balls[i].DX *= -1; balls[i].DY *= -1; balls[j].DX *= -1; balls[i].DY *= -1; break;
                case "monster repelent": balls[j].Radius /= 2; balls[j].DX *= -1; balls[j].DY *= -1; break;
                case "repelent monster": balls[i].Radius /= 2; balls[i].DX *= -1; balls[i].DY *= -1; break;
                case "monster regular": balls[i].Radius += balls[j].Radius; balls.RemoveAt(j); break;
                case "regular monster": balls[j].Radius += balls[i].Radius; balls.RemoveAt(i); break;
            }
            return balls;
        }

        public static List<Ball> GenerateBalls(PictureBox pictureBox, int numberOfRegularBalls, int numberOfRepelentBalls, int numberOfMonsterBalls)
        {
            List<Ball> balls = new List<Ball>();

            for (int i = 0; i < numberOfRegularBalls; i++)
            {
                Ball regularBall = new Ball().GetRandomBall("regular", pictureBox);
                balls.Add(regularBall);
            }
            for (int i = 0; i < numberOfRepelentBalls; i++)
            {
                Ball repelentBall = new Ball().GetRandomBall("repelent", pictureBox);
                balls.Add(repelentBall);
            }
            for (int i = 0; i < numberOfMonsterBalls; i++)
            {
                Ball monsterBall = new Ball().GetRandomBall("monster", pictureBox);
                balls.Add(monsterBall);
            }

            return balls;
        }

        public void Move()
        {
            this.Center = new Point(this.Center.X + this.DX, this.Center.Y + this.DY);
        }

        public static List<Ball> TreatForIntersectionBetweenBalls(List<Ball> balls)
        {
            for (int i = 0; i < balls.Count - 1; i++)
                for (int j = i + 1; j < balls.Count; j++)
                    if (balls[i].Intersects(balls[j]))
                        balls = Ball.TreatIntersectionBetweenBalls(balls, i, j);
            return balls;
        }

        public void TreatIntersectionWithPictureBoxBounds(PictureBox pictureBox)
        {
            if (this.Center.X + this.DX - this.Radius <= 0 || this.Center.X + this.DX + this.Radius >= pictureBox.Width)
                this.DX *= -1;
            if (this.Center.Y + this.DY - this.Radius <= 0 || this.Center.Y + this.DY + this.Radius >= pictureBox.Height)
                this.DY *= -1;
        }
    }
}
