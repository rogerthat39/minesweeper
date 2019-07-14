﻿using System;
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
        /// <summary>
        /// Place mines randomly in button grid, and give numbers to the buttons next to a mine
        /// </summary>
        /// <param name="mines">An array which contains the indexes of the mines in buttonsList</param>
        /// <param name="buttonsList">The list of all buttons</param>
        public void PlaceMines(int[] mines, List<Button> buttonsList)
        {
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

        /// <summary>
        /// Adds one to the given number, unless it's a mine
        /// </summary>
        /// <param name="a">The hidden value of a button (string)</param>
        /// <returns>The string with one added to it (or the mine)</returns>
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
                return a; //if it's a mine, just send back the same string "X"
            }
        }

        /// <summary>
        /// Return a list of indexes of all the buttons around the given index
        /// Takes into account if the index is on an edge - it won't return buttons on those sides
        /// </summary>
        /// <param name="index">The index number of the button</param>
        /// <returns>List of integers</returns>
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

        /// <summary>
        /// Clears numbers around 0's that are next to each other
        /// </summary>
        /// <param name="button">The button that was clicked</param>
        /// <param name="buttonsList">The list of buttons</param>
        public void ClickZeros(Button button, List<Button> buttonsList) 
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

        /// <summary>
        /// Change button style and reveal the hidden value (bomb or number)
        /// </summary>
        /// <param name="button">The button that was clicked</param>
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

        /// <summary>
        /// Toggle between flag icon, ?, and nothing (in that order)
        /// </summary>
        /// <param name="clickedButton">The button that was clicked</param>
        /// <returns>A value to change the minesToFind value based on whether a flag was
        /// added, taken away, or neither</returns>
        public int ToggleImage(Button clickedButton)
        {
            //if flag icon already there, take away the image and put ? icon in its place
            if (clickedButton.Image != null)
            {
                clickedButton.Image = null;
                clickedButton.Text = "?";
                return 1;
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
                return -1;
            }
            return 0;
        }
    }
}
