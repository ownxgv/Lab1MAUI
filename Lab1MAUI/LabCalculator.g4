grammar LabCalculator;

/*
 * Parser Rules
 */
compileUnit : expression EOF;

expression
    : LPAREN expression RPAREN                                       # ParenthesizedExpr
    | expression EXPONENT expression                                 # ExponentialExpr
    | expression operatorToken=(MULTIPLY | DIVIDE | MOD | DIV) expression # MultiplicativeExpr
    | expression operatorToken=(ADD | SUBTRACT) expression           # AdditiveExpr
    | '++' expression                                                # PreIncrementExpr 
    | '--' expression                                                # PreDecrementExpr   
    | expression '++'                                                # PostIncrementExpr  
    | expression '--'                                                # PostDecrementExpr  
    | NUMBER                                                         # NumberExpr
    | IDENTIFIER                                                     # IdentifierExpr
    ;

/*
 * Lexer Rules
 */
NUMBER      : INT ('.' INT)?;
IDENTIFIER  : [a-zA-Z]+[1-9][0-9]+;
INT         : ('0'..'9')+;
MULTIPLY    : '*';
DIVIDE      : '/';
SUBTRACT    : '-';
ADD         : '+';
EXPONENT    : '^';
LPAREN      : '(';
RPAREN      : ')';
COMMA       : ',';
MOD         : '%';
DIV         : '//';
INCREMENT   : '++';
DECREMENT   : '--';
WS          : [ \t\r\n]+ -> skip;
