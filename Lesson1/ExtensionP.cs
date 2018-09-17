using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1
{
    static class ExtensionP
    {
        public static IEnumerable ToArray(this PhoneBook souce)
        {
            string[] array = new string[souce.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = souce[i].ToString();
            }
            return array;
        }
    }
}
