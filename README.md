# Functional Programming with Context

Basic types usually allow you to define a value. For example `bool` in C# allows can be `true` or `false`. Often we need to say something more about the type than that. In Language-Ext there are a number of generics that wrap a type to add context. You might have a function that returns a value from some config, it could be a set or not set and if it is set it could be on or off, the function could have the signature:

```cs
public Option<bool> GetConfigItem(string key)
```

So in this instance we are trying to get a `bool` but the type we receive back also has the context of whether the config item actually existed or not. It can be `None`, `Some(true)` or `Some(false)`.

In Language-Ext Option has a Map function, this allows you to provide a function that acts on the wrapped value and transforms it in some way.

```cs
public Option<B> Map<B>(Func<A, B> f)
```

You can use it like:

```cs
var configValue = GetConfigItem("FOO");
var mappedValue = configValue.Map(x => !x);
```

In the example above we get a value from the config and then use map to negate the bool if it is set. If the value is `None` then it stay none otherwise `Some(true)` becomes `Some(false)` and `Some(false)` becomes `Some(true)`. Map allows us to write functions that act on the raw values but to run them within the context of the Option type. In Category Theory something that is mappable in this way is a Functor, I'm going to use Haskell notation to define a Functor (because Haskell is much simpler to define these mathematical concepts in):

```haskell
class Functor f where
    fmap :: (a -> b) -> f a -> f b
```

Haskell calls the Map function `fmap` and you can see it takes a function from type `a` to type `b` (`a -> b`) and a functor of type `a` (`f a`) and produces a functor of type `b` (`f b`). We can think of our C# Option example as similar to this:

`f a` is configValue with type `Option<bool>`

`a -> b` is `x => !x` with type `Func<bool, bool>`

`f b` is mappedValue with type `Option<bool>`


`Option` also provides a mechanism for composing multiple contextual functions together, `Bind`.

```cs
public Option<B> Bind<B>(Func<A, Option<B>> f)
```

