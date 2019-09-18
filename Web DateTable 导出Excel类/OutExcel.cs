using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///OutExcel 的摘要说明
/// </summary>
public class OutExcel
{

    public static void DataTable2Excel(System.Data.DataTable dtData)
    {
        System.Web.UI.WebControls.DataGrid dgExport = null;
        // 当前对话 

        System.Web.HttpContext curContext = System.Web.HttpContext.Current;
        // IO用于导出并返回excel文件 
        System.IO.StringWriter strWriter = null;
        System.Web.UI.HtmlTextWriter htmlWriter = null;

        if (dtData != null)
        {
            // 设置编码和附件格式 
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            curContext.Response.Charset = "gb2312";
            curContext.Response.AppendHeader("Content-Disposition", "attachment;filename=111111111111111.xls");

            // 导出excel文件 
            strWriter = new System.IO.StringWriter();
            htmlWriter = new System.Web.UI.HtmlTextWriter(strWriter);

            // 为了解决dgData中可能进行了分页的情况，需要重新定义一个无分页的DataGrid 
            dgExport = new System.Web.UI.WebControls.DataGrid();
            dgExport.DataSource = dtData.DefaultView;
            dgExport.AllowPaging = false;
            dgExport.DataBind();

            // 返回客户端 
            dgExport.RenderControl(htmlWriter);
            curContext.Response.Write("<meta http-equiv=\"content-type\" content=\"application/ms-excel; charset=gb2312\"/>" + strWriter.ToString());
            curContext.Response.End();
        }
    }

}