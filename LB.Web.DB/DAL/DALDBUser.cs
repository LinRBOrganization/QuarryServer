using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace LB.Web.DB.DAL
{
    public class DALDBUser
    {
        public void Insert(FactoryArgs args, out t_BigID UserID, t_String LoginName, t_String UserPassword, t_String UserName,
            t_ID UserType, t_ID UserSex)
        {
            UserID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value,true ));
            parms.Add(new LBDbParameter("LoginName", LBDbType.String, LoginName.Value));
            parms.Add(new LBDbParameter("UserPassword", LBDbType.String, UserPassword.Value));
            parms.Add(new LBDbParameter("UserName", LBDbType.String, UserName.Value));
            parms.Add(new LBDbParameter("UserType", LBDbType.Int32, UserType.Value));
            parms.Add(new LBDbParameter("UserSex", LBDbType.Int32, UserSex.Value));
            parms.Add(new LBDbParameter("ChangeBy", LBDbType.String, args.LoginName));
            parms.Add(new LBDbParameter("ChangeTime", LBDbType.DateTime, DateTime.Now));

            string strSQL = @"
insert into dbo.DBUser( LoginName, UserPassword, ForbidLogin, IsManager, ChangeBy, ChangeTime, UserType, UserName, UserSex)
values( @LoginName, @UserPassword, 0, 0, @ChangeBy, @ChangeTime, @UserType, @UserName, @UserSex)

set @UserID = @@identity
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            UserID.Value = Convert.ToInt64(parms["UserID"].Value);
        }

        public void Update(FactoryArgs args, t_BigID UserID, t_String LoginName, t_String UserPassword, t_String UserName,
            t_ID UserType, t_ID UserSex)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value));
            parms.Add(new LBDbParameter("LoginName", LBDbType.String, LoginName.Value));
            parms.Add(new LBDbParameter("UserPassword", LBDbType.String, UserPassword.Value));
            parms.Add(new LBDbParameter("UserName", LBDbType.String, UserName.Value));
            parms.Add(new LBDbParameter("UserType", LBDbType.Int32, UserType.Value));
            parms.Add(new LBDbParameter("UserSex", LBDbType.Int32, UserSex.Value));
            parms.Add(new LBDbParameter("ChangeBy", LBDbType.String, args.LoginName));
            parms.Add(new LBDbParameter("ChangeTime", LBDbType.DateTime, DateTime.Now));

            string strSQL = @"
update dbo.DBUser
set LoginName=@LoginName, 
    UserPassword=@UserPassword, 
    ForbidLogin=0, 
    ChangeBy=@ChangeBy, 
    ChangeTime=@ChangeTime, 
    UserType=@UserType, 
    UserName=@UserName, 
    UserSex=@UserSex
where UserID = @UserID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Delete(FactoryArgs args, t_BigID UserID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value));

            string strSQL = @"
delete dbo.DBUser
where UserID = @UserID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public DataTable GetUserByLoginName(FactoryArgs args, t_BigID UserID, t_String LoginName)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("UserID", LBDbType.Int64, UserID.Value));
            parms.Add(new LBDbParameter("LoginName", LBDbType.String, LoginName.Value));
            string strSQL = @"
if @UserID = 0
begin
    select LoginName
    from dbo.DBUser
    where LoginName=@LoginName
end
else
begin
    select LoginName
    from dbo.DBUser
    where LoginName=@LoginName and UserID<>@UserID
end
";
            return DBHelper.ExecuteQuery(args, strSQL, parms);
        }

    }
}