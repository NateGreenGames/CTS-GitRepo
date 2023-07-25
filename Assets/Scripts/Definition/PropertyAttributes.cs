using UnityEngine;
using System;

public class NamedArrayAttribute : PropertyAttribute
{
    public Type TargetEnum;
    public NamedArrayAttribute(Type TargetEnum)
    {
        this.TargetEnum = TargetEnum;
    }

}
public class ReadOnlyAttribute : PropertyAttribute
{

}
