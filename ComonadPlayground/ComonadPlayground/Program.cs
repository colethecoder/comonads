using LanguageExt;
using System;

namespace ComonadPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            var xs = new Seq<int>(new[] { 0, 1, 2, 3, 4, 5 });

            var ne = NonEmptyList<int>.Create(xs);

            ne.Match(
                Some: x => 
                            {
                                Console.WriteLine(x);
                                var y = x.Duplicate();
                                Console.WriteLine(y.ToString());
                            },
                None: () => Console.WriteLine("FAILED"));

        }
    }
}
