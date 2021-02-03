using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amongus_game_flow
{
    static public class RandomUtils
    {
        static public IEnumerable<T> random<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        static public IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }
    }
}
