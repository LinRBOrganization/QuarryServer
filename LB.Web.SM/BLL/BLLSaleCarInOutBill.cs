using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using LB.Web.SM.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.SM.BLL
{
    public class BLLSaleCarInOutBill : IBLLFunction
    {
        private DALSaleCarInOutBill _DALSaleCarInOutBill = null;
        public BLLSaleCarInOutBill()
        {
            _DALSaleCarInOutBill = new DAL.DALSaleCarInOutBill();
        }

        public override string GetFunctionName(int iFunctionType)
        {

            string strFunName = "";
            switch (iFunctionType)
            {
                case 14100:
                    strFunName = "InsertInBill";
                    break;
            }
            return strFunName;
        }

        public void InsertInBill(FactoryArgs args, out t_BigID SaleCarInBillID, out t_String SaleCarInBillCode, out t_DTSmall BillDate, t_BigID CarID,
            t_BigID ItemID,t_ID ReceiveType,t_ID CalculateType,t_Float CarTare, t_BigID CustomerID,t_String Description)
        {
            SaleCarInBillID = new t_BigID();
            SaleCarInBillCode = new t_String();
            BillDate = new t_DTSmall(DateTime.Now);

            if (CarID.Value == 0)
            {
                throw new Exception("车牌号不能为空！");
            }
            if (ItemID.Value == 0)
            {
                throw new Exception("货物名称不能为空！");
            }
            if (CustomerID.Value == 0)
            {
                throw new Exception("客户名称不能为空！");
            }

            //先校验该车辆是否存在入磅但是没有出磅的记录，如果存在则报错
            using (DataTable dtExistsNotOut = _DALSaleCarInOutBill.ExistsNotOut(args, CarID))
            {
                if (dtExistsNotOut.Rows.Count > 0)
                {
                    DateTime dtBillDate = Convert.ToDateTime(dtExistsNotOut.Rows[0]["BillDate"]);
                    throw new Exception("该车辆在[" + dtBillDate.ToString("yyyy-MM-dd HH:mm") + "入场，但是没有出场记录，本次操作失败！");
                }
            }

            //生成编码
            string strBillFont = "SM" + DateTime.Now.ToString("yyyyMMdd");
            using (DataTable dtBillCode = _DALSaleCarInOutBill.GetMaxBillCode(args))
            {
                if (dtBillCode.Rows.Count > 0)
                {
                    DataRow drBillCode = dtBillCode.Rows[0];
                    int iIndex = 1;
                    string strIndex = "";
                    if (drBillCode["SaleCarInBillCode"].ToString().TrimEnd().Contains(strBillFont))
                    {
                        iIndex = Convert.ToInt32(drBillCode["SaleCarInBillCode"].ToString().TrimEnd().Replace(strBillFont, ""));
                        iIndex += 1;
                        if (iIndex < 10)
                        {
                            strIndex = "00" + iIndex.ToString();
                        }
                        else if (iIndex < 100)
                        {
                            strIndex = "0" + iIndex.ToString();
                        }
                        else if (iIndex < 1000)
                        {
                            strIndex = iIndex.ToString();
                        }
                        SaleCarInBillCode.SetValueWithObject(strBillFont + strIndex);
                    }
                    else
                    {
                        SaleCarInBillCode.SetValueWithObject(strBillFont + "001");
                    }
                }
                else
                {
                    SaleCarInBillCode.SetValueWithObject(strBillFont + "001");
                }
            }

            _DALSaleCarInOutBill.InsertInBill(args, out SaleCarInBillID, SaleCarInBillCode, CarID, ItemID, 
                BillDate, ReceiveType, CalculateType, CarTare, CustomerID, Description);
        }
    }
}
