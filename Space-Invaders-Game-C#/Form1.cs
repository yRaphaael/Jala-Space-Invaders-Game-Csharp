using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Space_Invaders_Game_C_
{
    public partial class Form1 : Form
    {
        SoundPlayer shootSound = new SoundPlayer(@"C:\Users\rapha\source\repos\Space-Invaders-Game-C#\Space-Invaders-Game-C#\Resources\laserShoot.wav");
        bool goLeft, goRight;
        int playerSpeed = 6;
        int enemySpeed = 3;
        int score = 0;
        int enemyBulletTimer = 300;

        PictureBox[] invadersArray;

        bool shooting;
        bool isGameOver;

        public Form1()
        {
            InitializeComponent();
            gameSetup();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtScore_Click(object sender, EventArgs e)
        {

        }

        private void mainGameTimerEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: " + score;
            if (goLeft)
            {
                player.Left -= playerSpeed;
            }
            if (goRight)
            {
                player.Left += playerSpeed;
            }

            enemyBulletTimer -= 10;

            if (enemyBulletTimer < 1)
            {
                enemyBulletTimer = 300;
                makeBullet("invaderBullet");
            }

            foreach (Control x in this.Controls)
            {

                if (x is PictureBox && (string)x.Tag == "invaders")
                {
                    x.Left += enemySpeed;
                    if (x.Left > 1280)
                    {
                        x.Top += 65;
                        x.Left = -80;
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        gameOver("You've been invaded by the sad invaders, you are now sad!");
                    }

                    foreach (Control y in this.Controls)
                    {
                        if (y is PictureBox && (string)y.Tag == "bullet")
                        {
                            if (y.Bounds.IntersectsWith(x.Bounds))
                            {
                                this.Controls.Remove(x);
                                this.Controls.Remove(y);
                                score += 1;
                                shooting = false;
                            }
                        }
                    }
                }

                if (x is PictureBox && (string)x.Tag == "bullet")
                {
                    x.Top -= 20;

                    if (x.Top < 15)
                    {
                        this.Controls.Remove(x);
                        shooting = false;
                    }
                }

                if (x is PictureBox && (string)x.Tag == "invaderBullet")
                {
                    x.Top += 20;
                    if (x.Top > 720)
                    {
                        this.Controls.Remove(x);
                    }

                    if (x.Bounds.IntersectsWith(player.Bounds))
                    {
                        this.Controls.Remove(x);
                        gameOver("You've been killed by the sad bullet. Now you are sad forever!");
                    }
                }
            }

            if (score > 10)
            {
                enemySpeed = 9;
            }

            if (score == invadersArray.Length)
            {
                gameOver("Woohoo Happiness Found, Keep it safe!");
            }

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if(e.KeyCode == Keys.Space && shooting == false)
            {
                shooting = true;
                shootSound.Play();
                makeBullet("bullet");
            }
            if(e.KeyCode == Keys.Enter && isGameOver == true)
            {
                removeAll();
                gameSetup();

            }
        }

        private void makeInvaders()
        {
            invadersArray = new PictureBox[15];

            int left = 0;

            for(int i = 0; i < invadersArray.Length; i++)
            {

                invadersArray[i] = new PictureBox();
                invadersArray[i].Size = new Size(60, 50);
                invadersArray[i].Image = Properties.Resources.enemy3_stand;
                invadersArray[i].Top = 5;
                invadersArray[i].BackColor = Color.Transparent;
                invadersArray[i].Tag = "invaders";
                invadersArray[i].Left = left;
                invadersArray[i].SizeMode = PictureBoxSizeMode.StretchImage;
                this.Controls.Add(invadersArray[i]);
                left = left - 80;
            }
        }

        private void gameSetup()
        {
            txtScore.Text = "Score: 0";
            score = 0;
            isGameOver = false;

            enemyBulletTimer = 300;
            enemySpeed = 5;
            shooting = false;

            makeInvaders();
            gameTimer.Start();
        }

        private void gameOver(string message)
        {
            isGameOver = true;
            gameTimer.Stop();
            txtScore.Text = "Score: " + score + " " + message;

        }

        private void removeAll()
        {
            foreach (PictureBox i in invadersArray)
            {
                this.Controls.Remove(i);
            }

            foreach(Control x in this.Controls)
            {
                if(x is PictureBox)
                {
                    if((string)x.Tag == "bullet" || (string)x.Tag == "invaderBullet")
                    {
                        this.Controls.Remove(x);
                    }
                }
            }
        }

        private void makeBullet(string bulletTag)
        {
            PictureBox bullet = new PictureBox();
            bullet.Image = Properties.Resources.ship_attack;
            bullet.BackColor = Color.Transparent;
            bullet.SizeMode = PictureBoxSizeMode.StretchImage;
            bullet.Size = new Size(30, 20);
            bullet.Tag = bulletTag;
            bullet.Left = player.Left + player.Width -100 / 2;


            if ((string)bulletTag == "bullet")
            {
                bullet.Top = player.Top - 20;
            }
            else if ((string)bulletTag == "invaderBullet")
            {
                bullet.Top = -100;
            }
            
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }
    }
}
