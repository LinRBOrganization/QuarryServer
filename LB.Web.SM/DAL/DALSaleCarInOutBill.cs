using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.SM.DAL
{
    public class DALSaleCarInOutBill
    {
        //判断该车辆是否已出磅
        public DataTable ExistsNotOut(FactoryArgs args, t_BigID CarID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CarID", CarID));
            string strSQL = @"
    select BillDate
    from dbo.SaleCarInBill
    where CarID=@CarID and 
        SaleCarInBillID not in (select SaleCarInBillID from dbo.SaleCarInBill)
";
            return DBHelper.ExecuteQuery(args, strSQL, parms);
        }

        public DataTable GetMaxBillCode(FactoryArgs args)
        {

            string strSQL = @"
select top 1 SaleCarInBillCode
from dbo.SaleCarInBill
where BillDate<getdate()+1
order by SaleCarInBillCode desc
";
            return DBHelper.ExecuteQuery(args, strSQL);
        }

        public void InsertInBill(FactoryArgs args, out t_BigID SaleCarInBillID, t_String SaleCarInBillCode, t_BigID CarID,
            t_BigID ItemID, t_DTSmall BillDate, t_ID ReceiveType, t_ID CalculateType, t_Float CarTare, t_BigID CustomerID, t_String Description)
        {
            SaleCarInBillID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("SaleCarInBillID", SaleCarInBillID, true));
            parms.Add(new LBDbParameter("SaleCarInBillCode", SaleCarInBillCode));
            parms.Add(new LBDbParameter("CarID", CarID));
            parms.Add(new LBDbParameter("ItemID", ItemID));
            parms.Add(new LBDbParameter("BillDate", BillDate));
            parms.Add(new LBDbParameter("ReceiveType", ReceiveType));
            parms.Add(new LBDbParameter("CalculateType", CalculateType));
            parms.Add(new LBDbParameter("CarTare", CarTare));
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("CreateBy",new t_String(args.LoginName)));

            string strSQL = @"
insert into dbo.SaleCarInBill(  SaleCarInBillCode, CarID,PrintCount,
            ItemID, BillDate, ReceiveType, BillStatus, CalculateType, CarTare, CustomerID,Description,
            IsCancel,CreateBy, CreateTime)
values( @SaleCarInBillCode, @CarID,0,
        @ItemID, @BillDate, @ReceiveType, 0, @CalculateType, @CarTare, @CustomerID,@Description,
        0,@CreateBy,getdate())

set @SaleCarInBillID = @@identity
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            SaleCarInBillID.Value = Convert.ToInt64(parms["SaleCarInBillID"].Value);
        }


    }
}
