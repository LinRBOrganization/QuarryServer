using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using LB.Web.Contants.DBType;
using LB.Web.Base.Helper;
using LB.Web.MM.DAL;
using LB.Web.Base.Factory;

namespace LB.Web.MM.BLL
{
    public class BLLDBItemBase : IBLLFunction
    {
        private DALDBItemBase _DALDBItemBase = null;
        public BLLDBItemBase()
        {
            _DALDBItemBase = new DAL.DALDBItemBase();
        }
        
        public override string GetFunctionName(int iFunctionType)
        {
            
            string strFunName = "";
            switch (iFunctionType)
            {
                case 20300:
                    strFunName = "DBItemBase_Insert";
                    break;

                case 20301:
                    strFunName = "DBItemBase_Update";
                    break;

                case 20302:
                    strFunName = "DBItemBase_Delete";
                    break;
            }
            return strFunName;
        }

        public void DBItemBase_Insert(FactoryArgs args, out t_BigID ItemID, t_BigID ItemTypeID,
            t_String ItemCode, t_String ItemName, t_String ItemMode, t_Float ItemRate,
            t_BigID UOMID, t_String Description, t_Bool IsForbid)
        {
            _DALDBItemBase.Insert(args, out ItemID, ItemTypeID, ItemCode, ItemName, ItemMode,
                ItemRate, UOMID, Description, IsForbid);
        }

        public void DBItemBase_Update(FactoryArgs args, t_BigID ItemID, t_BigID ItemTypeID,
            t_String ItemCode, t_String ItemName, t_String ItemMode, t_Float ItemRate,
            t_BigID UOMID, t_String Description, t_Bool IsForbid)
        {
            _DALDBItemBase.Update(args, ItemID, ItemTypeID, ItemCode, ItemName, ItemMode,
                ItemRate, UOMID, Description, IsForbid);
        }

        public void DBItemBase_Delete(FactoryArgs args, t_BigID ItemID)
        {
            _DALDBItemBase.Delete(args, ItemID);
        }
    }
}