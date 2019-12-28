using LanguageExt;
using static LanguageExt.Prelude;
using System;

namespace ComonadPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var xs = new Seq<int>(new[] { 0, 1, 2, 3, 4, 5 });

            var gr = from g in FocusedGrid<int>.Create(new Seq<Seq<int>>(new[] { xs, xs, xs }))
                     from m in g.SeekS(2,1)
                     select m;

            gr.Match(
                Some: x => Grid(x),
                None: () => { Console.WriteLine("Invalid Grid"); return unit; });            

            var ne = NonEmptyList<int>.Create(xs);

            ne.Match(
                Some: x => 
                            {
                                Console.WriteLine(x);
                                var y = x.Duplicate();
                                Console.WriteLine(y.ToString());
                                var z = x.Extend(NonEmptyList<int>.TakeS(3));
                                z.Iter(a => Console.WriteLine("[" + string.Join(',', a)+ "]"));
                                var foo = z.Extend(b => b.Extract().Sum() / 3.0);
                                foo.Iter(a => Console.WriteLine(a));
                            },
                None: () => Console.WriteLine("FAILED"));

        }

        public static Unit Grid(FocusedGrid<int> grid)
        {
            Console.WriteLine(grid.ToString());
            
            var neighbours = new Seq<(int, int)>(new[] { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) });

            var foo = neighbours.Map(x => grid.PeekS(x.Item1, x.Item2));

            Console.WriteLine(string.Join(' ', foo.Somes()));


            return unit;
        }

    }
}
