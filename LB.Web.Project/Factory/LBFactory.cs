﻿using System;
using System.Collections.Generic;
using System.Web;
using LB.Web.DB.BLL;
using LB.Web.Base.Helper;
using LB.Web.RP.BLL;
using LB.Web.MI.BLL;

namespace LB.Web.Project.Factory
{
    public class LBFactory
    {
        public IBLLFunction GetAssemblyFunction(int iFunctionType)
        {
            switch (iFunctionType)
            {
                case 9000:
                case 9001:
                case 9002:
                    return new BLLSysViewType();
                case 10000:
                case 10001:
                case 10002:
                case 10003:
                    return new BLLDBUser();

                case 11000:
                case 11001:
                case 11002:
                case 11003:
                case 11010:
                case 11011:
                case 11012:
                    return new BLLPermission();

                case 12000:
                case 12001:
                case 12002:
                    return new BLLDbReportTemplate();

                case 13000:
                case 13001:
                    return new BLLDbSysLog();

                case 13100:
                case 13101:
                case 13102:
                    return new BLLUserPermission();

                case 13200:
                case 13201:
                case 13202:
                    return new BLLDbBackUpConfig();

                case 13300:
                case 13301:
                case 13302:
                case 13303:
                case 13304:
                case 13305:
                case 13306:

                    return new BLLRPReceiveBillHeader();

                case 13400:
                case 13401:
                case 13402:
                    return new BLLDbCustomer();
                case 13500:
                case 13501:
                case 13502:
                    return new BLLDbCar();
            }

            return null;
        }
    }
}