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
    public class Coin
    {
        private int quadrant;
        private Random random;
        private Canvas canvas = new Canvas();
        private Rectangle coin = new Rectangle();
        private BitmapImage bitmapImage;
        private ImageBrush coinFill;
        /// <summary>
        /// Description: Creates instance of the coins object
        /// Author: Nathan Pereboom
        /// </summary>
        /// <param name="q"></param>
        /// <param name="c"></param>
        /// <param name="r"></param>
        public Coin(int q, Canvas c, Random r)
        {
            quadrant = q;
            canvas = c;
            random = r;
            //quadrant is not yet used
        }
        /// <summary>
        /// Description: Creates the class, adds it to the canvas and sets up hitboxes
        /// Author: Nathan Pereboom
        /// </summary>
        public void generate()
        {
            try
            {
                canvas.Children.Remove(coin);
            }
            catch
            {

            }
            //Image
            bitmapImage = new BitmapImage(new Uri("coin.png", UriKind.Relative));
            coinFill = new ImageBrush(bitmapImage);
            coin.Fill = coinFill;

            //Coin size
            coin.Height = 20;
            coin.Width = 20;

            //Set Position

            Canvas.SetLeft(coin, random.Next(180) + quadrant * 200);
            Canvas.SetTop(coin, 150 + random.Next(415));

            //Add to canvas
            canvas.Children.Add(coin);
        }
        /// <summary>
        /// Description: Removes and resets current coin
        /// Auhtor: Nathan Pereboom and Riley de Gans
        /// </summary>
        public void remove()
        {
            canvas.Children.Remove(coin);
        }
        /// <summary>
        /// Description: Returns the hitpoints to be used in intersection check in player class
        /// Author: Riley de Gans
        /// </summary>
        /// <returns></returns>
        public PointCollection locations()
        {
            PointCollection returnPoints = new PointCollection();
            double pointX = Canvas.GetLeft(coin);
            double pointY = Canvas.GetTop(coin);
            returnPoints.Add(new Point(pointX, pointY + 10));
            returnPoints.Add(new Point(pointX + 20, pointY + 10));
            returnPoints.Add(new Point(pointX + 10, pointY));
            returnPoints.Add(new Point(pointX + 10, pointY + 20));
            return returnPoints;
        }
    }
}