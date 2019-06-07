using System;
using System.Collections.Generic;
using System.Drawing; //in order to use Color class
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //in order to use Button class

namespace minesweeper
{
    /// <summary>
    /// Contains information and methods for the game at large, including the win condition.
    /// </summary>
    /// <remarks>
    /// Contains a variable for keeping track of whether the game is running or not
    /// A method for checking if the game has been won or lost
    /// A method for what happens when the game is won
    /// A method for what happens when the game is lost
    /// </remarks>
    static class Game
    {
        public static bool CheckWinCondition(List<Button> ButtonsList)
        {
            for (int i = 0; i < ButtonsList.Count; i++)
            {
                //if there are any non-mine buttons that haven't been pressed, win condition not met
                if (ButtonsList[i].Name != "X" && ButtonsList[i].BackColor != Color.White)
                {
                    return false;
                }
            }
            return true;
        }

        public static void GameWon()
        {
            MessageBox.Show("You win!", "Game won"); //win message
            //stats?
        }

        public static void GameLost(List<Button> ButtonsList, int[] mines)
        {
            ButtonManager bm = new ButtonManager();

            //show position of all mines (excluding those already marked) 
            //and show any flags that were put in the wrong place
            for (int i = 0; i < ButtonsList.Count; i++)
            {
                if (mines.Contains(i))
                {
                    //check if there is a flag on the current button
                    if (ButtonsList[i].Image == null)
                    {
                        //show the position of a hidden mine
                        bm.ClickButton(ButtonsList[i]);
                    }
                    //if there's already a flag there (image not null), it's in the correct place, so should not be changed
                }
                else if (ButtonsList[i].Image != null)
                {
                    //if square doesn't contain a bomb and there is a flag there, show the flag was incorrect
                    ButtonsList[i].Image = Image.FromFile("not_bomb.png");
                }
            }

            //play explosion noise
            System.Media.SoundPlayer player = new System.Media.SoundPlayer
            {
                SoundLocation = "bomb.wav"
            };
            player.Play();
            player.Dispose();

            MessageBox.Show("You Lose", "Game Lost");
        }
    }
}
