# File Manager explanation
* The folder `Code/` contains the project (code)
    * Tiny
        * Startup = `Code/MyCompiler.Tiny/Program.cs` 
        * Code example txt = `Code/MyCompiler.Tiny/my-programm.txt` 
        * Token = `Code/MyCompiler.Core/Models/Tokens/TinyToken.cs` 
        * Lexical Analyze = `Code/MyCompiler.Core/Models/LexicalAnalyzes/TinyLexicalAnalyze.cs` 
        * Syntactic Analyze = `Code/MyCompiler.Core/Models/SyntacticAnalyzes/TinySyntacticAnalyzer.cs` 
    * Regular Expression (Thompson)
        * Startup = `Code/MyCompiler/Program.cs` 
        * Token = `Code/MyCompiler.Core/Models/Tokens/RegularExpressionToken.cs` 
        * Lexical Analyze = `Code/MyCompiler.Core/Models/LexicalAnalyzes/RegularExpressionLexicalAnalyzer.cs` 
        * Syntactic Analyze = `Code/MyCompiler.Core/Models/SyntacticAnalyzes/RegularExpressionSyntacticAnalyzer.cs` 
    * AnalisadorSintaticoDescendenteTabular
        * Startup = `Code/MyCompiler.AnalisadorSintaticoDescendenteTabular/Program.cs` 
        * Syntactic Analyze = `Code/MyCompiler.Core/Models/SyntacticAnalyzes/NRDSA/NonRecursiveDescendingSyntacticAnalysis.cs` 
* The folder `Executables/` contains the executable
    * Tiny `Executables/TinyApp`
        * .exe `Executables/TinyApp/MyCompiler.TinyApp.exe`
    * Regular Expression (Thompson) `Executables/RegularExpressionApp`
        * .exe `Executables/RegularExpressionApp/MyCompiler.RegularExpressionApp.exe`
    * AnalisadorSintaticoDescendenteTabular `Executables/AnalisadorSintaticoDescendenteTabular`
        * .exe `Executables/RegularExpressionApp/MyCompiler.AnalisadorSintaticoDescendenteTabular.exe`