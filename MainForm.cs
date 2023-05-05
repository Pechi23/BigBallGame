using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BigBallGame
{
    public partial class MainForm : Form
    {
        Random rnd = new Random();
        public MainForm()
        {
            InitializeComponent();
        }
        bool Finished;
        Bitmap bmp;
        Graphics grp;
        int numberOfRegularBalls = 20, numberOfRepelentBalls = 2, numberOfMonsterBalls = 1;
        private void simulate_Click(object sender, EventArgs e)
        {
            Finished = false;
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            
            List<Ball> balls = new List<Ball>();
            for (int i = 0; i < numberOfRegularBalls; i++)
            {
                Ball regularBall = new Ball().GetRandomBall("regular", pictureBox1);
                balls.Add(regularBall);
            }
            for (int i = 0; i < numberOfRepelentBalls; i++)
            {
                Ball repelentBall = new Ball().GetRandomBall("repelent", pictureBox1);
                balls.Add(repelentBall);
            }
            for (int i = 0; i < numberOfMonsterBalls; i++)
            {
                Ball monsterBall = new Ball().GetRandomBall("monster", pictureBox1);
                balls.Add(monsterBall);
            }
            
            while (!Finished)
            {
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                grp = Graphics.FromImage(bmp);
                
                Turn(balls,grp,pictureBox1);
                pictureBox1.Image = bmp;
                Thread.Sleep(10);
                pictureBox1.Refresh();

                if (balls.Count <= numberOfRepelentBalls + numberOfMonsterBalls + numberOfRegularBalls / 10)
                {
                    MessageBox.Show("This simulation has ended!");
                    Finished = true;
                }
            }
            
        }

        private void Turn(List<Ball> balls, Graphics grp,PictureBox pictureBox)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].Center.X + balls[i].DX - balls[i].Radius <= 0 || balls[i].Center.X + balls[i].DX + balls[i].Radius >= pictureBox.Width-10)
                    balls[i].DX *= -1;
                if (balls[i].Center.Y + balls[i].DY - balls[i].Radius <= 0 || balls[i].Center.Y + balls[i].DY + balls[i].Radius >= pictureBox.Height-10)
                    balls[i].DY *= -1;

                balls[i].Center = new Point(balls[i].Center.X + balls[i].DX, balls[i].Center.Y + balls[i].DY);
                balls[i].Show(grp);
            }
            for (int i = 0; i < balls.Count - 1; i++)
                for (int j = i + 1; j < balls.Count; j++)
                {
                    if (balls[i].Intersects(balls[j]))
                        balls = Ball.TreatIntersection(balls, i, j);
                }
        }
    }
}