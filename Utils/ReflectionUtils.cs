using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace VirtualDesktopWidget.Utils;

static class ReflectionUtils
{
	public static Func<TObject, TField>? FieldGetter<TObject, TField>(
		string fieldName,
		BindingFlags bindingFlags = BindingFlags.Default)
	{
		if (!TryGetFieldMetadata<TObject>(
			fieldName,
			bindingFlags,
			out var objectParam,
			out var fieldInfo
		))
		{
			return null;
		}

		return Expression
			.Lambda<Func<TObject, TField>>(
				Expression.Field(objectParam, fieldInfo),
				true,
				objectParam
			)
			.Compile();
	}

	public static Action<TObject, TField>? FieldSetter<TObject, TField>(
		string fieldName,
		BindingFlags bindingFlags = BindingFlags.Default)
	{
		if (!TryGetFieldMetadata<TObject>(
			fieldName,
			bindingFlags,
			out var objectParam,
			out var fieldInfo
		))
		{
			return null;
		}

		var valueParam = Expression.Parameter(typeof(TField));

		return Expression
			.Lambda<Action<TObject, TField>>(
				Expression.Assign(
					Expression.Field(objectParam, fieldInfo),
					Expression.Convert(valueParam, fieldInfo.FieldType)
				),
				true,
				objectParam,
				valueParam
			)
			.Compile();
	}

	static bool TryGetFieldMetadata<TObject>(
		string fieldName,
		BindingFlags bindingFlags,
		[NotNullWhen(true)]
		out ParameterExpression? objectParam,
		[NotNullWhen(true)]
		out FieldInfo? fieldInfo
	)
	{
		var objectType = typeof(TObject);
		fieldInfo = objectType.GetField(fieldName, bindingFlags);

		if (fieldInfo is null)
		{
			objectParam = null;
			return false;
		}

		objectParam = Expression.Parameter(objectType);
		return true;
	}
}
