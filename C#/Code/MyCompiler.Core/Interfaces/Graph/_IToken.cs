namespace MyCompiler.Core.Interfaces.Graph
{
    public interface _IToken<T>
    {
        string Value { get; }
        T GrammarClass { get; }
        int Line { get; }
        int? Collumn { get; }

        void ConcatValue(string value);
    }
}