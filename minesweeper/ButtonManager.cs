using System;
using System.Collections.Generic;
using System.Drawing; //in order to use Color class
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; //in order to use Button class

namespace minesweeper
{
    class ButtonManager
    {
        public void PlaceMines(int mine_num, int[] mines, List<Button> buttonsList)
        {
            Random random = new Random();
            int newRandomNum;

            for (int i = 0; i < mine_num; i++)
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

        public string AddOneToNum(string a)
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
        public List<int> GetIndexOfSurroundingButtons(int index)
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

        public void ClickZeros(Button button, List<Button> buttonsList) //clears numbers around 0's that are next to each other
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
    }
}
