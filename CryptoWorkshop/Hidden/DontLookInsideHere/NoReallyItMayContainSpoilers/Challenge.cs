using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoWorkshop.Hidden
{
    public abstract class Challenge
    {
        private static bool usedCheck = false;

        public static void AssertCheckOnce()
        {
            if (usedCheck)
            {
                throw new Exception("You can only use this one once to check if you succeeded.");
            }

            usedCheck = true;
        }
    }
}
