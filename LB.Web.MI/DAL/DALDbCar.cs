using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.MI.DAL
{
    public class DALDbCar
    {
        public void Car_Insert(FactoryArgs args,out t_BigID CarID, t_String CarNum, t_BigID CustomerID)
        {
            CarID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CarID", CarID, true));
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("CarNum", CarNum));
            parms.Add(new LBDbParameter("CreateBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("CreateTime", new t_DTSmall(DateTime.Now)));
            parms.Add(new LBDbParameter("ChangeBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
insert into dbo.DbCar(CustomerID,CarNum,CreateBy, CreateTime, ChangeBy, ChangeTime)
values( @CustomerID, @CarNum, @CreateBy, @CreateTime, @ChangeBy, @ChangeTime)

set @CarID = @@identity
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            CarID.Value = Convert.ToInt64(parms["CarID"].Value);
        }
        public void Car_Update(FactoryArgs args, t_BigID CarID, t_String CarNum, t_BigID CustomerID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CarID", CarID));
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("CarNum", CarNum));
            parms.Add(new LBDbParameter("ChangeBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
update dbo.DbCar
set CustomerID=@CustomerID,
    CarNum = @CarNum,
    ChangeBy = @ChangeBy,
    ChangeTime = @ChangeTime
where CarID  =@CarID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Customer_Delete(FactoryArgs args, t_BigID CarID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CarID", CarID));

            string strSQL = @"
delete dbo.DbCar
where CarID = @CarID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public DataTable GetCarByName(FactoryArgs args, t_BigID CarID, t_String CarNum)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CarID", CarID));
            parms.Add(new LBDbParameter("CarNum", CarNum));
            string strSQL = @"
if @CarID = 0
begin
    select CarNum
    from dbo.DbCar
    where CarNum=@CarNum
end
else
begin
    select CarNum
    from dbo.DbCar
    where CarNum=@CarNum and CarID<>@CarID
end
";
            return DBHelper.ExecuteQuery(args, strSQL, parms);
        }
    }
}
