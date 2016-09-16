﻿using System;
using System.Collections.Generic;
using System.Web;
using LB.Web.DB.BLL;
using LB.Web.Base.Helper;

namespace LB.Web.Project.Factory
{
    public class LBFactory
    {
        public IBLLFunction GetAssemblyFunction(int iFunctionType)
        {
            switch (iFunctionType)
            {
                case 10000:
                case 10001:
                case 10002:
                case 10003:
                    return new BLLDBUser();
                    break;

                case 11000:
                case 11001:
                case 11002:
                case 11003:
                case 11010:
                case 11011:
                case 11012:
                    return new BLLPermission();
                    break;

                case 9000:
                case 9001:
                case 9002:
                    return new BLLSysViewType();
                    break;
            }

            return null;
        }
    }
}