using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using LB.Web.MI.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.MI.BLL
{
    public class BLLDbCustomer: IBLLFunction
    {
        private DALDbCustomer _DALDbCustomer = null;
        public BLLDbCustomer()
        {
            _DALDbCustomer = new DAL.DALDbCustomer();
        }

        public override string GetFunctionName(int iFunctionType)
        {
            string strFunName = "";
            switch (iFunctionType)
            {
                case 13400:
                    strFunName = "Customer_Insert";
                    break;

                case 13401:
                    strFunName = "Customer_Update";
                    break;

                case 13402:
                    strFunName = "Customer_Delete";
                    break;
            }
            return strFunName;
        }

        public void Customer_Insert(FactoryArgs args, out t_BigID CustomerID, t_String CustomerName, t_String Contact, t_String Phone, t_String Address,
            t_Bool CarIsLimit, t_ID AmountType, t_String LicenceNum, t_String Description, t_Bool IsForbid, t_ID ReceiveType,
            t_Decimal CreditAmount, t_Bool IsDisplayPrice, t_Bool IsDisplayAmount, t_Bool IsPrintAmount, t_Bool IsAllowOverFul)
        {
            CustomerID = new t_BigID();

            using (DataTable dtCustomer = _DALDbCustomer.GetCustomerByName(args, CustomerID, CustomerName))
            {
                if (dtCustomer.Rows.Count > 0)
                {
                    throw new Exception("该客户名称已存在！");
                }
            }

            _DALDbCustomer.Customer_Insert(args, out CustomerID, CustomerName, Contact, Phone, Address, CarIsLimit, AmountType, LicenceNum, Description,
                IsForbid, ReceiveType, CreditAmount, IsDisplayPrice, IsDisplayAmount, IsPrintAmount, IsAllowOverFul);
        }

        public void Customer_Update(FactoryArgs args, t_BigID CustomerID, t_String CustomerName, t_String Contact, t_String Phone, t_String Address,
            t_Bool CarIsLimit, t_ID AmountType, t_String LicenceNum, t_String Description, t_Bool IsForbid, t_ID ReceiveType,
            t_Decimal CreditAmount, t_Bool IsDisplayPrice, t_Bool IsDisplayAmount, t_Bool IsPrintAmount, t_Bool IsAllowOverFul)
        {

            using (DataTable dtCustomer = _DALDbCustomer.GetCustomerByName(args, CustomerID, CustomerName))
            {
                if (dtCustomer.Rows.Count > 0)
                {
                    throw new Exception("该客户名称已存在！");
                }
            }

            _DALDbCustomer.Customer_Update(args, CustomerID, CustomerName, Contact, Phone, Address, CarIsLimit, AmountType, LicenceNum, Description,
                IsForbid, ReceiveType, CreditAmount, IsDisplayPrice, IsDisplayAmount, IsPrintAmount, IsAllowOverFul);
        }

        public void Customer_Delete(FactoryArgs args, t_BigID CustomerID)
        {
            using (DataTable dtCar = _DALDbCustomer.GetCarByCustomer(args, CustomerID))
            {
                if (dtCar.Rows.Count > 0)
                {
                    throw new Exception("该客户已关联车辆，无法删除！");
                }
            }

            _DALDbCustomer.Customer_Delete(args, CustomerID);
        }
    }
}
