namespace MudBlazor;

public class DrawerResult
{
    public object Data { get; }
    public Type DataType { get; }
    public bool Cancelled { get; }

    protected internal DrawerResult(object data, Type resultType, bool cancelled)
    {
        Data = data;
        DataType = resultType;
        Cancelled = cancelled;
    }

    public static DrawerResult Ok<T>(T result) => Ok(result, default);

    public static DrawerResult Ok<T>(T result, Type dialogType) => new(result, dialogType, false);

    public static DrawerResult Cancel() => new(default, typeof(object), true);
}