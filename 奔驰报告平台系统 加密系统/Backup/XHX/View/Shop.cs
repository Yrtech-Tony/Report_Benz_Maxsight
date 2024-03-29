﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XHX.Common;
using XHX.DTO;
using DevExpress.XtraEditors.Repository;
using Microsoft.Office.Interop.Excel;
//using XHX.WebService;

namespace XHX.View
{
    public partial class Shop : BaseForm
    {
        XtraGridDataHandler<ShopDto> dataHandler = null;
        localhost.Service webService = new localhost.Service();
        List<AreaDto> _allAreaDtoList = new List<AreaDto>();
        List<AreaDto> _saleBigAreaDtoList = new List<AreaDto>();
        List<AreaDto> _saleSmallAreaDtoList = new List<AreaDto>();
        List<AreaDto> _afterBigAreaDtoList = new List<AreaDto>();
        List<AreaDto> _afterSmallAreaDtoList = new List<AreaDto>();
        List<SalesOrAftersalesDto> _salesOrAftersalesDtoList = new List<SalesOrAftersalesDto>();
        MSExcelUtil msExcelUtil = new MSExcelUtil();
        GridCheckMarksSelection selection;
        internal GridCheckMarksSelection Selection
        {
            get
            {
                return selection;
            }
        }
        public Shop()
        {
            InitializeComponent();
            OnLoadView();
        }

        public void OnLoadView()
        {
            //SearchAllArea();
            //SearchSalesOrAfterSales();
            ////绑定查询条件中Cbo
            //CommonHandler.SetComboBoxEditItems(cboSaleBigArea, _saleBigAreaDtoList, "AreaName", "AreaCode");//销售大区
            //CommonHandler.SetComboBoxEditItems(cboAfterBigArea, _afterBigAreaDtoList, "AreaName", "AreaCode");//售后大区
            ////绑定Grid中Cbo
            //CommonHandler.BindComboBoxItems<AreaDto>(cboSaleBigAreaInGrid, _saleBigAreaDtoList, "AreaName", "AreaCode");//销售大区
            //CommonHandler.BindComboBoxItems<AreaDto>(cboAfterBigAreaInGrid, _afterBigAreaDtoList, "AreaName", "AreaCode");//售后大区

            //CommonHandler.BindComboBoxItems<SalesOrAftersalesDto>(cboSalesOrAfterSales, _salesOrAftersalesDtoList, "Name", "Code");//售后大区
            //CommonHandler.SetRowNumberIndicator(grvShop);
            //dataHandler = new XtraGridDataHandler<ShopDto>(grvShop);
            //grcShop.DataSource = new List<ShopDto>();
            //selection = new GridCheckMarksSelection(grvShop);
            //selection.CheckMarkColumn.VisibleIndex = 0;
        }
        public void InitializeView()
        {
            txtShopName.Text = "";
            grcShop.DataSource = null;
            SearchAllArea();
        }

        public override List<ButtonType> CreateButton()
        {
            List<ButtonType> list = new List<ButtonType>();
            list.Add(ButtonType.InitButton);
            list.Add(ButtonType.SearchButton);
            list.Add(ButtonType.AddRowButton);
            list.Add(ButtonType.DeleteRowButton);
            list.Add(ButtonType.SaveButton);

            return list;
        }
        public override void InitButtonClick()
        {
            base.InitButtonClick();
            InitializeView();
        }
        public override void SearchButtonClick()
        {
            SearchShop();
            if (base.UserInfoDto.RoleType != "C")
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, true);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, true);
            }
            else
            {
                this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
                this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
            }
        }
        public override void AddRowButtonClick()
        {
            //if (CommonHandler.GetComboBoxSelectedValue(cboSaleBigArea) == null || string.IsNullOrEmpty(CommonHandler.GetComboBoxSelectedValue(cboSaleBigArea).ToString()))
            //{
            //    CommonHandler.ShowMessage(MessageType.Information, "请先选择一个大区");
            //    return;
            //}
            ShopDto shop = new ShopDto();
            shop.SaleBig = "";
            shop.AfterBig = "";
            shop.UseChk = true;
            dataHandler.AddRow(shop);
        }
        public override void DeleteRowButtonClick()
        {
            if (CommonHandler.ShowMessage(MessageType.Confirm, "删除经销商时会把所有期相关的数据都删除，确定要进行吗？") == DialogResult.Yes)
            {
                dataHandler.DelCheckedRow(grvShop.Columns.ColumnByFieldName("CheckMarkSelection"));
                selection.ClearSelection();
            }
        }
        public override void SaveButtonClick()
        {
            grvShop.CloseEditor();
            grvShop.UpdateCurrentRow();

            if (base.UserInfoDto.RoleType != "S")
            {
                CommonHandler.ShowMessage(MessageType.Information, "没有权限");
            }
            foreach (ShopDto shop in grcShop.DataSource as List<ShopDto>)
            {
                if (string.IsNullOrEmpty(shop.ShopCode))
                {
                    CommonHandler.ShowMessage(MessageType.Information, "经销商代码不能为空");
                    grvShop.FocusedColumn = gcShopCode;
                    grvShop.FocusedRowHandle = (grcShop.DataSource as List<ShopDto>).IndexOf(shop);
                    return;
                }
                foreach (ShopDto s in dataHandler.DataList)
                {
                    if (s != shop)
                    {
                        if (s.ShopCode == shop.ShopCode)
                        {
                            CommonHandler.ShowMessage(MessageType.Information, "经销商代码不能重复");
                            grvShop.FocusedColumn = gcShopCode;
                            grvShop.FocusedRowHandle = (grcShop.DataSource as List<ShopDto>).IndexOf(s);
                            return;
                        }
                    }
                }
            }
            if (CommonHandler.ShowMessage(MessageType.Confirm, "确定要保存吗？") == DialogResult.Yes)
            {
                List<ShopDto> shopList = dataHandler.DataList;
                foreach (ShopDto shop in shopList)
                {
                    if (shop.StatusType == 'I' || shop.StatusType == 'U')
                    {
                        webService.SaveShop("",shop.SaleSmall, shop.AfterSmall, shop.ShopCode, shop.ShopName.Trim(), shop.UseChk, shop.UserName, shop.Province, shop.City
                            , shop.SalesOrAftersales, shop.GroupName, shop.CarType,"",shop.ShopCode);
                    }
                    else {
                        webService.DeleteShop(shop.ShopCode);
                    }
                }
            }
            SearchShop();
            CommonHandler.ShowMessage(MessageType.Information, "保存完毕");
        }

        private void SearchShop()
        {
            List<ShopDto> shopList = new List<ShopDto>();
            DataSet ds = webService.SearchShop("", txtShopName.Text);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShopDto shop = new ShopDto();
                    shop.ShopCode = Convert.ToString(ds.Tables[0].Rows[i]["ShopCode"]);
                    shop.ShopName = Convert.ToString(ds.Tables[0].Rows[i]["ShopName"]);
                    shop.SaleBig = Convert.ToString(ds.Tables[0].Rows[i]["SellBigAreaCode"]);
                    shop.SaleSmall = Convert.ToString(ds.Tables[0].Rows[i]["SellSmallAreaCode"]);
                    shop.AfterBig = Convert.ToString(ds.Tables[0].Rows[i]["AfterBigAreaCode"]);
                    shop.AfterSmall = Convert.ToString(ds.Tables[0].Rows[i]["AfterSmallAreaCode"]);
                    shop.Province = Convert.ToString(ds.Tables[0].Rows[i]["Province"]);
                    shop.City = Convert.ToString(ds.Tables[0].Rows[i]["City"]);
                    shop.SalesOrAftersales = Convert.ToString(ds.Tables[0].Rows[i]["SalesOrAfterSales"]);
                    shop.GroupName = Convert.ToString(ds.Tables[0].Rows[i]["GroupName"]);
                    shop.CarType = Convert.ToString(ds.Tables[0].Rows[i]["CarType"]);
                    shop.UseChk = Convert.ToBoolean(ds.Tables[0].Rows[i]["UseChk"]);
                    shopList.Add(shop);
                }
            }

            grcShop.DataSource = shopList;
        }
        private void SearchAllArea()
        {
            _allAreaDtoList.Clear();
            DataSet ds = webService.SearchArea("");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    AreaDto area = new AreaDto();
                    area.AreaCode = Convert.ToString(ds.Tables[0].Rows[i]["AreaCode"]);
                    area.AreaName = Convert.ToString(ds.Tables[0].Rows[i]["AreaName"]);
                    area.UpperAreaCode = Convert.ToString(ds.Tables[0].Rows[i]["UpperAreaCode"]);
                    area.AreaTypeCode = Convert.ToString(ds.Tables[0].Rows[i]["AreaTypeCode"]);
                    _allAreaDtoList.Add(area);
                }
            }

            //设置全局变量
            _saleBigAreaDtoList = (from x in _allAreaDtoList where x.AreaTypeCode == "01" select x).ToList<AreaDto>();
            _saleSmallAreaDtoList = (from x in _allAreaDtoList where x.AreaTypeCode == "02" select x).ToList<AreaDto>();
            _afterBigAreaDtoList = (from x in _allAreaDtoList where x.AreaTypeCode == "03" select x).ToList<AreaDto>();
            _afterSmallAreaDtoList = (from x in _allAreaDtoList where x.AreaTypeCode == "04" select x).ToList<AreaDto>();

            //添加‘全部’选项
            _saleBigAreaDtoList.Insert(0, new AreaDto("", "全部", "", "01"));
            _afterBigAreaDtoList.Insert(0, new AreaDto("", "全部", "", "01"));
        }
        private void SearchSalesOrAfterSales()
        {
            SalesOrAftersalesDto s1 = new SalesOrAftersalesDto();
            s1.Code = "01";
            s1.Name = "销售";
            _salesOrAftersalesDtoList.Insert(0, s1);
            SalesOrAftersalesDto s2 = new SalesOrAftersalesDto();
            s1.Code = "02";
            s1.Name = "售后";
            _salesOrAftersalesDtoList.Insert(0, s1);

        }
        private void Shop_Load(object sender, EventArgs e)
        {
            this.CSParentForm.EnabelButton(ButtonType.AddRowButton, false);
            this.CSParentForm.EnabelButton(ButtonType.SaveButton, false);
        }
        private void gridView1_ShowingEditor(object sender, CancelEventArgs e)
        {
            ShopDto shop = grvShop.GetRow(grvShop.FocusedRowHandle) as ShopDto;
            if (grvShop.FocusedColumn == gcShopCode)
            {
                if (shop.StatusType != 'I')
                {
                    e.Cancel = true;

                }
                else
                {
                    e.Cancel = false;
                }
            }
        }
        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (grcShop.DataSource == null || grvShop.RowCount == 0) return;
            ShopDto shop = grvShop.GetRow(e.RowHandle) as ShopDto;
            if (e.Column == gcShopCode && shop.StatusType != 'I')
            {
                e.Appearance.BackColor = Color.Gray;
            }
        }
        private void cboSaleBigArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit cbo = sender as DevExpress.XtraEditors.ComboBoxEdit;
            string saleBigAreaCode = CommonHandler.GetComboBoxSelectedValue(cbo) as string;
            List<AreaDto> areaDtoList = (from x in _saleSmallAreaDtoList where x.UpperAreaCode == saleBigAreaCode select x).ToList<AreaDto>();
            areaDtoList.Insert(0, new AreaDto("", "全部", "", "03"));
            CommonHandler.SetComboBoxEditItems(cboSaleSmallArea, areaDtoList, "AreaName", "AreaCode");
        }
        private void cboAfterBigArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.ComboBoxEdit cbo = sender as DevExpress.XtraEditors.ComboBoxEdit;
            string afterBigAreaCode = CommonHandler.GetComboBoxSelectedValue(cbo) as string;
            List<AreaDto> areaDtoList = (from x in _afterSmallAreaDtoList where x.UpperAreaCode == afterBigAreaCode select x).ToList<AreaDto>();
            areaDtoList.Insert(0, new AreaDto("", "全部", "", "04"));
            CommonHandler.SetComboBoxEditItems(cboAfterSmallArea, areaDtoList, "AreaName", "AreaCode");
        }
        private void grvShop_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column == gcSaleSmallArea)
            {
                ShopDto shopDto = grvShop.GetRow(e.RowHandle) as ShopDto;
                if (shopDto != null)
                {
                    RepositoryItemComboBox cboSaleSmallAreaInGrid = new RepositoryItemComboBox();
                    CommonHandler.BindComboBoxItems<AreaDto>(cboSaleSmallAreaInGrid, (from x in _saleSmallAreaDtoList where x.UpperAreaCode == shopDto.SaleBig select x).ToList<AreaDto>(), "AreaName", "AreaCode");
                    e.RepositoryItem = cboSaleSmallAreaInGrid;
                }
            }
            else if (e.Column == gcAfterSmall)
            {
                ShopDto shopDto = grvShop.GetRow(e.RowHandle) as ShopDto;
                if (shopDto != null)
                {
                    RepositoryItemComboBox cboAfterSmallAreaInGrid = new RepositoryItemComboBox();
                    CommonHandler.BindComboBoxItems<AreaDto>(cboAfterSmallAreaInGrid, (from x in _afterSmallAreaDtoList where x.UpperAreaCode == shopDto.AfterBig select x).ToList<AreaDto>(), "AreaName", "AreaCode");
                    e.RepositoryItem = cboAfterSmallAreaInGrid;
                }
            }
            else
            {
                //continue;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Workbook workbook = msExcelUtil.OpenExcelByMSExcel(btnModule.Text);
            Worksheet worksheet_FengMian = workbook.Worksheets["Shop"] as Worksheet;
            for (int i = 2; i < 500; i++)
            {
                string shopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "A", i);
                string projectCode = "MB03";
                if (!string.IsNullOrEmpty(shopCode))
                {
                    string shopName = msExcelUtil.GetCellValue(worksheet_FengMian, "B", i);
                    string areaCode = msExcelUtil.GetCellValue(worksheet_FengMian, "E", i);
                    string province = msExcelUtil.GetCellValue(worksheet_FengMian, "F", i);
                    string city = msExcelUtil.GetCellValue(worksheet_FengMian, "G", i);
                    string group = msExcelUtil.GetCellValue(worksheet_FengMian, "H", i);
                    string salesOrAftersales = msExcelUtil.GetCellValue(worksheet_FengMian, "I", i);
                    string carType = msExcelUtil.GetCellValue(worksheet_FengMian, "J", i);
                    string upperShopCode = msExcelUtil.GetCellValue(worksheet_FengMian, "K", i); ;
                    webService.SaveShop(projectCode, areaCode, "", shopCode, shopName, true, this.UserInfoDto.UserID, province, city, salesOrAftersales, group, carType, upperShopCode,shopCode);
                }
            }
            CommonHandler.ShowMessage(MessageType.Information, "上传完毕");
            SearchShop();
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
    }


}
