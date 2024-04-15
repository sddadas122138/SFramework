
using System;
using System.Collections;

using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Cfg.Cm
{
}
    namespace Cfg.H
{
    public interface ICfg
    {
        int Id { get; }
        void Initialize(int idx, string line);
        Dictionary<string, object> Kvs { get; }
        object Get(string key);

        void PreInitialize(int idx, string line);
        void AfterInitialize();
    }

    public interface ICC
    {
        void Initialize(Dictionary<string, object> o);

    }

    public partial class CTypeName
    {
        /// <summary>
        /// 存储表类型和表数据位置对应关系
        /// </summary>
        public static Dictionary<Type, string> Tns { get; private set; }

        /// <summary>
        /// 刷新配置 类型和路径资源对应关系
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        public static void UpdateCfgTypePathIns(Type type, string path)
        {
            if (Tns == null)
                return;

            Tns[type] = path;
        }

        /// <summary>
        /// 配置文件名
        /// 配置文件行ID
        /// 配置文件字段
        /// </summary>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        //public static object GetCfg(string name, string id, string key)
        //{
        //    return GetCfg(name, id)?.Get(key);
        //}
        //public static object GetCfg(string name, int id, string key)
        //{
        //    return GetCfg(name, id)?.Get(key);
        //}
        //public static ICfg GetCfg(string name, int id)
        //{
        //    return Get(name, id);
        //}
        //public static ICfg GetCfg(string name, string id)
        //{
        //    return Get(name, int.Parse(id));
        //}
    }

    public partial class CParse
    {
        public static readonly char[] DataDelimiters = new char[] { '\t' };
        public static string EMPTY_JSON_ARRAY = "[]";

        public static string ReadString(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? v.ToString() : string.Empty;
        }

        public static string ReadString(object o)
        {
            string str = o.ToString();
         string   sstr= str.Trim();
            return sstr;
        }

        public static List<T> ReadArray<T>(Dictionary<string, object> o, string key) where T : class, ICC, new()
        {
            if (o != null && o.TryGetValue(key, out var v))
            {
                var arr = v as List<object>;

                if (arr != null && arr.Count > 0)
                {
                    var cnt = arr.Count;
                    var list = new List<T>(cnt);

                    for (int i = 0; i < cnt; i++)
                    {
                        var t = new T();
                        t.Initialize(arr[i] as Dictionary<string, object>);
                        list.Add(t);
                    }

                    return list;
                }
            }

            return new List<T>(0);
        }

        public static List<int> ReadArrayI(Dictionary<string, object> o, string key)
        {
            if (o != null && o.TryGetValue(key, out var v))
            {
                var arr = v as List<object>;

                if (arr != null && arr.Count > 0)
                {
                    var cnt = arr.Count;
                    var list = new List<int>(cnt);

                    for (int i = 0; i < cnt; i++)
                    {
                        list.Add(ReadInteger(arr[i].ToString()));
                    }

                    return list;
                }
            }

            return new List<int>(0);
        }

        public static List<float> ReadArrayF(Dictionary<string, object> o, string key)
        {
            if (o != null && o.TryGetValue(key, out var v))
            {
                var arr = v as List<object>;

                if (arr != null && arr.Count > 0)
                {
                    var cnt = arr.Count;
                    var list = new List<float>(cnt);

                    for (int i = 0; i < cnt; i++)
                    {
                        list.Add(ReadFloat(arr[i].ToString()));
                    }

                    return list;
                }
            }

            return new List<float>(0);
        }

        public static List<string> ReadArrayS(Dictionary<string, object> o, string key)
        {
            if (o != null && o.TryGetValue(key, out var v))
            {
                var arr = v as List<object>;

                if (arr != null && arr.Count > 0)
                {
                    var cnt = arr.Count;
                    var list = new List<string>(cnt);

                    for (int i = 0; i < cnt; i++)
                    {
                        list.Add(arr[i].ToString());
                    }

                    return list;
                }
            }

            return new List<string>(0);
        }

        public static long ReadLong(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? ReadLong(v.ToString()) : 0;
        }

        public static long ReadLong(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (long.TryParse(s, out var l))
                {
                    return l;
                }
                else
                {
                    return (long)ReadDouble(s);
                }
            }

            return 0;
        }

        public static int ReadInteger(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? ReadInteger(v.ToString()) : 0;
        }

        public static int ReadInteger(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                if (int.TryParse(s, out var i))
                {
                    return i;
                }
                else
                {
                    return (int)ReadFloat(s);
                }
            }

            return 0;
        }

        public static int ReadIntegerAlt(string s, int idx, int len)
        {
            if (s == null) { return 0; }

            var ret = 0;

            const int CharVal0 = 0x30;

            if (len > s.Length - idx) { len = s.Length - idx; }

            for (int i = idx; i < len; i++)
            {
                var charVal = s[i] - CharVal0;

                if (charVal >= 0 && charVal <= 9)
                {
                    ret = ret * 10 + charVal;
                }
                else
                {
                    break;
                }
            }

            return ret;
        }

        public static Dictionary<string, T> ReadDictionary<T>(object o) where T : class, ICC, new()
        {
            var dic = o as Dictionary<string, object>;

            if (dic != null && dic.Count > 0)
            {
                var cnt = dic.Count;
                var map = new Dictionary<string, T>(cnt);

                foreach (var kv in dic)
                {
                    var t = new T();
                    t.Initialize(kv.Value as Dictionary<string, object>);
                    map.Add(kv.Key, t);
                }

                return map;
            }

            return new Dictionary<string, T>(0);
        }

        public static Dictionary<string, object> ReadMap(string s)
        {
            return ToMap(s);
        }

        public static T ReadMap<T>(string s) where T : class, ICC, new()
        {
            var map = ToMap(s);
            var t = new T();
            t.Initialize(map);
            return t;
        }

        public static List<T> ReadArr<T>(string s) where T : class, ICC, new()
        {
            var arr = ToArray(s);

            if (arr != null && arr.Count > 0)
            {
                var cnt = arr.Count;
                var list = new List<T>(cnt);

                for (int i = 0; i < cnt; i++)
                {
                    var t = new T();
                    t.Initialize(arr[i] as Dictionary<string, object>);
                    list.Add(t);
                }

                return list;
            }

            return new List<T>(0);
        }

        public static List<int> ReadArrInt(string s)
        {
            var arr = ToArray(s);

            if (arr != null && arr.Count > 0)
            {
                var cnt = arr.Count;
                var list = new List<int>(cnt);

                for (int i = 0; i < cnt; i++)
                {
                    list.Add(int.Parse(arr[i].ToString()));
                }

                return list;
            }

            return new List<int>(0);
        }

        public static List<float> ReadArrFloat(string s)
        {
            var arr = ToArray(s);

            if (arr != null && arr.Count > 0)
            {
                var cnt = arr.Count;
                var list = new List<float>(cnt);

                for (int i = 0; i < cnt; i++)
                {
                    list.Add(ReadFloat(arr[i].ToString()));
                }

                return list;
            }

            return new List<float>(0);
        }

        public static List<string> ReadArrString(string s)
        {
            var arr = ToArray(s);

            if (arr != null && arr.Count > 0)
            {
                var cnt = arr.Count;
                var list = new List<string>(cnt);

                for (int i = 0; i < cnt; i++)
                {
                    list.Add(arr[i].ToString());
                }

                return list;
            }

            return new List<string>(0);
        }

        public static List<object> ReadArr(string s)
        {
            return ToArray(s);
        }

        public static double ReadDouble(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? ReadDouble(v.ToString()) : 0;
        }

        public static double ReadDouble(string s)
        {
            return (!string.IsNullOrEmpty(s) && double.TryParse(s, out var f)) ? f : 0;
        }

        public static float ReadFloat(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? ReadFloat(v.ToString()) : 0f;
        }

        public static bool ReadBoolean(string s)
        {
            return (!string.IsNullOrEmpty(s) && bool.TryParse(s, out var b)) ? b : false;
        }

        public static bool ReadBoolean(Dictionary<string, object> o, string key)
        {
            return (o != null && o.TryGetValue(key, out var v)) ? ReadBoolean(v.ToString()) : false;
        }

        public static float ReadFloat(string s)
        {
            return (!string.IsNullOrEmpty(s) && float.TryParse(s, out var f)) ? f : 0f;
        }

        public static T ReadObject<T>(Dictionary<string, object> o, string key) where T : class, ICC, new()
        {
            if (o != null && o.TryGetValue(key, out var map))
            {
                var t = new T();
                t.Initialize(map as Dictionary<string, object>);
                return t;
            }

            return null;
        }

        private static List<object> ToArray(string s)
        {
            if (!string.IsNullOrEmpty(s) && EMPTY_JSON_ARRAY != s)
            {
                var list = MiniJSON.Deserialize(s) as List<object>;

                if (list != null)
                {
                    return list;
                }
            }

            return null;
        }

        private static Dictionary<string, object> ToMap(string s)
        {
           return MiniJSON.Deserialize(s) as Dictionary<string, object>;
        }
    }

    /// <summary>
    /// 配表继承的扩展基类
    /// </summary>
    public class TBaseConfigEx
    {
        /// <summary>
        /// 是否需要生成配置
        /// 需要的话，自己在配置类中更改此数据
        /// </summary>
        public virtual bool IsGenerateConfig { get; set; } = false;

        /// <summary>
        /// 生成新的配置信息数据
        /// </summary>
        /// <param name="id"></param>
        public virtual void GenerateConfig(int id) { }

        /// <summary>
        /// 生成新的配置信息数据
        /// </summary>
        /// <param name="arr"></param>
        public virtual void GenerateConfig(params object[] arr) { }
    }

    public class CH<T> where T : TBaseConfigEx, ICfg, new()
    {
        private static Dictionary<int, T> s_ConfigMap;
        private static List<T> s_ConfigList;

        protected static Dictionary<int, T> ConfigMap
        {
            get
            {
                if (s_ConfigMap == null) { Init(); }
                return s_ConfigMap;
            }
        }

        private static List<T> ConfigList
        {
            get
            {
                if (s_ConfigList == null) { s_ConfigList = new List<T>(ConfigMap.Values); }
                return s_ConfigList;
            }
        }
      
        private static void Init()
        {
            s_ConfigMap = new Dictionary<int, T>();
               var type = typeof(T);
          
            string path = CTypeName.Tns[type];
            path= Path.GetFileNameWithoutExtension(path);
            Debug.Log(path);
            TextAsset TA = Resources.Load<TextAsset>(path);

            string text = TA.ToString();
            var lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);


            for(int i=1;i< lines.Length; i++)
            {
                var cfg = new T();
                var icfg = cfg as ICfg;
                 lines[i] = lines[i].Replace(" ", "");
                icfg.PreInitialize(i - 1, lines[i]);
                   
                    if (!s_ConfigMap.ContainsKey(icfg.Id))
                    {
                        s_ConfigMap.Add(icfg.Id, cfg);
                        Debug.Log(s_ConfigMap.Count);
                    }
                    else
                    {
                        Debug.Log("哈哈");
                    }
                
             
            }
            

        }

        /// <summary>
        /// 反向初始化，清理数据
        /// 用于随机地图信息，在重新登陆的时候，可能切换了服务器，那么就需要清理数据信息
        /// </summary>
        public static void UnInit()
        {

            if (s_ConfigMap != null)
            {
                s_ConfigMap.Clear();
                s_ConfigMap = null;
            }

            if (s_ConfigList != null)
            {
                s_ConfigList.Clear();
                s_ConfigList = null;
            }
        }

        public static T I(int id)
        {
            //       if (ConfigMap.TryGetValue(id, out var cfg))
            //       {
            //           return cfg;
            //       }

            //       //如果取不到数据，那么默认创建一个空的数据
            //       cfg = new T();

            //       if (cfg.IsGenerateConfig)
            //       {
            //           cfg = new T();
            //           if (cfg.IsGenerateConfig)
            //           {
            //               cfg.GenerateConfig(id);

            //               //同一个数据只需要生成一次
            //            // 生成配置后，如果数据的Id和请求的不同，则视为生成数据失败
            //            if (cfg.Id != id)
            //{
            //                D.Error?.Log($" config [{typeof(T)}]: Generate data failed. Id = {id}");
            //                return null;
            //            }

            //               //同一个数据只需要生成一次
            //               //添加到缓存列表中
            //               //当在多线程处理的时候，有可能出现数组越界异常，为了安全所以添加lock，但是性能会降低一些
            //               lock(s_ConfigMap)
            //               {
            //                   s_ConfigMap.Add(cfg.Id, cfg);
            //               }
            //               return cfg;
            //           }
            //           else
            //           {
            //               D.Error?.Log($" config [{typeof(T)}]: Generate data failed. Id = {id}");
            //               return null;
            //           }

            //       }

            //       D.Error?.Log($" config [{typeof(T)}]: Data not found. Id = {id}");

            //       return null;

            if (ConfigMap.TryGetValue(id, out var cfg))
            {
                return cfg;
            }

            //如果取不到数据，那么默认创建一个空的数据
            //此处是不是要区分一下，哪些配表需要重新获取数据呢
            if (cfg == null)
                cfg = new T();


            if (cfg.IsGenerateConfig)
            {
                //D.Debug?.Log("CfgH id========{0}", id);

                try
                {
                    //同一个数据只需要生成一次
                    //添加到缓存列表中
                    cfg.GenerateConfig(id);

                }
                catch (System.Exception ex)
                {
                  //  D.Error?.Log($"load config error {ex}");
                    //显示加载错误提示？
                  //  Logo.OnLoadResFailAction();
                    return null;
                }

                // 生成配置后，如果数据的Id和请求的不同，则视为生成数据失败
                if (cfg.Id != id)
	            {
	               // D.Error?.Log($" config [{typeof(T)}]: Generate data failed. Id = {id}");
	                return null;
	            }
	
	            //当在多线程处理的时候，有可能出现数组越界异常，为了安全所以添加lock，但是性能会降低一些
	            lock (s_ConfigMap)
	            {
                    s_ConfigMap.Add(id, cfg);
                }
	            return cfg;
	        }
            

            if (id != 0)
            {
              //  D.Error?.Log($" config [{typeof(T)}]: Data not found. Id = {id}");
            }
            
            return null;

        }

        public static List<T> Is()
        {
            return new List<T>(ConfigList);
        }

        public static Dictionary<int, T> Im()
        {
            return new Dictionary<int, T>(ConfigMap);
        }

        public static Dictionary<int, T> RawDict()
        {
            return ConfigMap;
        }

        public static List<T> RawList()
        {
            return ConfigList;
        }

        /// <summary>
        /// 立刻解析所有行数
        /// </summary>
        public static void LoadAllLine()
        {
            if (ConfigMap == null) return;
            foreach (var item in ConfigMap)
            {
                item.Value.AfterInitialize();
            }
        }

    }


   public static class MiniJSON
        {
            public static string SafeGetString(Dictionary<string, object> dict, string key)
            {
                string value = string.Empty;

                if (dict != null && dict.ContainsKey(key))
                {
                    value = dict[key].ToString();
                }

                return value;
            }

            /// <summary>
            /// Parses the string json into a value
            /// </summary>
            /// <param name="json">A JSON string.</param>
            /// <returns>An List&lt;object&gt;, a Dictionary&lt;string, object&gt;, a double, an integer,a string, null, true, or false</returns>
            public static object Deserialize(string json)
            {
                // save the string for debug information
                if (json == null)
                {
                    return null;
                }

                return Parser.Parse(json);
            }

            sealed class Parser : IDisposable
            {
                public const int MAX_JSON_LENGTH = 100000;
                public const int MAX_PARSED_VALUE_COUNT = 100000;

                const string WORD_BREAK = "{}[],:\"";

                public static bool IsWordBreak(char c)
                {
                    return Char.IsWhiteSpace(c) || WORD_BREAK.IndexOf(c) != -1;
                }

                enum TOKEN
                {
                    NONE,
                    CURLY_OPEN,
                    CURLY_CLOSE,
                    SQUARED_OPEN,
                    SQUARED_CLOSE,
                    COLON,
                    COMMA,
                    STRING,
                    NUMBER,
                    TRUE,
                    FALSE,
                    NULL
                };

                StringReader json;
                int valueCount;

                Parser(string jsonString)
                {
                    if (jsonString.Length > MAX_JSON_LENGTH)
                    {
                        throw new OverflowException($"Input Json is too long. Json:{jsonString} Length:{jsonString.Length}");
                    }

                    json = new StringReader(jsonString);
                }

                public static object Parse(string jsonString)
                {
                    if (string.IsNullOrEmpty(jsonString))
                    {
                        return null;
                    }

                    using (var instance = new Parser(jsonString))
                    {
                        return instance.ParseValue();
                    }
                }

                public void Dispose()
                {
                    json.Dispose();
                    json = null;
                }

                Dictionary<string, object> ParseObject()
                {
                    Dictionary<string, object> table = new Dictionary<string, object>();

                    // ditch opening brace
                    json.Read();

                    // {
                    while (true)
                    {
                        switch (NextToken)
                        {
                            case TOKEN.NONE:
                                return null;
                            case TOKEN.COMMA:
                                continue;
                            case TOKEN.CURLY_CLOSE:
                                return table;
                            default:
                                // name
                                string name = ParseString();
                                if (name == null)
                                {
                                    return null;
                                }

                                // :
                                if (NextToken != TOKEN.COLON)
                                {
                                    return null;
                                }
                                // ditch the colon
                                json.Read();

                                // value
                                table[name] = ParseValue();
                                break;
                        }
                    }
                }

                List<object> ParseArray()
                {
                    List<object> array = new List<object>();

                    // ditch opening bracket
                    json.Read();

                    // [
                    var parsing = true;
                    while (parsing)
                    {
                        TOKEN nextToken = NextToken;

                        switch (nextToken)
                        {
                            case TOKEN.NONE:
                                return null;
                            case TOKEN.COMMA:
                                continue;
                            case TOKEN.SQUARED_CLOSE:
                                parsing = false;
                                break;
                            default:

                                if (TryParseByToken(nextToken, out var value))
                                {
                                    array.Add(value);
                                }
                                else
                                {
                                    json.Read();
                                }

                                break;
                        }
                    }

                    return array;
                }

                object ParseValue()
                {
                    TOKEN nextToken = NextToken;
                    TryParseByToken(nextToken, out var value);
                    return value;
                }

                bool TryParseByToken(TOKEN token, out object obj)
                {
                    obj = null;
                    var result = true;

                    valueCount++;

                    if (valueCount > MAX_PARSED_VALUE_COUNT)
                    {
                        throw new OverflowException("Internal Parser Error: Too many parsed values.");
                    }

                    switch (token)
                    {
                        case TOKEN.STRING:
                            obj = ParseString();
                            break;
                        case TOKEN.NUMBER:
                            obj = ParseNumber();
                            break;
                        case TOKEN.CURLY_OPEN:
                            obj = ParseObject();
                            break;
                        case TOKEN.SQUARED_OPEN:
                            obj = ParseArray();
                            break;
                        case TOKEN.TRUE:
                            obj = true;
                            break;
                        case TOKEN.FALSE:
                            obj = false;
                            break;
                        case TOKEN.NULL:
                            //obj = null;
                            break;
                        default:
                            //obj = null;
                            result = false;
                            break;
                    }

                    return result;
                }

                private StringBuilder _sb;
                string ParseString()
                {
                    if (_sb == null) { _sb = new StringBuilder(); }

                    _sb.Clear();

                    char c;

                    // ditch opening quote
                    json.Read();

                    bool parsing = true;

                    while (parsing)
                    {
                        if (json.Peek() == -1)
                        {
                            parsing = false;
                            break;
                        }

                        c = NextChar;
                        switch (c)
                        {
                            case '"':
                                parsing = false;
                                break;
                            case '\\':
                                if (json.Peek() == -1)
                                {
                                    parsing = false;
                                    break;
                                }

                                c = NextChar;
                                switch (c)
                                {
                                    case '"':
                                    case '\\':
                                    case '/':
                                        _sb.Append(c);
                                        break;
                                    case 'b':
                                        _sb.Append('\b');
                                        break;
                                    case 'f':
                                        _sb.Append('\f');
                                        break;
                                    case 'n':
                                        _sb.Append('\n');
                                        break;
                                    case 'r':
                                        _sb.Append('\r');
                                        break;
                                    case 't':
                                        _sb.Append('\t');
                                        break;
                                    case 'u':
                                        var hex = new char[4];

                                        for (int i = 0; i < 4; i++)
                                        {
                                            hex[i] = NextChar;
                                        }

                                        _sb.Append((char)Convert.ToInt32(new string(hex), 16));
                                        break;
                                }
                                break;
                            default:
                                _sb.Append(c);
                                break;
                        }
                    }

                    return _sb.ToString();
                }

                object ParseNumber()
                {
                    string number = NextWord;

                    if (number.IndexOf('.') == -1)
                    {
                        long parsedInt;
                        Int64.TryParse(number, out parsedInt);
                        return parsedInt;
                    }

                    double parsedDouble;
                    Double.TryParse(number, out parsedDouble);
                    return parsedDouble;
                }

                void EatWhitespace()
                {
                    while (json.Peek() != -1 && Char.IsWhiteSpace(PeekChar))
                    {
                        json.Read();
                    }
                }

                char PeekChar
                {
                    get
                    {
                        return Convert.ToChar(json.Peek());
                    }
                }

                char NextChar
                {
                    get
                    {
                        return Convert.ToChar(json.Read());
                    }
                }

                string NextWord
                {
                    get
                    {
                        StringBuilder word = new StringBuilder();

                        while (!IsWordBreak(PeekChar))
                        {
                            word.Append(NextChar);

                            if (json.Peek() == -1)
                            {
                                break;
                            }
                        }

                        return word.ToString();
                    }
                }

                TOKEN NextToken
                {
                    get
                    {
                        EatWhitespace();

                        if (json.Peek() == -1)
                        {
                            return TOKEN.NONE;
                        }

                        switch (PeekChar)
                        {
                            case '{':
                                return TOKEN.CURLY_OPEN;
                            case '}':
                                json.Read();
                                return TOKEN.CURLY_CLOSE;
                            case '[':
                                return TOKEN.SQUARED_OPEN;
                            case ']':
                                json.Read();
                                return TOKEN.SQUARED_CLOSE;
                            case ',':
                                json.Read();
                                return TOKEN.COMMA;
                            case '"':
                                return TOKEN.STRING;
                            case ':':
                                return TOKEN.COLON;
                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                            case '-':
                                return TOKEN.NUMBER;
                        }

                        switch (NextWord)
                        {
                            case "false":
                                return TOKEN.FALSE;
                            case "true":
                                return TOKEN.TRUE;
                            case "null":
                                return TOKEN.NULL;
                        }

                        return TOKEN.NONE;
                    }
                }
            }

            /// <summary>
            /// Converts a IDictionary / IList object or a simple type (string, int, etc.) into a JSON string
            /// </summary>
            /// <param name="json">A Dictionary&lt;string, object&gt; / List&lt;object&gt;</param>
            /// <returns>A JSON encoded string, or null if object 'json' is not serializable</returns>
            public static string Serialize(object obj)
            {
                return Serializer.Serialize(obj);
            }

            sealed class Serializer
            {
                StringBuilder builder;

                Serializer()
                {
                    builder = new StringBuilder();
                }

                public static string Serialize(object obj)
                {
                    var instance = new Serializer();

                    instance.SerializeValue(obj);

                    return instance.builder.ToString();
                }

                void SerializeValue(object value)
                {
                    IList asList;
                    IDictionary asDict;
                    string asStr;

                    if (value == null)
                    {
                        builder.Append("null");
                    }
                    else if ((asStr = value as string) != null)
                    {
                        SerializeString(asStr);
                    }
                    else if (value is bool)
                    {
                        builder.Append((bool)value ? "true" : "false");
                    }
                    else if ((asList = value as IList) != null)
                    {
                        SerializeArray(asList);
                    }
                    else if ((asDict = value as IDictionary) != null)
                    {
                        SerializeObject(asDict);
                    }
                    else if (value is char)
                    {
                        SerializeString(new string((char)value, 1));
                    }
                    else
                    {
                        SerializeOther(value);
                    }
                }

                void SerializeObject(IDictionary obj)
                {
                    bool first = true;

                    builder.Append('{');

                    foreach (object e in obj.Keys)
                    {
                        if (!first)
                        {
                            builder.Append(',');
                        }

                        SerializeString(e.ToString());
                        builder.Append(':');

                        SerializeValue(obj[e]);

                        first = false;
                    }

                    builder.Append('}');
                }

                void SerializeArray(IList anArray)
                {
                    builder.Append('[');

                    bool first = true;

                    foreach (object obj in anArray)
                    {
                        if (!first)
                        {
                            builder.Append(',');
                        }

                        SerializeValue(obj);

                        first = false;
                    }

                    builder.Append(']');
                }

                void SerializeString(string str)
                {
                    builder.Append('\"');

                    char[] charArray = str.ToCharArray();
                    foreach (var c in charArray)
                    {
                        switch (c)
                        {
                            case '"':
                                builder.Append("\\\"");
                                break;
                            case '\\':
                                builder.Append("\\\\");
                                break;
                            case '\b':
                                builder.Append("\\b");
                                break;
                            case '\f':
                                builder.Append("\\f");
                                break;
                            case '\n':
                                builder.Append("\\n");
                                break;
                            case '\r':
                                builder.Append("\\r");
                                break;
                            case '\t':
                                builder.Append("\\t");
                                break;
                            default:
                                int codepoint = Convert.ToInt32(c);
                                if ((codepoint >= 32) && (codepoint <= 126))
                                {
                                    builder.Append(c);
                                }
                                else
                                {
                                    builder.Append("\\u");
                                    builder.Append(codepoint.ToString("x4"));
                                }
                                break;
                        }
                    }

                    builder.Append('\"');
                }

                void SerializeOther(object value)
                {
                    // NOTE: decimals lose precision during serialization.
                    // They always have, I'm just letting you know.
                    // Previously floats and doubles lost precision too.
                    if (value is float)
                    {
                        builder.Append(((float)value).ToString("R"));
                    }
                    else if (value is int
                      || value is uint
                      || value is long
                      || value is sbyte
                      || value is byte
                      || value is short
                      || value is ushort
                      || value is ulong)
                    {
                        builder.Append(value);
                    }
                    else if (value is double
                      || value is decimal)
                    {
                        builder.Append(Convert.ToDouble(value).ToString("R"));
                    }
                    else
                    {
                        SerializeString(value.ToString());
                    }
                }
            }
        }
    
}
