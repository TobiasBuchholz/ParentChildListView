using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<int>> GroupConsecutive(this IEnumerable<int> src) 
        {
            var more = false; 
            
            IEnumerable<int> ConsecutiveSequence(IEnumerator<int> csi) {
                int prevCurrent;
                do {
                    yield return (prevCurrent = csi.Current);
                } while((more = csi.MoveNext()) && csi.Current-prevCurrent == 1);
            }

            var si = src.GetEnumerator();
            if(si.MoveNext()) {
                do {
                    yield return ConsecutiveSequence(si).ToList();
                }
                while(more);
            }
        }
    }
}