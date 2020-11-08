using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Core.Utils
{
    public class MiscUtils
    {
        public static T ParseEnum<T>(string name) {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static void Swap<T>(ref T a, ref T b) {
            T tmp = a;
            a = b;
            b = tmp;
        }
        private static Dictionary<int, int> Intersect(PropertyInfo[] first, PropertyInfo[] second)
        {
            var intermediateArray = new PropertyInfo[Math.Min(first.Length, second.Length)];
            Dictionary<int, int> aIndexb = new Dictionary<int, int>();
            int resultCount = 0;
            for (int index = 0; index < first.Length; index++)
            {
                bool found = false;
                int secondIndex;
                for (secondIndex = 0; secondIndex < second.Length; secondIndex++)
                {
                    if (first[index].Name == second[secondIndex].Name)
                    {
                        found = true;
                        break;
                    }
                }
                if (found)
                {
                    bool unique = true;
                    for (int resultIndex = 0; resultIndex < resultCount; resultIndex++)
                    {
                        if (first[index].Name == intermediateArray[resultIndex].Name)
                        {
                            unique = false;
                            break;
                        }
                    }
                    if (unique)
                    {
                        resultCount++;
                        intermediateArray[resultCount - 1] = first[index];
                        aIndexb.Add(index, secondIndex);
                    }
                }
            }
            
            return aIndexb;
        }

        public static void Swap<T1, T2>(ref T1 a, ref T2 b)
        {
            PropertyInfo[] ainfo = a.GetType().GetProperties();
            PropertyInfo[] binfo = b.GetType().GetProperties();

            var abinfo = Intersect(ainfo, binfo);
            if (abinfo.Count == 0)
            {
                return;
            }
            foreach (var el in abinfo)
            {
                int aIndex = el.Key;
                int bIndex = el.Value;

                var intemediateValue = Convert.ChangeType(ainfo[aIndex].GetValue(a), binfo[bIndex].PropertyType);
                ainfo[aIndex].SetValue(a, Convert.ChangeType(binfo[bIndex].GetValue(b), ainfo[aIndex].PropertyType));
                binfo[bIndex].SetValue(b, intemediateValue);
            }
        }
    }

}
