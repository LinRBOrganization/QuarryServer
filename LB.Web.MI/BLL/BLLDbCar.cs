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
    public class BLLDbCar: IBLLFunction
    {
        private DALDbCar _DALDbCar = null;
        public BLLDbCar()
        {
            _DALDbCar = new DAL.DALDbCar();
        }

        public override string GetFunctionName(int iFunctionType)
        {
            string strFunName = "";
            switch (iFunctionType)
            {
                case 13500:
                    strFunName = "Car_Insert";
                    break;

                case 13501:
                    strFunName = "Car_Update";
                    break;

                case 13502:
                    strFunName = "Car_Delete";
                    break;
            }
            return strFunName;
        }

        public void Car_Insert(FactoryArgs args, out t_BigID CarID,t_String CarNum, t_BigID CustomerID)
        {
            CarID = new t_BigID();

            using (DataTable dtCar = _DALDbCar.GetCarByName(args, CarID, CarNum))
            {
                if (dtCar.Rows.Count > 0)
                {
                    throw new Exception("该车牌号码已存在！");
                }
            }

            _DALDbCar.Car_Insert(args, out CarID, CarNum, CustomerID);
        }

        public void Car_Update(FactoryArgs args, t_BigID CarID, t_String CarNum, t_BigID CustomerID)
        {

            using (DataTable dtCar = _DALDbCar.GetCarByName(args, CarID, CarNum))
            {
                if (dtCar.Rows.Count > 0)
                {
                    throw new Exception("该车牌号码已存在！");
                }
            }

            _DALDbCar.Car_Update(args, CarID, CarNum, CustomerID);
        }

        public void Car_Delete(FactoryArgs args, t_BigID CarID)
        {

            _DALDbCar.Customer_Delete(args, CarID);
        }
    }
}
