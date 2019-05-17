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
using System.Windows.Threading;

namespace rocketRiotv2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Author: Riley, Conner and Nathan
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Player player;
        Random random = new Random();
        Zapper zappers;
        Coin[] coins = new Coin[4];
        Button btnStartGame = new Button();
        Menu menu;
        Label lblScore = new Label();
        int lastCollected;
        int score = 0;
        SoundPlayer sp;
        /// <summary>
        /// Logic that runs after intialize component
        /// Author: Conner
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            menu = new Menu(canvas, gameTimer, btnStartGame, lblScore);
            menu.generate();
            btnStartGame.Click += btnStartGame_Click;
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            player = new Player(0, 300, 6, 0, playerCanvas);
            zappers = new Zapper(canvas, random);
        }
        /// <summary>
        /// Description: Performs logic for every tick of the game, move the player, does all checks, does logic related to checks
        /// /// Author: Riley de Gans, Conner Warboy and Nathan Pereboom
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            player.move();
            player.animate();
            if (player.pastScreen())
            {
                zappers.generate();
                for (int i = 0; i < 4; i++)
                {
                    try
                    {
                        coins[i].remove();
                    }
                    catch
                    {

                    }
                    coins[i] = new Coin(i, canvas, random);
                    coins[i].generate();
                    lastCollected = -1;
                }
            }
            if (player.intersectWith(zappers.locations()))
            {
                menu.loseGame(score);
                gameTimer.Stop();
                player.reset();
                zappers.reset();
                for (int i = 0; i < 4; i++)
                {
                    coins[i].remove();
                }

                score = 0;
            }
            for (int i = 0; i < 4; i++)
            {
                if (player.intersectWith(coins[i].locations()))
                {
                    coins[i].remove();
                    if (lastCollected != i)
                    {
                        score++;
                        lblScore.Content = "Score: " + score;
                        lastCollected = i;
                    }
                }
            }
        }
        /// <summary>
        /// Description: Click event for start game, creates classes, sets up canvas
        /// Author: RIley de Gans and Conner warboys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            player.reset();
            zappers.generate();
            player.generate();
            for (int i = 0; i < 4; i++)
            {
                coins[i] = new Coin(i, canvas, random);
                coins[i].generate();
            }
            sp = new SoundPlayer("Rocket Man Soundtrack.wav");
            sp.PlayLooping();
            gameTimer.Start();
            menu.startGame();
        }
    }
}
