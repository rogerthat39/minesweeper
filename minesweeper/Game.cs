using System;
using System.Collections.Generic;
using System.Drawing; //in order to use Color class
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //in order to use Button class

namespace minesweeper
{
    class Game
    {
        private bool isGameStillGoing; //true if game is running, false if game has been won or lost

        public bool IsGameStillGoing { get => isGameStillGoing; set => isGameStillGoing = value; }

        public Game()
        {
            isGameStillGoing = true;
        }

        public bool CheckWinCondition(List<Button> buttonsList)
        {
            //go through all buttons in buttonsList
            for (int i = 0; i < buttonsList.Count; i++)
            {
                //if there are any non-mine buttons that haven't been pressed, win condition not met
                if (buttonsList[i].Name != "X" && buttonsList[i].BackColor != Color.White)
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
            IsGameStillGoing = false;
        }

        public void GameLost(List<Button> buttonsList, int[] mines, Button faceButton)
        {
            ButtonManager bm = new ButtonManager();

            //show position of all mines (excluding those already marked) 
            //and show any flags that were put in the wrong place
            for (int i = 0; i < buttonsList.Count; i++)
            {
                if (mines.Contains(i))
                {
                    //check if there is a flag on the current button
                    if (buttonsList[i].Image == null)
                    {
                        //show the position of a hidden mine
                        bm.ClickButton(buttonsList[i]);
                    }
                    //if there's already a flag there (image not null), it's in the correct place, so should not be changed
                }
                else if (buttonsList[i].Image != null)
                {
                    //if square doesn't contain a bomb and there is a flag there, show the flag was incorrect
                    buttonsList[i].Image = Image.FromFile("not_bomb.png");
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
            IsGameStillGoing = false; //so that you can't click any other mines after the game is lost
        }
    }
}
