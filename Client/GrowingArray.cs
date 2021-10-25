using System;
using System.Collections.Generic;
using System.Text;

namespace NotAGame
{
    class GrowingArray
    {
        private int[] intArray;
        private int currentPosition = 0;
        public GrowingArray()
        {
            intArray = new int[2];
        }
        public void Add(int intToAdd)
        {
            intArray[currentPosition] = intToAdd;
            currentPosition++;
            if (currentPosition >= intArray.Length)
            {
                //grow array
                int[] tempArray = new int[intArray.Length * 2];
                //copy the data to the new array
                for (int i = 0; i < intArray.Length; i++)
                {
                    tempArray[i] = intArray[i];
                }
                intArray = tempArray;
            }
        }
        public void Display()
        {
            foreach (int x in intArray)
            {
                Console.WriteLine(x);
            }
        }
        public int Length()
        {
            return intArray.Length;
        }
    }
}
