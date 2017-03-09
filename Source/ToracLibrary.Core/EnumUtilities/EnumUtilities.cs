using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ToracLibrary.Core.ExtensionMethods.StringExtensions;

namespace ToracLibrary.Core.EnumUtilities
{

    /// <summary>
    /// Utilities for enum's
    /// </summary>
    public static class EnumUtility
    {

        #region Get Values

        /// <summary>
        /// Retrieve all the members of an enum
        /// </summary>
        /// <typeparam name="T">Type Of The Enum</typeparam>
        /// <returns>IEnumerable of your enum's</returns>
        /// <remarks>Usage = var values = EnumUtil.GetValues &lt;Foos&lt;();</remarks>
        public static IEnumerable<T> GetValuesLazy<T>() where T : struct
        {
            //loop through the types
            foreach (var EnumType in Enum.GetValues(typeof(T)))
            {
                //return this value
                yield return (T)EnumType;
            }
        }

        #endregion

        #region Try Parse Methods

        /// <summary>
        /// Convert an string to an enum without having to cast it from an object. The code for this is clunky when having to write it inline. create this helper method for it
        /// </summary>
        /// <typeparam name="T">Type Of Enum To Cast Too</typeparam>
        /// <param name="InputToConvertToEnum">String Value To Convert To The Enum</param>
        /// <returns>NullableType Of Your Enum</returns>
        public static T? TryParseToNullable<T>(string InputToConvertToEnum) where T : struct
        {
            //use the overload
            return TryParseToNullable<T>(InputToConvertToEnum, false);
        }

        /// <summary>
        /// Convert an string to an enum without having to cast it from an object. The code for this is clunky when having to write it inline. create this helper method for it
        /// </summary>
        /// <typeparam name="T">Type Of Enum To Cast Too</typeparam>
        /// <param name="InputToConvertToEnum">String Value To Convert To The Enum</param>
        /// <param name="IgnoreCase">Ignore case when trying to convert</param>
        /// <returns>NullableType Of Your Enum</returns>
        public static T? TryParseToNullable<T>(string InputToConvertToEnum, bool IgnoreCase) where T : struct
        {
            //validate that this is an enum
            ValidateEnum<T>(false);

            //make sure the input value is filled out
            if (InputToConvertToEnum.HasValue())
            {
                //go try to parse the enum value...(true means it was converted, false means it failed)
                if (Enum.TryParse(InputToConvertToEnum, IgnoreCase, out T ConvertedEnum))
                {
                    //conversion completed succesfully...return the enum
                    return ConvertedEnum;
                }
            }

            //return null if we get to here
            return null;
        }

        #endregion

        #region Get Custom Attribute Off Of Enum Member

        /// <summary>
        /// Method will retrieve a customm attribute off of the enum passed in.
        /// the items in your enum.
        /// </summary>
        /// <typeparam name="T">Custom Attribute Type To Look For</typeparam>
        /// <param name="EnumValueToRetrieve">Enum Value To Retrieve The Attribute Off Of</param>
        /// <returns>Description Attribute Value Or Null If The Description Tag Is Not Found</returns>
        /// <remarks>Custom Attribute If Found. Otherwise Null</remarks>
        public static T CustomAttributeGet<T>(Enum EnumValueToRetrieve) where T : Attribute
        {
            //System.ComponentModel.DescriptionAttribute
            //[TestAttribute("Equals This Number")]
            //Equals = 1

            //grab the custom attributes now 
            return EnumFieldValueGet(EnumValueToRetrieve).GetCustomAttribute(typeof(T)) as T;
        }

        /// <summary>
        /// Returns if the custom attribute is found off of the enum value passed in
        /// </summary>
        /// <typeparam name="T">Custom Attribute Type To Look For</typeparam>
        /// <param name="EnumValueToCheck">Enum value to check if the attribute is specified on</param>
        /// <returns>if the attribute is specified on the enum value passed in</returns>
        public static bool HasCustomAttributeDefined<T>(Enum EnumValueToCheck) where T : Attribute
        {
            //return if the attibute is defined
            return EnumFieldValueGet(EnumValueToCheck).IsDefined(typeof(T));
        }

        /// <summary>
        /// Returns the field info for the given enum value.
        /// </summary>
        /// <param name="EnumValueToFetchFieldInfoFor">Enum value to get the field info for</param>
        /// <returns>FieldInfo for the enum passed in</returns>
        private static FieldInfo EnumFieldValueGet(Enum EnumValueToFetchFieldInfoFor)
        {
            //grab the type of the enum passed in, then grab the field info object from the enum value passed in
            return EnumValueToFetchFieldInfoFor.GetType().GetField(EnumValueToFetchFieldInfoFor.ToString());
        }

        #endregion

        #region Validate Method

        /// <summary>
        /// Validates T to ensure it's an enum. If ValidateIfEnumIsBitMaskFlag is set it will check to see if it's a bit mask. ie flags attribute it set
        /// </summary>
        /// <typeparam name="T">type of t or enum to check in</typeparam>
        /// <param name="ValidateIfEnumIsBitMaskFlag">If ValidateIfEnumIsBitMaskFlag is set it will check to see if it's a bit mask. ie flags attribute it set</param>
        /// <remarks>will raise an error if it fails</remarks>
        private static void ValidateEnum<T>(bool ValidateIfEnumIsBitMaskFlag) where T : struct
        {
            //first make sure T is an enum
            if (!typeof(T).IsEnum)
            {
                //throw the exception because we need an enum type
                throw new ArgumentException($"Type '{typeof(T).FullName}' isn't an enum.");
            }

            //now check to make sure it's a flag (bitmask flag)
            if (ValidateIfEnumIsBitMaskFlag && !Attribute.IsDefined(typeof(T), typeof(FlagsAttribute)))
            {
                throw new ArgumentException($"Type '{typeof(T).FullName}' doesn't have the 'Flags' attribute");
            }
        }

        #endregion

        #region Bit Masks

        #region Public Static Methods

        //example of how to create the bit mask enum
        //[Flags]
        //public enum Days : int
        //{
        //    None = 0,
        //    Sunday = 1,
        //    Monday = 2,
        //    Tuesday = 4,
        //    Wednesday = 8,
        //    Thursday = 16,
        //    Friday = 32,
        //    Saturday = 64
        //}

        #region Add Item

        #region Public Static Methods

        /// <summary>
        /// Takes a bit mask and add's to it and returns the updated enum value
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="WorkingEnumValue">The working enum. So the combination of all the enums that have been joined together</param>
        /// <param name="ValueToAdd">value to add to the working enum value</param>
        /// <returns>the new updated enum that the value to add and the working enum value have been merged into</returns>
        public static T BitMaskAddItem<T>(T WorkingEnumValue, T ValueToAdd) where T : struct
        {
            //validate that this is an enum and a bit mask
            ValidateEnum<T>(true);

            //add the logical or's together then parse it and return it
            return BitMaskAddItemHelper(WorkingEnumValue, ValueToAdd);
        }

        /// <summary>
        /// Takes a bit mask and add's to it and returns the updated enum value
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="WorkingEnumValue">The working enum. So the combination of all the enums that have been joined together</param>
        /// <param name="ValuesToAdd">values to add to the working enum value</param>
        /// <returns>the new updated enum that the value to add and the working enum value have been merged into</returns>
        public static T BitMaskAddItem<T>(T WorkingEnumValue, params T[] ValuesToAdd) where T : struct
        {
            //validate that this is an enum and a bit mask
            ValidateEnum<T>(true);

            //loop through all the values and keep adding them to the base item
            foreach (var EnumToAdd in ValuesToAdd)
            {
                //go add this enum to the list
                WorkingEnumValue = BitMaskAddItemHelper(WorkingEnumValue, EnumToAdd);
            }

            //go return the working enum value
            return WorkingEnumValue;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Takes a bit mask and add's to it and returns the updated enum value. Used privately so we don't need to validate it. If we want to add a bunch of items we don't want to validate on each loop
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="WorkingEnumValue">The working enum. So the combination of all the enums that have been joined together</param>
        /// <param name="ValueToAdd">value to add to the working enum value</param>
        /// <returns>the new updated enum that the value to add and the working enum value have been merged into</returns>
        private static T BitMaskAddItemHelper<T>(T WorkingEnumValue, T ValueToAdd) where T : struct
        {
            //add the logical or's together then parse it and return it
            return TryParseToNullable<T>((Convert.ToInt64(WorkingEnumValue) | Convert.ToInt64(ValueToAdd)).ToString()).Value;
        }

        #endregion

        #endregion

        #region Contains Values

        /// <summary>
        /// Check to see if the value to check for (bit mask) is in the working enum value. ie is part of the bit mask.
        /// </summary>
        /// <typeparam name="T">Value of the enum</typeparam>
        /// <param name="WorkingEnumValue">Working Enum Value To Look In For The ValueToCheckFor</param>
        /// <param name="ValueToCheckFor">Value To Check For In The Enum</param>
        /// <returns>True if it is in the enum. Ie is selected</returns>
        public static bool BitMaskContainsValue<T>(T WorkingEnumValue, T ValueToCheckFor) where T : struct
        {
            //validate that this is an enum and a bit mask
            ValidateEnum<T>(true);

            //go use the helper and return the result
            return BitMaskContainsValueHelper(WorkingEnumValue, ValueToCheckFor);
        }

        /// <summary>
        /// Check to see if any of the values passed in (bit mask) is in the working enum value. ie is part of the bit mask. Added this overload so you don't need to build up the bit mask in the calling code
        /// </summary>
        /// <typeparam name="T">Value of the enum</typeparam>
        /// <param name="WorkingEnumValue">Working Enum Value To Look In For The ValueToCheckFor</param>
        /// <param name="EnumValueToCheckFor">Array of Values To Check For In The Enum</param>
        /// <returns>True 1 of the enum values you are checking for is found. Ie is selected</returns>
        public static bool BitMaskContainsValue<T>(T WorkingEnumValue, params T[] EnumValueToCheckFor) where T : struct
        {
            //validate that this is an enum and a bit mask
            ValidateEnum<T>(true);

            //let's loop through all the params items and build up my enum to check
            foreach (var thisEnumValueToCheck in EnumValueToCheckFor)
            {
                //does the working bit mask value contain the enum we are checking for
                if (BitMaskContainsValueHelper(WorkingEnumValue, thisEnumValueToCheck))
                {
                    //it does contain the value, return true right away
                    return true;
                }
            }

            //we never found it...return false
            return false;
        }

        #endregion

        /// <summary>
        /// Returns all the selected flags in the working enum value.
        /// </summary>
        /// <typeparam name="T">Type of the enum</typeparam>
        /// <param name="WorkingEnumValue">Working enum value to look in</param>
        /// <returns>list of flags that are selected. Uses yield to chain. Use ToArray() to send the values to an array</returns>
        public static IEnumerable<T> BitMaskSelectedItemsLazy<T>(T WorkingEnumValue) where T : struct
        {
            //validate that this is an enum and a bit mask
            ValidateEnum<T>(true);

            //loop through all the possible enum values
            foreach (var thisEnumValue in GetValuesLazy<T>())
            {
                //check to see if it's part of the bit mask
                if (BitMaskContainsValueHelper(WorkingEnumValue, thisEnumValue))
                {
                    //it is part of the bit mask, return it
                    yield return thisEnumValue;
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Check to see if the value to check for (bit mask) is in the working enum value. ie is part of the bit mask.
        /// Helper method is used so we don't have to validate the enum each time when called inside a loop such as BitMaskSelectedItems
        /// </summary>
        /// <typeparam name="T">Value of the enum</typeparam>
        /// <param name="WorkingEnumValue">Working Enum Value To Look In For The ValueToCheckFor</param>
        /// <param name="ValueToCheckFor">Value To Check For In The Enum</param>
        /// <returns>True if it is in the enum. Ie is selected</returns>
        private static bool BitMaskContainsValueHelper<T>(T WorkingEnumValue, T ValueToCheckFor) where T : struct
        {
            //check to see if the value to check for is in the working enum value
            return (Convert.ToInt64(WorkingEnumValue) & Convert.ToInt64(ValueToCheckFor)) == Convert.ToInt64(ValueToCheckFor);
        }

        #endregion

        #endregion

    }

}
