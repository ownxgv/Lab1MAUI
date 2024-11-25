using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1MAUI
{
    class LabCalculatorVisitor : LabCalculatorBaseVisitor<double>
    {
        Dictionary<string, double> tableIdentifier = new Dictionary<string, double>();


        private double GetZeroElement(LabCalculatorParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabCalculatorParser.ExpressionContext>(0));
        }

        private double GetFirstElement(LabCalculatorParser.ExpressionContext context)
        {
            return Visit(context.GetRuleContext<LabCalculatorParser.ExpressionContext>(1));
        }
        public override double VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitNumberExpr(LabCalculatorParser.NumberExprContext context)
        {
            var result = double.Parse(context.GetText());
            

            return result;
        }

        
        public override double VisitIdentifierExpr(LabCalculatorParser.IdentifierExprContext context)
        {
            var result = context.GetText();
            double value;
        
            if (tableIdentifier.TryGetValue(result.ToString(), out value))
            {
                return value;
            }
            else
            {
                return 0.0;
            }
        }

        public override double VisitParenthesizedExpr(LabCalculatorParser.ParenthesizedExprContext context)
        {
            return Visit(context.expression());
        }

        public override double VisitExponentialExpr(LabCalculatorParser.ExponentialExprContext context)
        {
            var left = GetZeroElement(context);
            var right = GetFirstElement(context);

 
            return System.Math.Pow(left, right);
        }

        public override double VisitAdditiveExpr(LabCalculatorParser.AdditiveExprContext context)
        {
            var left = GetZeroElement(context);
            var right = GetFirstElement(context);

            if (context.operatorToken.Type == LabCalculatorLexer.ADD)
            {
                
                return left + right;
            }
            else 
            {
                
                return left - right;
            }
        }

        public override double VisitPreIncrementExpr(LabCalculatorParser.PreIncrementExprContext context)
        {
            var value = GetZeroElement(context);
            value += 1;                                
            return value;                                
        }

        public override double VisitPostIncrementExpr(LabCalculatorParser.PostIncrementExprContext context)
        {
            
            double value = GetZeroElement(context);
            value += 1;
            return value;                             
        }

        public override double VisitPreDecrementExpr(LabCalculatorParser.PreDecrementExprContext context)
        {
            var value = GetZeroElement(context);
            value -= 1;                                   
                                                          
            return value;                                 
        }

        public override double VisitPostDecrementExpr(LabCalculatorParser.PostDecrementExprContext context)
        {

            double value = GetZeroElement(context);
            value -= 1;
            return value;                              
        }
        public override double VisitMultiplicativeExpr(LabCalculatorParser.MultiplicativeExprContext context)
        {
            var left = GetZeroElement(context);
            var right = GetFirstElement(context);

            if (context.operatorToken.Type == LabCalculatorLexer.MULTIPLY)
            {

                return left * right;
            }
            else if (context.operatorToken.Type == LabCalculatorLexer.MOD)
            {

                return (int)left % (int)right;
            }
            else if (context.operatorToken.Type == LabCalculatorLexer.DIV)
            {
                return (int)(left / right);
            }
            else 
            {
                return left / right;
            }
        }

       
    }

}
