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

    def test_factorial_with_while(self):
        code = """
            PROGRAM
            VAR x : INTEGER;
            VAR fact : INTEGER;
            BEGIN
                READ x;
                IF 0<x THEN
                    fact := 1;
                    WHILE x <= 0 DO
                    BEGIN
                        fact :=fact*x;
                        x := x - 1
                    END;
                    WRITE fact
                END
            END
        """

        byte_stream = bytes(
            [
                0x4F, 0x00, 0x10, 0x5A, 0x0A, 0x00, 
                0x00, 0x00, 0x00, 0x00, 0x57, 0x41, 
                0x06, 0x00, 0x44, 0x00, 0x00, 0x40, 
                0x06, 0x00, 0x24, 0x5C, 0x43, 0x00, 
                0x44, 0x01, 0x00, 0x41, 0x08, 0x00, 
                0x40, 0x06, 0x00, 0x44, 0x00, 0x00, 
                0x25, 0x5C, 0x3F, 0x00, 0x40, 0x08, 
                0x00, 0x40, 0x06, 0x00, 0x03, 0x41, 
                0x08, 0x00, 0x40, 0x06, 0x00, 0x44, 
                0x01, 0x00, 0x02, 0x41, 0x06, 0x00, 
                0x5A, 0x1E, 0x00, 0x40, 0x08, 0x00, 
                0x58, 0x61
            ])
        tokens, historic = compileGrammarService.compile(code)
        generator = CodeGenerator(tokens)

        generator.compile()
        string_of_bytes = generator.bytecode

        self.assertEqual(string_of_bytes.hex(), byte_stream.hex())

    def test_factorial_with_repeat(self):
        code = """
            PROGRAM
            VAR x : INTEGER;
            VAR fact : INTEGER;
            BEGIN
                READ x;
                IF 0<x THEN
                    fact := 1;
                    REPEAT
                        BEGIN
                            fact :=fact*x;
                            x := x - 1
                        END
                    UNTIL x IS 0 ;
                    WRITE fact
                END
            END
        """

        byte_stream = bytes(
            [
                0x4F, 0x00, 0x10, 0x5A, 0x0A, 
                0x00, 0x00, 0x00, 0x00, 0x00, 
                0x57, 0x41, 0x06, 0x00, 0x44, 
                0x00, 0x00, 0x40, 0x06, 0x00, 
                0x24, 0x5C, 0x40, 0x00, 0x44, 
                0x01, 0x00, 0x41, 0x08, 0x00, 
                0x40, 0x08, 0x00, 0x40, 0x06, 
                0x00, 0x03, 0x41, 0x08, 0x00, 
                0x40, 0x06, 0x00, 0x44, 0x01, 
                0x00, 0x02, 0x41, 0x06, 0x00, 
                0x40, 0x06, 0x00, 0x44, 0x00, 
                0x00, 0x20, 0x5C, 0x1E, 0x00, 
                0x40, 0x08, 0x00, 0x58, 0x61
            ])
        tokens, historic = compileGrammarService.compile(code)
        generator = CodeGenerator(tokens)

        generator.compile()
        string_of_bytes = generator.bytecode

        self.assertEqual(string_of_bytes.hex(), byte_stream.hex())


if __name__ == '__main__':
    unittest.main()
    