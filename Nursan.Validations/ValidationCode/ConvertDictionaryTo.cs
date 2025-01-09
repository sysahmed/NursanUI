using System.Reflection;

namespace Nursan.Validations.ValidationCode
{
    public static class ConvertDictionaryTo<T>
    {

        public static T GetObject(IDictionary<string, object> d)
        {
            PropertyInfo[] props = typeof(T).GetProperties();
            T res = Activator.CreateInstance<T>();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanWrite && d.ContainsKey(props[i].Name))
                {
                    props[i].SetValue(res, d[props[i].Name], null);
                }
            }
            return res;
        }

        public static IDictionary<string, object> GetDictionary(T o)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            PropertyInfo[] props = typeof(T).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanRead)
                {
                    res.Add(props[i].Name, props[i].GetValue(o, null));
                }
            }
            return res;
        }

    }

}
