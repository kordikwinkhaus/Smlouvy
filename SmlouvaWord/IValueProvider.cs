namespace SmlouvaWord
{
    interface IValueProvider
    {
        bool GetValue(string name, out string result);
    }
}
