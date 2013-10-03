using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Esri.FormGenerator;
using FormGeneratorAdmin;
using System.Web.UI.HtmlControls;
//using Esri.Data;
//using Esri.Web.FormGenerator.Utilities;
//using Sitecore.Data;

namespace Esri.Web.FormGenerator
{
    public partial class FormGenerator : System.Web.UI.UserControl
    {

        //private string SitecoreID = string.Empty;                                                 //dynamic from the form...
        private static string SitecoreID = "{70738B8A-F40B-45CB-8497-15A1F6B91F7E}";                //static testing..
        private Form newForm = new Form();
        private FormGeneratorData data = new FormGeneratorData();

        protected void Page_Load(object sender, EventArgs e)
        {
            //current item select from sitecore item id....
            //Sitecore.Data.Items.Item currentItem = Sitecore.Context.Item;

            //Checks sitecore id and set's the sitecore Id...
            //if (currentItem.ID.Guid != Guid.Empty)
            //{
            //SitecoreID = currentItem.ID.Guid.ToString();
            loadData();
            //}
        }

        private void loadData()
        {
            //Instantiate call to data layer..
            DataSet getData = data.GetData(SitecoreID);

            //validate data...
            if (data != null)
                BuildData(getData);
        }

        private void BuildData(DataSet data)
        {
            //Collection set for returned data...
            DataTable formData          = new DataTable();
            DataTable formContainers    = new DataTable();
            DataTable formElements      = new DataTable();
            DataTable formChildElements = new DataTable();
            DataTable elementActions    = new DataTable();
            DataTable formActions       = new DataTable();
            DataTable elementBehaviors  = new DataTable();

            //Load Data...
            formData            = data.Tables[0];
            formContainers      = data.Tables[1];
            formElements        = data.Tables[2];
            formChildElements   = data.Tables[3];
            elementActions      = data.Tables[4];
            formActions         = data.Tables[5];
            elementBehaviors    = data.Tables[6];

            //Build Objects here...
            BuildFormObjects(newForm, formData, formContainers, formElements, formChildElements, elementActions, formActions, elementBehaviors);
        }

        private void BuildFormObjects(Form newForm, DataTable formData, DataTable formContainers, DataTable formElements, DataTable formChildElements, DataTable elementActions, DataTable formActions, DataTable elementBehaviors)
        {
            //Instantiate new lists
            newForm.ContainerNames = new List<string>();
            newForm.Elements = new List<Element>();

            #region Validation

            #region FormData

            //Validation
            if (!formData.Rows[0]["Name"].Equals(null))
                newForm.Name = formData.Rows[0]["Name"].ToString();

            if (!formData.Rows[0]["ItemID"].Equals(null))
                newForm.SitecoreID = formData.Rows[0]["ItemID"].ToString();

            if (!formData.Rows[0]["CSSClass"].Equals(null))
                newForm.CssClass = formData.Rows[0]["CSSClass"].ToString();

            if (!formData.Rows[0]["Header"].Equals(null))
                newForm.Header = formData.Rows[0]["Header"].ToString();

            #endregion

            #region FormContainer

            newForm.ContainerNames = new List<string>();
            foreach (DataRow row in formContainers.Rows)
            {
                //Validation
                if (!row["ContainerID"].Equals(null))
                    newForm.ContainerNames.Add(row["ContainerID"].ToString());
            }

            #endregion

            #region FormElement

            foreach (DataRow row in formElements.Rows)
            {
                Element element = new Element();

                //Validation
                if (!row["FormControl_ID"].Equals(null))
                    element.FormControlID = Convert.ToInt32(row["FormControl_ID"]);
                if (!row["LabelName"].Equals(null))
                    element.LabelName = row["LabelName"].ToString();
                if (!row["OrderNumber"].Equals(null))
                    element.OrderNumber = Convert.ToInt32(row["OrderNumber"].ToString());
                if (!row["ContainerName"].Equals(null))
                    element.ControllerName = row["ContainerName"].ToString();
                if (!row["ElementType"].Equals(null))
                    element.Type = row["ElementType"].ToString();
                if (!row["Validate"].Equals(null))
                    element.Validate = Convert.ToBoolean(row["Validate"].ToString());
                if (!row["ControlName"].Equals(null))
                    element.ControlName = row["ControlName"].ToString();
                if (!row["AprimoFieldName"].Equals(null))
                    element.AprimoName = row["AprimoFieldName"].ToString();
                if (!row["ControlList_ID"].Equals(null))
                    element.ControlID = (int)row["ControlList_ID"];


                #region ChildElements

                if (element.Type.ToLower().Equals("group"))
                {
                    //validation for element type group name
                    if (!row["GroupName"].Equals(null))
                        element.GroupName = row["GroupName"].ToString();
                    
                    //iteration step for child element within the group element type
                    element.ChildElements = formChildElements.AsEnumerable().Select(child => new Element
                    {
                        FormControlID   = child.Field<int>("FormControl_ID"),
                        ControlID       = child.Field<int>("ControlList_ID"),
                        LabelName       = child.Field<string>("Text"),
                        ControlValue    = child.Field<string>("Value"),
                    }).ToList();
                }

                #endregion

                #region Action

                ElementAction action = new ElementAction();
                element.Actions = new List<ElementAction>();

                for (int i = 0; i < elementActions.Rows.Count; i++)
                {
                    //Validation
                    if (!elementActions.Rows[i]["ActionName"].Equals(null))
                    {
                        element.Actions.Add(new ElementAction { Action = elementActions.Rows[i]["ActionName"].ToString()});
                        element.Actions.Add(action);
                    }
                }

                #region FormActions
                for (int i = 0; i < formActions.Rows.Count; i++)
                {
                    //Validation
                    if (!formActions.Rows[i]["ECASReturnURL"].Equals(null))
                    {
                        element.Actions.Add(new ElementAction { ActionParameters =new KeyValuePair<string,string>("ECAS Login Required", formActions.Rows[i]["ECASReturnURL"].ToString())});
                        element.Actions.Add(action);
                    }
                }


                #endregion

                #endregion

                #region Behaviors

                foreach (DataRow behaviorRow in elementBehaviors.Rows)
                {
                    ElementBehavior elementBehavior = new ElementBehavior();

                    //Validation
                    if (behaviorRow["BehaviorName"].Equals(null))
                    {
                        elementBehavior.Behavior = behaviorRow["BehaviorName"].ToString();
                        element.Behaviors.Add(elementBehavior);
                    }
                }

                #endregion

                //add element to list of elements..
                newForm.Elements.Add(element);
            }

            #endregion

            #endregion

            //Build out HTML Objects...
            SetHTMLObjects(newForm);
        }

        private void SetHTMLObjects(Form newForm)
        {
            //Controls to be built out..
            List<TextBox> TextBoxes             = new List<TextBox>();
            List<DropDownList> DropDownLists    = new List<DropDownList>();
            List<Button> Buttons                = new List<Button>();

            /*Main layout for divs and spans per object, including the child divs within the element object type*/
            HtmlGenericControl dynamicDemandDiv  = new HtmlGenericControl("div");
            HtmlGenericControl dynamicActionsDiv = new HtmlGenericControl("div");
            HtmlGenericControl dynamicChildDiv   = new HtmlGenericControl("div");
            

            dynamicActionsDiv.ID = "actions";
            dynamicDemandDiv.ID = "db-form-hidden";
            dynamicChildDiv.Attributes.Add("class", "group");

            dynamicActionsDiv.ClientIDMode  = ClientIDMode.Static;
            dynamicDemandDiv.ClientIDMode   = ClientIDMode.Static;
            dynamicChildDiv.ClientIDMode    = ClientIDMode.Static;

            //Set Header..
            litHeader.Text = newForm.Header;

            ///TODO:
            ///Create a new approach for element actions, but for the moment this is work when needing the correct action per element type.
            List<ElementAction> lElementActions = new List<ElementAction>();
            foreach (Element e in newForm.Elements){
                if (e.Actions.Count > 0){
                    lElementActions = e.Actions;
                    break;
                }
            }

            //Iteration of the actions within the element action list..
            foreach (ElementAction item in lElementActions){
                HtmlGenericControl actionDiv = new HtmlGenericControl("input");
                //validation against single action type
                if (!String.IsNullOrEmpty(item.Action)){
                    actionDiv.Attributes.Add("type", "hidden");
                    actionDiv.Attributes.Add("value", item.Action);
                    dynamicActionsDiv.Controls.Add(actionDiv);
                }
                //Validation against multi action type
                if (!String.IsNullOrEmpty(item.ActionParameters.Value)){
                    actionDiv.ID = "ecasRedirect";
                    actionDiv.Attributes.Add("type", "hidden");
                    actionDiv.Attributes.Add("value", item.ActionParameters.Key);
                    actionDiv.Attributes.Add("redirect", item.ActionParameters.Value);
                    actionDiv.ClientIDMode = ClientIDMode.Static;
                    dynamicActionsDiv.Controls.Add(actionDiv);
                }
            }

            //Creating dynamic holders
            foreach (string container in newForm.ContainerNames){
                Panel newPanel = new Panel();
                newPanel.ID = "pnl" + container.ToLower();
                newPanel.ClientIDMode = ClientIDMode.Static;
                pnlContainer.Controls.Add(newPanel);
            }

            //Creating dynamic objects  
            foreach (Element element in newForm.Elements){
                //Create a new span each iteration through the elements..
                HtmlGenericControl dynamicDiv = new HtmlGenericControl("div");
                dynamicDiv.Attributes.Add("class", "set");
                dynamicDiv.Attributes.Add("fcid", element.FormControlID.ToString());
                dynamicDiv.ClientIDMode = ClientIDMode.Static;

                HtmlGenericControl dynamicSpan = new HtmlGenericControl("span");
                dynamicSpan.ClientIDMode = ClientIDMode.Static;

                //Set's each objects property set here..
                SetHtmlObjectsProperties(element, ref dynamicDiv, ref dynamicSpan, ref dynamicChildDiv);
            }

            //Build Dynamic Demand Base...
            pnlContainer.Controls.Add(dynamicDemandDiv);
            pnlContainer.Controls.Add(dynamicActionsDiv);
        }

        private void SetHtmlObjectsProperties(Element element, ref HtmlGenericControl dynamicDiv, ref HtmlGenericControl dynamicSpan, ref HtmlGenericControl dynamicChildDiv)
        {
            //Textbox
            if (element.Type.ToLower().Equals("textbox")){
                TextBox newTexbox = new TextBox();

                newTexbox.ID = "txt" + element.AprimoName.ToLower().Replace(" ", "");
                newTexbox.ClientIDMode = ClientIDMode.Static;
                newTexbox.Attributes.Add("validate", element.Validate.ToString());
                newTexbox.Attributes.Add("controlName", element.ControlName.ToLower().ToString());
                newTexbox.Attributes.Add("apr", element.AprimoName);
                

                //remove id
                if (newTexbox.ID.Equals("txtorg"))
                    newTexbox.ID = "company";

                //update div and span elements
                dynamicDiv.ID = element.LabelName;
                dynamicSpan.InnerText = element.LabelName;

                //add span to div...
                dynamicDiv.Controls.Add(dynamicSpan);
                dynamicDiv.Controls.Add(newTexbox);

                AddEditControlsToSet(dynamicDiv, element);

                //Find and store the controls in the correct div..
                pnlContainer.FindControl("pnl" + element.ControllerName.ToLower()).Controls.Add(dynamicDiv);
            }
            //Dropdown
            if (element.Type.ToLower().Equals("dropdownlist")){
                DropDownList newDropDownList = new DropDownList();
                DataTable constantValues = data.GetConstantValues(element.ControlID);
                FormGeneratorTools.BindObject(newDropDownList, constantValues, "Text", "Value", "-Select-");

                newDropDownList.ClientIDMode = ClientIDMode.Static;
                newDropDownList.ID = "ddl" + element.AprimoName;
                newDropDownList.Attributes.Add("validate", element.Validate.ToString());
                newDropDownList.Attributes.Add("controlName", element.ControlName.ToLower().ToString());
                newDropDownList.Attributes.Add("apr", element.AprimoName);

                //update span text value
                dynamicDiv.ID = element.LabelName;
                dynamicSpan.InnerText = element.LabelName;

                //add span to div
                dynamicDiv.Controls.Add(dynamicSpan);
                dynamicDiv.Controls.Add(newDropDownList);

                AddEditControlsToSet(dynamicDiv, element);

                //Find and store the controls in the correct div...
                pnlContainer.FindControl("pnl" + element.ControllerName.ToLower()).Controls.Add(dynamicDiv);
            }

            //Checkbox list
            if (element.Type.ToLower().Equals("checkbox")){
                CheckBox newCheckbox = new CheckBox();
                newCheckbox.ID = "chk" + element.AprimoName;
                newCheckbox.Attributes.Add("validate", element.Validate.ToString());
                newCheckbox.Attributes.Add("controlName", element.ControlName.ToLower().ToString());
                newCheckbox.Attributes.Add("apr", element.AprimoName);

                //update span text value
                dynamicDiv.ID = element.LabelName;
                dynamicSpan.InnerText = element.LabelName;

                //add span to div
                dynamicDiv.Controls.Add(dynamicSpan);
                dynamicSpan.Controls.Add(newCheckbox);

                //Find and store the controls in the correct div
                pnlContainer.FindControl("pn1" + element.ControllerName.ToLower()).Controls.Add(dynamicDiv);
            }

            //Multi-line
            if (element.Type.ToLower().Equals("multi-line")){
                TextBox newMultiLine = new TextBox();
                newMultiLine.TextMode = TextBoxMode.MultiLine;
                newMultiLine.ID = "txt" + element.AprimoName;
                newMultiLine.Attributes.Add("validate", element.Validate.ToString());
                newMultiLine.Attributes.Add("controlName", element.ControlName.ToLower().ToString());
                newMultiLine.Attributes.Add("apr", element.AprimoName);

                //update span text value
                dynamicDiv.ID = element.LabelName;
                dynamicSpan.InnerText = element.LabelName;

                //add span to div
                dynamicDiv.Controls.Add(dynamicSpan);
                dynamicSpan.Controls.Add(newMultiLine);

                //Find and store the controls in the correct div.
                pnlContainer.FindControl("pn1" + element.ControlName.ToLower().ToString());
            }

            //Button
            if (element.Type.ToLower().Equals("submit")){
                HtmlGenericControl newButton = new HtmlGenericControl("input");
                newButton.ClientIDMode = ClientIDMode.Static;
                newButton.ID = "btn" + element.LabelName;
                newButton.Attributes.Add("value", element.LabelName);
                newButton.Attributes.Add("type", "button");
                newButton.Attributes.Add("controlName", element.ControlName.ToLower().ToString());
                newButton.Attributes.Add("class", "buttonBlue floatfix");
                newButton.Attributes.Add("data-getw_action", "fg submit form");
                newButton.Attributes.Add("data-getw_category=", "esri.forms");

                //Find and store the controls in the correct div
                pnlContainer.FindControl("pnl" + element.ControllerName.ToLower()).Controls.Add(newButton);
            }

            //Group (child items within a element)
            if (element.Type.ToLower().Equals("group")){
                foreach (Element child in element.ChildElements)
                {
                    SetHtmlChildObjectsProperties(element, child, ref dynamicChildDiv);
                }
                pnlContainer.FindControl("pnl" + element.ControllerName.ToLower()).Controls.Add(dynamicChildDiv);
            }
        }

        private void SetHtmlChildObjectsProperties(Element parent, Element child, ref HtmlGenericControl dynamicChildDiv)
        {
            //validation for child element type..
            if(parent.ControlName.ToLower().Contains("checkboxes")){
                CheckBox newCheckBox = new CheckBox();
                newCheckBox.ID = "chk" + child.LabelName.ToLower();
                newCheckBox.ClientIDMode = ClientIDMode.Static;
                newCheckBox.Text = child.LabelName;
                newCheckBox.Attributes.Add("text", child.LabelName);
                newCheckBox.Attributes.Add("group", parent.GroupName);
                
                //Add control to the child div..
                dynamicChildDiv.Controls.Add(newCheckBox);
            }

            if(parent.ControlName.ToLower().Contains("radio")){
                RadioButton newRadioButton = new RadioButton();
                newRadioButton.ID = "rb" + child.LabelName.ToLower();
                newRadioButton.Text = child.LabelName;
                newRadioButton.ClientIDMode = System.Web.UI.ClientIDMode.Static;
                newRadioButton.Attributes.Add("text", child.LabelName);
                newRadioButton.Attributes.Add("group", parent.GroupName);

                //Add control to the child div...
                dynamicChildDiv.Controls.Add(newRadioButton);
            }
        }

        private void AddEditControlsToSet(HtmlGenericControl dynamicDiv, Element element)
        {
            //create edit buttons
            HtmlGenericControl btnRemove = new HtmlGenericControl("input");
            btnRemove.Attributes.Add("type", "button");
            btnRemove.Attributes.Add("class", "remove");
            btnRemove.Attributes.Add("value", "X");
            dynamicDiv.Controls.Add(btnRemove);

            CheckBox chkValidate = new CheckBox();
            chkValidate.CssClass = "doValidate";
            chkValidate.Text = "Validate";
            chkValidate.ID = "btn_remove_" + element.ControlID.ToString();
            chkValidate.ClientIDMode = ClientIDMode.Static;
            dynamicDiv.Controls.Add(chkValidate);        
        }
    }
}