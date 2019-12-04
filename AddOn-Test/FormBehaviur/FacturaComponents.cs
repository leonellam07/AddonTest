using SAPbouiCOM;
using System;

namespace AddOn_Test.FormBehaviur
{
    public class FacturaComponents
    {

        private string FormID = "frmFactura";
        private string XMLFile = System.Windows.Forms.Application.StartupPath + "\\Forms\\Facturas.srf";
        public Form MainForm { get; }

        public FacturaComponents()
        {
            AppContext.LoadFromXml(XMLFile, FormID);
            MainForm = AppContext.SBOApplication.Forms.Item(FormID);

            btnBuscar = MainForm.Items.Item("btnBuscar").Specific;

            txtFecha = MainForm.Items.Item("txtFecha").Specific;

            grdFacturas = MainForm.Items.Item("grdFac").Specific;

            setDataSources();
            setBoundComponents();
            setDataTables();
        }

        #region Componentes
        public Button btnBuscar { get; }
        public EditText txtFecha { get; }
        public Grid grdFacturas { get; }
        #endregion

        #region DataSource

        private void setBoundComponents()
        {
            txtFecha.DataBind.SetBound(true, "", "UDDate");
            txtFecha.String = DateTime.Now.ToString("dd/MM/yyyy");
        }


        private void setDataSources()
        {
            if (!existsDataSource("UD", "UDDate")) MainForm.DataSources.UserDataSources.Add("UDDate", SAPbouiCOM.BoDataType.dt_DATE, 30);
        }

        public DataTable dtFacturas { get; set; }

        private void setDataTables()
        {
            if(MainForm.DataSources.DataTables.Count > 0) { return; }

            if (!existsDataSource("DT", "DTFactura")) dtFacturas = MainForm.DataSources.DataTables.Add("DTFactura");
        }


        private bool existsDataSource(string type, string uniqueID)
        {
            switch (type)
            {
                case "UD":
                    for (int i = 0; i < MainForm.DataSources.UserDataSources.Count; i++)
                    {
                        if (MainForm.DataSources.UserDataSources.Item(i).UID == uniqueID) { return true; }
                    }
                    break;
                case "DT":
                    for (int i = 0; i < MainForm.DataSources.DataTables.Count; i++)
                    {
                        if (MainForm.DataSources.DataTables.Item(i).UniqueID == uniqueID) { return true; }
                    }
                    break;
                case "DB":
                    for (int i = 0; i < MainForm.DataSources.DBDataSources.Count; i++)
                    {
                        if (MainForm.DataSources.DBDataSources.Item(i).TableName == uniqueID) { return true; }
                    }
                    break;
            }

            return false;
        }
        #endregion

    }
}