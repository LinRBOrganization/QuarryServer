using LB.Web.Base.Factory;
using LB.Web.Base.Helper;
using LB.Web.Contants.DBType;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace LB.Web.DB.DAL
{
    public class DALDbReportTemplate
    {
        public DataTable ExistsTemplateName(FactoryArgs args, t_String ReportTemplateName,t_BigID ReportTypeID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReportTemplateName", ReportTemplateName));
            parms.Add(new LBDbParameter("ReportTypeID", ReportTypeID));
            string strSQL = @"
select *
from dbo.DbReportTemplate
where rtrim(ReportTemplateName)=rtrim(@ReportTemplateName) and
        ReportTypeID = @ReportTypeID
";
            DataTable dtReturn = DBHelper.ExecuteQuery(args, strSQL, parms);
            return dtReturn;
        }

        public void Insert(FactoryArgs args,
           out t_BigID ReportTemplateID, t_String ReportTemplateName, t_DTSmall TemplateFileTime, t_ID TemplateSeq,
           t_String Description,t_Image TemplateData, t_BigID ReportTypeID)
        {
            ReportTemplateID = new t_BigID();
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReportTemplateID", ReportTemplateID, true));
            parms.Add(new LBDbParameter("ReportTemplateName",  ReportTemplateName));
            parms.Add(new LBDbParameter("TemplateFileTime", TemplateFileTime));
            parms.Add(new LBDbParameter("TemplateSeq",  TemplateSeq));
            parms.Add(new LBDbParameter("Description",  Description));
            parms.Add(new LBDbParameter("TemplateData", TemplateData));
            parms.Add(new LBDbParameter("ReportTypeID",  ReportTypeID));
            string strSQL = @"
insert into dbo.DbReportTemplate( ReportTemplateName, TemplateFileTime,TemplateSeq,Description,TemplateData,ReportTypeID)
values( @ReportTemplateName, @TemplateFileTime,@TemplateSeq,@Description,@TemplateData,@ReportTypeID)

set @ReportTemplateID = @@identity
";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
            ReportTemplateID.SetValueWithObject(parms["ReportTemplateID"].Value);
        }

        public void Update(FactoryArgs args,
           t_BigID ReportTemplateID, t_String ReportTemplateName, t_DTSmall TemplateFileTime, t_ID TemplateSeq,
           t_String Description,t_Image TemplateData)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReportTemplateID", ReportTemplateID));
            parms.Add(new LBDbParameter("ReportTemplateName", ReportTemplateName));
            parms.Add(new LBDbParameter("TemplateFileTime", TemplateFileTime));
            parms.Add(new LBDbParameter("TemplateSeq", TemplateSeq));
            parms.Add(new LBDbParameter("Description", Description));
            parms.Add(new LBDbParameter("TemplateData", TemplateData));
            string strSQL = @"
update dbo.DbReportTemplate
set ReportTemplateName = @ReportTemplateName, 
    TemplateFileTime=@TemplateFileTime,
    TemplateSeq=@TemplateSeq,
    Description=@Description,
    TemplateData=@TemplateData
where ReportTemplateID = @ReportTemplateID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }

        public void Delete(FactoryArgs args,
          t_BigID ReportTemplateID)
        {
            LBDbParameterCollection parms = new LBDbParameterCollection();
            parms.Add(new LBDbParameter("ReportTemplateID", ReportTemplateID));
            string strSQL = @"
delete dbo.DbReportTemplate
where ReportTemplateID = @ReportTemplateID

";
            DBHelper.ExecuteNonQuery(args, System.Data.CommandType.Text, strSQL, parms, false);
        }
    }
}
