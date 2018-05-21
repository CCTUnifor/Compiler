import unittest
import io

from Core.Entities.Grammar import Grammar
from Core.Services.TableDescendentSintaticAnalyzer import TableService
from Core.Services.CodeGenerator import CodeGenerator

grammar_file_directory = "misc/grammars/Gramática "
grammar_name = "Tiny"
grammar_file_name = grammar_file_directory + grammar_name

with io.open(grammar_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()
    g = Grammar(fileTxt)

    compileGrammarService = TableService(g)
    compileGrammarService.compileGrammar()

class teste_code_generator(unittest.TestCase):

    def test_factorial_with_repeat(self):
        code = """
            PROGRAM
            VAR x : INTEGER;
            VAR y : INTEGER;
            VAR z : INTEGER;
            BEGIN
                READ x;
                IF x > 0 THEN
                    y := 1;
                    z := 1;
                    REPEAT
                    BEGIN
                        y := y * x;
                        x := x - z
                    END
                    UNTIL x = 0;
                    WRITE y
                END
            END
        """

        intermediate_code = [
                "0: IN 0,0,0",
                "2: LDC 1,1,0",
                "3: LDC 2,1,0",
                "4: MUL 1,1,0",
                "5: SUB 0,0,2",
                "6: JNE 0,-3(7)",
                "7: OUT 1,0,0",
                "1: JLE 0,6(7)",
                "8: HALT 0,0,0"
            ]
        tokens, historic = compileGrammarService.compile(code)
        generator = CodeGenerator(tokens)

        generator.compile()
        string_code = generator.intermediate_code

        self.assertListEqual(string_code, intermediate_code)
    
    def test_factorial_with_while(self):
        code = """
            PROGRAM
            VAR x : INTEGER;
            VAR y : INTEGER;
            VAR z : INTEGER;
            BEGIN
                READ x;
                IF x > 0 THEN
                    y := 1;
                    z := 1;
                    WHILE x != 0 DO
                    BEGIN
                        y := y * x;
                        x := x - z
                    END;
                    WRITE y
                END
            END
        """

        intermediate_code = [
                "0: IN 0,0,0",
                "2: LDC 1,1,0",
                "3: LDC 2,1,0",
                "4: MUL 1,1,0",
                "5: SUB 0,0,2",
                "6: JNE 0,-3(7)",
                "7: OUT 1,0,0",
                "1: JLE 0,6(7)",
                "8: HALT 0,0,0"
            ]
        tokens, historic = compileGrammarService.compile(code)
        generator = CodeGenerator(tokens)

        generator.compile()
        string_code = generator.intermediate_code
        print(intermediate_code)
        print()
        print(string_code)
        self.assertListEqual(string_code, intermediate_code)



if __name__ == '__main__':
    unittest.main()
    