using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Services;
using LB.Web.Project.Factory;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Common;
using LB.Web.Contants.DBType;
using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using LB.Web.DB.BackUp;
using LB.Web.Base.Base.Helper;

namespace LB.Web.Project
{
    /// <summary>
    /// LBWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class LBWebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public DataSet RunProcedure(int ProcedureType,string strLoginName, byte[] bSerializeValue, byte[] bSerializeDataType,
            out DataTable dtOut,out string ErrorMsg,out bool bolIsError)
        {
            dtOut = null;
            bolIsError = false;
            DataSet dsReturn = null;
            ErrorMsg = "";
            try
            {
                SQLServerDAL.GetConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

                DataTable dtParmValue = new DataTable("SPIN");
                List<Dictionary<object, object>> lstDictValue = DeserializeObject(bSerializeValue) as  List<Dictionary<object, object>>;
                Dictionary<object, object> dictDataType = DeserializeObject(bSerializeDataType) as Dictionary<object, object>;

                foreach(KeyValuePair<object,object> keyvalue in dictDataType)
                {
                    dtParmValue.Columns.Add(keyvalue.Key.ToString(), GetType(keyvalue.Value.ToString()));
                }

                foreach(Dictionary<object, object> dictValue in lstDictValue)
                {
                    DataRow drNew = dtParmValue.NewRow();
                    foreach(KeyValuePair<object, object> keyvalue in dictValue)
                    {
                        drNew[keyvalue.Key.ToString()] = keyvalue.Value;
                    }
                    dtParmValue.Rows.Add(drNew);
                }
                dtParmValue.AcceptChanges();

                DBHelper.Provider = new DBMSSQL();
                SqlConnection con = new SqlConnection(SQLServerDAL.GetConnectionString);
                string strDBName = con.Database;
                DBMSSQL.InitSettings(5000, con.DataSource, strDBName, true, "", "");
                con.Close();

                LBFactory factory = new LBFactory();
                IBLLFunction function = factory.GetAssemblyFunction(ProcedureType);

                if (function == null)
                {
                    #region -- 调用存储过程 --

                    DataTable dtView = SQLServerDAL.Query("select * from dbo.SysSPType where SysSPType=" + ProcedureType);
                    if (dtView.Rows.Count > 0)
                    {
                        DataRow drView = dtView.Rows[0];
                        string strSysSPName = drView["SysSPName"].ToString().TrimEnd();
                        SQLServerDAL.ExecuteProcedure(strSysSPName, dtParmValue, out dtOut, out dsReturn);
                    }
                    else
                    {
                        throw new Exception("存储过程号【" + ProcedureType + "】不存在！");
                    }

                    #endregion
                }
                else
                {
                    #region -- 调用中间层程序方法 --


                    string strMethod = function.GetFunctionName(ProcedureType);
                    string str = function.ToString();

                    string strLoad = str.Substring(0, str.IndexOf('.',8));
                    Assembly s = Assembly.Load(strLoad);
                    Type tpe = s.GetType(str);


                    //调用GetName方法
                    MethodInfo method = tpe.GetMethod(strMethod);

                    Dictionary<string, string> dictParmType = new Dictionary<string, string>();
                    ParameterInfo[] parameterInfos = method.GetParameters();
                    foreach(ParameterInfo parmInfo in parameterInfos)
                    {
                        if(parmInfo.ParameterType.Name != "FactoryArgs")
                        {
                            string strParmTypeName = parmInfo.ParameterType.Name.Replace("&", "");
                            if (!dictParmType.ContainsKey(strParmTypeName))
                            {
                                dictParmType.Add(parmInfo.Name, strParmTypeName);
                            }
                        }
                    }

                    int iRowIndex = 0;

                    if (dtParmValue == null || dtParmValue.Rows.Count == 0)
                        return null;

                    foreach (DataRow drParmValue in dtParmValue.Rows)
                    {
                        //获取需要传入的参数
                        ParameterInfo[] parms = method.GetParameters();

                        FactoryArgs factoryArgs = new FactoryArgs(strDBName, strLoginName, null, null);
                        Dictionary<int, string> dictOutFieldName = new Dictionary<int, string>();
                        object[] objValue = new object[parms.Length];
                        int iParmIndex = 0;
                        foreach (ParameterInfo ss in parms)
                        {
                            string strParmName = ss.Name;
                            if (ss.ParameterType == typeof(FactoryArgs))
                            {
                                objValue[iParmIndex] = factoryArgs;
                            }
                            else if (ss.Attributes != ParameterAttributes.Out)
                            {
                                if (dtParmValue.Columns.Contains(strParmName))
                                {
                                    DataColumn dc = dtParmValue.Columns[strParmName];
                                    object value = null;
                                    if(dc.DataType== typeof(long)||
                                        dc.DataType == typeof(decimal) ||
                                        dc.DataType == typeof(float) ||
                                        dc.DataType == typeof(double) ||
                                        dc.DataType == typeof(int) ||
                                        dc.DataType == typeof(byte) ||
                                        dc.DataType == typeof(bool))
                                    {
                                        if (drParmValue[strParmName] == DBNull.Value)
                                        {
                                            if (dictParmType.ContainsKey(strParmName))
                                            {
                                                ILBDbType lbType = LBDBType.GetILBDbType( dictParmType[strParmName]);
                                                value = lbType;
                                            }
                                            else
                                            {
                                                value = new t_Decimal();
                                            }
                                        }
                                        else
                                        {
                                            if (dictParmType.ContainsKey(strParmName))
                                            {
                                                ILBDbType lbType = LBDBType.GetILBDbType(dictParmType[strParmName]);
                                                lbType.SetValueWithObject(drParmValue[strParmName]);
                                                value = lbType;
                                            }
                                            else
                                            {
                                                value = new t_Decimal(drParmValue[strParmName]);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (dictParmType.ContainsKey(strParmName))
                                        {
                                            ILBDbType lbType = LBDBType.GetILBDbType(dictParmType[strParmName]);
                                            lbType.SetValueWithObject(drParmValue[strParmName]);
                                            value = lbType;
                                        }
                                        else
                                        {
                                            value = new t_String(drParmValue[strParmName]);
                                        }
                                    }
                                    objValue[iParmIndex] = value;

                                }
                            }
                            else
                            {
                                if (dictParmType.ContainsKey(strParmName))
                                {
                                    ILBDbType lbType = LBDBType.GetILBDbType(dictParmType[strParmName]);
                                    lbType.SetValueWithObject(null);
                                    objValue[iParmIndex] = lbType;
                                }

                                if (dtOut == null)
                                {
                                    dtOut = new DataTable("Out");
                                }
                                if (!dtOut.Columns.Contains(strParmName))
                                {
                                    dtOut.Columns.Add(strParmName, typeof(object));
                                }
                                dictOutFieldName.Add(iParmIndex, strParmName);
                            }

                            iParmIndex++;
                        }

                        if (dtOut != null)
                        {
                            dtOut.Rows.Add(dtOut.NewRow());
                        }

                        //获取Car对象
                        object obj = s.CreateInstance(str);
                        
                        //如果有返回值接收下
                        method.Invoke(obj, objValue);
                        int iobjReturnIndex = 0;
                        foreach (object objReturn in objValue)
                        {
                            if (objReturn is FactoryArgs)
                            {
                                FactoryArgs args = (FactoryArgs)objReturn;
                                if (args.SelectResult != null)
                                {
                                    if (dsReturn == null)
                                    {
                                        dsReturn = new DataSet("DSResult");
                                    }
                                    args.SelectResult.TableName = "Return" + iRowIndex.ToString();
                                    dsReturn.Tables.Add(args.SelectResult.Copy());
                                }
                            }
                            if (dictOutFieldName.ContainsKey(iobjReturnIndex))
                            {
                                if(objReturn is ILBDbType)
                                {
                                    ILBDbType lbtype = objReturn as ILBDbType;
                                    if (lbtype.Value != null)
                                    {
                                        dtOut.Rows[0][dictOutFieldName[iobjReturnIndex]] = lbtype.Value;
                                    }
                                }
                            }
                            iobjReturnIndex++;
                        }
                        iRowIndex++;
                    }

                    #endregion -- 调用中间层程序方法 --
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.InnerException.Message;
                bolIsError = true;
            }
            return dsReturn;
        }

        [WebMethod]
        public DataTable RunView(int iViewType, string strLoginName, string strFieldNames, string strWhere, string strOrderBy,
            out string ErrorMsg, out bool bolIsError)
        {
            DataTable dtReturn = null;
            bolIsError = false;
            ErrorMsg = "";
            BackUpHelper.StartBackUp(AppDomain.CurrentDomain.BaseDirectory);
            //LogHelper.WriteLog("正在调用" + iViewType.ToString());
            try
            {
                SQLServerDAL.GetConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                //LogHelper.WriteLog(SQLServerDAL.GetConnectionString);
                DataTable dtView = SQLServerDAL.Query("select * from dbo.SysViewType where SysViewType=" + iViewType);
                //LogHelper.WriteLog("查询语句成功！");
                if (dtView.Rows.Count == 0)
                {
                    LogHelper.WriteLog("查询出错！视图号：【" + iViewType + "】不存在！");
                    throw new Exception("查询出错！视图号：【" + iViewType+"】不存在！");
                }
                //LogHelper.WriteLog("SysViewName");
                string strSysViewName = dtView.Rows[0]["SysViewName"].ToString().TrimEnd();
                //LogHelper.WriteLog(strSysViewName);
                DataTable dtViewExists = SQLServerDAL.Query(@"
select * from sysobjects 
where id = object_id(N'["+strSysViewName+@"]')
");
                if (dtViewExists.Rows.Count == 0)
                {
                    throw new Exception("查询出错！视图名称：【" + strSysViewName + "】不存在！");
                }

                string strFields = string.IsNullOrEmpty(strFieldNames) ? "*" : strFieldNames;
                //LogHelper.WriteLog(strFields);
                strWhere =  string.IsNullOrEmpty(strWhere)?"":"where "+strWhere;
                strOrderBy =  string.IsNullOrEmpty(strOrderBy)?"":"Order By "+strOrderBy;
                string strSQL = @"
select {0}
from {1}
{2}
{3}
";
                //LogHelper.WriteLog(strSQL);
                strSQL = string.Format(strSQL, strFields, strSysViewName, strWhere, strOrderBy);
                dtReturn = SQLServerDAL.Query(strSQL);
                dtReturn.TableName = "Result";
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.InnerException.Message;
                //LogHelper.WriteLog(ErrorMsg);
                bolIsError = true;
            }
            return dtReturn;
        }

        [WebMethod]
        public DataTable RunDirectSQL(string strLoginName, string strSQL,
            out string ErrorMsg, out bool bolIsError)
        {
            DataTable dtReturn = null;
            bolIsError = false;
            ErrorMsg = "";

            try
            {
                SQLServerDAL.GetConnectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                dtReturn = SQLServerDAL.Query(strSQL);
                dtReturn.TableName = "Result";
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.InnerException.Message;
                bolIsError = true;
            }
            return dtReturn;
        }

        [WebMethod]
        public void User_Insert(string strAccount,string strPassword,string strName)
        {
            /*StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO DBUser(UserAccount,UserPassword,UserName) ");
            sb.Append("VALUES(?UserAccount,?UserPassword,?UserName) ");
            MySqlParameter[] parameters = {
                                     new MySqlParameter("?UserAccount", MySqlDbType.String),
                                     new MySqlParameter("?UserPassword", MySqlDbType.String),
                                     new MySqlParameter("?UserName", MySqlDbType.String)
                                 };
            parameters[0].Value = strAccount;
            parameters[1].Value = strPassword;
            parameters[2].Value = strName;
            DBConn.ExecuteNonQuery(sb.ToString(), CommandType.Text, parameters);*/

            /*DataTable dtSP = new DataTable();
            dtSP.Columns.Add("UserID",typeof(long));
            dtSP.Columns.Add("UserAccount",typeof(string));
            dtSP.Columns.Add("UserPassword",typeof(string));
            dtSP.Columns.Add("UserName",typeof(string));

            DataRow drNew = dtSP.NewRow();
            drNew["UserAccount"] = "林汝斌";
            drNew["UserPassword"] = "林汝斌";
            drNew["UserName"] = "林汝斌";
            dtSP.Rows.Add(drNew);
            drNew = dtSP.NewRow();
            drNew["UserAccount"] = "林汝斌1";
            drNew["UserPassword"] = "林汝斌1";
            drNew["UserName"] = "林汝斌1";
            dtSP.Rows.Add(drNew);

            DataTable dtOut;
            DataSet dsReturn;
            SQLServerDAL.ExecuteProcedure("DBUser_Insert111", dtSP, out dtOut, out dsReturn);
            //IDataParameter[] parameters = new 
            throw new Exception("eeee");*/

            LBFactory factory = new LBFactory();
            IBLLFunction function = factory.GetAssemblyFunction(10000);
            string strMethod = function.GetFunctionName(10000);
            string str = function.ToString();

            Assembly s = Assembly.Load("LB.Web.Project");


            Type tpe = s.GetType(str);

            //调用GetName方法
            MethodInfo method = tpe.GetMethod(strMethod);

            //获取需要传入的参数
            ParameterInfo[] parms = method.GetParameters();

            //这里是判断参数类型
            foreach (ParameterInfo ss in parms)
            {
                if (ss.ParameterType == typeof(string))
                {
                    Console.WriteLine("Yes");
                }
            }

            //获取Car对象
            object obj = s.CreateInstance("RecleTest.Car");

            //如果有返回值接收下
            method.Invoke(obj, new object[] { "小小" });
        }

        [WebMethod]
        public bool ConnectServer()
        {
            return true;
        }

        [WebMethod]
        public DataTable ReadClientFileInfo()
        {
            return ClientHelper.GetLocalFile();
        }

        [WebMethod]
        public void ReadFileByte(string strFileFullName, int iPosition,int iMaxLength, out byte[] bSplitFile)
        {
            bSplitFile = null;
            //int iMaxLength = 2048;//每次最大的下载长度
            string strStartUp = Path.Combine(HttpRuntime.AppDomainAppPath, "bin\\Client");
            string strFullName = Path.Combine(strStartUp, strFileFullName);
            if (File.Exists(strFullName))
            {
                using (FileStream fileStream = new FileStream(strFullName, FileMode.Open, FileAccess.Read))
                {
                    fileStream.Seek(iPosition, SeekOrigin.Begin);

                    if (fileStream.Length - iPosition > iMaxLength)
                    {
                        bSplitFile = new byte[iMaxLength];
                    }
                    else
                    {
                        bSplitFile = new byte[fileStream.Length - iPosition];
                    }

                    fileStream.Read(bSplitFile, 0, bSplitFile.Length);
                    fileStream.Close();
                }
            }
        }

        //反序列化
        public static object DeserializeObject(byte[] pBytes)
        {
            object newOjb = null;
            if (pBytes == null)
            {
                return newOjb;
            }

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream(pBytes);
            memoryStream.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
            newOjb = formatter.Deserialize(memoryStream);
            memoryStream.Close();

            return newOjb;
        }

        private static Type GetType(string strType)
        {
            if (strType.Contains("DataTable"))
            {
                return typeof(DataTable);
            }
            return Type.GetType(strType, true, true); ;
        }
    }
}
