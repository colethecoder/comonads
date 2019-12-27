using LanguageExt;
using System;

namespace ComonadPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var xs = new Seq<int>(new[] { 0, 1, 2, 3, 4, 5 });

            var gr = FocusedGrid<int>.Create(new Seq<Seq<int>>(new[] { xs, xs, xs }));

            gr.Match(
                Some: x => Console.WriteLine(x.Map(y => y == 2 || y == 4 ? "#" : ".")), 
                None: () => Console.WriteLine("Invalid Grid"));

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
    }
}
