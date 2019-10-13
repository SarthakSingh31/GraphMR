using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Expression
{
    public abstract float eval(float x, float z);
}

public class Function : Expression
{
	public Func<float, float> operation;
	public Expression child;

	public Function(Func<float, float> operation, Expression child)
	{
		this.operation = operation;
		this.child = child;
	}

	public override float eval(float x, float z)
	{
		return operation(child.eval(x, z));
	}

	public override string ToString()
	{
		return $"Func({child.ToString()})";
	}
}

public class Operator : Expression
{
	public Func<float, float, float> operation;
	public Expression left;
	public Expression right;

	public Operator(Func<float, float, float> operation, Expression left, Expression right) {
		this.operation = operation;
		this.left = left;
		this.right = right;
	}

	public override float eval(float x, float z)
	{
		return operation(left.eval(x, z), right.eval(x, z));
	}

	public override string ToString()
	{
		return $"Op({left.ToString()}, {right.ToString()})";
	}
}

public class ConstExpr : Expression
{
	private float value;

	public ConstExpr(float val)
	{
		value = val;
	}

	public override float eval(float x, float z)
	{
		return value;
	}

	public override string ToString()
	{
		return value.ToString();
	}
}

public class VarExpr : Expression
{
	private char var;

	public VarExpr(char var)
	{
		this.var = var;
	}

	public override float eval(float x, float z)
	{
		if (var.Equals('x'))
		{
			return x;
		} else if (var.Equals('z'))
		{
			return z;
		}

		throw new Exception("unsupporeted var, must be either x or z");
	}

	public override string ToString()
	{
		return var.ToString();
	}
}