using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Serialization.JasonSerializer.PrimitiveTypes;

namespace ToracLibrary.Serialization.JasonSerializer
{

    /// <summary>
    /// A json serializer using expression tree's. More of a "can" i do it with expression trees.
    /// Currently tested with the following
    /// string and int properties off of an object
    /// singular sub object
    /// array sub object.
    /// 
    /// It currently is faster then json.net but not faster then JIL. Jil used straight IL so i will never get that fast
    /// </summary>
    public class JasonSerializerContainer
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public JasonSerializerContainer()
        {
            //Create the singular cache func. Used an object as a value because i need to way to be dynamic with all the different func types in a single collection
            SingleObjectCachedSerializer = new ConcurrentDictionary<string, object>();

            //cache for SerializeIEnumerable
            GenericSerializeIEnumerableCache = new ConcurrentDictionary<string, MethodInfo>();

            //go build the type lookup
            TypeLookup = new List<BasePrimitiveTypeOutput>
            {
                new StringPrimitiveTypeOutput(),
                new IntPrimitiveTypeOutput(),
                new DateTimePrimitiveTypeOutput()
            }.ToDictionary(x => x.TypeToOutput);
        }

        #endregion

        #region Instance Properties

        /// <summary>
        /// Create the singular cache func. Used an object as a value because i need to way to be dynamic with all the different func types in a single collection
        /// </summary>
        private ConcurrentDictionary<string, object> SingleObjectCachedSerializer { get; set; }

        /// <summary>
        /// Holds the caches for the method info when we go to use SerializeIEnumerable
        /// </summary>
        private ConcurrentDictionary<string, MethodInfo> GenericSerializeIEnumerableCache { get; set; }

        /// <summary>
        /// Holds the type lookup with it's rules
        /// </summary>
        private Dictionary<Type, BasePrimitiveTypeOutput> TypeLookup { get; }

        #endregion

        #region Public Instance Methods

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <typeparam name="T">Type to serialize</typeparam>
        /// <param name="ObjectToSerialize">object to serialize</param>
        /// <returns>string which is the json result of the serialization</returns>
        public string SerializeJson<T>(T ObjectToSerialize) where T : class
        {
            //throw type of t into a variable
            var TypeOfT = typeof(T);

            //if this is an ienumerable...we need to handle this differently from the root object
            if (SerializerTreeBuilder.IsEnumerableLikeType(TypeOfT))
            {
                //grab the generic type serializer
                return (string)GetRootLevelSerializeIEnumerable(SerializerTreeBuilder.ElementTypeInEnumerable(TypeOfT)).Invoke(this, new object[] { ObjectToSerialize });
            }

            //it's a regular object...go serialize this
            return GetSingleObjectSerializer<T>().Invoke(ObjectToSerialize, this, new StringBuilder()).ToString();
        }

        #endregion

        #region Private Instance Methods

        /// <summary>
        /// Get's the single object func serializer. Will either get it from the cache or will build it.
        /// </summary>
        /// <typeparam name="TClass">object type to retrieve.</typeparam>
        /// <returns>serializer func</returns>
        private Func<TClass, JasonSerializerContainer, StringBuilder, StringBuilder> GetSingleObjectSerializer<TClass>() where TClass : class
        {
            //grab the type of so we can grab the key
            var TypeOfT = typeof(TClass);

            //get the func try from the cache
            object CacheSerializerTryGet;

            //go try to grab the item from the cache
            if (!SingleObjectCachedSerializer.TryGetValue(TypeOfT.Name, out CacheSerializerTryGet))
            {
                //we don't have it in the cache...go build it
                var BuiltSerializer = SerializerTreeBuilder.SerializeBuilder<TClass>(TypeLookup).Compile();

                //add it to the cache
                SingleObjectCachedSerializer.TryAdd(TypeOfT.Name, BuiltSerializer);

                //return it
                return BuiltSerializer;
            }

            //we have it in the cache...go return it
            return ((Func<TClass, JasonSerializerContainer, StringBuilder, StringBuilder>)CacheSerializerTryGet);
        }

        /// <summary>
        /// Get's the SerializeIEnumerable from the cache or builds it. Caching so we don't have to call it over and over and rebuild the generic type
        /// </summary>
        /// <param name="ElementType">element of each element in the enumerable</param>
        /// <returns>method info</returns>
        private MethodInfo GetRootLevelSerializeIEnumerable(Type ElementType)
        {
            //try item to get from cache
            MethodInfo TryGetFromCache;

            //try to grab it from the cache
            if (GenericSerializeIEnumerableCache.TryGetValue(ElementType.Name, out TryGetFromCache))
            {
                //it's in the cache, return it
                return TryGetFromCache;
            }

            //we need to build the generic function
            var MethodInfo = typeof(JasonSerializerContainer).GetMethod(nameof(JasonSerializerContainer.SerializeIEnumerable), BindingFlags.NonPublic | BindingFlags.Instance).MakeGenericMethod(ElementType);

            //stick it in the cache
            GenericSerializeIEnumerableCache.TryAdd(ElementType.Name, MethodInfo);

            //return it now
            return MethodInfo;
        }

        #endregion

        #region Internal Serialization

        /// <summary>
        /// Serialization for IEnumerable. 
        /// </summary>
        /// <typeparam name="T">Type of each element in the array</typeparam>
        /// <param name="EnumerableToSerialize">enumerable to serialize</param>
        /// <returns>Json that was serialized from the enumerable</returns>
        internal string SerializeIEnumerable<T>(IEnumerable<T> EnumerableToSerialize) where T : class
        {
            //go grab the serializer for T.
            var SingleObjectSerializer = GetSingleObjectSerializer<T>();

            //create the string builder
            var JsonBuilder = new StringBuilder();

            //add the start of the array
            JsonBuilder.Append("[");

            //loop through each row and add to it
            foreach (var item in EnumerableToSerialize)
            {
                //add this row item
                SingleObjectSerializer(item, this, JsonBuilder).Append(",");
            }

            //remove the last comma
            JsonBuilder = JsonBuilder.Remove(JsonBuilder.Length - 1, 1);

            //tack on the end array item
            JsonBuilder.Append("]");

            //return the json value
            return JsonBuilder.ToString();
        }

        #endregion

    }

}
