namespace MyCompiler.Core.Interfaces
{
    public interface IToken<T>
    {
        string Value { get; }
        T GrammarClass { get; }
        int Line { get; }
        int? Collumn { get; }

        void ConcatValue(string value);
    }
}