using LanguageExt;
using static LanguageExt.Prelude;
using System;

namespace ComonadPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new[] {
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };

            var gr = from g in FocusedGrid<int>.Create(start)
                     from b in Some(g.Map(x => x == 1))
                     from m in b.SeekS(2,1)
                     select m;

            gr.Match(
                Some: x => Grid(x),
                None: () => { Console.WriteLine("Invalid Grid"); return unit; });

            var xs = new Seq<int>(new[] { 0, 1, 2, 3, 4, 5 });

            var ne = NonEmptyList<int>.Create(xs);

            ne.Match(
                Some: x => List(x),
                None: () => { Console.WriteLine("FAILED"); return unit; } );

        }

        public static Unit Grid(FocusedGrid<bool> grid)
        {
            PrintGrid(grid);

            var foo = neighbours.Map(x => grid.PeekS(x.Item1, x.Item2));

            Console.WriteLine(string.Join(' ', foo.Somes()));

            return unit;
        }

        private static Seq<(int, int)> neighbours =>
            new Seq<(int, int)>(new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) });

        private static Unit PrintGrid(FocusedGrid<bool> grid)
        {
            Console.WriteLine(grid.Map(x => x ? '#' : '.').ToString());
            return unit;
        }

        public static Unit List(NonEmptyList<int> list)
        {
            Console.WriteLine(list);
            var y = list.Duplicate();
            Console.WriteLine(y.ToString());
            var z = list.Extend(NonEmptyList<int>.TakeS(3));
            z.Iter(a => Console.WriteLine("[" + string.Join(',', a) + "]"));
            var foo = z.Extend(b => b.Extract().Sum() / 3.0);
            foo.Iter(a => Console.WriteLine(a));
            return unit;
        }

    }
}
