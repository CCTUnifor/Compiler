namespace MyCompiler.Core.Models.SyntacticAnalyzes
{
    public class Choice : RegEx
    {
        private RegEx thisOne;
        private RegEx thatOne;

        public Choice(RegEx term, RegEx regex)
        {
            thisOne = term;
            thatOne = regex;
        }
    }
}