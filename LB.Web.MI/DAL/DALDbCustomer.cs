using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.MI.DAL
{
    public class DALDbCustomer
    {
        public void Customer_Insert(FactoryArgs args, out t_BigID CustomerID,t_String CustomerName, t_String Contact, t_String Phone, t_String Address, 
            t_Bool CarIsLimit,t_ID AmountType, t_String LicenceNum, t_String Description,t_Bool IsForbid,t_ID ReceiveType,
            t_Decimal CreditAmount,t_Bool IsDisplayPrice, t_Bool IsDisplayAmount, t_Bool IsPrintAmount, t_Bool IsAllowOverFul)
        {
            CarIsLimit.IsNullToZero();
            IsDisplayPrice.IsNullToZero();
            IsDisplayAmount.IsNullToZero();
            IsPrintAmount.IsNullToZero();
            IsAllowOverFul.IsNullToZero();
            CreditAmount.IsNullToZero();
            IsForbid.IsNullToZero();
            CreditAmount.IsNullToZero();

            CustomerID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CustomerID", CustomerID, true));
            parms.Add(new LBDbParameter("CustomerName", CustomerName));
            parms.Add(new LBDbParameter("Contact", Contact));
            parms.Add(new LBDbParameter("Phone", Phone));
            parms.Add(new LBDbParameter("Address", Address));
            parms.Add(new LBDbParameter("CarIsLimit", CarIsLimit));
            parms.Add(new LBDbParameter("AmountType", AmountType));
            parms.Add(new LBDbParameter("LicenceNum", LicenceNum));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("IsForbid", IsForbid));
            parms.Add(new LBDbParameter("ReceiveType", ReceiveType));
            parms.Add(new LBDbParameter("CreditAmount", CreditAmount));
            parms.Add(new LBDbParameter("IsDisplayPrice", IsDisplayPrice));
            parms.Add(new LBDbParameter("IsDisplayAmount", IsDisplayAmount));
            parms.Add(new LBDbParameter("IsPrintAmount", IsPrintAmount));
            parms.Add(new LBDbParameter("IsAllowOverFul", IsAllowOverFul));
            parms.Add(new LBDbParameter("CreateBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("CreateTime", new t_DTSmall(DateTime.Now)));
            parms.Add(new LBDbParameter("ChangeBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
insert into dbo.DbCustomer(CustomerName, Contact, Phone, Address, CarIsLimit, AmountType, LicenceNum, 
    Description, IsForbid, ReceiveType, CreditAmount, IsDisplayPrice, IsDisplayAmount, IsPrintAmount, IsAllowOverFul, 
    CreateBy, CreateTime, ChangeBy, ChangeTime)
values( @CustomerName, @Contact, @Phone, @Address, @CarIsLimit, @AmountType, @LicenceNum, 
    @Description, @IsForbid, @ReceiveType, @CreditAmount, @IsDisplayPrice, @IsDisplayAmount, @IsPrintAmount, @IsAllowOverFul, 
    @CreateBy, @CreateTime, @ChangeBy, @ChangeTime)

set @CustomerID = @@identity
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            CustomerID.Value = Convert.ToInt64(parms["CustomerID"].Value);
        }

        public void Customer_Update(FactoryArgs args, t_BigID CustomerID, t_String CustomerName, t_String Contact, t_String Phone, t_String Address,
            t_Bool CarIsLimit, t_ID AmountType, t_String LicenceNum, t_String Description, t_Bool IsForbid, t_ID ReceiveType,
            t_Decimal CreditAmount, t_Bool IsDisplayPrice, t_Bool IsDisplayAmount, t_Bool IsPrintAmount, t_Bool IsAllowOverFul)
        {
            CarIsLimit.IsNullToZero();
            IsDisplayPrice.IsNullToZero();
            IsDisplayAmount.IsNullToZero();
            IsPrintAmount.IsNullToZero();
            IsAllowOverFul.IsNullToZero();
            CreditAmount.IsNullToZero();
            IsForbid.IsNullToZero();
            CreditAmount.IsNullToZero();

            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("CustomerName", CustomerName));
            parms.Add(new LBDbParameter("Contact", Contact));
            parms.Add(new LBDbParameter("Phone", Phone));
            parms.Add(new LBDbParameter("Address", Address));
            parms.Add(new LBDbParameter("CarIsLimit", CarIsLimit));
            parms.Add(new LBDbParameter("AmountType", AmountType));
            parms.Add(new LBDbParameter("LicenceNum", LicenceNum));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("IsForbid", IsForbid));
            parms.Add(new LBDbParameter("ReceiveType", ReceiveType));
            parms.Add(new LBDbParameter("CreditAmount", CreditAmount));
            parms.Add(new LBDbParameter("IsDisplayPrice", IsDisplayPrice));
            parms.Add(new LBDbParameter("IsDisplayAmount", IsDisplayAmount));
            parms.Add(new LBDbParameter("IsPrintAmount", IsPrintAmount));
            parms.Add(new LBDbParameter("IsAllowOverFul", IsAllowOverFul));
            parms.Add(new LBDbParameter("ChangeBy", new t_String(args.LoginName)));
            parms.Add(new LBDbParameter("ChangeTime", new t_DTSmall(DateTime.Now)));

            string strSQL = @"
udpate dbo.DbCustomer
set CustomerName = @CustomerName, 
    Contact=@Contact, 
    Phone=@Phone, 
    Address=@Address, 
    CarIsLimit=@CarIsLimit, 
    AmountType=@AmountType, 
    LicenceNum=@LicenceNum, 
    Description=@Description, 
    IsForbid=@IsForbid, 
    ReceiveType=@ReceiveType, 
    CreditAmount=@CreditAmount, 
    IsDisplayPrice=@IsDisplayPrice, 
    IsDisplayAmount=@IsDisplayAmount, 
    IsPrintAmount=@IsPrintAmount, 
    IsAllowOverFul=@IsAllowOverFul, 
    ChangeBy=@ChangeBy, 
    ChangeTime=@ChangeTime

where CustomerID =  @CustomerID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Customer_Delete(FactoryArgs args, t_BigID CustomerID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CustomerID", CustomerID));

            string strSQL = @"
delete dbo.DbCustomer
where CustomerID = @CustomerID
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public DataTable GetCustomerByName(FactoryArgs args, t_BigID CustomerID, t_String CustomerName)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            parms.Add(new LBDbParameter("CustomerName", CustomerName));
            string strSQL = @"
if @CustomerID = 0
begin
    select CustomerName
    from dbo.DbCustomer
    where CustomerName=@CustomerName
end
else
begin
    select CustomerName
    from dbo.DbCustomer
    where CustomerName=@CustomerName and CustomerID<>@CustomerID
end
";
            return DBHelper.ExecuteQuery(args, strSQL, parms);
        }

        public DataTable GetCarByCustomer(FactoryArgs args, t_BigID CustomerID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("CustomerID", CustomerID));
            string strSQL = @"
select *
from dbo.DbCar
where CustomerID = @CustomerID
";
            return DBHelper.ExecuteQuery(args, strSQL, parms);
        }
    }
}
