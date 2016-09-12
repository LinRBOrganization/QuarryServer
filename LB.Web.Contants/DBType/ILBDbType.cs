using System;
using System.Collections.Generic;
using System.Text;

namespace LB.Web.Contants.DBType
{
	public interface ILBDbType 
	{
		object Value
		{
			get;
			set;
		}

		void SetValueWithObject( object value );

		string DBTypeName
		{
			get;
		}
	}

    public class LBDBType
    {
        public const string t_BigID = "t_BigID";
        public const string t_Bool = "t_Bool";
        public const string t_DTSmall = "t_DTSmall";
        public const string t_Float = "t_Float";
        public const string t_ID = "t_ID";
        public const string t_String = "t_String";
        public const string t_nText = "t_nText";
        public const string t_Decimal = "t_Decimal";
        public const string t_SmallID = "t_SmallID";
        public const string t_Byte = "t_Byte";
        public const string t_Image = "t_Image";
        public const string t_Table = "t_Table";
        public const string t_Object = "t_Object";

        public static ILBDbType GetILBDbType(string strDBTypeName)
        {
            switch (strDBTypeName)
            {
                case LBDBType.t_BigID:
                    return new t_BigID();
                    break;
                case LBDBType.t_Bool:
                    return new t_Bool();
                    break;
                case LBDBType.t_DTSmall:
                    return new t_DTSmall();
                    break;
                case LBDBType.t_Float:
                    return new t_Float();
                    break;
                case LBDBType.t_ID:
                    return new t_ID();
                    break;
                case LBDBType.t_String:
                    return new t_String();
                    break;
                case LBDBType.t_nText:
                    return new t_nText();
                    break;
                case LBDBType.t_Decimal:
                    return new t_Decimal();
                    break;
                case LBDBType.t_SmallID:
                    return new t_SmallID();
                    break;
                case LBDBType.t_Byte:
                    return new t_Byte();
                    break;
                case LBDBType.t_Image:
                    return new t_Image();
                    break;
                case LBDBType.t_Table:
                    return new t_Table();
                    break;
                case LBDBType.t_Object:
                    return new t_Object();
                    break;
            }
            return new t_String();
        }
    }

    
}
