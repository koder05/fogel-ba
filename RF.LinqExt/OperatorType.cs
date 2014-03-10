using System;

namespace RF.LinqExt
{
	public enum OperatorType
	{
		None,
		Equals,
		NotEquals,
		LessThan,
		MoreThan,
		LessOrEquals,
		MoreOrEquals,
		Like,
		NotLike,
		StartsWith,
		Between,
		IsTrue,
		IsFalse,
		IsNull,
		IsNotNull,
		In,
		EqualsWithNull,
        Query,
		And,
		Or,
		Top,
        Condition
	}
}