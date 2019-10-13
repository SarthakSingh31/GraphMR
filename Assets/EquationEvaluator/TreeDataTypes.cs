using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Expression
{
    public Expression left;
    public Expression right;
    public abstract float eval();
}

public abstract class Function : Expression
{

}

public abstract class Operator : Expression
{

}
