using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CentralTracking
{
    public partial class EntitiesDisplay : Form
    {
        public CentralTrackingEntities ct;

        public EntitiesDisplay()
        {
            InitializeComponent();
            ct = new CentralTrackingEntities();

            LoadEntityTypes();
        }

        /// <summary>
        /// Loads the EntityType list and calls the CellClick to show the Entities for the top EntityType in the list
        /// </summary>
        private void LoadEntityTypes()
        {
            EntityTypeList.DataSource = ct.EntityTypes.ToList();
            int columns = EntityTypeList.Columns.Count;

            for (int i = 2; i < columns; i++)
            {
                EntityTypeList.Columns[i].Visible = false;
            }

            if (EntityTypeList.Rows.Count > 0)
            {
                EntityTypeList.Rows[0].Selected = true;
                EntityTypeList_CellClick(new object(), new DataGridViewCellEventArgs(0, 0));
            }
        }

        /// <summary>
        /// Loads the Entities according to the EntityType that was selected. It calls the CellClick event
        /// for the EntityList to load the top Entity shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityTypeList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            EntityType entityType = ((EntityType)EntityTypeList.SelectedRows[0].DataBoundItem);

            EntityList.DataSource = ct.Entities.Where(p => p.EntityType.EntityTypeId == entityType.EntityTypeId).ToList();

            EntityList.Enabled = true;

            int columns = EntityList.Columns.Count;
            for (int i = 3; i < columns; i++)
            {
                EntityList.Columns[i].Visible = false;
            }

            EntityAttributeValueList.DataSource = null;
            EntityAttributeValueList.ClearSelection();

            if (EntityList.Rows.Count > 0)
            {
                EntityList.Rows[0].Selected = true;
                EntityList_CellClick(new object(), new DataGridViewCellEventArgs(0, 0));
            }
        }

        /// <summary>
        /// Displays all of the EntityAttributeValues for the selected Entity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntityList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Entity entity = ((Entity)EntityList.SelectedRows[0].DataBoundItem);

            

            EntityAttributeValueList.DataSource = (
                                                    from E in ct.Entities
                                                    join EAV in ct.EntityAttributeValues on E.EntityId equals EAV.EntityId
                                                    join A in ct.Attributes on EAV.AttributeId equals A.AttributeId
                                                    join ADT in ct.AttributeDataTypes on A.AttributeDataTypeId equals ADT.AttributeDataTypeId
                                                    join AST in ct.AttributeSelectionTypes on A.AttributeSelectionTypeId equals AST.AttributeSelectionTypeId
                                                    join V in ct.Values on EAV.ValueId equals V.ValueId
                                                    where E.EntityId == entity.EntityId
                                                    select new { A.AttributeId, A.AttributeDescription, ADT.AttributeDataTypeDescription, AST.AttributeSelectionTypeDescription, V.ValueId, V.StringValue }
                                                   ).ToList();

            EntityAttributeValueList.Enabled = true;
        }
    }
}
