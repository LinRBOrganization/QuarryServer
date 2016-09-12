using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace LB.Web.DB.DAL
{
    public class DALPermission
    {
        public void GetUserPermission(FactoryArgs args, t_BigID UserID, t_String PermissionCode,
            out t_String PermissionDataName, out t_String PermissionName, out t_Bool HasPermission)
        {
            PermissionDataName = new t_String();
            PermissionName = new t_String();
            HasPermission = new t_Bool(1);
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value));
            parms.Add(new LBDbParameter("PermissionCode", LBDbType.String, PermissionCode.Value));
            parms.Add(new LBDbParameter("PermissionDataName", LBDbType.String, PermissionDataName.Value, true));
            parms.Add(new LBDbParameter("PermissionName", LBDbType.String, PermissionName.Value, true));
            parms.Add(new LBDbParameter("HasPermission", LBDbType.Boolean, HasPermission.Value, true));
            string strSQL = @"
select  p.PermissionDataName,
        isnull(d.HasPermission,0) as HasPermission,
        s.PermissionName
from dbo.DbPermissionData p
	inner join dbo.DbPermission s on
		s.PermissionID = p.PermissionID
	left outer join dbo.DbUserPermission d on
		p.PermissionDataID = d.PermissionDataID and
        d.UserID = @UserID
where PermissionCode=@PermissionCode
";
            DataTable dtReturn = DBHelper.ExecuteQuery(args, strSQL, parms);
            if (dtReturn.Rows.Count > 0)
            {
                PermissionDataName.Value = dtReturn.Rows[0]["PermissionDataName"].ToString();
                PermissionName.Value = dtReturn.Rows[0]["PermissionName"].ToString();
                HasPermission.Value = dtReturn.Rows[0]["HasPermission"].ToString() == "1" ? (byte)1 : (byte)0;
            }
        }

        /// <summary>
        /// 校验是否超级管理员
        /// </summary>
        /// <param name="args"></param>
        /// <param name="UserID"></param>
        public bool VerifyIsAdmin(FactoryArgs args, t_BigID UserID)
        {
            bool bolIsAdmin = false;
            int UserType=0;
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value));
            parms.Add(new LBDbParameter("UserType", LBDbType.Int32, UserType,true));
            string strSQL = @"
select @UserType = UserType
from dbo.DbUser
where UserID=@UserID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            UserType = Convert.ToInt32(parms["UserType"].Value);
            if (UserType == 2)
            {
                bolIsAdmin = true;
            }
            return bolIsAdmin;
        }

        public DataTable GetPermission(FactoryArgs args, t_BigID PermissionID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("PermissionID", LBDbType.Int64, PermissionID.Value));
            string strSQL = @"
select *
from dbo.DbPermission
where PermissionID=@PermissionID
";
            DataTable dtReturn = DBHelper.ExecuteQuery(args, strSQL, parms);
            return dtReturn;
        }

        public DataTable GetPermissionByName(FactoryArgs args, t_String PermissionName)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("PermissionName", LBDbType.String, PermissionName.Value));
            string strSQL = @"
select *
from dbo.DbPermission
where PermissionName=@PermissionName
";
            DataTable dtReturn = DBHelper.ExecuteQuery(args, strSQL, parms);
            return dtReturn;
        }

        public void InsertPermission(FactoryArgs args, out t_BigID PermissionID, t_BigID ParentPermissionID, t_String PermissionName)
        {
            PermissionID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("PermissionID", LBDbType.Int64, PermissionID.Value, true));
            parms.Add(new LBDbParameter("PermissionName", LBDbType.String, PermissionName.Value));
            parms.Add(new LBDbParameter("ParentPermissionID", LBDbType.Int64, ParentPermissionID.Value));
            string strSQL = @"
insert into dbo.DbPermission( PermissionName, ParentPermissionID)
values( @PermissionName, nullif(@ParentPermissionID,0))

set @PermissionID = @@PermissionID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            PermissionID.SetValueWithObject( parms["PermissionID"].Value);
        }


        public void UpdatePermission(FactoryArgs args, t_BigID PermissionID, t_String PermissionName)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("PermissionID", LBDbType.Int64, PermissionID.Value));
            parms.Add(new LBDbParameter("PermissionName", LBDbType.String, PermissionName.Value));
            string strSQL = @"
update into dbo.DbPermission
set PermissionName = @PermissionName
where PermissionID = @PermissionID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }
    }
}