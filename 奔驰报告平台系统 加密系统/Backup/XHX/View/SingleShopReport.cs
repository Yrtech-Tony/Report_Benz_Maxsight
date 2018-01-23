using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.DTO.SingleShopReport;
using XHX.DTO;
using XHX.Common;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;

namespace XHX.View
{
    public partial class SingleShopReport : BaseForm
    {
        public static localhost.Service service = new localhost.Service();
        MSExcelUtil msExcelUtil = new MSExcelUtil();
        public SingleShopReport()
        {
            InitializeComponent();
            XHX.Common.BindComBox.BindProject(cboProjects);
            btnModule.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        public override List<BaseForm.ButtonType> CreateButton()
        {
            List<XHX.BaseForm.ButtonType> list = new List<XHX.BaseForm.ButtonType>();
            return list;
        }

        private void btnModule_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.Filter = "Excel(*.xlsx)|";
            ofp.FilterIndex = 2;
            if (ofp.ShowDialog() == DialogResult.OK)
            {
                btnModule.Text = ofp.FileName;
            }
        }

        private void btnAllCharter_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["全国环节得分"] as Worksheet;
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            string allScore = msExcelUtil.GetCellValue(worksheet_FengMian, "A", 2).ToString();
            string CharterScore_A = msExcelUtil.GetCellValue(worksheet_FengMian, "B", 2).ToString();
            string CharterScore_B = msExcelUtil.GetCellValue(worksheet_FengMian, "C", 2).ToString();
            string CharterScore_C = msExcelUtil.GetCellValue(worksheet_FengMian, "D", 2).ToString();
            string CharterScore_D = msExcelUtil.GetCellValue(worksheet_FengMian, "E", 2).ToString();
            string CharterScore_E = msExcelUtil.GetCellValue(worksheet_FengMian, "F", 2).ToString();
            string CharterScore_F = msExcelUtil.GetCellValue(worksheet_FengMian, "G", 2).ToString();
            string CharterScore_G = msExcelUtil.GetCellValue(worksheet_FengMian, "H", 2).ToString();
            service.UploadAllScore(projectCode, allScore, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "A", CharterScore_A, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "B", CharterScore_B, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "C", CharterScore_C, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "D", CharterScore_D, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "E", CharterScore_E, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "F", CharterScore_F, "", this.UserInfoDto.UserID);
            service.UploadAllCharterScore(projectCode, "G", CharterScore_G, "", this.UserInfoDto.UserID);
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
        }

        private void btnAreaCharter_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["区域环节得分"] as Worksheet;
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            for (int i = 2; i < 15; i++)
            {
                string areaCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i).ToString();
                if (!string.IsNullOrEmpty(areaCode))
                {
                    string score = msExcelUtil.GetCellValue(worksheet_FengMian, "C", i).ToString();
                    string areaTypeCode = msExcelUtil.GetCellValue(worksheet_FengMian, "B", i).ToString();
                    service.UploadAreaScore(projectCode, areaCode, score, "", areaTypeCode, this.UserInfoDto.UserID);
                    for (int j = 4; j < 15; j++)
                    {
                        string charterCode = msExcelUtil.GetCellValue(worksheet_FengMian, j, 1).ToString();
                        if (!string.IsNullOrEmpty(charterCode))
                        {
                            service.UploadAreaCharterScore(projectCode, areaCode, charterCode,
                                msExcelUtil.GetCellValue(worksheet_FengMian, j, i).ToString(), "", "", this.UserInfoDto.UserID);
                        }
                    }
                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
        }

        private void btnShopCharter_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["经销商环节得分"] as Worksheet;
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            for (int i = 2; i < 500; i++)
            {
                string shopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i).ToString();
                if (!string.IsNullOrEmpty(shopCode))
                {
                    for (int j = 2; j < 15; j++)
                    {
                        string charterCode = msExcelUtil.GetCellValue(worksheet_FengMian, j, 1).ToString();
                        if (!string.IsNullOrEmpty(charterCode))
                        {
                            service.UploadShopCharterScore(projectCode, shopCode, msExcelUtil.GetCellValue(worksheet_FengMian, j, 1).ToString(),
                                msExcelUtil.GetCellValue(worksheet_FengMian, j, i).ToString(), "", this.UserInfoDto.UserID);
                        }
                    }
                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
        }

        private void btnShopSubject_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["经销商得分"] as Worksheet;
            string projectCode = CommonHandler.GetComboBoxSelectedValue(cboProjects).ToString();
            for (int i = 3; i < 500; i++)
            {
                string shopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i).ToString();
                if (!string.IsNullOrEmpty(shopCode))
                {
                    string orderNO_All = msExcelUtil.GetCellValue(worksheet_FengMian, "B", i).ToString();
                    string orderNO_Area = msExcelUtil.GetCellValue(worksheet_FengMian, "C", i).ToString();
                    string score = msExcelUtil.GetCellValue(worksheet_FengMian, "D", i).ToString();
                    service.UploadShopScore(projectCode, shopCode, score, Convert.ToInt32(orderNO_All), 0, Convert.ToInt32(orderNO_Area), "", this.UserInfoDto.UserID);
                    for (int j = 5; j < 50; j++)
                    {
                        string subjectCode = msExcelUtil.GetCellValue(worksheet_FengMian, j, 1).ToString();
                        string score1 = msExcelUtil.GetCellValue(worksheet_FengMian, j, i).ToString();
                        string fullScore = msExcelUtil.GetCellValue(worksheet_FengMian, j, 2).ToString();
                        if(!string.IsNullOrEmpty(subjectCode))
                        service.UploadShopSubjectScore(projectCode, shopCode, subjectCode, score1, "", fullScore, this.UserInfoDto.UserID);
                    }
                }
               
            }
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
        }
    }
}
