using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM;

namespace AddOn_Test.FormBehaviur
{
    public class Factura
    {
        public FacturaComponents Components = new FacturaComponents();

        public Factura()
        {
            setEvents();
            Components.MainForm.Visible = true;
            Components.MainForm.PaneLevel = 1;
        }

        private void setEvents()
        {
            Components
                .btnBuscar
                .ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(ClickAfter_btnGuardar);
        }

        private void ClickAfter_btnGuardar(object sboObject, SBOItemEventArg pVal)
        {
            Components.btnBuscar.Item.Enabled = false;
            try
            {
                string fecha = Components.txtFecha.Value;

                string query =
                    string.Format("Select top 10 \"DocEntry\",\"CardCode\", \"DocTotal\", \"DocDate\" from \"OINV\" where \"DocDate\" = '{0}'", fecha);

                Components.dtFacturas.ExecuteQuery(query);
                Components.grdFacturas.DataTable = Components.dtFacturas;

            }
            catch (Exception ex)
            {
                AppContext
                   .SBOApplication
                   .StatusBar
                   .SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                Components.btnBuscar.Item.Enabled = true;
            }
        }
    }
}
