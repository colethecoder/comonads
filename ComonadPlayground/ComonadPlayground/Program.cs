using LanguageExt;
using static LanguageExt.Prelude;
using System;
using System.Threading;
using Comonad.Types;

namespace ComonadPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var gr = from g in FocusedGrid<int>.Create(PentaDecathlon)
                     from b in Some(g.Map(x => x == 1))
                     select b;

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
            var i = 0;
            PrintGrid(grid, i);

            while (i<=120)
            {
                i++;
                grid = grid.Extend(GameOfLifeCell);
                PrintGrid(grid, i);
                Thread.Sleep(1000);
            }

            return unit;
        }

        private static bool GameOfLifeCell(FocusedGrid<bool> grid) =>
            grid.Extract() ? Seq(2, 3).Contains(CountLiveNeighbours(grid))
                           : CountLiveNeighbours(grid) == 3;

        private static int CountLiveNeighbours(FocusedGrid<bool> grid) =>
            neighbours.Map(x => grid.PeekS(x.Item1, x.Item2))
                      .Somes()
                      .Where(x => x)
                      .Count();

        private static Seq<(int, int)> neighbours =>
            new Seq<(int, int)>(new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) });

        private static Unit PrintGrid(FocusedGrid<bool> grid, int iteration)
        {
            Console.Clear();
            Console.WriteLine(grid.Map(x => x ? '#' : '.').ToString());
            Console.WriteLine($"\nIteration: {iteration}");
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

        private static int[][] PentaDecathlon =>
            new[] {
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 1, 1, 0, 1, 1, 1, 1, 0, 1, 1, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new [] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            };
    }
}
