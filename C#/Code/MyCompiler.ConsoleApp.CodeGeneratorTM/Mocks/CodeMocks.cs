namespace MyCompiler.ConsoleApp.CodeGeneratorCMS.Mocks
{
    public static class CodeMocks
    {
        public static string Input1 => "PROGRAM  \n" +
                                       "VAR a : INTEGER ; \n" +
                                       "BEGIN \n" +
                                       "	READ ( a ) ;\n" +
                                       "	IF ( a > 10 ) THEN\n" +
                                       "		WRITE ( a )\n" +
                                       "	END\n" +
                                       "END\n";

        public static string Input2 => "PROGRAM\n" +
                                       "VAR x1 : INTEGER ;\n" +
                                       "BEGIN\n" +
                                       "	WHILE x1 < 100 DO\n" +
                                       "	BEGIN\n" +
                                       "		x1 := x1 + 1 ;\n" +
                                       "		WRITE ( x1 )\n" +
                                       "	END\n" +
                                       "END\n";

        public static string Input3 => "PROGRAM\n" +
                                       "VAR x1 : INTEGER ;\n" +
                                       "BEGIN\n" +
                                       "	REPEAT\n" +
                                       "	BEGIN\n" +
                                       "		x1 := x1 + 1 ;\n" +
                                       "		WRITE ( x1 )\n" +
                                       "	END\n" +
                                       "	UNTIL x1 < 100\n" +
                                       "END\n";

        public static string Input4 => "PROGRAM\n" +
                                       "VAR num : INTEGER ;\n" +
                                       "VAR max : INTEGER ;\n" +
                                       "VAR min : INTEGER ;\n" +
                                       "BEGIN\n" +
                                       "	READ ( num ) ;\n" +
                                       "	max := num ;\n" +
                                       "	min := num ;\n" +
                                       "	WHILE num != 999 DO\n" +
                                       "	BEGIN\n" +
                                       "		IF num > max THEN \n" +
                                       "			max := num \n" +
                                       "		END ;\n" +
                                       "		IF num < min THEN\n" +
                                       "			min := num\n" +
                                       "		END ;\n" +
                                       "		READ ( num )\n" +
                                       "	END ;\n" +
                                       "	WRITE ( max ) ;\n" +
                                       "	WRITE ( min ) \n" +
                                       "END\n";

        public static string Fatorial => "PROGRAM  \n" +
                                         "VAR r0 : INTEGER ; \n" +
                                         "VAR r1 : INTEGER ; \n" +
                                         "VAR r2 : INTEGER ; \n" +
                                         "BEGIN \n" +
                                         "	  READ ( r0 ) ;\n" +
                                         "	  IF ( r0 > 0 ) THEN\n" +
                                         "	  	  r1 := 1 ;\n" +
                                         "        r2 := 1\n" +
                                         "	  END;\n" +
                                         "    REPEAT\n" +
                                         "    BEGIN\n" +
                                         "        r1 := r1 * r0 ;\n" +
                                         "        r0 := r0 - r2\n" +
                                         "    END\n" +
                                         "    UNTIL r0 = 0\n" +
                                         "END\n";
    }
}
