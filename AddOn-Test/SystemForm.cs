using AddOn_Test.FormBehaviur;
using SAPbouiCOM;
using System;
using System.Windows.Forms;

namespace AddOn_Test
{
    public class SystemForm
    {
        public SystemForm()
        {
            try
            {
                AppContext.setApplication();

                setMenuItems();
                setEvents();
                setFilters();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error al inicial el Add-on", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(0);
            }
        }

        private void setMenuItems()
        {
            Menus menus = AppContext.SBOApplication.Menus;

            MenuCreationParams creationParams = AppContext.SBOApplication.CreateObject(BoCreatableObjectType.cot_MenuCreationParams);
            SAPbouiCOM.MenuItem menuItem = AppContext.SBOApplication.Menus.Item("43520");
            menus = menuItem.SubMenus;

            //Se agrega menu de add-on <<linea de Creditos>>
            if (AppContext.SBOApplication.Menus.Exists("Facturas")) { AppContext.SBOApplication.Menus.RemoveEx("Facturas"); }

            creationParams.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            creationParams.UniqueID = "Facturas";
            creationParams.String = "Facturas";
            creationParams.Enabled = true;
            creationParams.Position = 1;
            menus.AddEx(creationParams);

            //Se agrega submenu de formulario <<Aumento de Creditos>>
            menuItem = AppContext.SBOApplication.Menus.Item("Facturas");
            menus = menuItem.SubMenus;
            creationParams.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            creationParams.UniqueID = "FacturasDoc";
            creationParams.String = "Facturas Documentos";
            menus.AddEx(creationParams);

        }

        private void setEvents()
        {
            AppContext
                .SBOApplication
                .MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(SBOApp_MenuEvent);
        }

        private void SBOApp_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {

            try
            {
                if (!pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        case "FacturasDoc": Factura factura = new Factura(); break;
                    }
                }

                BubbleEvent = false;
            }
            catch (Exception ex)
            {
                AppContext
                    .SBOApplication
                    .StatusBar
                    .SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            BubbleEvent = true;
        }



        private void setFilters()
        {
            SAPbouiCOM.EventFilter eventFilter; //No se instancia
            SAPbouiCOM.EventFilters eventFilters = new SAPbouiCOM.EventFilters();

            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_DATA_UPDATE);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_DATA_DELETE);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            eventFilter = eventFilters.Add(SAPbouiCOM.BoEventTypes.et_CLICK);

            eventFilter.AddEx("60004");
            AppContext.SBOApplication.SetFilter(eventFilters);
        }
    }
}