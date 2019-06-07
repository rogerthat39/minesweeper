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
    /// Contains methods for creating and altering the grid of buttons that make up the minefield.
    /// </summary>
    class ButtonManager
    {
        private List<Button> buttonsList = new List<Button>();

        public List<Button> ButtonsList { get => buttonsList; set => buttonsList = value; }

        private int minesToFind;

        public int MinesToFind { get => minesToFind; set => minesToFind = value; }

        public void PlaceMines(int[] mines)
        {
            //reset minesToFind to the number of mines
            minesToFind = mines.Length;

            Random random = new Random();
            int newRandomNum;

            for (int i = 0; i < mines.Length; i++)
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
            for (int i = 0; i < buttonsList.Count(); i++)
            {
                if (buttonsList[i].Name == "X") //if current button is a mine
                {
                    //+1 to every button surrounding the current mine
                    List<int> indexList = GetIndexOfSurroundingButtons(i);

                    foreach (int j in indexList)
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

        public void ClickZeros(Button button) //clears numbers around 0's that are next to each other
        {
            List<Button> btnsWithValueZero = new List<Button>();
            btnsWithValueZero.Add(button); //first value is the current clicked button

            while (btnsWithValueZero.Count > 0)
            {
                //get index of current button, and indexes of buttons surrounding it
                int index = buttonsList.IndexOf(btnsWithValueZero[0]);
                List<int> surroundingIndexes = GetIndexOfSurroundingButtons(index);

                //click all buttons surrounding the button with value 0
                foreach (int i in surroundingIndexes)
                {
                    Button surroundingButton = buttonsList[i];

                    //if surrounding button is not clicked, and was not flagged by the player, click it
                    if (surroundingButton.BackColor == Color.Gray && surroundingButton.Image == null)
                    {
                        ClickButton(surroundingButton);

                        //if the value (name) of a surrounding btn is 0, add it to the list of btns to click
                        if (surroundingButton.Name == "0")
                        {
                            btnsWithValueZero.Add(surroundingButton);
                        }
                    }
                }
                //remove current button (first item in list) from list of buttons to click
                btnsWithValueZero.RemoveAt(0);
            }
        }

        public void ClickButton(Button button)
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

        public void ToggleImage(Button clickedButton)
        {
            //if flag icon already there, take away the image and put ? icon in its place
            if (clickedButton.Image != null)
            {
                clickedButton.Image = null;
                clickedButton.Text = "?";
                minesToFind++;
            }
            //if there's a ?, remove it to show a blank button
            else if (clickedButton.Text == "?")
            {
                clickedButton.Text = "";
            }
            //if there isn't an image, show the flag icon
            else if (clickedButton.BackColor == Color.Gray) //only put flag on buttons that are un-clicked
            {
                clickedButton.Image = Image.FromFile("flag.png");
                minesToFind--;
            }
        }
    }
}
