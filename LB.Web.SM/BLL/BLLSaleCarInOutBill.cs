using LB.Web.Base.Base.Helper;
using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using LB.Web.SM.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
                case 14101:
                    strFunName = "GetCarNotOutBill";
                    break;

                case 14102:
                    strFunName = "InsertOutBill";
                    break;
            }
            return strFunName;
        }

        public void InsertInBill(FactoryArgs args, out t_BigID SaleCarInBillID, out t_String SaleCarInBillCode, out t_DTSmall BillDate, t_BigID CarID,
            t_BigID ItemID, t_ID ReceiveType, t_ID CalculateType, t_Float CarTare, t_BigID CustomerID, t_String Description,
            t_Image MonitoreImg1, t_Image MonitoreImg2, t_Image MonitoreImg3, t_Image MonitoreImg4)
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

            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            string strCameraPath = Path.Combine(strPath, "LBCameraPicture");
            if (!Directory.Exists(strCameraPath))
            {
                Directory.CreateDirectory(strCameraPath);
            }
            string strInBillPath = Path.Combine(strCameraPath, "InBillPicture");
            if (!Directory.Exists(strInBillPath))
            {
                Directory.CreateDirectory(strInBillPath);
            }
            string strDatePath = Path.Combine(strInBillPath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(strDatePath))
            {
                Directory.CreateDirectory(strDatePath);
            }
            if (MonitoreImg1.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarInBillID.Value.ToString() + "_Image1.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg1.Value);
            }
            if (MonitoreImg2.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarInBillID.Value.ToString() + "_Image2.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg2.Value);
            }
            if (MonitoreImg3.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarInBillID.Value.ToString() + "_Image3.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg3.Value);
            }
            if (MonitoreImg4.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarInBillID.Value.ToString() + "_Image4.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg4.Value);
            }
        }

        public void GetCarNotOutBill(FactoryArgs args, t_BigID CarID)
        {
            args.SelectResult = _DALSaleCarInOutBill.GetCarNotOutBill(args, CarID);
        }

        public void InsertOutBill(FactoryArgs args, out t_BigID SaleCarOutBillID, out t_DTSmall BillDate, t_BigID SaleCarInBillID, t_BigID CarID, 
            t_ID ReceiveType, t_ID CalculateType, t_Decimal Price, t_Decimal Amount, t_Decimal TotalWeight, 
            t_Decimal SuttleWeight, t_String Description, t_Image MonitoreImg1, t_Image MonitoreImg2, t_Image MonitoreImg3, t_Image MonitoreImg4)
        {
            BillDate = new t_DTSmall(DateTime.Now);
            if (SaleCarInBillID.Value == null && SaleCarInBillID.Value == 0)
            {
                throw new Exception("该车辆未匹配到入场订单，请重新选择入场订单！");
            }
            if (CarID.Value == null || CarID.Value == 0)
            {
                throw new Exception("车牌号码不存在或者车牌号码为空，请重新选择车牌号码！");
            }
            //校验该入场单是否已出场
            using (DataTable dtOut = _DALSaleCarInOutBill.GetCarOutBillByInBillID(args, SaleCarInBillID))
            {
                if (dtOut.Rows.Count > 0)
                {
                    DataRow drOut = dtOut.Rows[0];
                    DateTime dtOutBillDate = Convert.ToDateTime(drOut["BillDate"]);
                    throw new Exception("该入场订单已生成出场记录，出场时间为【" + dtOutBillDate.ToString("yyyy-MM-dd HH:mm") + "】,请重新选择入场订单！");
                }
            }
            //校验该入场订单记录的车牌号码与当前输入的车牌是否一致
            using (DataTable dtInBill = _DALSaleCarInOutBill.GetSaleCarInBill(args, SaleCarInBillID))
            {
                if (dtInBill.Rows.Count > 0)
                {
                    DataRow drInBill = dtInBill.Rows[0];
                    long InCarID = LBConverter.ToInt64(drInBill["CarID"]);
                    if (CarID.Value != InCarID)
                    {
                        throw new Exception("输入的车牌号码与入场订单车牌号码不一致！");
                    }
                }
            }

            //校验该车辆是否存在多张入场未出场的订单
            using (DataTable dtExistsNotOut = _DALSaleCarInOutBill.ExistsNotOut(args, CarID))
            {
                if (dtExistsNotOut.Rows.Count > 1)
                {
                    throw new Exception("该车辆存在【"+ dtExistsNotOut.Rows.Count + "】张入场但是未出场的订单！无法出场！");
                }
            }

            _DALSaleCarInOutBill.InsertOutBill(args, out SaleCarOutBillID, SaleCarInBillID, CarID, BillDate,
                TotalWeight, SuttleWeight, Price, Amount, ReceiveType, CalculateType, Description);

            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            string strCameraPath = Path.Combine(strPath, "LBCameraPicture");
            if (!Directory.Exists(strCameraPath))
            {
                Directory.CreateDirectory(strCameraPath);
            }
            string strOutBillPath = Path.Combine(strCameraPath, "OutBillPicture");
            if (!Directory.Exists(strOutBillPath))
            {
                Directory.CreateDirectory(strOutBillPath);
            }
            string strDatePath = Path.Combine(strOutBillPath, DateTime.Now.ToString("yyyy-MM-dd"));
            if (!Directory.Exists(strDatePath))
            {
                Directory.CreateDirectory(strDatePath);
            }
            if (MonitoreImg1.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarOutBillID.Value.ToString() + "_Image1.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg1.Value);
            }
            if (MonitoreImg2.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarOutBillID.Value.ToString() + "_Image2.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg2.Value);
            }
            if (MonitoreImg3.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarOutBillID.Value.ToString() + "_Image3.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg3.Value);
            }
            if (MonitoreImg4.Value != null)
            {
                string strImagePath = Path.Combine(strDatePath, SaleCarOutBillID.Value.ToString() + "_Image4.jpg");
                CommonHelper.SaveFile(strImagePath, MonitoreImg4.Value);
            }
        }
    }
}
