/*
 * Name: Riley de Gans, Conner Warboy, Nathan Pereboom
 * Date: April 30th - May 17th
 * Description: A bootleg version fo jetpack joyride, rocket riot. Use the up arrow jey to fly, let go to fall, collect coins for score, avoid zappers and pause button in top right
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Media;
using System.IO;
namespace rocketRiotv2
{
    public class Player
    {
        private double xPosition, yPosition, xSpeed, ySpeed, gravity;
        private bool falling = true;
        private Canvas canvas = new Canvas();
        private Rectangle playerSprite = new Rectangle ();
        private Polygon hitBox = new Polygon();
        private ImageBrush spritefill;//Image for the player
        private BitmapImage bitmapImage;//Image file to use
        /// <summary>
        /// Description: Create a player object
        /// Auhtor: Riley de Gans
        /// </summary>
        /// <param name="xP"></param>
        /// <param name="yP"></param>
        /// <param name="xS"></param>
        /// <param name="yS"></param>
        public Player(double xP, double yP, double xS, double yS, Canvas c)
        {
            xPosition = xP;
            yPosition = yP;
            xSpeed = xS;
            ySpeed = yS;
            canvas = c;
            playerSprite.Height = 75;
            playerSprite.Width = 75;
        }
        /// <summary>
        /// Description: updates the x position, y position, x speed and y speed with some logic and keyboard input
        /// Author: Riley de Gans and Nathan Pereboom
        /// </summary>
        public void move()
        {       
            if (Keyboard.IsKeyDown(Key.Up))
            {
                if (falling)
                {
                    ySpeed = 0;
                    falling = false;
                }
                else
                {
                    ySpeed += 0.2;//change based on difficulty
                }
            }
            else
            {
                if (falling)
                {
                    ySpeed -= gravity;
                }
                else
                {
                    ySpeed = 0;
                    falling = true;
                }
            }
            if (falling)
            {
                if (yPosition + ySpeed > 30 )
                {
                    yPosition += ySpeed;
                }
                else
                {
                    yPosition = 30;
                }
            }
            else
            {
                if (yPosition + ySpeed < 450)
                {
                    yPosition += ySpeed;
                }
                else
                {
                    yPosition = 450;
                }
            }
            if (xPosition + xSpeed < 800)
            {
                xPosition += xSpeed;
            }
            else
            {
                xPosition = 800;
            }
        }
        /// <summary>
        /// Description: Physically moves the player on canvas from coordinates
        /// Author: Riley de Gans and Nathan Pereboom
        /// </summary>
        public void animate()
        {
            Canvas.SetBottom(playerSprite, yPosition);
            Canvas.SetLeft(playerSprite, xPosition);
            Canvas.SetBottom(hitBox, yPosition - 5);
            Canvas.SetLeft(hitBox, xPosition);
        }
        /// <summary>
        /// Description: Returns true if one of the points is within the players polygon
        /// Author: Riley de Gans
        /// </summary>
        /// <param name="hits"></param>
        /// <returns></returns>
        public bool intersectWith(PointCollection hits)
        {
            bool hitTrue = false;
            for (int i = 0; i < hits.Count; i++)
            {
                if (canvas.InputHitTest(hits[i]) == hitBox)
                {
                    hitTrue = true;
                }
            }
            return hitTrue;
        }
        /// <summary>
        /// Description: Return whether or not the player is past the side of the screen, if it is true the player is reset to the other side
        /// Author: Riley de Gans
        /// </summary>
        /// <returns></returns>
        public bool pastScreen()
        {
            if (xPosition == 800)
            {
                xPosition = 0;
                xSpeed += 0.8;
                gravity += 0.1;
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Description: Resets everything in the player
        /// Author: Riley de Gans
        /// </summary>
        public void reset()
        {
            try
            {
                canvas.Children.Remove(playerSprite);
                canvas.Children.Remove(hitBox);
            }
            catch{

            }
            falling = true;
            xSpeed = 6;
            ySpeed = 0;
            xPosition = 0;
            yPosition = 300;
            gravity = 0.4;
        }
        /// <summary>
        /// Description: Generates the player and adds it to the canvas with the values from the constructor
        /// Author: Riley de Gans
        /// </summary>
        public void generate()
        {

            //bitmapImage = new BitmapImage(new Uri("spriteFill2.png", UriKind.Relative));
            //spritefill = new ImageBrush(bitmapImage);
            hitBox = new Polygon();
            bitmapImage = new BitmapImage(new Uri("spriteFill2.png", UriKind.Relative));
            spritefill = new ImageBrush(bitmapImage);

            playerSprite.Fill = spritefill;
            canvas.Children.Add(playerSprite);
            Canvas.SetBottom(playerSprite, yPosition);
            Canvas.SetLeft(playerSprite, xPosition);

            //hitBox.Stroke = Brushes.Red;
            hitBox.StrokeThickness = 2;
            hitBox.FillRule = FillRule.EvenOdd;
            hitBox.Fill = Brushes.Transparent;
            StreamReader sr = new StreamReader("playerPoints.txt");
            List<Point> points = new List<Point>();
            while (!sr.EndOfStream)
            {
                string currentLine = sr.ReadLine();
                double xPosition, yPosition;
                double.TryParse(currentLine.Split(',')[0], out xPosition);
                double.TryParse(currentLine.Split(',')[1], out yPosition);
                Point point = new Point(xPosition, yPosition);

                /*
                 */
                //solution to polygons, put polygon behind the player and make it transparent
                //move the polygon with the player


                points.Add(point);
            }
            PointCollection myPointCollection = new PointCollection();
            for (int i = 0; i < points.Count; i++)
            {
                myPointCollection.Add(points[i]);
            }
            hitBox.Points = myPointCollection;
            canvas.Children.Add(hitBox);
            gravity = 0.4;
        }
    }
}
