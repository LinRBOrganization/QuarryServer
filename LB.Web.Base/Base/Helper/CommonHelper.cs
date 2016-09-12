using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace LB.Web.Base.Helper
{
    public class CommonHelper
    {
        public static SqlDbType GetSqlDbType(LBDbType dbType)
        {
            switch (dbType)
            {
                case LBDbType.String:
                    return SqlDbType.NVarChar;
                case LBDbType.Date:
                    return SqlDbType.Date;
                case LBDbType.DateTime:
                    return SqlDbType.SmallDateTime;
                case LBDbType.Double:
                case LBDbType.Decimal:
                    return SqlDbType.Decimal;
                case LBDbType.Int16:
                    return SqlDbType.SmallInt ;
                case LBDbType.Int32:
                    return SqlDbType.Int;
                case LBDbType.Int64:
                    return SqlDbType.BigInt;
                case LBDbType.Byte:
                    return SqlDbType.TinyInt;
                case LBDbType.Boolean:
                    return SqlDbType.Bit;
                case LBDbType.Object:
                    return SqlDbType.Image;
                case LBDbType.NText:
                    return SqlDbType.NText;
                case LBDbType.Binary:
                    return SqlDbType.Binary;
                default:
                    return SqlDbType.NVarChar;
            }
        }

        public static int GetSqlDbTypeSize(LBDbType dbType)
        {
            switch (dbType)
            {
                case LBDbType.String:
                    return 2000;
                case LBDbType.Date:
                    return 100;
                case LBDbType.DateTime:
                    return 100;
                case LBDbType.Double:
                case LBDbType.Decimal:
                    return 100;
                case LBDbType.Int16:
                    return 100;
                case LBDbType.Int32:
                    return 100;
                case LBDbType.Int64:
                    return 100;
                case LBDbType.Byte:
                    return 10;
                case LBDbType.Boolean:
                    return 1;
                case LBDbType.Object:
                    return 2000;
                case LBDbType.NText:
                    return 2000;
                case LBDbType.Binary:
                    return 5;
                default:
                    return 2000;
            }
        }

        public static byte GetSqlDbTypePrecision(LBDbType dbType)
        {
            switch (dbType)
            {
                case LBDbType.Double:
                case LBDbType.Decimal:
                case LBDbType.Int16:
                case LBDbType.Int32:
                case LBDbType.Int64:
                case LBDbType.Byte:
                case LBDbType.Boolean:
                case LBDbType.Binary:
                    return 10;
                default:
                    return 0;
            }
        }

        public static byte GetSqlDbTypeScale(LBDbType dbType)
        {
            switch (dbType)
            {
                case LBDbType.Double:
                case LBDbType.Decimal:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}