using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using LB.Web.RP.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.RP.BLL
{
    public class BLLRPReceiveBillHeader : IBLLFunction
    {
        private DALRPReceiveBillHeader _DALRPReceiveBillHeader = null;
        public BLLRPReceiveBillHeader()
        {
            _DALRPReceiveBillHeader = new DAL.DALRPReceiveBillHeader();
        }

        public override string GetFunctionName(int iFunctionType)
        {

            string strFunName = "";
            switch (iFunctionType)
            {
                case 13300:
                    strFunName = "Insert";
                    break;
                case 13301:
                    strFunName = "Update";
                    break;
                case 13302:
                    strFunName = "Delete";
                    break;
                case 13303:
                    strFunName = "Approve";
                    break;
                case 13304:
                    strFunName = "UnApprove";
                    break;
                case 13305:
                    strFunName = "Cancel";
                    break;
                case 13306:
                    strFunName = "UnCancel";
                    break;
            }
            return strFunName;
        }

        public void Insert(FactoryArgs args, out t_BigID ReceiveBillHeaderID, out t_String ReceiveBillCode, t_DTSmall BillDate, t_BigID CustomerID, t_Decimal ReceiveAmount,
            t_String Description)
        {
            ReceiveBillCode = new t_String();
            //生成编码
            string strBillFont = "RP" + DateTime.Now.ToString("yyMMdd");
            using (DataTable dtBillCode = _DALRPReceiveBillHeader.GetMaxBillCode(args))
            {
                if (dtBillCode.Rows.Count > 0)
                {
                    DataRow drBillCode = dtBillCode.Rows[0];
                    int iIndex = 1;
                    string strIndex = "";
                    if (drBillCode["ReceiveBillCode"].ToString().TrimEnd().Contains(strBillFont))
                    {
                        iIndex = Convert.ToInt32(drBillCode["ReceiveBillCode"].ToString().TrimEnd().Replace(strBillFont, ""));
                        iIndex += 1;
                        if (iIndex < 10)
                        {
                            strIndex = "0" + iIndex.ToString();
                        }
                        else
                        {
                            strIndex = iIndex.ToString();
                        }
                        ReceiveBillCode.SetValueWithObject(strBillFont + strIndex);
                    }
                    else
                    {
                        ReceiveBillCode.SetValueWithObject(strBillFont + "01");
                    }
                }
                else
                {
                    ReceiveBillCode.SetValueWithObject(strBillFont + "01");
                }
            }

            _DALRPReceiveBillHeader.Insert(args, out ReceiveBillHeaderID, ReceiveBillCode, BillDate, CustomerID, ReceiveAmount, Description);
        }

        public void Update(FactoryArgs args, t_BigID ReceiveBillHeaderID, t_DTSmall BillDate, t_Decimal ReceiveAmount,
            t_String Description)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (bolIsApprove)
                    {
                        throw new Exception("该充值单已审核，无法进行修改！");
                    }
                    if (bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法进行修改！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法进行修改！");
                }
            }
            _DALRPReceiveBillHeader.Update(args, ReceiveBillHeaderID, BillDate, ReceiveAmount, Description);
        }

        public void Delete(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (bolIsApprove)
                    {
                        throw new Exception("该充值单已审核，无法进行删除！");
                    }
                    if (bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法进行删除！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法进行删除！");
                }
            }
            _DALRPReceiveBillHeader.Delete(args, ReceiveBillHeaderID);
        }

        public void Approve(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (bolIsApprove)
                    {
                        throw new Exception("该充值单已审核，无法再进行审核！");
                    }
                    if (bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法进行审核！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法进行审核！");
                }
            }
            _DALRPReceiveBillHeader.Approve(args, ReceiveBillHeaderID);
        }

        public void UnApprove(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (!bolIsApprove)
                    {
                        throw new Exception("该充值单未审核，无法执行取消审核！");
                    }
                    if (bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法执行取消审核！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法执行取消审核！");
                }
            }
            _DALRPReceiveBillHeader.UnApprove(args, ReceiveBillHeaderID);
        }

        public void Cancel(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (bolIsApprove)
                    {
                        throw new Exception("该充值单已审核，无法再进行作废！");
                    }
                    if (bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法进行作废！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法进行作废！");
                }
            }
            _DALRPReceiveBillHeader.Cancel(args, ReceiveBillHeaderID);
        }

        public void UnCancel(FactoryArgs args, t_BigID ReceiveBillHeaderID)
        {
            using (DataTable dtHeader = _DALRPReceiveBillHeader.GetRPReceiveBillHeader(args, ReceiveBillHeaderID))
            {
                if (dtHeader.Rows.Count > 0)
                {
                    DataRow drHeader = dtHeader.Rows[0];
                    bool bolIsApprove = LBConverter.ToBoolean(drHeader["IsApprove"]);
                    bool bolIsCancel = LBConverter.ToBoolean(drHeader["IsCancel"]);
                    if (bolIsApprove)
                    {
                        throw new Exception("该充值单未审核，无法执行取消审核！");
                    }
                    if (!bolIsCancel)
                    {
                        throw new Exception("该充值单已作废，无法执行取消审核！");
                    }
                }
                else
                {
                    throw new Exception("该充值单已删除，无法执行取消审核！");
                }
            }
            _DALRPReceiveBillHeader.UnCancel(args, ReceiveBillHeaderID);
        }
    }
}
