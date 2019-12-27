using System;
using LanguageExt;
using static LanguageExt.Prelude;

namespace ComonadPlayground
{
    public class FocusedGrid<A>
    {
        private readonly Seq<Seq<A>> Grid;
        private readonly int X;
        private readonly int Y;

        private FocusedGrid(Seq<Seq<A>> grid, int x, int y) =>
            (Grid, X, Y) = (grid, x, y);

        public static Option<FocusedGrid<A>> Create(Seq<Seq<A>> grid) =>
            grid.Count > 0 &&
            grid.Distinct(x => x.Count).Count == 1
            ? Some(new FocusedGrid<A>(grid, 0, 0))
            : None;

        public FocusedGrid<FocusedGrid<A>> Duplicate() =>
            new FocusedGrid<FocusedGrid<A>>(
                Grid.Map(
                     (y, a) => a.Map(
                                (x, b) => new FocusedGrid<A>(Grid, x, y)).ToSeq()).ToSeq()
                      , 0, 0);

        public A Extract() =>
            Grid.ElementAt(Y).ElementAt(X);

        public FocusedGrid<B> Extend<B>(Func<FocusedGrid<A>, B> f) =>
            Duplicate().Map(f);

        public FocusedGrid<B> Map<B>(Func<A, B> f) =>
            new FocusedGrid<B>(
                    Grid.Map(y => y.Map(x => f(x)).ToSeq()).ToSeq(),
                    X, Y);

        public (int X, int Y) Pos => (X, Y);

        public Option<A> TakeX(int x) =>
            X + x >= 0 && X + x < Grid[0].Count
            ? Some(Grid.ElementAt(Y).ElementAt(x + X))
            : None;

        public override string ToString() =>
            string.Join('\n', Grid.Map(y => string.Join(' ', y.Map(x => x.ToString()))));
    }
}
