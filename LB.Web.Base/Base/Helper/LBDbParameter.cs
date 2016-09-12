using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace LB.Web.Base.Helper
{
	public class LBDbParameter : MarshalByRefObject
	{
		public string ParameterName;
		public LBDbType LBDBType;
		public ParameterDirection Direction;
		public Object Value;

		public LBDbParameter()
		{
		}

		public LBDbParameter( string parameterName, LBDbType dbtype,object value )
		{
			ParameterName = parameterName;
            LBDBType = dbtype;
			Direction = ParameterDirection.Input;
			Value = value;
		}

		public LBDbParameter( string parameterName, LBDbType dbtype, object value, bool output )
		{
			ParameterName = parameterName;
            LBDBType = dbtype;
			Direction = ParameterDirection.InputOutput;
			Value = value;
		}

		/*public DbParameter( string parameterName, string dbTypeName, ParameterDirection direction )
		{
			ParameterName = parameterName;
			DBTypeName = dbTypeName;
			Direction = direction;
		}

		public DbParameter( string parameterName, string dbTypeName, ParameterDirection direction, Object value )
		{
			ParameterName = parameterName;
			DBTypeName = dbTypeName;
			Direction = direction;
			Value = value;
		}*/

		public void IsNullToEmptyOrZero()
		{
			if( Value == DBNull.Value || Value == null )
			{
				switch(LBDBType)
				{
					case LBDbType.String:
                    case LBDbType.DateTime:
                    case LBDbType.Date:
                    case LBDbType.NText:
                        Value = "";
						break;

                    default:
						Value = 0;
						break;
				}
			}
		}

		public void NullIfEmptyOrZero()
		{
			if( Value != DBNull.Value && Value != null )
			{
                switch (LBDBType)
                {
                    case LBDbType.String:
                    case LBDbType.DateTime:
                    case LBDbType.Date:
                    case LBDbType.NText:
                        if ( string.IsNullOrEmpty( Value.ToString().Trim() ) )
						{
							Value = DBNull.Value;
						}
						break;

                    default:
                        try
						{
							decimal decTemp = Convert.ToDecimal( Value );
							if( decTemp == 0 )
							{
								Value = DBNull.Value;
							}
						}
						catch
						{
						}
						break;
				}
			}
		}
	}
}
