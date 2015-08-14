using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ToracLibrary.Core.ReflectionDynamic
{

    /// <summary>
    /// Retrieve custom attributes at runtime using reflection
    /// </summary>
    public static class AttributeHelpers
    {

        #region Find Attribute

        /// <summary>
        /// Find a custom attribute in a property based on a property name passed in as a string
        /// </summary>
        /// <typeparam name="T">Type Of The Custom Attribute To Grab</typeparam>
        /// <param name="ObjectType">Object Type. The Type Of The Object Which Contains The (Property Name) PropertyNameToCheckOffOfObject</param>
        /// <param name="PropertyNameToCheckOffOfObject">Property Name To Grab The Custom Attribute Off Of</param>
        /// <returns>Custom Attribute. Will Return Null If Not Found</returns>
        public static T FindAttributeInPropertyName<T>(Type ObjectType, string PropertyNameToCheckOffOfObject) where T : Attribute
        {
            //grab the object's properties that you passed in
            PropertyInfo PropertyWeWereLookingFor = ObjectType.GetProperty(PropertyNameToCheckOffOfObject);

            //make sure we have that property
            if (PropertyWeWereLookingFor == null)
            {
                //throw the error because we can't find the property
                throw new NullReferenceException($"Can't Find Property Named: {PropertyNameToCheckOffOfObject} In Object Of Type: {ObjectType.Name}");
            }

            //we have the property, use the helper method to find the itme
            return FindAttribute<T>((MemberInfo)PropertyWeWereLookingFor);
        }

        /// <summary>
        /// Find An Attribute Type
        /// </summary>
        /// <typeparam name="T">Type Of The Attribute You Are Trying To Look For</typeparam>
        /// <param name="ItemsPropertyInfo">Property Info</param>
        /// <returns>Custom Attribute</returns>
        /// <remarks>Uses the generic type that you specify for the generic method / Returns null</remarks>
        public static T FindAttribute<T>(PropertyInfo ItemsPropertyInfo) where T : Attribute
        {
            //use the overload Field Info and Property Info Derive From Member Info
            return FindAttribute<T>((MemberInfo)ItemsPropertyInfo);
        }

        /// <summary>
        /// Find An Attribute Type
        /// </summary>
        /// <typeparam name="T">Type Of The Attribute You Are Trying To Look For</typeparam>
        /// <param name="FieldInfoToGetAttributeOffOf">Field Info To Get The Attribute Off Of</param>
        /// <returns>Custom Attribute</returns>
        /// <remarks>Uses the generic type that you specify for the generic method / Returns null</remarks>
        public static T FindAttribute<T>(FieldInfo FieldInfoToGetAttributeOffOf) where T : Attribute
        {
            //use the overload Field Info and Property Info Derive From Member Info
            return FindAttribute<T>((MemberInfo)FieldInfoToGetAttributeOffOf);
        }

        /// <summary>
        /// Find An Attribute Type
        /// </summary>
        /// <typeparam name="T">Type Of The Attribute You Are Trying To Look For</typeparam>
        /// <param name="MemberInfoToGetAttributeOffOf">Member Info To Get The Attribute Off Of</param>
        /// <returns>Custom Attribute</returns>
        /// <remarks>Uses the generic type that you specify for the generic method / Returns null</remarks>
        public static T FindAttribute<T>(MemberInfo MemberInfoToGetAttributeOffOf) where T : Attribute
        {
            //this gets unit tested in Enums_UnitTest GetDescription_Test1 & GetDescription_Test2

            //cast type of T
            Type TType = typeof(T);

            //try to grab the custom attribute
            Attribute ResultOfSearchInProperty = MemberInfoToGetAttributeOffOf.GetCustomAttribute(TType);

            //if we didn't find it just go return null
            if (ResultOfSearchInProperty == null)
            {
                //never found it
                return null;
            }

            //we found the custom attribute, cast it and return it
            return (T)Convert.ChangeType(ResultOfSearchInProperty, TType);
        }

        #endregion

        #region Get all the properties that have TAttribute defined. Will only return the properties that have it defined, won't return the actual attribute value

        /// <summary>
        /// Get all the properties that have TAttribute as a custom attribute. Does not look at the actual attribute value. Just that it exists
        /// </summary>
        /// <typeparam name="TAttributeType">Attribute type to find</typeparam>
        /// <param name="ObjectType">Object type to look through each property to see if it has TAttribute type declared as an attribute on that property</param>
        /// <param name="IncludeInheritItems">Include inherited items?</param>
        /// <returns>All the properties that have the attribute declared on that specific property info</returns>
        public static IEnumerable<PropertyInfo> PropertiesThatHasAttributeDefinedLazy<TAttributeType>(Type ObjectType, bool IncludeInheritItems) where TAttributeType : Attribute
        {
            //cache the type of TAttribute
            Type TypeOfT = typeof(TAttributeType);

            //let's loop through each property info that has the object type defined
            foreach (PropertyInfo PropertyInfoToTest in ObjectType.GetProperties().Where(x => x.IsDefined(TypeOfT, IncludeInheritItems)))
            {
                //it's defined, return it
                yield return PropertyInfoToTest;
            }
        }

        /// <summary>
        /// Get all the properties that have TAttribute as a custom attribute. Does not look at the actual attribute value. Just that it exists
        /// </summary>
        /// <typeparam name="TAttributeType">Attribute type to find</typeparam>
        /// <typeparam name="TObjectType">Object type to look through each property to see if it has TAttribute type declared as an attribute on that property</typeparam>
        /// <param name="IncludeInheritItems">Include inherited items?</param>
        /// <returns>All the properties that have the attribute declared on that specific property info</returns>
        public static IEnumerable<PropertyInfo> PropertiesThatHasAttributeDefinedLazy<TObjectType, TAttributeType>(bool IncludeInheritItems)
            where TAttributeType : Attribute
            where TObjectType : class
        {
            //use the overload
            return PropertiesThatHasAttributeDefinedLazy<TAttributeType>(typeof(TObjectType), IncludeInheritItems);
        }

        #endregion

        #region Get all the properties that have TAttribute. Will return the property and the attribute value (slower then PropertiesThatHasAttributeLazy. Only use if you need the attribute value)

        /// <summary>
        /// Grab all the properties that contain this attribute and return the property info and the attribute value
        /// </summary>
        /// <typeparam name="TAttributeType">Attribute type to find</typeparam>
        /// <param name="ObjectType">Object type to look through its properties</param>
        /// <param name="IncludeInheritItems">Include inherited items?</param>
        /// <returns>ienumerable of a tuple of property info and the attribute.</returns>
        /// <remarks>Lazy loads the result using yield return</remarks>
        public static IEnumerable<KeyValuePair<PropertyInfo, TAttributeType>> PropertiesThatHasAttributeWithAttributeValueLazy<TAttributeType>(Type ObjectType, bool IncludeInheritItems) where TAttributeType : Attribute
        {
            //let's loop through each property info
            foreach (PropertyInfo PropertyInfoToTest in ObjectType.GetProperties())
            {
                //attempt to get the custom attribute
                var CustomAttributeAttemptToFind = PropertyInfoToTest.GetCustomAttribute<TAttributeType>(IncludeInheritItems);

                //did we find it
                if (CustomAttributeAttemptToFind != null)
                {
                    //we found the attribute, return the pair
                    yield return new KeyValuePair<PropertyInfo, TAttributeType>(PropertyInfoToTest, CustomAttributeAttemptToFind);
                }
            }
        }

        /// <summary>
        /// Grab all the properties that contain this attribute and return the property info and the attribute value
        /// </summary>
        /// <typeparam name="TAttributeType">Attribute type to find</typeparam>
        /// <typeparam name="TObjectType">Object type to look through its properties</typeparam>
        /// <param name="IncludeInheritItems">Include inherited items?</param>
        /// <returns>ienumerable of a tuple of property info and the attribute.</returns>
        /// <remarks>Lazy loads the result using yield return</remarks>
        public static IEnumerable<KeyValuePair<PropertyInfo, TAttributeType>> PropertiesThatHasAttributeWithAttributeValueLazy<TObjectType, TAttributeType>(bool IncludeInheritItems)
            where TAttributeType : Attribute
            where TObjectType : class
        {
            //use the overload
            return PropertiesThatHasAttributeWithAttributeValueLazy<TAttributeType>(typeof(TObjectType), IncludeInheritItems);
        }

        #endregion

    }

}
