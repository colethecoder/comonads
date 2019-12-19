using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageExt.Prelude;

namespace ComonadPlayground
{
    public class NonEmptyList<A>
    {
        private readonly Seq<A> Values;
        
        private NonEmptyList(Seq<A> seq)
        {
            Values = seq;
        }

        public static Option<NonEmptyList<A>> Create(Seq<A> seq) =>
            seq.Match(
                Empty: () => None,
                Seq: xs => Some(new NonEmptyList<A>(seq)));

        public NonEmptyList<NonEmptyList<A>> Duplicate() =>
            new NonEmptyList<NonEmptyList<A>>(
                Values.Map((x, y) => new NonEmptyList<A>(Values.Skip(x)))
                      .ToSeq());

        public A Extract() =>
            Values.Head();

        public NonEmptyList<B> Extend<B>(Func<NonEmptyList<A>, B> f) =>
            new NonEmptyList<B>(Extend(Values, f));

        private static Seq<B> Extend<B>(Seq<A> values, Func<NonEmptyList<A>, B> f) =>
            values.Match(
                Empty: () => throw new Exception("Impossible"),
                Head:  x  => Seq1(f(new NonEmptyList<A>(Seq1(x)))),
                Tail:  (x, xs) => Seq1(f(new NonEmptyList<A>(Seq1(x)))).Concat(Extend(xs, f)));

        public NonEmptyList<B> Map<B>(Func<A, B> f) =>
            new NonEmptyList<B>(Values.Map(f));

        public Func<NonEmptyList<A>, A> IndexQuery(int ix) =>
            wa => wa.Index(ix);

        /// <summary>
        /// Really Unsafe
        /// </summary>
        /// <param name="ix"></param>
        /// <returns></returns>
        private A Index(int ix) =>
            Values.ElementAt(ix);

        public override string ToString() =>
            Values.Fold(string.Empty, (x,y) => x + y.ToString());
    }
}
