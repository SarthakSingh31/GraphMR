using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathParser
{
	private static readonly string[] validOperators = { "-" , "+", "/", "*", "^", "sin", "cos", "tan", "neg" };
	private static readonly string[] binaryOperators = { "-", "+", "/", "*", "^" };
	private static readonly string[] functions = { "sin", "cos", "tan", "neg" };

	public static Expression parse (string equationStr)
	{
		Stack<Expression> output = new Stack<Expression>();
		Stack<string> operators = new Stack<string>();

		while (!(equationStr.Length <= 0))
		{
			string token = getToken(equationStr);
			equationStr = equationStr.Substring(token.Length);
			if (char.IsDigit(token[0]))
			{
				output.Push(new ConstExpr(float.Parse(token)));
			} else if (token.ToLower()[0].Equals('x') || token.ToLower()[0].Equals('z'))
			{
				if (token.Length > 1)
				{
					throw new Exception($"function name {token} cannot begin with x or z");
				}
				output.Push(new VarExpr(token.ToLower()[0]));
			} else if (isFunction(token))
			{ // token is a function
				operators.Push(token);
			} else if (isBinary(token))
			{ // token is an binary
				if (token.Length > 1)
				{
					throw new ArgumentException($"function name {token} cannot begin with symbol");
				}
				while ((operators.Count > 0) && tokenIsLowPrecedence(operators.Peek(), token))
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
		return output.Pop();
	}

	private static Expression applyStackOperation(Stack<string> operators, Stack<Expression> output)
	{
		string op = operators.Pop();
		
		if (isBinary(op))
		{
			// operator is binary
			Expression rightExpr = output.Pop();
			Expression leftExpr = output.Pop();
			switch (op)
			{
				case "+":
					return new Operator((left, right) => left + right, leftExpr, rightExpr, "+");
					break;
				case "^":
					return new Operator((left, right) => (float)Math.Pow(left, right), leftExpr, rightExpr, "^");
					break;
				case "*":
					return new Operator((left, right) => left * right, leftExpr, rightExpr, "*");
					break;
				case "-":
					return new Operator((left, right) => left - right, leftExpr, rightExpr, "-");
					break;
				case "/":
					return new Operator((left, right) => left / right, leftExpr, rightExpr, "/");
					break;
				default:
					throw new ArgumentException($"operator {op} not supported");
			}
		}
		else
		{
			Expression childExpr = output.Pop();
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

	private static bool tokenIsLowPrecedence(string stackOp, string tokenOp)
	{
		bool funcAtTop = isFunction(stackOp); //false
		bool opAtTop = isBinary(stackOp); //true
		bool isLowerPrecedence = Array.IndexOf(validOperators, tokenOp) < Array.IndexOf(validOperators, stackOp); // 2 < 0 -> false
		bool isNotLeftParen = !stackOp.Equals("("); // true 
		bool ret = (funcAtTop || // thing at top of stack is function
				(opAtTop && isLowerPrecedence)) &&
				isNotLeftParen; // (false || (true && false)) && true -> false
		return ret;
	}

	private static bool isOperator(string token)
	{
		return Array.Exists(validOperators, el => el.Equals(token));
	}

	private static bool isBinary(string token)
	{
		return Array.Exists(binaryOperators, el => el.Equals(token));
	}

	private static bool isFunction(string token)
	{
		return Array.Exists(functions, (func) => func.Equals(token));
	}

	public static string getToken(string eqStr)
	{
		int tokenLength = 1;

		if (char.IsLetter(eqStr[0])) {
			int i = 0;
			while (i < eqStr.Length && char.IsLetter(eqStr[i]))
			{
				i++;
			}
			tokenLength = i;
		} else if (char.IsDigit(eqStr[0]))
		{
			int i = 0;
			while (i < eqStr.Length && (char.IsDigit(eqStr[i]) || eqStr[i].Equals('.')))
			{
				i++;
			}
			tokenLength = i;
		}

		string token = eqStr.Substring(0, tokenLength);
		return token;
	}
}
