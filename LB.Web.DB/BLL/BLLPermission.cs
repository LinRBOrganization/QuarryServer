using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using LB.Web.DB.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace LB.Web.DB.BLL
{
    public class BLLPermission : IBLLFunction
    {
        private DALPermission _DALPermission = null;
        public BLLPermission()
        {
            _DALPermission = new DAL.DALPermission();
        }

        public override string GetFunctionName(int iFunctionType)
        {

            string strFunName = "";
            switch (iFunctionType)
            {
                case 11000:
                    strFunName = "GetUserPermission";
                    break;
                case 11001:
                    strFunName = "InsertPermission";
                    break;
                case 11002:
                    strFunName = "UpdatePermission";
                    break;
                case 11003:
                    strFunName = "DeletePermission";
                    break;
            }
            return strFunName;
        }

        public void GetUserPermission(FactoryArgs args, t_BigID UserID, t_String PermissionCode,
            out t_String PermissionDataName, out t_String PermissionName, out t_Bool HasPermission)
        {
            PermissionDataName =new t_String();
            PermissionName = new t_String();

            bool bolIsAdmin = _DALPermission.VerifyIsAdmin(args,UserID);
            if (bolIsAdmin)//超级管理员
            {
                HasPermission = new t_Bool(1);
            }
            else
            {
                _DALPermission.GetUserPermission(args, UserID, PermissionCode, out PermissionDataName, out PermissionName, out HasPermission);
            }
        }

        public void InsertPermission(FactoryArgs args,out t_BigID PermissionID, t_String PermissionName, t_BigID ParentPermissionID)
        {
            PermissionID =new t_BigID();
            using (DataTable dtPermission = _DALPermission.GetPermission(args, ParentPermissionID))
            {
                if (ParentPermissionID.Value==null || dtPermission.Rows.Count > 0)//校验上级权限组是否存在
                {
                    using (DataTable dtExistsName = _DALPermission.GetPermissionByName(args, PermissionName))
                    {
                        if (dtExistsName.Rows.Count == 0)//不存在，可添加
                        {
                            //_DALPermission.InsertPermission(args,out PermissionID,ParentPermissionID)
                        }
                        else
                        {
                            throw new Exception("当前权限分类名称已存在！");
                        }
                    }
                }
                else
                {
                    throw new Exception("上级权限分类不存在，无法在该权限分类下级添加权限分类！");
                }
            }
        }
    }
}