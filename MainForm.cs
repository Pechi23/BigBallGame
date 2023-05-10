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

            List<Ball> balls = Ball.GenerateBalls(pictureBox1, numberOfRegularBalls, numberOfRepelentBalls, numberOfMonsterBalls);
            
            while (!Finished)
            {
                bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                grp = Graphics.FromImage(bmp);
                
                Turn(balls,grp,pictureBox1);

                pictureBox1.Image = bmp;

                Thread.Sleep(8);
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
                balls[i].TreatIntersectionWithPictureBoxBounds(pictureBox);
                balls[i].Move();
                balls[i].Show(grp);
            }
            balls = Ball.TreatForIntersectionBetweenBalls(balls);
        }
    }
}