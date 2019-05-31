namespace LabelTool.LabelItems
{
    public class Shell<T>
    {
        protected T BaseObject;

        public Shell(T baseObject)
        {
            BaseObject = baseObject;
        }


        public override string ToString()
        {
            return BaseObject.ToString();
        }
        /*
        public T Parse(string str)
        {
            MethodInfo mi = typeof(T).GetMethod("Parse", new Type[1]{ typeof(string) });

            if (mi != null)
                return (T)mi.Invoke(null, new object[] { str });

            return Activator.CreateInstance<T>();
        }
        */
        public static implicit operator T(Shell<T> shell)
        {
            return shell.BaseObject;
        }
    }
}
