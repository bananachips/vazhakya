using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
	public class Utilities
	{
		static Random randomGen = new Random();
		public static void Shuffle<T>(ref List<T> itemList)
		{
			int length = itemList.Count;
			int lastElement = length;
			while (lastElement > 1)
			{
				int randomPos = randomGen.Next(0, lastElement - 1);
				//swap values
				T temp = itemList[lastElement - 1];
				itemList[lastElement - 1] = itemList[randomPos];
				itemList[randomPos] = temp;
				lastElement--;
			}
			
		}
	}
}
