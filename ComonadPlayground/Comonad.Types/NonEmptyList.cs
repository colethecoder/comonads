using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using static LanguageExt.Prelude;

namespace Comonad.Types
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
            Duplicate().Map(f);

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

        public static Func<NonEmptyList<A>, Seq<A>> TakeS(int windowSize) => x =>
            x.Take(windowSize);

        public Seq<A> Take(int toTake) =>
            Values.Take(toTake);

        public override string ToString() =>
            Values.Fold(string.Empty, (x,y) => x + y.ToString());

        public Unit Iter(Action<A> a) =>
            Values.Iter(a);
    }
}
