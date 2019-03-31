using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //set up the initial game
            CreateButtons();
            PlaceMines();
        }

        //global variables
        int FIELD_SIZE = 16; //creates a 16 by 16 grid
        List<Button> buttonsList = new List<Button>();
        int[] mines = new int[40]; //position (index) of mines in buttonsList
        bool gameStillGoing = true; //will turn false if game is won or lost

        //custom methods
        private void CreateButtons()
        {
            //loops for each row
            for (int r = 0; r < FIELD_SIZE; r++)
            {
                //loops for each column
                for (int c = 0; c < FIELD_SIZE; c++)
                {
                    Button newButton = new Button();
                    newButton.Location = new Point(9 + 22 * c, 19 + 22 * r); //puts buttons in a row or column next to the previous button
                    newButton.Size = new Size(23, 23);
                    newButton.Name = "0";
                    newButton.FlatStyle = FlatStyle.Flat;
                    newButton.BackColor = Color.Gray;

                    buttonsList.Add(newButton); //will add buttons to list in the order they are created

                    gbxButtons.Controls.Add(newButton); //if you don't add controls to gbx, they won't show up on the screen
                    newButton.MouseUp += newButton_MouseUp; //creates a click event for the button
                }
            }
        }

        private void PlaceMines()
        {
            //placing mines
            Random random = new Random();
            int newRandomNum;
            int MINE_NUM = mines.Length; //defines the number of mines that will be used in this game

            for (int i = 0; i < MINE_NUM; i++)
            {
                newRandomNum = random.Next(0, 256); //picks a random number from 0 to 255

                //adds number to mines list if it's not in there already
                if (!mines.Contains(newRandomNum))
                {
                    mines[i] = newRandomNum;
                    buttonsList[newRandomNum].Name = "X"; //puts an X on the button that has a mine
                }
                else
                {
                    i--; //new random number is generated if there's already a mine in that position
                }
            }

            //giving numbers to buttons adjacent to a mine
            for(int i = 0; i < buttonsList.Count(); i++)
            {
                if(buttonsList[i].Name == "X") //if current button is a mine
                {
                    //+1 to every button surrounding the current mine
                    List<int> indexList = GetIndexOfSurroundingButtons(i);

                    foreach(int j in indexList)
                    {
                        buttonsList[j].Name = AddOneToNum(buttonsList[j].Name);
                    }
                }
            }
        }

        private string AddOneToNum(string a)
        {
            //if targeted value is not a mine, add one to it and send back a string
            if (a != "X")
            {
                int b = int.Parse(a) + 1;
                return b.ToString();
            }
            else
            {
                return a; //if it is a mine, just send back the same string "X"
            }
        }

        //return a list of indexes of all the buttons around the given index
        private List<int> GetIndexOfSurroundingButtons(int index)
        {
            //also needs to be changed for when there are different sized boards (that aren't 16)
            List<int> indexes = new List<int>();

            if (index == 0) //top left corner
            {
                //can only use right, bottom, and bottom right diagonal
                int[] indexArray = { index + 1, index + 16, index + 17 };
                indexes.AddRange(indexArray);
            }
            else if (index == 15) //top right corner
            {
                //can only use left, bottom, and bottom left diagonal
                int[] indexArray = { index - 1, index + 16, index + 15 };
                indexes.AddRange(indexArray);
            }
            else if (index == 240) //bottom left corner
            {
                //can only use right, top, and top right diagonal
                int[] indexArray = { index + 1, index - 16, index - 15 };
                indexes.AddRange(indexArray);
            }
            else if (index == 255) //bottom right corner
            {
                //can only use left, above, and top left diagonal
                int[] indexArray = { index - 1, index - 16, index - 17 };
                indexes.AddRange(indexArray);
            }
            else if (index < 16) //top row
            {
                //can only use right, left, below, bottom left diagonal, bottom right diagonal
                int[] indexArray = { index + 1, index - 1, index + 16, index + 15, index + 17 };
                indexes.AddRange(indexArray);
            }
            else if (index > 239) //bottom row
            {
                //can only use right, left, above, top right diagonal, top left diagonal
                int[] indexArray = { index + 1, index - 1, index - 16, index - 15, index - 17 };
                indexes.AddRange(indexArray);
            }
            else if (index % 16 == 0) //far left column
            {
                //can only use right, above, below, top right diagonal, bottom right diagonal
                int[] indexArray = { index + 1, index - 16, index + 16, index - 15, index + 17 };
                indexes.AddRange(indexArray);
            }
            else if (index % 16 == 15) //far right column
            {
                //can only use left, above, below, top left diagonal, bottom left diagonal
                int[] indexArray = { index - 1, index - 16, index + 16, index - 17, index + 15 };
                indexes.AddRange(indexArray);
            }
            else //not on any outer edge
            {
                //can use all sides
                //ie. right, left, above, below, top right diagonal, top left diagonal, bottom left diagonal, bottom right diagonal
                int[] indexArray = { index + 1, index - 1, index - 16, index + 16, index - 15, index - 17, index + 15, index + 17 };
                indexes.AddRange(indexArray);
            }
            return indexes;
        }

        private bool CheckWinCondition()
        {
            //go through all buttons in buttonsList
            for(int i=0; i<buttonsList.Count; i++)
            {
                //if there are any non-mine buttons that haven't been pressed, return false
                if (buttonsList[i].Name != "X" && buttonsList[i].BackColor != Color.White)
                {
                    return false;
                }
            }
            return true;
        }

        private void GameWon()
        {
            MessageBox.Show("You win!", "Game won"); //win message
            gameStillGoing = false; //make it so that you can't click any other mines after game has ended
        }

        private void GameLost()
        {
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
                        ClickButton(buttonsList[i]);
                    }
                    //if there's already a flag there (image not null), it's in the correct place, so should not be changed
                }
                else if (buttonsList[i].Image != null)
                {
                    //if square doesn't contain a bomb and there is a flag there, show the flag was incorrect
                    buttonsList[i].Image = Image.FromFile("not_bomb.png");
                }
            }

            //show exploding animation or make explosion noises
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "bomb.wav";
            player.Play();
            player.Dispose();

            //show player they have lost
            MessageBox.Show("You Lose", "Game Lost");

            //make it so that you can't click any other mines after the game is lost
            gameStillGoing = false;
        }

        private void ClickButton(Button button)
        {
            //changing how the clicked button looks
            button.BackColor = Color.White;

            //if the button is a mine, show the mine icon instead of the text
            if (button.Name == "X")
            {
                button.Image = Image.FromFile("bomb.png");
            }
            else //otherwise show the text
            {
                //showing the hidden value of the button
                button.Text = button.Name;
            }
        }

        private void ClickZeros(Button button) //clears numbers around 0's that are next to each other
        {
            List<Button> btnsWithValueZero = new List<Button>(); //where buttons with value '0' will be stored
            btnsWithValueZero.Add(button); //first value is the current clicked button

            while (btnsWithValueZero.Count > 0) //while the list is not empty
            {
                int index = buttonsList.IndexOf(btnsWithValueZero[0]); //index of first item in list of btns with '0' that was clicked
                List<int> surroundingIndexes = GetIndexOfSurroundingButtons(index); //index numbers of all buttons around current button

                //click all buttons surrounding the button with value 0
                foreach (int i in surroundingIndexes)
                {
                    Button surroundingButton = buttonsList[i];

                    //if surrounding button is not clicked, and was not flagged by the player, click it
                    if (surroundingButton.BackColor == Color.Gray && surroundingButton.Image == null)
                    {
                        ClickButton(surroundingButton);

                        //if the value (name) of a surrounding btn is 0, add it to the list of btns to click, otherwise don't add it
                        if (surroundingButton.Name == "0")
                        {
                            btnsWithValueZero.Add(surroundingButton);
                        }
                    }
                }
                //remove previously clicked button (first item in list) from list of buttons to click
                btnsWithValueZero.RemoveAt(0);
            }
        }

        //when any button on the 'mine field' is clicked with a right or left click
        private void newButton_MouseUp(object sender, MouseEventArgs e)
        {
            if (gameStillGoing)
            {
                //code from stackoverflow - https://stackoverflow.com/questions/14479143/what-is-the-use-of-object-sender-and-eventargs-e-parameters

                //object 'sender' is the clicked button, but since it is of type object (and not button) we can't apply button rules to it
                Button button = sender as Button; //changes sender to Button type                

                //if the right mouse button was clicked, toggle between flag icon, ?, and nothing (in that order)
                if (e.Button == MouseButtons.Right)
                {
                    //if flag icon already there, take away the image and put ? icon in its place
                    if (button.Image != null)
                    {
                        button.Image = null;
                        button.Text = "?";
                    }
                    //if there's a ?, remove it to show a blank button
                    else if(button.Text == "?")
                    {
                        button.Text = "";
                    }
                    //if there isn't an image, show the flag icon
                    else if (button.BackColor == Color.Gray) //only put flag on buttons that are un-clicked
                    {
                        button.Image = Image.FromFile("flag.png");
                    }
                }
                //if left mouse button clicked, display hidden value
                //if user has flagged button, ignore the left click
                else if(e.Button == MouseButtons.Left && button.Image == null)
                {
                    //change button style and display hidden value
                    ClickButton(button);

                    //additional tasks to complete if the button was a 0 or a mine
                    if (button.Name == "0")
                    {
                        ClickZeros(button); //make surrounding 0's (and numbers) also clicked
                    }
                    else if (button.Name == "X") //if a mine was clicked
                    {
                        GameLost();
                    }

                    //invoke win state if win condition is met
                    if (CheckWinCondition())
                    {
                        GameWon();
                    }
                }
            }
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            //clears and resets values on the buttons for the next game
            gameStillGoing = true;
            foreach (Button button in buttonsList)
            {
                button.Name = "0";
                button.Text = "";
                button.Image = null;
                button.BackColor = Color.Gray; //resets color back to normal
            }
            PlaceMines();
        }
    }
}
