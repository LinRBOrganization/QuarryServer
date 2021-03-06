﻿using System;
using System.Collections.Generic;
using System.Text;

namespace LB.Web.Contants.DBType
{
	public struct t_Decimal : ILBDbType
	{
		private decimal? m_Value;

		public string DBTypeName
		{
			get
			{
				return LBDBType.t_Decimal;
			}
		}

		object ILBDbType.Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				if( value == null || value == DBNull.Value )
				{
					m_Value = null;
				}
				else if( value is int )
				{
					m_Value = Convert.ToDecimal( value ); 
				}
				else if( value is decimal )
				{
					m_Value = (decimal)value;
				}
				else
				{
					throw new Exception( "Value must be type of decimal." );
				}
			}
		}

		public void SetValueWithObject( object value )
		{
			if( value == null || value == DBNull.Value )
			{
				m_Value = null;
			}
			else if( value is int )
			{
				m_Value = Convert.ToDecimal( value );
			}
			else if( value is decimal )
			{
				m_Value = (decimal)value;
			}
			else
			{
				try
				{
					m_Value = Convert.ToDecimal( value );
				}
				catch
				{
					throw new Exception( "Value must be type of decimal." );
				}
			}
		}

		public decimal? Value
		{
			get
			{
				return m_Value;
			}
			set
			{
				m_Value = value;
			}
		}

		public t_Decimal( decimal? value )
		{
			m_Value = value;
		}

		public t_Decimal( object value )
		{
			if( value == null || value == DBNull.Value )
			{
				m_Value = null;
			}
			else if( value is decimal )
			{
				m_Value = (decimal)value;
			}
			else if( value is int )
			{
				m_Value = Convert.ToDecimal( value );
			}
			else
			{
				Type t = value.GetType();
				throw new Exception( "Value must be type of decimal." );
			}
		}

		public void IsNullToZero()
		{
			if( m_Value == null )
			{
				m_Value = 0;
			}
		}

		public void NullIfZero()
		{
			if( m_Value == 0 )
			{
				m_Value = null;
			}
		}
	}
}
