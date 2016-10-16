using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.RP.DAL
{
    public class DALRPReceiveBillHeader
    {
        public void Insert(FactoryArgs args, out t_BigID ReceiveBillHeaderID,t_String ReceiveBillCode, t_DTSmall BillDate, t_BigID CustomerID, t_Decimal ReceiveAmount,
            t_String Description)
        {
            ReceiveBillHeaderID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID, true));
            parms.Add(new LBDbParameter("ReceiveBillCode", ReceiveBillCode));
            parms.Add(new LBDbParameter("BillDate", BillDate));
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("ReceiveAmount", ReceiveAmount));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("CreatedBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("CreateTime", new t_DTSmall(DateTime.Now)));
            parms.Add(new LBDbParameter("ChangedBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
insert into dbo.RPReceiveBillHeader( ReceiveBillCode, BillDate, CustomerID, ReceiveAmount, Description,CreatedBy,CreateTime,ChangedBy,ChangeTime,IsApprove,IsCancel)
values( @ReceiveBillCode, @BillDate, @CustomerID, @ReceiveAmount, @Description,@CreatedBy,@CreateTime,@ChangedBy,@ChangeTime,0,0)

set @ReceiveBillHeaderID = @@identity

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            ReceiveBillHeaderID.SetValueWithObject(parms["ReceiveBillHeaderID"].Value);
        }


        public void Update(FactoryArgs args, t_BigID ReceiveBillHeaderID, t_DTSmall BillDate, t_Decimal ReceiveAmount,
            t_String Description)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));
            parms.Add(new LBDbParameter("BillDate", BillDate));
            parms.Add(new LBDbParameter("ReceiveAmount", ReceiveAmount));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("ChangedBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
update dbo.RPReceiveBillHeader
set BillDate = @BillDate,
    ReceiveAmount = @ReceiveAmount,
    Description = @Description,
    ChangedBy = @ChangedBy,
    ChangeTime = @ChangeTime
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Delete(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));

            string strSQL = @"
delete dbo.RPReceiveBillHeader
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Approve(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));
            parms.Add(new LBDbParameter("ApproveBy", new t_String(args.LoginName)));

            string strSQL = @"
update dbo.RPReceiveBillHeader
set IsApprove = 1,
    ApproveTime = getdate(),
    ApproveBy = @ApproveBy
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void UnApprove(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));

            string strSQL = @"
update dbo.RPReceiveBillHeader
set IsApprove = 0,
    ApproveTime = null,
    ApproveBy = ''
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Cancel(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));
            parms.Add(new LBDbParameter("CancelBy", new t_String(args.LoginName)));

            string strSQL = @"
update dbo.RPReceiveBillHeader
set IsCancel = 1,
    CancelTime = getdate(),
    CancelBy = @CancelBy
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void UnCancel(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));

            string strSQL = @"
update dbo.RPReceiveBillHeader
set IsApprove = 0,
    CancelTime = null,
    CancelBy = ''
where ReceiveBillHeaderID = @ReceiveBillHeaderID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public DataTable GetMaxBillCode(FactoryArgs args )
        {

            string strSQL = @"
select top 1 ReceiveBillCode
from dbo.RPReceiveBillHeader
where BillDate<getdate()+1
order by ReceiveBillCode desc
";
           return DBHelper.ExecuteQuery(args,strSQL);
        }

        public DataTable GetRPReceiveBillHeader(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReceiveBillHeaderID", ReceiveBillHeaderID));
            string strSQL = @"
select *
from dbo.RPReceiveBillHeader
where ReceiveBillHeaderID = @ReceiveBillHeaderID
";
            return DBHelper.ExecuteQuery(args, strSQL,parms);
        }
    }
}
