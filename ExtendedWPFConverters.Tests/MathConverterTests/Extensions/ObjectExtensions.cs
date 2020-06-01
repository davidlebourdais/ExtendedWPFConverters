using System;

namespace EMA.ExtendedWPFConverters.Tests.Utils
{
    public static class ObjectExtensions
    {
        #region IsNumeric
        // From https://stackoverflow.com/questions/1130698/checking-if-an-object-is-a-number-in-c-sharp.

        /// <summary>
        /// Extension method, call for any object, eg "if (x.IsNumeric())..."
        /// </summary>
        public static bool IsNumeric(this object x) => (x==null ? false : IsNumeric(x.GetType()));

        /// <summary>
        /// Method where you know the type of the object.
        /// </summary>
        public static bool IsNumeric(Type type) => IsNumeric(type, Type.GetTypeCode(type));

        /// <summary>
        /// Method where you know the type and the type code of the object.
        /// </summary>
        public static bool IsNumeric(Type type, TypeCode typeCode) 
            => (typeCode == TypeCode.Decimal || (type.IsPrimitive && typeCode != TypeCode.Object && typeCode != TypeCode.Boolean && typeCode != TypeCode.Char));
        #endregion
    }
}
