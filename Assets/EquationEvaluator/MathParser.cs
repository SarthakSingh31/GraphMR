using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathParser
{
	private static readonly char[] validOperators = { '-' , '+', '/', '*', '^' };

    public static Expression parse (string equationStr)
	{
		MonoBehaviour.print("parsing");
		Stack<Expression> output = new Stack<Expression>();
		Stack<string> operators = new Stack<string>();

		while (!(equationStr.Length <= 0))
		{
			string token = getToken(equationStr);
			if (char.IsDigit(token[0]))
			{
				output.Push(new ConstExpr(float.Parse(token)));
			} else if (token[0].Equals('x') || token[0].Equals('z'))
			{
				if (token.Length > 1)
				{
					throw new Exception($"function name {token} cannot begin with x or z");
				}
				output.Push(new VarExpr(token[0]));
			} else if (char.IsLetter(token[0]))
			{ // token is a function
				operators.Push(token);
			} else if (isOperator(token))
			{ // token is an operator
				if (token.Length > 1)
				{
					throw new ArgumentException($"function name {token} cannot begin with symbol");
				}
				while (tokenIsLowPrecedence(operators.Peek()[0], token[0]))
				{
					output.Push(applyStackOperation(operators, output));
				}
				operators.Push(token);
			} else if (token.Equals("("))
			{
				if (token.Length > 1)
				{
					throw new ArgumentException($"function name {token} cannot begin with (");
				}
				operators.Push(token);
			} else if (token.Equals(")"))
			{
				if (token.Length > 1)
				{
					throw new ArgumentException($"function name {token} cannot begin with )");
				}
				while ((operators.Count > 0) && !operators.Peek()[0].Equals('('))
				{
					output.Push(applyStackOperation(operators, output));
				}
				if (operators.Count <= 0)
				{
					throw new ArgumentException("mismatched parenthesis");
				}
				if (operators.Peek().Equals("("))
				{
					operators.Pop();
				}
			}
		}
		while (operators.Count > 0)
		{
			output.Push(applyStackOperation(operators, output));
		}
		MonoBehaviour.print("Finished parsing");
		return output.Pop();
	}

	private static Expression applyStackOperation(Stack<string> operators, Stack<Expression> output)
	{
		string op = operators.Pop();
		
		if (isOperator(op))
		{
			// operator is binary
			Expression rightExpr = output.Pop();
			Expression leftExpr = output.Pop();
			MonoBehaviour.print($"Applying {op}({leftExpr.ToString()},{rightExpr.ToString()})");
			switch (op)
			{
				case "+":
					return new Operator((left, right) => left + right, leftExpr, rightExpr);
					break;
				case "^":
					return new Operator((left, right) => (float)Math.Pow(left, right), leftExpr, rightExpr);
					break;
				case "*":
					return new Operator((left, right) => left * right, leftExpr, rightExpr);
					break;
				case "-":
					return new Operator((left, right) => left - right, leftExpr, rightExpr);
					break;
				case "/":
					return new Operator((left, right) => left / right, leftExpr, rightExpr);
					break;
				default:
					throw new ArgumentException($"operator {op} not supported");
			}
		}
		else
		{
			Expression childExpr = output.Pop();
			MonoBehaviour.print($"Applying {op}({childExpr.ToString()})");
			switch (op)
			{
				case "sin":
					return new Function((child) => (float) Math.Sin(child), childExpr);
					break;
				case "cos":
					return new Function((child) => (float) Math.Cos(child), childExpr);
					break;
				case "tan":
					return new Function((child) => (float) Math.Tan(child), childExpr);
					break;
				case "neg":
					return new Function((child) => -1 * child, childExpr);
					break;
				default:
					throw new ArgumentException($"operator {op} not supported");
			}
		}
	}

	private static bool tokenIsLowPrecedence(char stackOp, char tokenOp)
	{
		bool funcAtTop = char.IsLetter(stackOp);
		bool opAtTop = char.IsSymbol(stackOp);
		bool isLowerPrecedence = Array.IndexOf(validOperators, tokenOp) <= Array.IndexOf(validOperators, stackOp);
		bool isNotLeftParen = !stackOp.Equals('(');
		return (funcAtTop || // thing at top of stack is function
				(opAtTop && isLowerPrecedence) &&
				isNotLeftParen);
	}

	private static bool isOperator(string token)
	{
		return Array.Exists(validOperators, el => el.Equals(token[0]));
	}

	private static string getToken(string eqStr)
	{
		int tokenLength = 1;
		if (char.IsLetterOrDigit(eqStr[0]))
		{
			//starts with either letter or digit
			if (char.IsLetter(eqStr[0])) {
				int i = 0;
				while (char.IsLetter(eqStr[i]))
				{
					i++;
				}
				tokenLength = i;
			}

			if (char.IsDigit(eqStr[0]))
			{
				int i = 0;
				while (char.IsDigit(eqStr[i]) || eqStr[i].Equals('.'))
				{
					i++;
				}
				tokenLength = i;
			}
		}
		string token = eqStr.Substring(0, tokenLength);
		MonoBehaviour.print($"Token: {token}");
		return token;
	}
}
