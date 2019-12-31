using LanguageExt;
using System;
using static LanguageExt.Prelude;

namespace Comonad.Types
{
    public class Store<S,A>
    {
        private readonly S Position;

        private readonly Map<S, A> Data;

        private Store(Map<S, A> data, S position) =>
            (Data, Position) = (data, position);

        public static Store<S, A> Create(Map<S, A> data, S position) =>
            new Store<S,A>(data, position);

        public Store<S, Store<S, A>> Duplicate() =>
            new Store<S, Store<S, A>>(
                toMap(Data.Map(x => (x.Key, new Store<S,A>(Data, x.Key)))), 
                Position);

        public A Extract() =>
            Data[Position];

        public Store<S, B> Extend<B>(Func<Store<S, A>, B> f) =>
            Duplicate().Map(f);

        public Store<S, B> Map<B>(Func<A, B> f) =>
            new Store<S, B>(Data.Map(f), Position);

        public S Pos() =>
            Position;

        public A Peek(S s) =>
            Data[s];

        public A PeekS(Func<S, S> f) =>
            Peek(f(Position));

        public Store<S, A> Seek(S s) =>
            new Store<S, A>(Data, s);

        public Store<S, A> SeekS(Func<S, S> f) =>
            Seek(f(Position));
    }
}
