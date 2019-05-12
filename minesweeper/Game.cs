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
    class Game
    {
        private bool isGameStillGoing; //true if game is running, false if game has been won or lost

        public bool IsGameStillGoing { get => isGameStillGoing; set => isGameStillGoing = value; }

        public Game()
        {
            isGameStillGoing = true;
        }

        public bool CheckWinCondition()
        {
            ButtonManager bm = new ButtonManager();
            for (int i = 0; i < bm.ButtonsList.Count; i++)
            {
                //if there are any non-mine buttons that haven't been pressed, win condition not met
                if (bm.ButtonsList[i].Name != "X" && bm.ButtonsList[i].BackColor != Color.White)
                {
                    return false;
                }
            }
            return true;
        }

        public void GameWon(Button faceButton)
        {
            MessageBox.Show("You win!", "Game won"); //win message
            faceButton.Image = Image.FromFile("face_win.png");
            IsGameStillGoing = false; //so that you can't click any other mines after the game is finished
        }

        public void GameLost(int[] mines, Button faceButton)
        {
            ButtonManager bm = new ButtonManager();

            //show position of all mines (excluding those already marked) 
            //and show any flags that were put in the wrong place
            for (int i = 0; i < bm.ButtonsList.Count; i++)
            {
                if (mines.Contains(i))
                {
                    //check if there is a flag on the current button
                    if (bm.ButtonsList[i].Image == null)
                    {
                        //show the position of a hidden mine
                        bm.ClickButton(bm.ButtonsList[i]);
                    }
                    //if there's already a flag there (image not null), it's in the correct place, so should not be changed
                }
                else if (bm.ButtonsList[i].Image != null)
                {
                    //if square doesn't contain a bomb and there is a flag there, show the flag was incorrect
                    bm.ButtonsList[i].Image = Image.FromFile("not_bomb.png");
                }
            }

            //play explosion noise
            System.Media.SoundPlayer player = new System.Media.SoundPlayer
            {
                SoundLocation = "bomb.wav"
            };
            player.Play();
            player.Dispose();

            faceButton.Image = Image.FromFile("face_lose.png");
            MessageBox.Show("You Lose", "Game Lost");
            IsGameStillGoing = false; //so that you can't click any other mines after the game is finished
        }
    }
}
